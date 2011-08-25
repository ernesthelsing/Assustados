using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Manager
{
    /// <summary>
    /// Classe que representa cada entrada(item) de um menu, onde cada uma terá um evento para quando for selecionado
    /// </summary>
    class MenuEntry
    {
        #region Fields

        /// <summary>
        /// Texto processado para o item
        /// </summary>
        private string text;
        /// <summary>
        /// Efeito de tela para quando for selecionado
        /// </summary>
        private float selectionFade;

        /// <summary>
        /// A posição onde vai ser desenhada o item
        /// </summary>
        Vector2 position;

        #endregion

        #region Properties

        /// <summary>
        /// Obtém e atribui o texto do item
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Obtém e atribui a posição do item
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Evento gerado quando a entrada(item) do menu é selecionado
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Selected;

        /// <summary>
        /// Método para chamar o evento selecionado
        /// </summary>
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            // Se o evento estiver nulo
            if (this.Selected != null)
            {
                // Inicia o evento
                this.Selected(this, new PlayerIndexEventArgs(playerIndex));
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Construtor
        /// </summary>
        public MenuEntry(string text)
        {
            this.text = text;
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Atualiza a entrada do menu
        /// </summary>
        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // Cria um tempo maior para as alterações se desenvolverem mais gradualmente
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            // Se está selecionado
            if (isSelected)
            {
                this.selectionFade = Math.Min(this.selectionFade + fadeSpeed, 1);
            }
            // senão...
            else
            {
                this.selectionFade = Math.Max(this.selectionFade - fadeSpeed, 0);
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Desenha o item
        /// </summary>
        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // Se estiver selecionado fica amarelo, senão branco
            Color color = isSelected ? Color.Yellow : Color.White;

            // Movimentar o item selecionado
            double time = gameTime.ElapsedGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = 1 + pulsate * 0.05f * this.selectionFade;

            // Modifica a transparência durante as transições
            color *= screen.TransitionAlpha;

            // Desenha o texto, centralizando no meio de cada linha
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;
            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, text, position, color, 0, origin, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Consulta quanto de espaço este item requer no menu
        /// </summary>
        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }

        /// <summary>
        /// Para centralizar na tela
        /// </summary>
        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(this.text).X;
        }

        #endregion
    }
}
