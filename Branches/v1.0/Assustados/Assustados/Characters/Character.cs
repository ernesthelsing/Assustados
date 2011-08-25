using Assustados.Levels;
using Assustados.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Characters
{
    public abstract class Character
    {
        #region Fields
        
        /// <summary>
        /// Fase corrente
        /// </summary>
        protected Level currentLevel;

        /// <summary>
        /// Animação do sprite
        /// </summary>
        protected AnimatedSprite animatedSprite;

        /// <summary>
        /// Último estado do sprite
        /// </summary>
        protected SpriteState lastSpriteState;

        /// <summary>
        /// Velocidade e movimentação
        /// </summary>
        protected Vector2 velocity;
        protected float horizontalMovement;
        protected float verticalMovement;

        #endregion

        #region Properties

        /// <summary>
        /// Posição do personagem de acordo com o sprite
        /// </summary>
        public Vector2 Position
        {
            get { return this.animatedSprite.Position; }
            set { this.animatedSprite.Position = value; }
        }

        /// <summary>
        /// Obtém a estrutura do personagem
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle((int)this.Position.X, (int)this.Position.Y, this.animatedSprite.SourceRectangle.Width, this.animatedSprite.SourceRectangle.Height);
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        /// <param name="level">Fase corrente</param>
        /// <param name="startPosition">Posição inicial</param>
        protected Character(Level level, Vector2 startPosition)
        {
            this.animatedSprite = new AnimatedSprite();

            // Fase corrente
            this.currentLevel = level;

            // Posição inicial do personagem
            this.Position = startPosition;

            // Carrega os conteúdos do personagem
            this.LoadContent();
        }

        /// <summary>
        /// Carrega imagens e sons
        /// </summary>
        public abstract void LoadContent();

        #endregion

        #region Update

        /// <summary>
        /// Atualiza a movimentação e posição
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            // Atualiza a movimentação
            this.animatedSprite.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        #endregion

        #region Draw

        /// <summary>
        /// Desenha o sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this.animatedSprite.Draw(spriteBatch);
        }

        #endregion
    }
}
