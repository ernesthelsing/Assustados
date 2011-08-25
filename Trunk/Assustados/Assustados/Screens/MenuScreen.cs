using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Manager
{
    /// <summary>
    /// Classe base para telas que tem um menu de opções
    /// </summary>
    abstract class MenuScreen : GameScreen
    {
        #region Fields

        /// <summary>
        /// Lista de itens do menu
        /// </summary>
        List<MenuEntry> menuEntries = new List<MenuEntry>();
        
        /// <summary>
        /// Item selecionado
        /// </summary>
        int selectedEntry = 0;
        
        /// <summary>
        /// Título do menu
        /// </summary>
        private string menuTitle;

        #endregion

        #region Properties

        /// <summary>
        /// Obtém a lista de entradas(itens) do menu
        /// </summary>
        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        public MenuScreen(string menuTitle)
        {
            this.menuTitle = menuTitle;

            this.TransitionOnTime = TimeSpan.FromSeconds(0.5);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Verifica a entrada do teclado
        /// </summary>
        public override void HandleInput(InputState inputState)
        {
            // Se mover para entrada(item) anterior
            if (inputState.IsMenuUp(this.ControllingPlayer))
            {
                // Decrementa para subir pelos itens
                this.selectedEntry--;

                // Se for menor que zera
                if (this.selectedEntry < 0)
                {
                    // Vai para o último item do menu
                    this.selectedEntry = this.menuEntries.Count - 1;
                }
            }

            // Se mover para entrada(item) anterior
            if (inputState.IsMenuDown(this.ControllingPlayer))
            {
                // Incrementa para descer pelos itens
                this.selectedEntry++;

                // Se for maior que a quantidade de itens do menu
                if (this.selectedEntry >= this.menuEntries.Count)
                {
                    // Vai para o primeiro item do menu
                    this.selectedEntry = 0;
                }
            }

            // Controlador nulo inicialmente, que será passado através de 'OnSelectItem' e 'OnCancel'
            PlayerIndex playerIndex;

            // Se algum item do menu foi selecionado
            if (inputState.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                OnSelectEntry(selectedEntry, playerIndex);
            }
            // Senão...
            else if (inputState.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                OnCancel(playerIndex);
            }
        }

        /// <summary>
        /// Manipula uma entrada no menu
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }

        /// <summary>
        /// Manipula quando o menu é cancelado
        /// </summary>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            this.ExitScreen();
        }

        /// <summary>
        /// Sobrecarga de 'OnCancel' para disparar evento de calcelar
        /// </summary>
        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            this.OnCancel(e.PlayerIndex);
        }

        #endregion

        #region Update

        /// <summary>
        /// Atualiza a posição do item na tela
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Faz o slide do menu durante as transições
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Y = 175; o valor de X será alterado para cada item
            Vector2 position = new Vector2(0f, 175f);

            // Atualiza a posição de cada item
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];
                
                // Cada item deve ser centralizada horizontalmente
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // Define a posição do item
                menuEntry.Position = position;

                // Muda Y para o próximo item
                position.Y += menuEntry.GetHeight(this);
            }
        }

        /// <summary>
        /// Atualiza o menu
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Atualização de todos os itens do menu
            for (int i = 0; i < menuEntries.Count; i++)
            {
                // Verifica se o item está selecionado
                bool isSelected = this.IsActive && (i == this.selectedEntry);
                
                // Atualiza o item do menu
                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Desenha o menu
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Posição dos itens do menu na tela
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            for (int i = 0; i < menuEntries.Count; i++)
            {
                // Recupera o item
                MenuEntry menuEntry = menuEntries[i];
                
                // Verifica se o item está selecionado
                bool isSelected = IsActive && (i == selectedEntry);

                // Desenha o item do menu
                menuEntry.Draw(this, isSelected, gameTime);
            }

            // Variável para tornar mais lento o movimento
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Desenha o título do menu
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            Vector2 titleOrigin = font.MeasureString(this.menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0, titleOrigin, titleScale, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        #endregion
    }
}
