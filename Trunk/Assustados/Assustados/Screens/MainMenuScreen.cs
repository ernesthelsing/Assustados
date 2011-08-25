using Microsoft.Xna.Framework;

namespace Assustados.Manager
{
    /// <summary>
    /// Classe que representa o menu principal do game
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Fields

        /// <summary>
        /// Música do menu
        /// </summary>
        //private Song song;

        /// <summary>
        /// TODO: Para um continue
        /// </summary>
        //MenuEntry resumeGameItem;

        #endregion

        #region Initialization

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        public MainMenuScreen()
            : base("Assustados")
        {
            // Cria os itens do menu principal
            MenuEntry playGameMenuEntry = new MenuEntry("Jogar");
            MenuEntry optionsMenuEntry = new MenuEntry("Opções");
            MenuEntry exitMenuEntry = new MenuEntry("Sair");

            // Cria os eventos dos itens
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Adiciona os itens
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        /// <summary>
        /// Carregando o som do menu
        /// </summary>
        public override void LoadContent()
        {
            //this.song = this.ScreenManager.Game.Content.Load<Song>("Musics/MainMenu");

            //MediaPlayer.Play(song);
            //MediaPlayer.IsRepeating = true;
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Evento para quando 'New Game' for selecionado
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            // Começando o game
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, "Quem tem medo de Bicho-Papão?",  new GameplayScreen(0));
        }

        /// <summary>
        /// Evento para quando 'Options' for selecionado
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// Quando o jogador cancela o menu principal é perguntado se quer sair do jogo
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            // Mensagem que será colocada na tela
            string message = "Confirmar a Saída?";

            // Instância de tela popup (message box)
            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message, false);

            // Cria o evento para confirmar a saída
            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            // Adiciona a entrada(item) à coleção do gerenciador de telas
            this.ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        /// <summary>
        /// Evento para quando for confirmado a saída do game
        /// </summary>
        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            // TODO: Para a música do menu
            //MediaPlayer.Stop();

            // Fecha o game
            this.ScreenManager.Game.Exit();
        }

        #endregion
    }
}
