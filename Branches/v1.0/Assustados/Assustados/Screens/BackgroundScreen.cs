using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Manager
{
    /// <summary>
    /// Classe que serve de fundo para todas as telas de menu
    /// </summary>
    class BackgroundScreen : GameScreen
    {
        #region Fields

        /// <summary>
        /// Gerenciador de conte�do da tela
        /// </summary>
        private ContentManager content;

        /// <summary>
        /// Textura e nome do fundo da tela
        /// </summary>
        private Texture2D backgroundTexture;
        private string assetNameTexture;

        #endregion

        #region Initialization

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        public BackgroundScreen() //(assetNameTexture)
        {
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5);

            // Nome da textura
            //this.assetNameTexture = assetNameTexture;
        }

        /// <summary>
        /// Carrega os conte�dos gr�ficos desta tela
        /// </summary>
        public override void LoadContent()
        {
            // Se estiver nulo
            if (this.content == null)
            {
                // Cria o gerenciador de conte�do pr�prio, para quando entrar no game poder ser descarregado
                this.content = new ContentManager(this.ScreenManager.Game.Services, "Content");
            }

            // Carrega a textura padr�o das telas de menu
            this.backgroundTexture = content.Load<Texture2D>("Screens/main");
            //this.backgroundTexture = this.content.Load<Texture2D>(string.Concat("Screens/", assetNameTexture));
        }

        /// <summary>
        /// Descarrega o gerenciador de conte�do desta tela
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion

        #region Update and Draw


        /// <summary>
        /// Ao contr�rio das outras telas esta fica sobreposta pelas outra sempre, por isso o �ltimo par�metro � sempre falso
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        #endregion

        #region Draw

        /// <summary>
        /// Desenha o fundo de tela no formato fullscreen
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();
            spriteBatch.Draw(this.backgroundTexture, fullscreen, new Color(this.TransitionAlpha, this.TransitionAlpha, this.TransitionAlpha));
            spriteBatch.End();
        }

        #endregion
    }
}
