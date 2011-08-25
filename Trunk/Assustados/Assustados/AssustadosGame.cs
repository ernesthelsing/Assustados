using Assustados.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados
{
    /// <summary>
    /// CLASSE PRINCIPAL
    /// O game inicia aqui!
    /// </summary>
    public class AssustadosGame : Game
    {
        #region Fields

        /// <summary>
        /// Gerencia a interface gr�fica do device
        /// Exemplo: o tamanho da tela, se � ou n�o fullscreen
        /// </summary>
        GraphicsDeviceManager graphics;

        /// <summary>
        /// Gerenciador de telas
        /// </summary>
        ScreenManager screenManager;

        #endregion

        /// <summary>
        /// CONSTRUTOR
        /// Setar o tamanho da tela;
        /// Adicionar o ScreenManager e a tela de Menu
        /// </summary>
        public AssustadosGame()
        {
            // Nome da pasta que vai conter o conte�do do game
            this.Content.RootDirectory = "Content";

            // Criando o gerenciador de gr�ficos do device
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;
            
            // Criando o gerenciador de telas
            this.screenManager = new ScreenManager(this);
            // Adicionando o gerenciador de telas aos componentes do game
            this.Components.Add(this.screenManager);

            // Ativando as primeiras telas
            this.screenManager.AddScreen(new BackgroundScreen(), null);
            this.screenManager.AddScreen(new MainMenuScreen(), null);
        }

        /// <summary>
        /// Carregar os gr�ficos do game
        /// </summary>
        protected override void LoadContent()
        {
            Content.Load<Texture2D>("Screens/gradient");
        }

        /// <summary>
        /// O desenho do jogo vai ser feito no ScreenManager
        /// </summary>
        /// <param name="gameTime">DeltaTime do game</param>
        protected override void Draw(GameTime gameTime)
        {
            // Limpa a tela do device a cada loop
            GraphicsDevice.Clear(Color.Black);

            // A��o de desenhar do framework XNA
            base.Draw(gameTime);
        }
    }
}
