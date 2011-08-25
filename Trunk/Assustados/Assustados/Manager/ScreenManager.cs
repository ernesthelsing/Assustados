using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Manager
{
    /// <summary>
    /// Classe gerenciadora de telas do game. 
    /// Mant�m uma lista de telas, chamando seus 'Updates' e 'Draws'
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields

        /// <summary>
        /// Lista de telas do game
        /// </summary>
        private List<GameScreen> screens = new List<GameScreen>();
        private List<GameScreen> screensToUpdate = new List<GameScreen>();

        /// <summary>
        /// Entrada do teclado
        /// </summary>
        private InputState input = new InputState();

        /// <summary>
        /// Desenha os gr�ficos do game
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Fonte para mostrar as op��es das telas de menu
        /// </summary>
        private SpriteFont font;

        /// <summary>
        /// Textura inicial em branco
        /// </summary>
        private Texture2D blankTexture;

        /// <summary>
        /// Foi inicializado o game
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// Habilita o trace/debug
        /// </summary>
        private bool traceEnabled;

        #endregion

        #region Properties

        /// <summary>
        /// Obt�m o objeto �nico para desenhar todas as telas
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        /// <summary>
        /// Obt�m a fonte para as telas de menu
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
        }

        /// <summary>
        /// Obt�m e seta se o tarce/debug estar� habilitado
        /// </summary>
        public bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        public ScreenManager(AssustadosGame game)
            : base(game)
        {
        }

        /// <summary>
        /// Inicializa o gerenciador de telas
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            isInitialized = true;
        }

        /// <summary>
        /// Carrega s conte�dos
        /// </summary>
        protected override void LoadContent()
        {
            // Gerenciador de conte�dos do gerenciador de telas do game
            ContentManager content = this.Game.Content;

            // Instancia o spriteBatch �nico
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            // Carrega a fonte
            this.font = content.Load<SpriteFont>("Fonts/menufont");
            // Carrega o fundo em branco
            this.blankTexture = content.Load<Texture2D>("Screens/blank");

            // Carrega todas as telas da cole��o
            foreach (GameScreen screen in screens)
            {
                screen.LoadContent();
            }
        }

        /// <summary>
        /// Descarrega os conte�dos
        /// </summary>
        protected override void UnloadContent()
        {
            // Descarrega todas as telas da cole��o
            foreach (GameScreen screen in screens)
            {
                screen.UnloadContent();
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Chama a atualiza��o de cada tela do game
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Lendo a entrada do teclado e GamePad
            this.input.Update();

            // Limpando a c�pia da lista principal
            this.screensToUpdate.Clear();

            // Criando uma c�pia da lista principal para evitar confus�o 
            foreach (GameScreen screen in this.screens)
            {
                // Adiciona a tela � lista de c�pias
                this.screensToUpdate.Add(screen);
            }

            // Outra tela n�o est� ativa?
            bool otherScreenHasFocus = !this.Game.IsActive;
            // N�o abrangido por outra tela
            bool coveredByOtherScreen = false;

            // Enquanto houver telas para serem atualizadas
            while (this.screensToUpdate.Count > 0)
            {
                // Pega a �ltima tela (de baixo para cima na listagem)
                GameScreen screen = this.screensToUpdate[this.screensToUpdate.Count - 1];
                // Remove da lista de c�pias
                this.screensToUpdate.RemoveAt(this.screensToUpdate.Count - 1);

                // Atualiza a tela
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                // Se a tela estiver sendo aberta ou ativa
                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
                {
                    // Se for a primeira tela, manipula a entrada do teclado da mesma
                    if (!otherScreenHasFocus)
                    {
                        // L� a entrada do teclado nesta tela
                        screen.HandleInput(this.input);
                        // Atribui como verdadeiro o foco para outra tela
                        otherScreenHasFocus = true;
                    }

                    // Se n�o � uma tela popup
                    if (!screen.IsPopup)
                    {
                        // Informa que h� telas subsequentes a ela
                        coveredByOtherScreen = true;
                    }
                }
            }

            // Imprime o trace/debug?
            if (this.traceEnabled)
            {
                this.TraceScreens();
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Imprime uma lista de todas as telas, para depura��o
        /// </summary>
        private void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            // Carrega a lista com o nome da telas
            foreach (GameScreen screen in this.screens)
            {
                screenNames.Add(screen.GetType().Name);
            }

            // Imprime o nome da telas
            Trace.WriteLine(string.Join(", ", screenNames.ToArray()));
        }

        /// <summary>
        /// Faz com que cada tela chame seu m�todo de desenhar
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Para todas as telas
            foreach (GameScreen screen in this.screens)
            {
                // Se a tela estiver fechada n�o desenha
                if (screen.ScreenState == ScreenState.Hidden)
                {
                    continue;
                }

                // Desenha a tela
                screen.Draw(gameTime);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adiciona uma nova tela � lista do gerenciador de telas
        /// </summary>
        public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer)
        {
            screen.ControllingPlayer = controllingPlayer;
            screen.ScreenManager = this;
            screen.IsExiting = false;

            // Se j� tiver sido inicializada com um dipositivo gr�fico
            if (this.isInitialized)
            {
                // Carrega os conte�dos da tela
                screen.LoadContent();
            }

            // Adiciona � cole��o de telas
            this.screens.Add(screen);
        }

        /// <summary>
        /// Remove uma tela da lista do gerencidor de telas
        /// N�o oferece transi��o gradual de fechamento, � imediata
        /// </summary>
        public void RemoveScreen(GameScreen screen)
        {
            // Se j� tiver sido inicializada com um dipositivo gr�fico
            if (this.isInitialized)
            {
                // Descarrega os conte�dos da tela
                screen.UnloadContent();
            }

            // Remove da lista de telas e de sua c�pia
            this.screens.Remove(screen);
            this.screensToUpdate.Remove(screen);
        }

        /// <summary>
        /// Retorna um array contendo todas telas
        /// </summary>
        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }

        /// <summary>
        /// Desenha um sprite fullscreen preto-transparente no estilo popup
        /// </summary>
        public void FadeBackBufferToBlack(float alpha)
        {
            // Pega o objeto que representa da tela
            Viewport viewport = GraphicsDevice.Viewport;

            // Desenha o fundo transl�cido
            spriteBatch.Begin();
            spriteBatch.Draw(blankTexture,new Rectangle(0, 0, viewport.Width, viewport.Height), Color.Black * alpha);
            spriteBatch.End();
        }

        #endregion
    }
}
