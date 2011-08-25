using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Manager
{
    /// <summary>
    /// Classe instanciada entre as telas de menu ou do game. Ficar� aberta o tempo necess�rio de carregar a fase
    /// </summary>
    class LoadingScreen : GameScreen
    {
        #region Fields

        /// <summary>
        /// Mensagem da janela de carregamento
        /// </summary>
        private string displayText;
        /// <summary>
        /// Carregamento lento?
        /// </summary>
        private bool loadingIsSlow;
        /// <summary>
        /// As outra telas est�o ativas?
        /// </summary>
        private bool otherScreensAreGone;
        /// <summary>
        /// Telas para atualiza��o
        /// </summary>
        GameScreen[] screensToLoad;

        #endregion

        #region Initialization

        /// <summary>
        /// Construtor privado, somente intanciado pelo m�todo est�tico 'Load'
        /// </summary>
        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow, string displayText, GameScreen[] screensToLoad)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;

            // Se estiver vazia a string, seta os valores padr�es
            if (string.IsNullOrEmpty(displayText))
            {
                displayText = "Loading...";
                this.TransitionOnTime = TimeSpan.FromSeconds(0.5);
            }
            else
            {
                this.displayText = displayText;
                this.TransitionOnTime = TimeSpan.FromSeconds(5);
            }
        }

        /// <summary>
        /// Ativa a tela de 'loading'
        /// </summary>
        public static void Load(ScreenManager screenManager, bool loadingIsSlow, PlayerIndex? controllingPlayer, string displayText, params GameScreen[] screensToLoad)
        {
            // Percorre todas as telas
            foreach (GameScreen screen in screenManager.GetScreens())
            {
                // Fecha cada tela
                screen.ExitScreen();
            }

            // Inicializa a tela de 'loading'
            LoadingScreen loadingScreen = new LoadingScreen(screenManager, loadingIsSlow, displayText, screensToLoad);

            // Adiciona � cole��o de telas
            screenManager.AddScreen(loadingScreen, controllingPlayer);
        }

        #endregion

        #region Update

        /// <summary>
        /// Atualiza a tela de 'loading'
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Se todas as telas foram finalizadas, � o momento de atualiz�-las
            if (otherScreensAreGone)
            {
                // Remove a tela de 'loading'
                this.ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in this.screensToLoad)
                {
                    if (screen != null)
                    {
                        this.ScreenManager.AddScreen(screen, this.ControllingPlayer);
                    }
                }

                // Ap�s o carregamento ter terminado, � usado o rein�cio do tempo decorrido no game
                this.ScreenManager.Game.ResetElapsedTime();
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Desenha a tela de 'loading'
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Se est� somente a tela de 'loading' ativa e todas as outras foram desativadas
            if ((this.ScreenState == ScreenState.Active) && (this.ScreenManager.GetScreens().Length == 1))
            {
                // Todas as telas foram removidas
                this.otherScreensAreGone = true;
            }

            // O carregamento da tela de jogo � mais lento, por isso � desenhando a tela de 'loading'
            if (this.loadingIsSlow)
            {
                SpriteBatch spriteBatch = this.ScreenManager.SpriteBatch;
                SpriteFont font = this.ScreenManager.Font;

                Viewport viewport = this.ScreenManager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = font.MeasureString(this.displayText);
                Vector2 textPosition = (viewportSize - textSize) / 2;

                Color color = Color.White * TransitionAlpha;

                // Desenha o texto
                spriteBatch.Begin();
                spriteBatch.DrawString(font, this.displayText, textPosition, color);
                spriteBatch.End();
            }
        }

        #endregion
    }
}
