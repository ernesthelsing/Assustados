using System;
using System.Collections.Generic;
using System.IO;
using Assustados.Characters;
using Assustados.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Levels
{
    public class Level : IDisposable
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private Random random;

        /// <summary>
        /// Gerenciador de conteúdo da fase
        /// </summary>
        private ContentManager content;

        /// <summary>
        /// Posição inicial/ressurreição do personagem na fase
        /// </summary>
        private Vector2 start;

        /// <summary>
        /// Conjunto de tiles da fase 
        /// </summary>
        private Tile[,] tiles; // Tiles do chão
            
        /// Implementação rápida de "layers" na engine. Os 'tiles' são os primeiros a serem
        /// desenhados na tela, depois os personagens e então os gráficos em 'walls'. Para 
        /// estes não há teste de colisão
        /// </remarks>
        private Tile[,] walls; // Tiles acima do chão e dos personagens
        private bool bnHasLayers = false;


        /// <summary>
        /// Personagem jogável
        /// </summary>
        private Player player;

        /// <summary>
        /// Inimigo
        /// </summary>
        private Monster monster;

        /// <summary>
        /// Nome da fase
        /// </summary>
        private LevelName levelName;

        /// <summary>
        /// Se a fase está completa e deve sair dela
        /// </summary>
        private bool completed;

        /// <summary>
        /// Fundo da fase
        /// </summary>
        private Texture2D backgroundTexture;

        /// <summary>
        /// Som de saída da fase
        /// </summary>
        private SoundEffect exitSound;

        /// <summary>
        /// Sons de efeito da fase e do monstro
        /// </summary>
        private SoundEffectsManager soundEffectLevel;
        private SoundEffectsManager soundEffectMonster;

        /// <summary>
        /// Viewport
        /// </summary>
        private Viewport viewport;

        /// <summary>
        /// Posições de M no mapa txt
        /// </summary>
        List<Vector2> positionsM;


        #endregion

        #region Properties

        public ContentManager Content { get { return this.content; } }

        /// <summary>
        /// Largura da fase em quantidade de objetos
        /// </summary>
        public int Width { get { return tiles.GetLength(0); } }
        
        /// <summary>
        /// Altura da fase em quantidade de objetos
        /// </summary>
        public int Height { get { return tiles.GetLength(1); } }

        /// <summary>
        /// Largura da fase em comprimento
        /// </summary>
        public int HorizontalLength { get { return this.Width * Tile.Width; } }
        
        /// <summary>
        /// Altura da fase em comprimento
        /// </summary>
        public int VerticalLenght { get { return this.Height * Tile.Height; } }

        /// <summary>
        /// Recupera o personagem
        /// </summary>
        public Player Player { get { return this.player; } }

        /// <summary>
        /// Recupera se a fase está completa
        /// </summary>
        public bool Completed { get { return this.completed; } }

        /// <summary>
        /// Recupera o nome do level
        /// Utilizado
        /// </summary>
        public string LevelName { get { return this.levelName.ToString(); } }

        /// <summary>
        /// Recupera o viewport
        /// </summary>
        public Viewport Viewport { get { return this.viewport; } }
        
        #endregion

        #region Initialize

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        /// <param name="serviceProvider">Interface para apontar o gerenciador de conteúdo (Content)</param>
        /// <param name="levelName">Nome da fase</param>
        public Level(IServiceProvider serviceProvider, Viewport viewport, LevelName levelName)
        {
            // Inicializa o gerenciador de conteúdo
            if (this.content == null)
            {
                this.content = new ContentManager(serviceProvider, "Content");
            }

            // Viewport
            this.viewport = viewport;

            // Nome da fase
            this.levelName = levelName;

            // Inicializa as posições de M com uma lista que armazenará todas as posições x e y de M no txt 
            this.positionsM = new List<Vector2>();

            // Carrega as texturas e sons da fase
            this.LoadContent();

            // Inicializa a posição do monstro fazendo um random entre os M no arquivo txt da fase
            this.random = new Random(DateTime.Now.Millisecond);
            int m = this.random.Next(positionsM.Count - 1);

            LoadMonsterTile((int)positionsM[m].X, (int)positionsM[m].Y);
        }
        
        /// <summary>
        /// Carrega os gráficos e sons da fase
        /// </summary>
        private void LoadContent()
        {
            // Atribui uma textura de fundo para a fase
            this.backgroundTexture = content.Load<Texture2D>("Backgrounds/" + this.LevelName);
            
            // Carrega a estrutura da fase
            string path = string.Concat("Content/Levels/", this.LevelName, ".txt");
            this.ReadFileLevel(path);

            // Carrega o som de saída da fase
            //this.exitSound = this.content.Load<SoundEffect>("Effects/???");

            // Carrega os som da fase
            this.soundEffectLevel = new SoundEffectsManager(this.content, "Effects/Levels/" + this.LevelName);
            this.soundEffectMonster = new SoundEffectsManager(this.content, "Effects/Monsters/" + this.LevelName);

            // Tocando a música da fase
            //MediaPlayer.Play(this.content.Load<Song>("Songs/???"));
            //MediaPlayer.IsRepeating = true;
        }

        /// <summary>
        /// Lê o arquivo e prepara uma grid com todos os objetos lá contidos
        /// Após isto, cria uma iteração para carragar os objetos na fase
        /// </summary>
        /// <param name="path">Caminho absoluto para o arquivo da fase</param>
        private void ReadFileLevel(string path)
        {
          // Carrega todas as linhas da fase
          List<string> lines = new List<string>();
          List<string> linesLayers = new List<string>(); // Linhas do arquivo com informações do layer
          string line;
          
          using (StreamReader reader = new StreamReader(path))
          {
            while ((line = reader.ReadLine()) != null)
            {
              if (line[0] != '~' && !bnHasLayers) // '~' é o separador de layers
              {
                // Copia as linhas para o mapa 'tiles'
                lines.Add(line);
              }
              else
              {
                if (!bnHasLayers)
                {
                  bnHasLayers = true;
                }
                else
                {
                  // Copia as linhas do layer para o mapa
                  linesLayers.Add(line);
                }
              }
            }
          }

          // Instanciando a grid de objetos
          this.tiles = new Tile[lines[0].Length, lines.Count];

          // Looping para cada posição da grid
          for (int y = 0; y < this.Height; ++y)
          {
            for (int x = 0; x < this.Width; ++x)
            {
              this.tiles[x, y] = this.LoadTiles(lines[y][x], x, y);
            }
          }

          if (bnHasLayers)
          {
            // Repete tudo para os layers
            // Instanciando a grid de objetos
            this.walls = new Tile[linesLayers[0].Length, linesLayers.Count];

            // Looping para cada posição da grid
            for (int y = 0; y < this.Height; ++y)
            {
              for (int x = 0; x < this.Width; ++x)
              {
                this.walls[x, y] = this.LoadTiles(linesLayers[y][x], x, y);
              }
            }
          }
        }

        /// <summary>
        /// Carrega cada objeto com sua aparência e comportamento
        /// </summary>
        /// <param name="tile">Caracter que representa o objeto que será desenhado</param>
        /// <param name="x">Posição X do objeto</param>
        /// <param name="y">Posição Y do objeto</param>
        /// <returns>Retorna o objeto carregado</returns>
        private Tile LoadTiles(char tile, int x, int y)
        {
            switch (tile)
            {
                // Espaço em branco sem colisão
                case '.':
                    return new Tile(null, Collision.Passable);
                // Espaço com tile com colisão
                case '#':
                    return LoadTile("Tiles/Block", Collision.Impassable);
                // Posição inicial do personagem principal
                case '1':
                    return LoadStartTile(x, y);
                // Monstro da fase
                case 'M':
                    positionsM.Add(new Vector2(x, y));
                    return new Tile(null, Collision.Passable);
                    //return LoadMonsterTile(x, y);
                // Caracter de objeto desconhecido
                default:
                    throw new NotSupportedException(string.Format("O caracter {0} não é suportado na posição X:{1} Y:{2}!", tile, x, y));
            }   
        }

        /// <summary>
        /// Carrega a textura e o comportamento dos objetos da fase
        /// </summary>
        private Tile LoadTile(string assetname, Collision collision)
        {
            // Objeto com textura e colisão passadas por parâmetro
            return new Tile(this.content.Load<Texture2D>(assetname), collision);
        }

        /// <summary>
        /// Instancia o personagem principal na fase
        /// </summary>
        private Tile LoadStartTile(int x, int y)
        {
            // Atribui a primeira posição de ressureição
            this.start = Mathematics.GetBottomCenter(this.GetBounds(x, y));

            // Inicializa o personagem principal
            this.player = new Player(this, this.start);

            // Posição onde está o personagem principal não tem colisão
            return new Tile(null, Collision.Passable);
        }

        /// <summary>
        /// Acrescenta o monstro na fase
        /// </summary>
        private void LoadMonsterTile(int x, int y)
        {
            // Atribui a posição inicial
            Vector2 posicao = Mathematics.GetBottomCenter(this.GetBounds(x, y));

            // Inicializa o personagem principal
            this.monster = new Monster(this, posicao);
        }

        #endregion

        #region Update
        
        /// <summary>
        /// Atualiza a fase
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Atualiza o monstro
            this.monster.Update(gameTime, this.random, this.positionsM);

            // Atualiza o personagem
            this.player.Update(gameTime);

            // Atualiza os sons da fase
            this.UpdateSounds();
        }

        /// <summary>
        /// Avalia os sons (volume) da fase
        /// </summary>
        private void UpdateSounds()
        {
            // Avalia distânica entre o personagem e o monstro
            float distance = Mathematics.GetDistance2Points(this.player.Position, this.monster.Position);

            // Obtém o volume relativo
            float newVolume = Mathematics.GetVolumeFromDistance(distance);

            // Altera o volume da música conforme a distância
            this.soundEffectMonster.SoundEffectInstance.Volume = newVolume;
            this.soundEffectLevel.SoundEffectInstance.Volume = 1 - newVolume;
        }

        #endregion

        #region Draw

        /// <summary>
        /// Desenha a fase
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Viewport viewport, Matrix camera)
        {
            // Desenha o fundo da fase (fixo - sem movimentação da câmera)
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(this.backgroundTexture, viewport.Bounds, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, camera);

            // Desenha os objetos da fase
            this.DrawTiles(spriteBatch, tiles);

            // Desenha o monstro
            this.monster.Draw(spriteBatch);

            // Desenha o personagem
            this.player.Draw(spriteBatch);

            // Desenha os layers extras
            if (bnHasLayers)
              this.DrawTiles(spriteBatch, walls);
            spriteBatch.End();
        }

        /// <summary>
        /// Desenha todos os objetos da fase
        /// </summary>
        private void DrawTiles(SpriteBatch spriteBatch, Tile [,] drawTiles)
        {
            // FIXME: renderizar somente o que efetivamente vai aparecer na tela

            // Para cada posição da fase
            for (int y = 0; y < this.Height; ++y)
            {
                for (int x = 0; x < this.Width; ++x)
                {
                    // Obtém a textura do objeto
                    Texture2D texture = drawTiles[x, y].Texture;

                    // Se o objeto for visível
                    if (texture != null)
                    {
                        // Desenha o objeto na fase
                        Vector2 position = new Vector2(x, y) * Tile.Size;
                        spriteBatch.Draw(texture, position, Color.White);
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Obtém a estrutura de um objeto no game
        /// </summary>        
        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle(x * Tile.Width, y * Tile.Height, Tile.Width, Tile.Height);
        }

        /// <summary>
        /// Obtém o tipo de colisão de acordo com a posição 
        /// </summary>
        public Collision GetCollision(int x, int y)
        {
            // Caso extrapole os limites horizontais da fase
            if (x < 0 || x >= this.Width)
            {
                return Collision.Impassable;
            }
            // Caso extrapole os limites verticais da fase
            if (y < 0 || y >= this.Height)
            {
                return Collision.Impassable;
            }

            // Retorna o tipo de colisão do objeto
            return this.tiles[x, y].Collision;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.content.Unload();
        }

        #endregion
    }
}
