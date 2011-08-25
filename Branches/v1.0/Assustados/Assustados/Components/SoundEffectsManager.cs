using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Assustados.Components
{
    public class SoundEffectsManager
    {
        #region Properties

        /// <summary>
        /// Objeto de efeito de som
        /// </summary>
        public SoundEffect SoundEffect { get; private set; }

        /// <summary>
        /// Instância, permitindo alterações no volume
        /// </summary>
        public SoundEffectInstance SoundEffectInstance { get; private set; }

        /// <summary>
        /// Volume do som
        /// </summary>
        public float Volume { get; set; }

        #endregion

        #region initialize

        public SoundEffectsManager(ContentManager content, string soundPath)
        {
            this.SoundEffect = content.Load<SoundEffect>(soundPath);
            this.SoundEffectInstance = this.SoundEffect.CreateInstance();

            // Iniciamos o som
            this.SoundEffectInstance.Play();
        }

        #endregion
    }
}
