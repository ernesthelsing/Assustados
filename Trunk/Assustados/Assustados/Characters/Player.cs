using System;
using Assustados.Animations;
using Assustados.Components;
using Assustados.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assustados.Characters
{
    public class Player : Character
    {
        #region Constants

        private const float MOVEACCELERATION = 8000;
        private const float MAXMOVESPEED = 800;
        private const float GROUNDDRAGFACTOR = 0.5f;

        #endregion

        #region Fields

        /// <summary>
        /// Último estado do teclado
        /// </summary>
        private KeyboardState lastKeyboardState;

        /// <summary>
        /// Game Over?
        /// </summary>
        private bool isGameOver;

        #endregion

        #region Properties

        /// <summary>
        /// Game Over?
        /// </summary>
        public bool IsGameOver
        {
            get { return this.isGameOver; }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        public Player(Level level, Vector2 startPosition)
            : base(level, startPosition)
        {
        }

        /// <summary>
        /// Carrega imagens e sons do personagem
        /// </summary>
        public override void LoadContent()
        {
            // Imagem do personagem
            Texture2D playerTexture = this.currentLevel.Content.Load<Texture2D>("Sprites/Player/Player");

            AnimationElement element;

            // Parado para baixo
            element = new AnimationElement(SpriteState.IdleDown, playerTexture, 4, 3, 1, 0);
            this.animatedSprite.AddAnimation(element);
            // Parado para esquerda
            element = new AnimationElement(SpriteState.IdleLeft, playerTexture, 4, 3, 1, 3);
            this.animatedSprite.AddAnimation(element);
            // Parado para direita
            element = new AnimationElement(SpriteState.IdleRight, playerTexture, 4, 3, 1, 6);
            this.animatedSprite.AddAnimation(element);
            // Parado para cima
            element = new AnimationElement(SpriteState.IdleUp, playerTexture, 4, 3, 1, 9);
            this.animatedSprite.AddAnimation(element);

            // Caminhando para baixo
            element = new AnimationElement(SpriteState.WalkDown, playerTexture, 4, 3, 8, new int[] { 0, 1, 2 });
            this.animatedSprite.AddAnimation(element);
            // Caminhando para esquerda
            element = new AnimationElement(SpriteState.WalkLeft, playerTexture, 4, 3, 8, new int[] { 3, 4, 5 });
            this.animatedSprite.AddAnimation(element);
            // Caminhando para direita
            element = new AnimationElement(SpriteState.WalkRight, playerTexture, 4, 3, 8, new int[] { 6, 7, 8 });
            this.animatedSprite.AddAnimation(element);
            // Caminhando para cima
            element = new AnimationElement(SpriteState.WalkUp, playerTexture, 4, 3, 8, new int[] { 9, 10, 11 });
            this.animatedSprite.AddAnimation(element);

            // Setando 1º estado
            this.animatedSprite.SetState(SpriteState.IdleDown);

            // Setando o 1º estado do sprite
            this.lastSpriteState = SpriteState.IdleDown;
        }

        #endregion

        #region Update

        /// <summary>
        /// Atualiza a movimentação, posição, avalia física e colisões
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Manipula entrada do teclado
            this.HandleInput();

            // Executa física no personagem
            this.ApplyPhysics(gameTime);

            // Atualiza a movimentação (na classe Character)
            base.Update(gameTime);
        }

        /// <summary>
        /// Manipular as entradas do teclado
        /// </summary>
        private void HandleInput()
        {
            // Recupera o estado atual do teclado
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            // Obter o movimento análogo horizontal do gamepad
            this.horizontalMovement = gamePadState.ThumbSticks.Left.X * 1;
            this.verticalMovement = gamePadState.ThumbSticks.Left.Y * 1;

            // Ignorar pequenos movimentos para evitar andar no mesmo lugar
            if (Math.Abs(this.horizontalMovement) < 0.5)
                this.horizontalMovement = 0;
            if (Math.Abs(this.verticalMovement) < 0.5)
                this.verticalMovement = 0;

            // Variável que vai armazenar o próximo estado
            SpriteState newSpriteState = this.lastSpriteState;

            // Verifica qual o estado anterior do teclado
            if (this.lastKeyboardState.IsKeyDown(Keys.Down))
            {
                newSpriteState = SpriteState.IdleDown;
            }
            else if (this.lastKeyboardState.IsKeyDown(Keys.Up))
            {
                newSpriteState = SpriteState.IdleUp;
            }
            else if (this.lastKeyboardState.IsKeyDown(Keys.Left))
            {
                newSpriteState = SpriteState.IdleLeft;
            }
            else if (this.lastKeyboardState.IsKeyDown(Keys.Right))
            {
                newSpriteState = SpriteState.IdleRight;
            }

            // Verifica qual tecla foi apertada
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                newSpriteState = SpriteState.WalkDown;
                this.verticalMovement = 1;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                newSpriteState = SpriteState.WalkUp;
                this.verticalMovement = -1;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                newSpriteState = SpriteState.WalkLeft;
                this.horizontalMovement = -1;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                newSpriteState = SpriteState.WalkRight;
                this.horizontalMovement = 1;
            }

            // Atualiza o estado do sprite
            this.animatedSprite.SetState(newSpriteState);

            // Guarda o último estado do sprite
            this.lastSpriteState = newSpriteState;

            // Guarda o último estado do teclado
            this.lastKeyboardState = keyboardState;
        }

        /// <summary>
        /// Executar a física e colisões
        /// </summary>
        /// <param name="gameTime"></param>
        private void ApplyPhysics(GameTime gameTime)
        {
            // Obtém tempo total em segundos do game
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Guarda a posição anterior
            Vector2 previousPosition = this.Position;

            // A velocidade combina movimentação com a aceleração mais o deltatime
            this.velocity.X += this.horizontalMovement * MOVEACCELERATION * elapsed;
            this.velocity.Y += this.verticalMovement * MOVEACCELERATION * elapsed;

            // Velocidade recebe fator do chão
            this.velocity *= GROUNDDRAGFACTOR;

            // Impede do personagem andar mais rápido que sua velocidade máxima
            this.velocity.X = MathHelper.Clamp(this.velocity.X, -MAXMOVESPEED, MAXMOVESPEED);
            this.velocity.Y = MathHelper.Clamp(this.velocity.Y, -MAXMOVESPEED, MAXMOVESPEED);

            // Aplica o novo posicionamento
            this.Position += this.velocity * elapsed;
            this.Position = new Vector2((float)Math.Round(this.Position.X), (float)Math.Round(this.Position.Y));

            // Verifica se o personagem está colidindo
            this.HandleCollisions();

            // Se a colisão barrou o movimento no eixo X, zerar a velocidade
            if (this.Position.X == previousPosition.X)
            {
                this.velocity.X = 0;
            }
            // Se a colisão barrou o movimento no eixo Y, zerar a velocidade
            if (this.Position.Y == previousPosition.Y)
            {
                this.velocity.Y = 0;
            }
        }

        /// <summary>
        /// Avalia se o personagem está colidindo
        /// </summary>
        private void HandleCollisions()
        {
            // Estrutura da imagem do personagem
            Rectangle bounds = this.BoundingRectangle;

            // Verifica colisão com objetos 
            int leftTile = (int)Math.Floor((float)bounds.Left / Tile.Width);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / Tile.Width));
            int topTile = (int)Math.Floor((float)bounds.Top / Tile.Height);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / Tile.Height));

            // Para cada colisão superior e inferior
            for (int y = topTile; y <= bottomTile; ++y)
            {
                // Para cada colisão lateral
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    // Obtém tipo de colisão com o objeto
                    Collision collision = this.currentLevel.GetCollision(x, y);

                    // Se o objeto é de colisão
                    if (collision != Collision.Passable)
                    {
                        // Calcula a profundidade da colisão (direção e magnitude)
                        Rectangle tileBounds = this.currentLevel.GetBounds(x, y);
                        Vector2 depth = Mathematics.GetIntersectionDepth(bounds, tileBounds);

                        // Se houver colisão
                        if (depth != Vector2.Zero)
                        {
                            // Profundidade da colisão nos eixos
                            float absDepthY = Math.Abs(depth.Y);
                            float absDepthX = Math.Abs(depth.X);

                            // Se estiver colidindo
                            if (collision == Collision.Impassable)
                            {
                                // Resolve colisão ao longo do eixo y
                                if (absDepthY < absDepthX)
                                {
                                    // Resolve colisão ao longo do eixo Y
                                    this.Position = new Vector2(this.Position.X, this.Position.Y + depth.Y);
                                }
                                else
                                {
                                    // Resolve colisão ao longo do eixo X
                                    this.Position = new Vector2(this.Position.X + depth.X, this.Position.Y);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Draw

        #endregion
    }
}
