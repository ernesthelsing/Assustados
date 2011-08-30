using Assustados.Animations;
using Assustados.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Assustados.Components;

namespace Assustados.Characters
{
    public class Monster : Character
    {
        #region Fields
        
        /// <summary>
        /// Tempo para atualizar o monstro na tela (alterar sua posição)
        /// </summary>
        private float timeUpdateMonster;
        
        #endregion

        #region Properties

        #endregion

        #region Initialize

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        public Monster(Level level, Vector2 startPosition)
            : base(level, startPosition)
        {
            // Tempo para atualizar o monstro
            this.timeUpdateMonster = 0;
        }

        /// <summary>
        /// Carrega imagens e sons do personagem
        /// </summary>
        public override void LoadContent()
        {
            // Imagem do inimigo
            Texture2D playerTexture = this.currentLevel.Content.Load<Texture2D>("Sprites/Monsters/" + this.currentLevel.LevelName);

            AnimationElement element;

            // Parado para baixo
            element = new AnimationElement(SpriteState.IdleDown, playerTexture, 4, 3, 1, 0);
            this.animatedSprite.AddAnimation(element);

            // Setando 1º estado
            this.animatedSprite.SetState(SpriteState.IdleDown);
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime, Random random, List<Vector2> positionsM)
        {
            //Pega o tempo em segundos do jogo e atribui a variável timeUpdateMonster
            timeUpdateMonster += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Quando o tempo for maior que 3 segundos o monstro é desenhado em uma posição aleatória de M no txt do Level.
            if (timeUpdateMonster >= 3.0)
            {
                // Gera número aleatório
                int number = random.Next(positionsM.Count - 1);

                // Nova posição
                this.Position = Mathematics.GetBottomCenter(this.currentLevel.GetBounds((int)positionsM[number].X, (int)positionsM[number].Y));

                // Zera o contador
                timeUpdateMonster = 0;
            }

            // Update Character
            base.Update(gameTime);
        }

        #endregion

        #region Draw

        #endregion
    }
}
