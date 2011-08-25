using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Components
{
    public class Chronometer
    {
        #region Fields

        /// <summary>
        /// Cor da font do tempo
        /// </summary>
        private Color color;
        
        /// <summary>
        /// Tempo inicial
        /// </summary>
        private TimeSpan startTime;
        
        /// <summary>
        /// Tempo final
        /// </summary>
        private TimeSpan finalTime;
        
        /// <summary>
        /// Tempo total do cronometro
        /// </summary>
        private TimeSpan time;

        /// <summary>
        /// Variavel booleana do tempo, indica se esta parado ou não
        /// </summary>
        private bool stoped;

        /// <summary>
        /// Fonte
        /// </summary>
        private SpriteFont font;
        
        /// <summary>
        /// Desenha milisegundos
        /// </summary>
        private bool drawMilliseconds;

        #endregion

        #region Properties

        /// <summary>
        /// Tempo do cronometro
        /// </summary>
        public TimeSpan Time
        {
            get { return this.time; }
            set { this.time = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        /// <param name="game">Referência para o jogo que o chamou</param>
        /// <param name="centerPosition">Posicao do centro do texto</param>
        /// <param name="font">Font carregada</param>
        /// <param name="color">Cor da font</param>
        public Chronometer(IServiceProvider serviceProvider, LevelName levelName)
        {
            if (levelName == LevelName.Bogeyman)
            {
                this.startTime = TimeSpan.FromMinutes(9.9);
                this.time = TimeSpan.FromMinutes(9.9);
            }

            this.finalTime = TimeSpan.FromMinutes(0.3);

            this.color = Color.Yellow;
            this.stoped = false;

            ContentManager content = new ContentManager(serviceProvider, "Content");
            this.font = content.Load<SpriteFont>("Fonts/chronometer");
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            if (!stoped)
            {
                this.time -= gameTime.ElapsedGameTime;
            }

            this.color = this.time < this.finalTime ? Color.Red : Color.Yellow;
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            string text = this.drawMilliseconds
                ? string.Format("{0}:{1}:{2}", string.Format("{0:00}", time.Minutes),
                                               string.Format("{0:00}", time.Seconds),
                                               time.Milliseconds >= 0 ? string.Format("{0:000}", this.time.Milliseconds) : "000")
                : string.Format("{0}:{1}", string.Format("{0:00}", time.Minutes),
                                           string.Format("{0:00}", time.Seconds));

            // Recebe o tamanho da escrita para calcular o centro
            Vector2 size = font.MeasureString(text);
            Vector2 newPosition = new Vector2(position.X - size.X, position.Y + size.Y / 1.7f);

            // Escreve o cronometro no centro conforme a posição indicada no construtor
            this.DrawFont(spriteBatch, text, newPosition, color);
        }

        #endregion

        #region Methods

        private void DrawFont(SpriteBatch spriteBatch, string display, Vector2 fontPosition, Color color)
        {
            spriteBatch.DrawString(this.font, display, fontPosition + new Vector2(2.0f, 2.0f), Color.Black);
            spriteBatch.DrawString(this.font, display, fontPosition, color);
        }

        /// <summary>
        /// Pausa o cronômetro
        /// </summary>
        public void Stop(bool stop)
        {
            this.stoped = stop;
        }

        /// <summary>
        /// Reinicia a contagem do cronometro
        /// </summary>
        public void Restart()
        {
            this.stoped = false;
            this.time = this.startTime;
        }

        #endregion
    }
}