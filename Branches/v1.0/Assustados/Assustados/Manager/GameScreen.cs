using System;
using Microsoft.Xna.Framework;

namespace Assustados.Manager
{
    /// <summary>
    /// Classe base para a cria��o de todos os menus do game
    /// </summary>
    public abstract class GameScreen
    {
        #region Fields

        /// <summary>
        /// Gerenciador de tela do game
        /// </summary>
        ScreenManager screenManager;

        /// <summary>
        /// Estado de transi��o corrente
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
        /// Transi��o da tela, entre 0 (ativa) at� 1 (sem nenhuma transi��o)
        /// </summary>
        private float transitionPosition = 1;

        /// <summary>
        /// Quando uma tela ser� apenas um popup sobre outra, a qual n�o precisar� ser fechada
        /// </summary>
        private bool isPopup = false;

        /// <summary>
        /// Para quando a tela der sa�da, ou seja, quando houver transfer�ncia de tela ou quando ela n�o for mais ser utilizada
        /// </summary>
        private bool isExiting = false;

        /// <summary>
        /// Verifica se a tela pode responder � entrada do teclado
        /// </summary>
        private bool otherScreenHasFocus;

        /// <summary>
        /// O controlador do menu. Se for nulo aceito qualquer entrada de jogador
        /// </summary>
        PlayerIndex? controllingPlayer;

        #endregion

        #region Properties

        /// <summary>
        /// Obt�m e atribui se a tela � do tipo popup, a qual abre sobre outra
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        /// <summary>
        /// Obt�m e atribui quanto tempo leva a abertura de uma tela
        /// </summary>
        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }

        /// <summary>
        /// Obt�m e atribui quanto tempo leva o fechamento de uma tela
        /// </summary>
        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }

        /// <summary>
        /// Obt�m e atribui a transi��o da tela entre 0 (ativa) e 1 (desativada)
        /// </summary>
        public float TransitionPosition
        {
            get { return transitionPosition; }
            protected set { transitionPosition = value; }
        }

        /// <summary>
        /// Obt�m a transpar�ncia de transi��o da tela, entre 255 (ativa) e 0 (desativada)
        /// </summary>
        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }

        /// <summary>
        /// Obt�m e atribui o estado de transi��o corrente
        /// </summary>
        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        /// <summary>
        /// Obt�m e atribui se a tela da sa�da para outra
        /// </summary>
        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set { isExiting = value; }
        }

        /// <summary>
        /// Obt�m se a tela est� ativa e poder� reponder a entrada do teclado
        /// </summary>
        public bool IsActive
        {
            get
            {
                return !this.otherScreenHasFocus && (this.screenState == ScreenState.TransitionOn || this.screenState == ScreenState.Active);
            }
        }

        /// <summary>
        /// Obt�m e atribui o gerenciador de tela do game
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        /// <summary>
        /// Obt�m e atribui o jogador que controla o menu (pode ser nulo)
        /// </summary>
        public PlayerIndex? ControllingPlayer
        {
            get { return controllingPlayer; }
            internal set { controllingPlayer = value; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Carrega os gr�ficos para a tela corrente
        /// </summary>
        public virtual void LoadContent() { }

        /// <summary>
        /// Descarrega os gr�ficos da tela corrente
        /// </summary>
        public virtual void UnloadContent() { }

        #endregion

        #region Update


        /// <summary>
        /// Atualiza a posi��o da transi��o dos controles de tela.
        /// E, sempre verifica se a tela est� ativa, desativada ou no meio de uma transi��o
        /// </summary>
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // Atribui se o foco est� em outra tela
            this.otherScreenHasFocus = otherScreenHasFocus;

            // Se a tela deve fechar
            if (this.isExiting)
            {
                // Atribui a tela como transi��o de fechamento
                this.screenState = ScreenState.TransitionOff;

                // Se n�o estiver em transi��o
                if (!this.UpdateTransition(gameTime, this.transitionOffTime, 1))
                {
                    // Remove a tela
                    this.screenManager.RemoveScreen(this);
                }
            }
            // Se abrangido por outra tela
            else if (coveredByOtherScreen)
            {
                // Se estiver em transi��o
                if (this.UpdateTransition(gameTime, this.transitionOffTime, 1))
                {
                    // Atribui para estar ainda em transi��o
                    this.screenState = ScreenState.TransitionOff;
                }
                // Sen�o...
                else
                {
                    // Fecha a transi��o
                    this.screenState = ScreenState.Hidden;
                }
            }
            // Sen�o...
            else
            {
                // Se est� com a transi��o em tela
                if (this.UpdateTransition(gameTime, this.transitionOnTime, -1))
                {
                    // Atribui a transi��o como ativa
                    this.screenState = ScreenState.TransitionOn;
                }
                else
                {
                    // Transi��o finalizada!
                    this.screenState = ScreenState.Active;
                }
            }
        }

        /// <summary>
        /// Auxilia na atualiza��o das posi��es de transi��o de tela
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

            // Atualiza posi��o da transi��o
            this.transitionPosition += transitionDelta * direction;

            // Se vai chegar no final da transi��o
            if ((direction < 0 && this.transitionPosition <= 0) || (direction > 0 && this.transitionPosition >= 1))
            {
                // Limita a posi��o entre 0 e 1
                this.transitionPosition = MathHelper.Clamp(this.transitionPosition, 0, 1);
                return false;
            }
            
            // Sen�o... ainda est� em transi��o
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
        /// Informa qual tela vai ser fechada, respeitando o tempo de transi��o at� o fechamento, 
        /// ao contr�rio do 'ScreenManager.RemoveScreen'
        /// </summary>
        public void ExitScreen()
        {
            // Se a tela corrente tem um tempo de transi��o igual a zero
            if (this.transitionOffTime == TimeSpan.Zero)
            {
                // Remove imediatamente a tela
                this.screenManager.RemoveScreen(this);
            }
            // Sen�o...
            else
            {
                // Atribui a sa�da com verdadeira, onde a transi��o ser� desligada antes de sair
                this.isExiting = true;
            }
        }

        #endregion
    }
}
