
namespace Assustados.Manager
{
    /// <summary>
    /// Classe que serve como menu 'Pause', onde o jogador pode voltara oa game ou sair
    /// </summary>
    class PauseMenuScreen : MenuScreen
    {
        #region Initialization

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        public PauseMenuScreen()
            : base("Pause")
        {
            // Cria as entradas(itens) do menu de 'Pause'
            MenuEntry resumeGameItem = new MenuEntry("Continuar o Jogo");
            MenuEntry quitGameItem = new MenuEntry("Sair do Jogo");

            // Cria os eventos das entradas(itens)
            resumeGameItem.Selected += this.OnCancel;
            quitGameItem.Selected += this.QuitGameMenuEntrySelected;

            // Adiciona as entradas(itens) ao menu
            this.MenuEntries.Add(resumeGameItem);
            this.MenuEntries.Add(quitGameItem);
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Evento para quando for escolhido sair do game
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            // Mensagem que será colocada na tela
            string message = "Confirmar a Saída?";

            // Instância de tela popup (message box)
            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message, false);

            // Cria o evento para confirmar a saída
            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            // Adiciona a entrada(item) à coleção do gerenciador de telas
            this.ScreenManager.AddScreen(confirmQuitMessageBox, this.ControllingPlayer);
        }
        
        /// <summary>
        /// Recebe a confirmação que o jogador quer dar saída no game
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            // Abre o menu principal se for confirmada a saída do game corrente
            LoadingScreen.Load(ScreenManager, false, null, null, new BackgroundScreen(), new MainMenuScreen());
        }

        #endregion
    }
}
