
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
            // Mensagem que ser� colocada na tela
            string message = "Confirmar a Sa�da?";

            // Inst�ncia de tela popup (message box)
            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message, false);

            // Cria o evento para confirmar a sa�da
            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            // Adiciona a entrada(item) � cole��o do gerenciador de telas
            this.ScreenManager.AddScreen(confirmQuitMessageBox, this.ControllingPlayer);
        }
        
        /// <summary>
        /// Recebe a confirma��o que o jogador quer dar sa�da no game
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            // Abre o menu principal se for confirmada a sa�da do game corrente
            LoadingScreen.Load(ScreenManager, false, null, null, new BackgroundScreen(), new MainMenuScreen());
        }

        #endregion
    }
}
