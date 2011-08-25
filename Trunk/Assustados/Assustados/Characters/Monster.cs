using Assustados.Animations;
using Assustados.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Characters
{
    public class Monster : Character
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Initialize

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        public Monster(Level level, Vector2 startPosition)
            : base(level, startPosition)
        {
        }

        /// <summary>
        /// Carrega imagens e sons do personagem
        /// </summary>
        public override void LoadContent()
        {
            // Imagem do inimigo
            Texture2D playerTexture = this.currentLevel.Content.Load<Texture2D>("Sprites/Monsters/" + this.currentLevel.LevelName);

            AnimationElement element;

            // Parado para baixo
            element = new AnimationElement(SpriteState.IdleDown, playerTexture, 4, 3, 1, 0);
            this.animatedSprite.AddAnimation(element);

            // Setando 1º estado
            this.animatedSprite.SetState(SpriteState.IdleDown);
        }

        #endregion

        #region Update

        #endregion

        #region Draw

        #endregion
    }
}
