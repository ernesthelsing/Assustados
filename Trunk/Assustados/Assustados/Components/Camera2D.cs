using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Components
{
    /// <summary>
    /// Classe para servir de câmera para as fases do game
    /// </summary>
    public class Camera2D
    {
        #region Fields

        /// <summary>
        /// Largura da fase em comprimento
        /// </summary>
        private int horizontalLength;

        /// <summary>
        /// Altura da fase em comprimento
        /// </summary>
        private int verticalLength;

        /// <summary>
        /// Zoom da câmera
        /// </summary>
        private float zoom;

        /// <summary>
        /// Rotação da câmera
        /// </summary>
        private float rotation;

        /// <summary>
        /// Matriz de tranformação
        /// </summary>
        private Matrix transform;

        /// <summary>
        /// Posição da câmera
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// Dimensões da tela
        /// </summary>
        private Viewport viewport;

        /// <summary>
        /// Guarda as últimas posições da câmera
        /// </summary>
        private float lastPositionX;
        private float lastPositionY;

        #endregion

        #region Constructor

        /// <summary>
        /// Construtor
        /// </summary>
        public Camera2D(Viewport viewport)
        {
            this.zoom = 1.0f;
            this.rotation = 0.0f;
            this.position = Vector2.Zero;
            this.viewport = viewport;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Atribui a largura da fase em comprimento
        /// </summary>
        public int HorizontalLength { set { this.horizontalLength = value; } }
        /// <summary>
        /// Atribui a altura da fase em comprimento
        /// </summary>
        public int VerticalLength { set { this.verticalLength = value; } }

        /// <summary>
        /// Obtém e atribui o zoom
        /// </summary>
        public float Zoom
        {
            get { return this.zoom; }
            // Evita que o zoom seja negativo
            set { this.zoom = value < 0.1f ? 0.1f : value; }
        }

        /// <summary>
        /// Obtém a atribui a posição da câmera
        /// </summary>
        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Função auxiliar para movimentar a câmera
        /// </summary>
        public void Move(Vector2 amount)
        {
            this.position += amount;
        }

        /// <summary>
        /// Atualiza a posição de desenhar a tela de acordo com a fase
        /// </summary>
        /// <returns>Retorna a matrix de transformação da câmera</returns>
        public Matrix Transformation()
        {
            // Eixos X e Y onde a câmera vai concentrar a vizualização do personagem nas fases do game
            float axisX = 0.0f;
            float axisY = 0.0f;

            this.TranslateAxisX(ref axisX);
            this.TranslateAxisY(ref axisY);

            this.transform =
                // Adicionando o zoom
                Matrix.CreateScale(new Vector3((float)Math.Pow(zoom, 3), (float)Math.Pow(zoom, 3), 0.0f)) *
                // Adicioando a rotação da câmera
                Matrix.CreateRotationZ(this.rotation) *
                // Adiciando a posição da câmera
                Matrix.CreateTranslation(new Vector3(axisX, axisY, 0.0f));

            return this.transform;
        }

        /// <summary>
        /// Ajusta a posição da câmera no eixo X
        /// </summary>
        private void TranslateAxisX(ref float axisX)
        {
            // Se a posição do personagem no eixo X multiplicado por 2 for maior/igual a largura da tela
            if ((this.position.X * 2.0f) >= this.viewport.Width)
            {
                // Ponto X ajustado para centralizar o personagem na fase
                axisX = (this.viewport.Width * 0.5f) - this.position.X;
                // Se a soma da largura da tela + o valor positivo do eixo X for > que o comprimento no eixo X da fase
                if (this.viewport.Width + (-axisX) > this.horizontalLength)
                {
                    // Eixo de X recebe a última posição de X
                    axisX = this.lastPositionX;
                }
                // Atualiza a última posição da câmera no eixo X
                this.lastPositionX = axisX;
            }
        }

        /// <summary>
        /// Ajusta a posição da câmera no eixo Y
        /// </summary>
        private void TranslateAxisY(ref float axisY)
        {
            // Se a posição do personagem no eixo Y multiplicado por 2 for maior/igual a altura da tela
            if ((this.position.Y * 2.0f) >= this.viewport.Height)
            {
                // Ponto Y ajustado para centralizar o personagem na fase
                axisY = (this.viewport.Height * 0.5f) - this.position.Y;
                // Se a soma da altura da tela + o valor positivo do eixo Y for > que o comprimento no eixo Y da fase
                if (this.viewport.Height + (-axisY) > this.verticalLength)
                {
                    // Eixo de Y recebe a última posição de Y
                    axisY = this.lastPositionY;
                }
                // Atualiza a última posição da câmera no eixo Y
                this.lastPositionY = axisY;
            }
        }

        #endregion
    }
}
