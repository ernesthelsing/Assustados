using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Assustados.Animations
{
    /// <summary>
    /// Animação do sprite
    /// </summary>
    public class AnimatedSprite : Sprite
    {
        #region Fields

        /// <summary>
        /// Estado corrente do sprite
        /// </summary>
        private SpriteState spriteState;

        /// <summary>
        /// Último estado do sprite
        /// </summary>
        private SpriteState lastSpriteState;

        /// <summary>
        /// Armazena o estado relacionado com a animação
        /// </summary>
        private Dictionary<SpriteState, AnimationElement> animationElements;

        /// <summary>
        /// Indica o frame corrente da animação
        /// </summary>
        private float index;

        #endregion

        #region Initialize

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        public AnimatedSprite()
        {
            this.animationElements = new Dictionary<SpriteState, AnimationElement>();
        }

        #endregion

        #region Update

        /// <summary>
        /// Atualiza o sprite
        /// </summary>
        /// <param name="gameTime">Game Time(Total de segundos)</param>
        public void Update(float gameTime)
        {
            this.index += gameTime * this.animationElements[this.spriteState].FramesPerSecond;

            if (this.index > (animationElements[this.spriteState].Rectangles.Count))
            {
                this.index = 0;
            }

            this.SourceRectangle = animationElements[this.spriteState].Rectangles[(int)this.index];
        }

        /// <summary>
        /// Seta o estado corrente
        /// </summary>
        /// <param name="spriteState">Indica o estado selecionado</param>
        public void SetState(SpriteState spriteState)
        {
            if (spriteState != this.lastSpriteState)
            {
                bool result = this.animationElements.Keys.Contains(spriteState);
                
                if (result == true)
                {
                    this.spriteState = spriteState;
                    this.Image = animationElements[this.spriteState].Image;
                    this.index = 0;

                    this.lastSpriteState = spriteState;
                }
            }
        }

        /// <summary>
        /// Adiciona uma nova animação
        /// </summary>
        /// <param name="animationElement">Indica o elemento da animação</param>
        public void AddAnimation(AnimationElement animationElement)
        {
            animationElements.Add(animationElement.SpriteState, animationElement);
        }

        #endregion
    }
}
