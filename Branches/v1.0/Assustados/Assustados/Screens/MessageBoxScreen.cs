using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Manager
{
    /// <summary>
    /// Classe para janela popup (message box) para confirmação
    /// </summary>
    class MessageBoxScreen : GameScreen
    {
        #region Fields

        /// <summary>
        /// Mensagem da janela de popup
        /// </summary>
        private string message;

        /// <summary>
        /// Imagem
        /// </summary>
        private Texture2D gradientTexture;

        #endregion

        #region Events

        /// <summary>
        /// Cria o evento para quando for aceito a pergunta da janela popup
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Accepted;

        /// <summary>
        /// Cria o evento para quando for cancelado a pergunta da janela popup
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        #endregion

        #region Initialization

        /// <summary>
        /// Construtor que chama por padrão o outro construtor com o texto auxiliar
        /// </summary>
        public MessageBoxScreen(string message)
            : this(message, true)
        {
        }

        /// <summary>
        /// Construtor com texto auxiliar
        /// </summary>
        public MessageBoxScreen(string message, bool includeUsageText)
        {
            // Texto auxiliar
            const string usageText = "\nA button, Space, Enter = ok" +
                                     "\nB button, Esc = cancel";

            // se é para incluir o texto
            if (includeUsageText)
            {
                this.message = string.Concat(message, usageText);
            }
            // Senão...
            else
            {
                this.message = message;
            }

            // Atribui a tela como um popup (abre sobre outra)
            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        /// <summary>
        /// Carrega os gráficos desta tela, com seu próprio gerenciador de conteúdo
        /// </summary>
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
            gradientTexture = content.Load<Texture2D>("Screens/gradient");
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Responde ao input, aceitando ou cancelando a mensagem da janela
        /// </summary>
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;

            // Se o 'ESC' for selecionado
            if (input.IsMenuSelect(this.ControllingPlayer, out playerIndex))
            {
                // Se o evento for diferente de nulo
                if (this.Accepted != null)
                {
                    this.Accepted(this, new PlayerIndexEventArgs(playerIndex));
                }
                // Chama saída do game
                this.ExitScreen();
            }
            else if (input.IsMenuCancel(this.ControllingPlayer, out playerIndex))
            {
                // Se o evento for diferente de nulo
                if (this.Cancelled != null)
                {
                    this.Cancelled(this, new PlayerIndexEventArgs(playerIndex));
                }
                // Chama saída da tela
                this.ExitScreen();
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Desenha a janela popup
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            // Escurece o fundo da tela debaixo do popup
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Centraliza a mensagem do popup na tela
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            // O fundo da tela incluindo sua borda
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad, (int)textPosition.Y - vPad, 
                                                          (int)textSize.X + hPad * 2, (int)textSize.Y + vPad * 2);

            // Cor do popup durante a transição
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();
            // Desenha o fundo
            spriteBatch.Draw(gradientTexture, backgroundRectangle, color);
            // Desenha a mensagem no popup
            spriteBatch.DrawString(font, message, textPosition, color);
            spriteBatch.End();
        }


        #endregion
    }
}
