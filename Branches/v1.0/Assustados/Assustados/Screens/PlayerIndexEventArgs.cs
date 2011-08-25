using System;
using Microsoft.Xna.Framework;

namespace Assustados.Manager
{
    /// <summary>
    /// Classe para verificar o jogador que controla o menu, utilizado no método 'MenuItem.Selected'
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
        /// Obtém o jogador que disparou o evento
        /// </summary>
        public PlayerIndex PlayerIndex { get { return this.playerIndex; } }
    }
}
