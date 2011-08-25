using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Animations
{
    /// <summary>
    /// Sprite
    /// </summary>
    public class Sprite
    {
        #region Properties

        /// <summary>
        /// Posição do sprite
        /// </summary>
        public Vector2 Position { get; set; }
        
        /// <summary>
        /// Imagem do sprite
        /// </summary>
        public Texture2D Image
        {
            get;
            protected set;
        }

        /// <summary>
        /// Quadro (retângulo) do sprite
        /// </summary>
        public Rectangle SourceRectangle
        {
            get;
            protected set;
        }

        #endregion

        #region Initialize
        
        /// <summary> 
        /// Carrega a imagem do sprite e define seu quadro (retãngulo)
        /// Ex: Imagem inteira (sem animação)
        /// </summary>
        public void LoadContent(Texture2D image)
        {
            this.Image = image;
            this.SourceRectangle = new Rectangle(0, 0, this.Image.Width, this.Image.Height);            
        }

        #endregion

        #region Draw

        /// <summary>
        /// Desenha o sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Image, this.Position, this.SourceRectangle, Color.White);
        }

        #endregion
    }
}
