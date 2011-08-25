using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Animations
{
    /// <summary>
    /// Representa o elemento necessário para execução da animação
    /// </summary>
    public struct AnimationElement
    {
        #region Fields

        /// <summary>
        /// O estado do sprite
        /// </summary>
        private SpriteState spriteState;

        /// <summary>
        /// Imagem da animação
        /// </summary>
        private Texture2D image;

        /// <summary>
        /// Indica quantos frames serão desenhados por segundo
        /// </summary>
        private int framesPerSecond;

        /// <summary>
        /// Quantidade de linhas da animação
        /// </summary>
        private int rows;

        /// <summary>
        /// Quantidade de colunas da animação
        /// </summary>
        private int columns;

        /// <summary>
        /// Regiões (retângulos) de cada frame
        /// </summary>
        private List<Rectangle> rectangles;

        /// <summary>
        /// Largura do frame
        /// </summary>
        private int frameWidth;

        /// <summary>
        /// Altura do frame
        /// </summary>
        private int frameHeight;

        #endregion        

        #region Properties

        /// <summary>
        /// Recupera quantos frames serão desenhados por segundo
        /// </summary>
        public int FramesPerSecond
        {
            get { return this.framesPerSecond; }
        }

        /// <summary>
        /// Recupera o estado do sprite corrente
        /// </summary>
        public SpriteState SpriteState
        {
            get { return this.spriteState; }
        }

        /// <summary>
        /// Recupera a imagem
        /// </summary>
        public Texture2D Image
        {
            get { return this.image; }
        }

        /// <summary>
        /// Recupera a quantidade de linhas
        /// </summary>
        public int Rows
        {
            get { return this.rows; }
        }

        /// <summary>
        /// Recupera a quantidade de colunas
        /// </summary>
        public int Columns
        {
            get { return this.columns; }
        }

        /// <summary>
        /// Recupera a coleção de retângulos
        /// </summary>
        public List<Rectangle> Rectangles
        {
            get { return this.rectangles; }
        }

        #endregion

        #region Initialize
        
        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        /// <param name="spriteState">Estado do sprite (tipo)</param>
        /// <param name="image">Imagem</param>
        /// <param name="rows">Quantidade de linhas</param>
        /// <param name="columns">Quantidade de colunas</param>
        /// <param name="framesPerSecond">frames por segundo</param>
        public AnimationElement(SpriteState spriteState, Texture2D image, int rows, int columns, int framesPerSecond)
        {
            // Inicializa as variáveis
            this.spriteState = spriteState;
            this.image = image;
            this.rows = rows;
            this.columns = columns;
            this.framesPerSecond = framesPerSecond;

            this.rectangles = new List<Rectangle>();

            this.frameWidth = this.image.Width / columns;
            this.frameHeight = this.image.Height / rows;

            // Monta os frames
            this.BuildFrames();
        }

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        /// <param name="spriteState">Estado do sprite (tipo)</param>
        /// <param name="image">Imagem</param>
        /// <param name="rows">Quantidade de linhas</param>
        /// <param name="columns">Quantidade de colunas</param>
        /// <param name="framesPerSecond">frames por segundo</param>
        public AnimationElement(SpriteState spriteState, Texture2D image, int rows, int columns, int framesPerSecond, params int[] usedFramesIndex)
        {
            // Inicializa as variáveis
            this.spriteState = spriteState;
            this.image = image;
            this.rows = rows;
            this.columns = columns;
            this.framesPerSecond = framesPerSecond;

            this.rectangles = new List<Rectangle>();

            this.frameWidth = this.image.Width / columns;
            this.frameHeight = this.image.Height / rows;

            // Monta os frames
            this.BuildFrames(usedFramesIndex);
        }

        /// <summary>
        /// Monta o quadro (retângulo) para cada frame da imagem
        /// </summary>
        private void BuildFrames()
        {
            Rectangle frame = new Rectangle();

            frame.Width = this.frameWidth;
            frame.Height = this.frameHeight;

            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    frame.X = j * this.frameWidth;
                    frame.Y = i * this.frameHeight;

                    this.rectangles.Add(frame);
                }
            }
        }

        /// <summary>
        /// Monta o quadro (retângulo) para cada frame da imagem
        /// </summary>
        /// <param name="usedFramesIndex">Apenas os frames que devem ser carregados</param>
        private void BuildFrames(int[] usedFramesIndex)
        {
            Rectangle frame = new Rectangle();

            frame.Width = this.frameWidth;
            frame.Height = this.frameHeight;
            
            int counter = 0;

            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    if (usedFramesIndex.Contains(counter))
                    {
                        frame.X = j * this.frameWidth;
                        frame.Y = i * this.frameHeight;

                        this.rectangles.Add(frame);
                    }
                    counter++;
                }
            }
        }

        #endregion
    }
}
