using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Components
{
    /// <summary>
    /// Classe 'singleton' para controlar estado e condição do jogador
    /// </summary>
    public class Status : IDisposable
    {
        #region Fields

        /// <summary>
        /// Única instância da classe
        /// </summary>
        public static Status InstanceStatus;
        
        /// <summary>
        /// Gerenciador de conteúdo do status do jogador
        /// </summary>
        private ContentManager content;

        /// <summary>
        /// Fase onde está o jogador
        /// </summary>
        private LevelName levelName;
        
        /// <summary>
        /// Vidas e textura
        /// </summary>
        private int lives;
        private Texture2D liveTexture;

        /// <summary>
        /// Total de moedas e textura
        /// </summary>
        private int coins;
        private Texture2D coinTexture;

        /// <summary>
        /// Tempo total de jogo, tempo restante na fase ou desafio e se deve ser disparado?
        /// </summary>
        private TimeSpan totalGameTime;
        private Chronometer chronometer;
        private bool isTriggerTimer;

        /// <summary>
        /// Total de pontos acumulados
        /// </summary>
        private int score;

        /// <summary>
        /// Fonte para escrever o status
        /// </summary>
        private SpriteFont fontGame;

        #endregion

        #region Initialize

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        private Status(IServiceProvider serviceProvider)
        {
            // Inicializa as variáveis
            this.lives = 2;
            this.coins = 0;
            this.score = 0;

            // Se o gerenciador de conteúdo for nulo
            if (this.content == null)
            {
                // Inicializa o gerenciador de conteúdo do status do jogador
                this.content = new ContentManager(serviceProvider, "Content");
            }

            // Carrega as texturas
            this.LoadContent();
        }

        /// <summary>
        /// Instancia o status do jogador para inicialização de fases
        /// </summary>
        public static void InitializeAllStatus(IServiceProvider serviceProvider, LevelName levelName, bool isTriggerTime)
        {
            // Se estiver nulo instancia um novo status
            if (Status.InstanceStatus == null)
            {
                Status.InstanceStatus = new Status(serviceProvider);
            }

            // Atualiza o território da fase
            Status.InstanceStatus.levelName = levelName;

            Status.InstanceStatus.isTriggerTimer = isTriggerTime;
            Status.InstanceStatus.chronometer = new Chronometer(serviceProvider, levelName);
        }

        /// <summary>
        /// Instancia o status do jogador para 'loadgame'
        /// </summary>
        public static void InitializeStatus(IServiceProvider serviceProvider)
        {
            // Se estiver nulo instancia um novo status
            if (Status.InstanceStatus == null)
            {
                Status.InstanceStatus = new Status(serviceProvider);
            }
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Obtém e atribui as vidas
        /// </summary>
        public int Lives
        {
            get { return this.lives; }
            set { this.lives = value; }
        }

        /// <summary>
        /// Obtém e atribui as moedas
        /// </summary>
        public int Coins
        {
            get { return this.coins; }
            set { this.coins = value; }
        }
        /// <summary>
        /// Obtém e atribui os pontos
        /// </summary>
        public int Score
        {
            get { return this.score; }
            set { this.score = value; }
        }
        
        /// <summary>
        /// Obtém se tem cronometro
        /// </summary>
        public bool IsTriggerTime
        {
            get { return this.isTriggerTimer; }
        }
        
        /// <summary>
        /// Obtém o cronômetro
        /// </summary>
        public Chronometer Chronometer { get { return this.chronometer; } }

        #endregion

        #region Load/Status Methods

        /// <summary>
        /// Carrega os gráficos do status do jogador
        /// </summary>
        private void LoadContent()
        {
            // Carrega a imagem da vida
            //this.liveTexture = this.content.Load<Texture2D>("Items/???");
            // Carrega a imagem da moeda
            //this.coinTexture = this.content.Load<Texture2D>("Items/???");
            // Carrefa a fonte do status
            this.fontGame = this.content.Load<SpriteFont>("Fonts/gamefont");
        }

        #endregion

        #region Update

        /// <summary>
        /// Atualiza o status
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // Incrementa o contador de tempo de jogo
            this.totalGameTime += gameTime.ElapsedGameTime;

            if (this.isTriggerTimer)
            {
                this.chronometer.Update(gameTime);
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Desenha a fonte de status do personagem
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Viewport viewport)
        {
            // Limite para inserção de título
            Rectangle titleSafeArea = viewport.TitleSafeArea;
            // Posição central da tela no eixo X
            float centerX = titleSafeArea.X + titleSafeArea.Width / 2.0f;
            
            // Posição das vidas
            Vector2 lifesPositon = new Vector2(titleSafeArea.X + 20.0f, 20.0f);
            Vector2 lifesFontPositon = new Vector2(this.liveTexture.Width + 20.0f, 50.0f);
            
            // Pontuação
            string scoreText = "PONTOS ";
            float scoreWidth = this.fontGame.MeasureString(scoreText).X;
            string scoreTotal = string.Format("{0:00000}", this.score);
            
            // moedas
            float resinWidth = this.fontGame.MeasureString(scoreTotal).X;

            spriteBatch.Begin();

            // Desenha as vidas
            spriteBatch.Draw(this.liveTexture, lifesPositon, Color.White);
            this.DrawFont(spriteBatch, string.Concat("x", this.lives), lifesFontPositon, Color.Yellow);

            // Desenha a pontuação
            this.DrawFont(spriteBatch, scoreText, new Vector2(centerX - (scoreWidth * 2.0f), 50.0f), Color.Yellow);
            this.DrawFont(spriteBatch, scoreTotal, new Vector2(centerX - scoreWidth, 50.0f), Color.White);

            // Desenha as moedas capturadas
            spriteBatch.Draw(this.coinTexture, new Vector2(centerX + resinWidth, 40.0f), Color.White);
            this.DrawFont(spriteBatch, this.coins.ToString(), new Vector2(centerX + (resinWidth * 1.8f), 50.0f), Color.White);

            // Desenha o cronômetro
            if (this.isTriggerTimer)
            {
                Vector2 rightPositon = new Vector2(titleSafeArea.Width - 20.0f, 20.0f);
                this.chronometer.Draw(spriteBatch, rightPositon);
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Desenha o texto com uma sombra por baixo
        /// </summary>
        private void DrawFont(SpriteBatch spriteBatch, string display, Vector2 fontPosition, Color color)
        {
            spriteBatch.DrawString(this.fontGame, display, fontPosition + new Vector2(2.0f, 2.0f), Color.Black);
            spriteBatch.DrawString(this.fontGame, display, fontPosition, color);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.content.Unload();
            this.content.Dispose();

            Status.InstanceStatus = null;
        }

        #endregion
    }
}