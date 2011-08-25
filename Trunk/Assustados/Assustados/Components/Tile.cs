using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Components
{
    /// <summary>
    /// Aparência e comportamento de colisão de um objteto
    /// </summary>
    public struct Tile
    {
        #region Fields

        /// <summary>
        /// Diâmetros do objeto
        /// </summary>
        public const int Width = 50;
        public const int Height = 50;

        /// <summary>
        /// Imagem do objeto
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Comportamento de colisão
        /// </summary>
        private Collision collision;

        /// <summary>
        /// Tamanho do objeto
        /// </summary>
        public static readonly Vector2 Size = new Vector2(Width, Height);

        #endregion

        #region Constructor

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        public Tile(Texture2D texture, Collision collision)
        {
            this.texture = texture;
            this.collision = collision;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Obtém a imagem do objeto
        /// </summary>
        public Texture2D Texture { get { return this.texture; } }
        
        /// <summary>
        /// Obtém e atribui o comportamento do objeto
        /// </summary>
        public Collision Collision 
        { 
            get { return this.collision; } 
            set { this.collision = value; } 
        }

        #endregion
    }
}