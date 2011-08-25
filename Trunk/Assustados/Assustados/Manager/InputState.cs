using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Assustados.Manager
{
    /// <summary>
    /// Classe para ler a entrada do teclado
    /// </summary>
    public class InputState
    {
        #region Fields

        /// <summary>
        /// Tamanho do array de inputs
        /// </summary>
        public const int MaxInputs = 4;

        /// <summary>
        /// Coleção de estados de input
        /// </summary>
        public readonly KeyboardState[] CurrentKeyboardStates;
        public readonly GamePadState[] CurrentGamePadStates;

        /// <summary>
        /// Coleção dos últimos estados de input
        /// </summary>
        public readonly KeyboardState[] LastKeyboardStates;
        public readonly GamePadState[] LastGamePadStates;

        /// <summary>
        /// Coleção para verificar se os controles estão conectados
        /// </summary>
        public readonly bool[] GamePadWasConnected;

        #endregion

        #region Initialization

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        public InputState()
        {
            // Inicializa os arrays de input
            CurrentKeyboardStates = new KeyboardState[MaxInputs];
            CurrentGamePadStates = new GamePadState[MaxInputs];

            LastKeyboardStates = new KeyboardState[MaxInputs];
            LastGamePadStates = new GamePadState[MaxInputs];

            GamePadWasConnected = new bool[MaxInputs];
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Lê o último estado do teclado
        /// </summary>
        public void Update()
        {
            // Percorre o array de inputs
            for (int i = 0; i < MaxInputs; i++)
            {
                // Guarda o input corrente no array dos últimos inputs
                this.LastKeyboardStates[i] = this.CurrentKeyboardStates[i];
                this.LastGamePadStates[i] = this.CurrentGamePadStates[i];

                // Recebe a nova entrada
                this.CurrentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
                this.CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

                // Se o controle estiver conectado...
                if (CurrentGamePadStates[i].IsConnected)
                {
                    GamePadWasConnected[i] = true;
                }
            }
        }

        /// <summary>
        /// Verifica se uma nova tecla foi pressionada
        /// </summary>
        /// <param name="key">Tecla</param>
        /// <param name="controllingPlayer">Jogador que chamou a ação... se nulo aceita qualquer entrada</param>
        /// <param name="playerIndex">Leva a detecção da tecla pressionada</param>
        public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Lê a entrada de um jogador especifíco
                playerIndex = controllingPlayer.Value;
                // Posição do jogador
                int i = (int)playerIndex;
                // Retorna se a entrada atual é igual à anterior
                return (this.CurrentKeyboardStates[i].IsKeyDown(key) && this.LastKeyboardStates[i].IsKeyUp(key));
            }
            // Senão...
            else
            {
                // Aceita a entrada de qualquer jogador
                return (this.IsNewKeyPress(key, PlayerIndex.One, out playerIndex) || this.IsNewKeyPress(key, PlayerIndex.Two, out playerIndex) ||
                        this.IsNewKeyPress(key, PlayerIndex.Three, out playerIndex) || this.IsNewKeyPress(key, PlayerIndex.Four, out playerIndex));
            }
        }

        /// <summary>
        /// Verifica se um novo botão foi pressionado
        /// </summary>
        /// <param name="button">Botão</param>
        /// <param name="controllingPlayer">Jogador que chamou a ação... se nulo aceita qualquer entrada</param>
        /// <param name="playerIndex">Leva a detecção do botão pressionado</param>
        public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Lê a entrada de um jogador especifíco
                playerIndex = controllingPlayer.Value;
                // Posição do jogador
                int i = (int)playerIndex;
                // Retorna se a entrada atual é igual à anterior
                return (this.CurrentGamePadStates[i].IsButtonDown(button) && this.LastGamePadStates[i].IsButtonUp(button));
            }
            // Senão...
            else
            {
                // Aceita a entrada de qualquer jogador
                return (this.IsNewButtonPress(button, PlayerIndex.One, out playerIndex) || this.IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                        this.IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) || this.IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
            }
        }

        /// <summary>
        /// Verifica se há uma ação de entrada no menu
        /// </summary>
        public bool IsMenuSelect(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            return this.IsNewKeyPress(Keys.Enter, controllingPlayer, out playerIndex)
                   || this.IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex)
                   || this.IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex)
                   || this.IsNewButtonPress(Buttons.A, controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Verifica se há uma ação de saída no menu
        /// </summary>
        public bool IsMenuCancel(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            return this.IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex)
                   || this.IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex)
                   || this.IsNewButtonPress(Buttons.B, controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Verifica se há uma ação para cima no menu
        /// </summary>
        public bool IsMenuUp(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return this.IsNewKeyPress(Keys.Up, controllingPlayer, out playerIndex)
                   || this.IsNewButtonPress(Buttons.DPadUp, controllingPlayer, out playerIndex)
                   || this.IsNewButtonPress(Buttons.LeftThumbstickUp, controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Verifica se há uma ação para baixo no menu
        /// </summary>
        public bool IsMenuDown(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return this.IsNewKeyPress(Keys.Down, controllingPlayer, out playerIndex)
                   || this.IsNewButtonPress(Buttons.DPadDown, controllingPlayer, out playerIndex)
                   || this.IsNewButtonPress(Buttons.LeftThumbstickDown, controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Verifica se há uma ação para pausar o game
        /// </summary>
        public bool IsPauseGame(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            // 'Pause', 'P' ou 'Esc' para parar o game | 'Start' no GamePad
            return this.IsNewKeyPress(Keys.Pause, controllingPlayer, out playerIndex)
                || this.IsNewKeyPress(Keys.P, controllingPlayer, out playerIndex)
                || this.IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex)
                || this.IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);

        }

        /// <summary>
        /// Verifica se há uma ação para recarregar a fase corrente
        /// </summary>
        public bool IsReload(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            // 'R' recarrega a fase corrente | 'Y' no GamePad
            return this.IsNewKeyPress(Keys.R, controllingPlayer, out playerIndex)
                   || this.IsNewButtonPress(Buttons.Y, controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Verifica se há uma ação para pular de fase (trapaça)
        /// </summary>
        public bool IsTrap(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            // 'T' pula p/ próxima fase | 'X' no GamePad
            return this.IsNewKeyPress(Keys.T, controllingPlayer, out playerIndex)
                   || this.IsNewButtonPress(Buttons.X, controllingPlayer, out playerIndex);
        }

        #endregion
    }
}
