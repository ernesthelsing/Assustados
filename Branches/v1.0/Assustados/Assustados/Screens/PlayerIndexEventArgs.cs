using System;
using Microsoft.Xna.Framework;

namespace Assustados.Manager
{
    /// <summary>
    /// Classe para verificar o jogador que controla o menu, utilizado no m�todo 'MenuItem.Selected'
    /// </summary>
    public class PlayerIndexEventArgs : EventArgs
    {
        /// <summary>
        /// Jogador
        /// </summary>
        private PlayerIndex playerIndex;

        /// <summary>
        /// Construtor
        /// </summary>
        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        /// <summary>
        /// Obt�m o jogador que disparou o evento
        /// </summary>
        public PlayerIndex PlayerIndex { get { return this.playerIndex; } }
    }
}
