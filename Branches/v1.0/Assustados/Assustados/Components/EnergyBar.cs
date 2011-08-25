using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assustados.Components
{
    public class EnergyBar
    {
        #region Fields

        /// <summary>
        /// Texturas
        /// </summary>
        private Texture2D boxTexture;
        private Texture2D energyTexture;

        /// <summary>
        /// Posições 
        /// </summary>
        private Vector2 boxPosition;
        private Vector2 energyPosition;
        private Vector2 recoveryPosition;

        /// <summary>
        /// Cores da energia
        /// </summary>
        private Color boxColor;
        private Color colorEmpty;
        private Color colorFull;
        
        /// <summary>
        /// Atributos da energia
        /// </summary>
        private int currentEnergy;
        private int maxEnergy;
        
        /// <summary>
        /// Atributos para recuperação
        /// </summary>
        private bool recovery;
        private float currentRecovery;
        private float maxRecovery;
        private float recoveryFactor;
        private float colorAlphaRecovery = 0.2f;
        
        /// <summary>
        /// Efeitos para as texturas
        /// </summary>
        private bool boxFlip;
        private bool energyFlip;

        #endregion

        #region Properties

        /// <summary>
        /// Propriedade para receber e setar a energia
        /// </summary>
        public int CurrentEnergy
        {
            get { return this.currentEnergy; }
            set
            {
                this.currentEnergy = value;
                // Força a energia permanecer entre 0 e 100%
                this.currentEnergy = (int)MathHelper.Clamp(this.currentEnergy, 0, this.maxEnergy);
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Construtor do objeto barra de energia, esta barra pode ser usada em qualquer tipo de jogo
        /// seja o jogo de luta ou até mesmo um RPG
        /// </summary>
        /// <param name="energy">Quantidade de energia, energia, vida</param>
        /// <param name="maxHealth">Quantidade máxima de energia, energia, vida</param>
        /// <param name="boxHealth">Textura da caixa da energia, energia, vida</param>
        /// <param name="energyTexture">Textura da barra de energia, energia, vida</param>
        /// <param name="boxPosition">Posição da caixa</param>
        /// <param name="energyPosition">Posição da energia, energia, vida</param>
        /// <param name="colorEmptyHealth">Cor para quando a energia estiver próxima a 0%</param>
        /// <param name="colorFullHealth">Cor para quando a energia estiver próximo de 100%</param>
        /// <param name="recovery">Opção para barra de recuperação de energia, energia, vida</param>
        /// <param name="recoveryFactor">Fator de tempo para recuperar a energia, energia, vida</param>
        /// <param name="boxFlip">Inverter a direção da caixa</param>
        /// <param name="energyFlip">Inverter a direçaõ da barra de energia, energia, vida</param>
        public EnergyBar(int energy, int maxEnergy, Color colorEmpty, Color colorFull, bool recovery, float recoveryFactor, bool boxFlip, 
                         bool energyFlip, Texture2D boxTexture, Texture2D energyTexture, Vector2 boxPosition, Vector2 energyPosition)
        {
            // Saude do personagem
            this.currentEnergy = energy;
            this.maxEnergy = maxEnergy;

            // Atribuições dos parametros do box
            this.boxTexture = boxTexture;
            this.boxPosition = boxPosition;
            this.boxColor = Color.White;

            // Atribuições dos parametros da energia
            this.energyTexture = energyTexture;
            this.energyPosition = energyPosition;
            this.colorEmpty = colorEmpty;
            this.colorFull = colorFull;

            // Atribui o valor de sombra
            this.recovery = recovery;
            // Atribuições dos parametros da sombra
            if (recovery)
            {
                this.recoveryFactor = recoveryFactor;
                this.recoveryPosition = this.energyPosition;
                this.currentRecovery = (float)this.currentEnergy;
                this.maxRecovery = (float)this.maxEnergy;
            }

            // Verifica se precisa inverter a imagem do box
            this.boxFlip = boxFlip;
            // Verifica se precisa inverter a imagem da energia
            this.energyFlip = energyFlip;
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime, Vector2 boxPosition, Vector2 energyPosition)
        {
            this.boxPosition = boxPosition;
            this.energyPosition = energyPosition;

            // Diminui a recuperação de energia, energia ou vida conforme a energia atual do personagem
            if (recovery && currentRecovery != currentEnergy)
            {
                // Redução conforme fator de recuperação
                this.currentRecovery -= 1 * this.recoveryFactor;
            }
            // Caso aumente a energia a barra de recuperação volta ao mesmo limite da barra de energia, energia ou vida
            if (recovery && currentRecovery < CurrentEnergy)
            {
                this.currentRecovery = (float)this.CurrentEnergy;
            }
        }

        #endregion

        #region Draw

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Desenha a possível recuperação de energia
            if (recovery)
            {
                // Recuperação diminui para a esquerda sem flip caso contrario diminui para direita
                spriteBatch.Draw(energyTexture,
                    energyPosition,
                    new Rectangle(0, 0, (int)(currentRecovery / (float)maxRecovery * energyTexture.Width), energyTexture.Height), 
                    new Color(Color.Red.R, Color.Red.G, Color.Red.B, colorAlphaRecovery),
                    0.0f,
                    ((energyFlip && !boxFlip) ? new Vector2((currentRecovery / (float)maxRecovery * energyTexture.Width) - energyTexture.Width, 0) : energyFlip && boxFlip ? new Vector2((currentRecovery / (float)maxRecovery * energyTexture.Width) - energyTexture.Width + (boxTexture.Width - energyTexture.Width) - 2 * ((boxPosition.X + boxTexture.Width) - (energyPosition.X + energyTexture.Width)), 0) : boxFlip ? new Vector2((boxTexture.Width - energyTexture.Width) + -2 * ((boxPosition.X + boxTexture.Width) - (energyPosition.X + energyTexture.Width)), 0) : Vector2.Zero),
                    1.0f,
                    (energyFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None),
                    0.0f);
            }

            // Energia diminui para a esquerda sem flip caso contrário diminui para direita
            spriteBatch.Draw(energyTexture,
                energyPosition,
                new Rectangle(0, 0, (int)(currentEnergy / (float)maxEnergy * energyTexture.Width), energyTexture.Height),
                Color.Lerp(colorFull, colorEmpty, currentEnergy / (float)maxEnergy),
                0.0f,
                ((energyFlip && !boxFlip) ? new Vector2((currentEnergy / (float)maxEnergy * energyTexture.Width) - energyTexture.Width, 0) : energyFlip && boxFlip ? new Vector2((currentEnergy / (float)maxEnergy * energyTexture.Width) - energyTexture.Width + (boxTexture.Width - energyTexture.Width) - 2 * ((boxPosition.X + boxTexture.Width) - (energyPosition.X + energyTexture.Width)), 0) : boxFlip ? new Vector2((boxTexture.Width - energyTexture.Width) + -2 * ((boxPosition.X + boxTexture.Width) - (energyPosition.X + energyTexture.Width)), 0) : Vector2.Zero),
                1.0f,
                (energyFlip ? SpriteEffects.None : SpriteEffects.FlipHorizontally),
                0.0f);

            // Desenha a caixa por tras da energia sem flip horizontal por padrão caso contrário da um flip na imagem
            spriteBatch.Draw(boxTexture,
                boxPosition,
                new Rectangle(0, 0, boxTexture.Width, boxTexture.Height),
                boxColor,
                0.0f,
                Vector2.Zero,
                1.0f,
                (boxFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None),
                0.0f);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Volta a energia até sua recuperação caso esteja usando uma recuperação de energia
        /// </summary>
        public void MaxRecovery()
        {
            if (recovery)
            {
                this.currentEnergy = (int)this.currentRecovery;
            }
        }

        /// <summary>
        /// Recupera a energia total
        /// </summary>
        public void FullRecovery()
        {
            this.currentEnergy = this.maxEnergy;
            this.currentRecovery = this.maxRecovery;
        }

        #endregion
    }
}