
namespace Assustados.Manager
{
    /// <summary>
    /// Classe de op��es do game, para personalizar algumas diretivas de jogo
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        #endregion

        #region Initialization

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        public OptionsMenuScreen()
            : base("Op��es")
        {
            // Cria os itens do menu de op��es
            // TODO

            // Inicializa os textos dos itens do menu de op��es
            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Voltar");

            // Cria os eventos dos itens
            back.Selected += OnCancel;

            // Adiciona os itens ao menu
            MenuEntries.Add(back);
        }

        /// <summary>
        /// Preenche os textos dos itens do menu atualizados
        /// </summary>
        void SetMenuEntryText()
        {
        }

        #endregion

        #region Handle Input


        #endregion
    }
}
