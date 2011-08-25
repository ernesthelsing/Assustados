using System;
using Microsoft.Xna.Framework;

namespace Assustados.Manager
{
    /// <summary>
    /// Classe base para a criação de todos os menus do game
    /// </summary>
    public abstract class GameScreen
    {
        #region Fields

        /// <summary>
        /// Gerenciador de tela do game
        /// </summary>
        ScreenManager screenManager;

        /// <summary>
        /// Estado de transição corrente
        /// </summary>
        ScreenState screenState = ScreenState.TransitionOn;

        /// <summary>
        /// Indica o tempo de abertura da tela quando ela for ativada
        /// </summary>
        private TimeSpan transitionOnTime = TimeSpan.Zero;

        /// <summary>
        /// Indica o tempo de fechamento da tela quando ela for desativa
        /// </summary>
        private TimeSpan transitionOffTime = TimeSpan.Zero;

        /// <summary>
        /// Transição da tela, entre 0 (ativa) até 1 (sem nenhuma transição)
        /// </summary>
        private float transitionPosition = 1;

        /// <summary>
        /// Quando uma tela será apenas um popup sobre outra, a qual não precisará ser fechada
        /// </summary>
        private bool isPopup = false;

        /// <summary>
        /// Para quando a tela der saída, ou seja, quando houver transferência de tela ou quando ela não for mais ser utilizada
        /// </summary>
        private bool isExiting = false;

        /// <summary>
        /// Verifica se a tela pode responder à entrada do teclado
        /// </summary>
        private bool otherScreenHasFocus;

        /// <summary>
        /// O controlador do menu. Se for nulo aceito qualquer entrada de jogador
        /// </summary>
        PlayerIndex? controllingPlayer;

        #endregion

        #region Properties

        /// <summary>
        /// Obtém e atribui se a tela é do tipo popup, a qual abre sobre outra
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        /// <summary>
        /// Obtém e atribui quanto tempo leva a abertura de uma tela
        /// </summary>
        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }

        /// <summary>
        /// Obtém e atribui quanto tempo leva o fechamento de uma tela
        /// </summary>
        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }

        /// <summary>
        /// Obtém e atribui a transição da tela entre 0 (ativa) e 1 (desativada)
        /// </summary>
        public float TransitionPosition
        {
            get { return transitionPosition; }
            protected set { transitionPosition = value; }
        }

        /// <summary>
        /// Obtém a transparência de transição da tela, entre 255 (ativa) e 0 (desativada)
        /// </summary>
        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }

        /// <summary>
        /// Obtém e atribui o estado de transição corrente
        /// </summary>
        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        /// <summary>
        /// Obtém e atribui se a tela da saída para outra
        /// </summary>
        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set { isExiting = value; }
        }

        /// <summary>
        /// Obtém se a tela está ativa e poderá reponder a entrada do teclado
        /// </summary>
        public bool IsActive
        {
            get
            {
                return !this.otherScreenHasFocus && (this.screenState == ScreenState.TransitionOn || this.screenState == ScreenState.Active);
            }
        }

        /// <summary>
        /// Obtém e atribui o gerenciador de tela do game
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        /// <summary>
        /// Obtém e atribui o jogador que controla o menu (pode ser nulo)
        /// </summary>
        public PlayerIndex? ControllingPlayer
        {
            get { return controllingPlayer; }
            internal set { controllingPlayer = value; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Carrega os gráficos para a tela corrente
        /// </summary>
        public virtual void LoadContent() { }

        /// <summary>
        /// Descarrega os gráficos da tela corrente
        /// </summary>
        public virtual void UnloadContent() { }

        #endregion

        #region Update


        /// <summary>
        /// Atualiza a posição da transição dos controles de tela.
        /// E, sempre verifica se a tela está ativa, desativada ou no meio de uma transição
        /// </summary>
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // Atribui se o foco está em outra tela
            this.otherScreenHasFocus = otherScreenHasFocus;

            // Se a tela deve fechar
            if (this.isExiting)
            {
                // Atribui a tela como transição de fechamento
                this.screenState = ScreenState.TransitionOff;

                // Se não estiver em transição
                if (!this.UpdateTransition(gameTime, this.transitionOffTime, 1))
                {
                    // Remove a tela
                    this.screenManager.RemoveScreen(this);
                }
            }
            // Se abrangido por outra tela
            else if (coveredByOtherScreen)
            {
                // Se estiver em transição
                if (this.UpdateTransition(gameTime, this.transitionOffTime, 1))
                {
                    // Atribui para estar ainda em transição
                    this.screenState = ScreenState.TransitionOff;
                }
                // Senão...
                else
                {
                    // Fecha a transição
                    this.screenState = ScreenState.Hidden;
                }
            }
            // Senão...
            else
            {
                // Se está com a transição em tela
                if (this.UpdateTransition(gameTime, this.transitionOnTime, -1))
                {
                    // Atribui a transição como ativa
                    this.screenState = ScreenState.TransitionOn;
                }
                else
                {
                    // Transição finalizada!
                    this.screenState = ScreenState.Active;
                }
            }
        }

        /// <summary>
        /// Auxilia na atualização das posições de transição de tela
        /// </summary>
        private bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            // O quanto devemos mover
            float transitionDelta;

            if (time == TimeSpan.Zero)
            {
                transitionDelta = 1;
            }
            else
            {
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);
            }

            // Atualiza posição da transição
            this.transitionPosition += transitionDelta * direction;

            // Se vai chegar no final da transição
            if ((direction < 0 && this.transitionPosition <= 0) || (direction > 0 && this.transitionPosition >= 1))
            {
                // Limita a posição entre 0 e 1
                this.transitionPosition = MathHelper.Clamp(this.transitionPosition, 0, 1);
                return false;
            }
            
            // Senão... ainda está em transição
            return true;
        }

        /// <summary>
        /// Chamado somente quanto a tela estiver ativa, diferente do 'Update'
        /// </summary>
        public virtual void HandleInput(InputState input) { }

        #endregion

        #region Draw

        /// <summary>
        /// Utilizado quando a tela for chamada
        /// </summary>
        public virtual void Draw(GameTime gameTime) { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Informa qual tela vai ser fechada, respeitando o tempo de transição até o fechamento, 
        /// ao contrário do 'ScreenManager.RemoveScreen'
        /// </summary>
        public void ExitScreen()
        {
            // Se a tela corrente tem um tempo de transição igual a zero
            if (this.transitionOffTime == TimeSpan.Zero)
            {
                // Remove imediatamente a tela
                this.screenManager.RemoveScreen(this);
            }
            // Senão...
            else
            {
                // Atribui a saída com verdadeira, onde a transição será desligada antes de sair
                this.isExiting = true;
            }
        }

        #endregion
    }
}
