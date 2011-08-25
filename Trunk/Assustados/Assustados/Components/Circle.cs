using Microsoft.Xna.Framework;

namespace Assustados.Components
{
    /// <summary>
    /// Representa um círculo 2D
    /// </summary>
    public struct Circle
    {
        #region Fields

        /// <summary>
        /// Posição central do círculo
        /// </summary>
        private Vector2 center;

        /// <summary>
        /// Raio do círculo
        /// </summary>
        private float radius;

        #endregion

        #region Constructor

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        public Circle(Vector2 position, float radius)
        {
            this.center = position;
            this.radius = radius;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Obtém o raio do círculo
        /// </summary>
        public float Radius { get { return this.radius; } }

        #endregion

        #region Methods

        /// <summary>
        /// Determina se o círculo interceptou um retângulo
        /// </summary>
        /// <returns>Se o círculo e um retãngulo se cruzam</returns>
        public bool Intersects(Rectangle rectangle)
        {
            Vector2 vector = new Vector2(MathHelper.Clamp(this.center.X, rectangle.Left, rectangle.Right), 
                                         MathHelper.Clamp(this.center.Y, rectangle.Top, rectangle.Bottom));

            Vector2 direction = this.center - vector;
            float distancedSquared = direction.LengthSquared();

            return ((distancedSquared > 0) && (distancedSquared < this.radius * this.radius));
        }

        #endregion
    }
}
