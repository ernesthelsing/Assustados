using System;
using Assustados.Components;
using Assustados.Levels;
using Assustados.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assustados.Manager
{
    /// <summary>
    /// Tela onde é desenvolvido o game. Aqui as fases gerenciam todas os 'Updates' e 'Draws' do personagens e objetos
    /// </summary>
    class GameplayScreen : GameScreen
    {
        RenderTarget2D render;
        Effect postProcessingEffect;

        #region Constants

        private const int NUMBER_LEVELS = 1;

        #endregion

        #region Fields

        /// <summary>
        /// Gerenciador de conteúdo do game
        /// </summary>
        private ContentManager content;

        /// <summary>
        /// Granularidade da tela de pause
        /// </summary>
        float pauseAlpha;

        /// <summary>
        /// Fase corrente do game
        /// </summary>
        private Level level;
        
        /// <summary>
        /// Indexador de fases do game
        /// </summary>
        private int levelIndex;
        
        /// <summary>
        /// Verifica se acabou o game
        /// </summary>
        private bool isEndGame;
        
        /// <summary>
        /// Câmera que acompanha o personagem principal
        /// Aonde o personagem tem posição absoluta e todo o cenário posição relativa
        /// Dando a impressão que é o persongem que se move
        /// </summary>
        private Camera2D camera;

        /// <summary>
        /// Som de início
        /// </summary>
        private SoundEffect startSound;

        #endregion

        #region Initialization

        /// <summary>
        /// CONSTRUTOR
        /// </summary>
        public GameplayScreen(int phaseIndex)
        {
            this.levelIndex = phaseIndex;

            this.TransitionOnTime = TimeSpan.FromSeconds(2.5);
            this.TransitionOffTime = TimeSpan.FromSeconds(1.5);
        }

        /// <summary>
        /// Carrega os gráficos e sons do game
        /// </summary>
        public override void LoadContent()
        {
            // Se o gerenciador de conteúdo for nulo
            if (this.content == null)
            {
                // Inicializa o gerenciador de conteúdo
                this.content = new ContentManager(this.ScreenManager.Game.Services, "Content");
            }

            var device = this.ScreenManager.GraphicsDevice;
            this.render = new RenderTarget2D(device, device.Viewport.Width, device.Viewport.Height,
                false, SurfaceFormat.Color, DepthFormat.Depth24);
            this.postProcessingEffect = this.content.Load<Effect>("PostProcessing/Dark");

            // Inicializa a câmera
            this.camera = new Camera2D(this.ScreenManager.GraphicsDevice.Viewport);

            // TODO: Carregar o som de início
            //this.startSound = this.content.Load<SoundEffect>("Sounds/???");
            //this.startSound.Play();

            // Carrega a primeira fase
            this.LoadNextLevel();

            // Após o carregamento ter terminado, reinicia o tempo decorrido no game
            this.ScreenManager.Game.ResetElapsedTime();
        }

        /// <summary>
        /// Carrega a próxima fase
        /// </summary>
        private void LoadNextLevel()
        {
            // Fim do jogo
            if (this.levelIndex == NUMBER_LEVELS)
            {
                this.isEndGame = true;
            }
            // Inicializa a próxima fase do game
            else
            {
                // Gera a próxima fase de acordo com o contador
                this.level = new Level(this.ScreenManager.Game.Services, this.ScreenManager.GraphicsDevice.Viewport, this.GetLevelName(this.levelIndex++));

                // Atribui o comprimento da fase corrente para o controle de câmera
                this.camera.HorizontalLength = this.level.HorizontalLength;
                this.camera.VerticalLength = this.level.VerticalLenght;
            }
        }

        /// <summary>
        /// Inicializa uma nova fase no game
        /// </summary>
        public LevelName GetLevelName(int stage)
        {
            switch (stage)
            {
                // Bogeyman´s House
                case 0:
                    return LevelName.Bogeyman;
                default:
                    throw new NotSupportedException(string.Format("A fase número {0} não existe no game!", stage));
            }
        }

        /// <summary>
        /// Recarrega a fase corrente
        /// </summary>
        private void ReloadCurrentLevel()
        {
            // Decrementa o contador para indicar que será carregada a mesma fase
            --this.levelIndex;
            // Carrega a fase corrente do game
            this.LoadNextLevel();
        }

        /// <summary>
        /// Descarrega o gerenciador de conteúdo do game
        /// </summary>
        public override void UnloadContent()
        {
            this.content.Unload();
        }

        #endregion

        #region Update

        /// <summary>
        /// Atualização do game
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Granularidade da tela de pause
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            // Se a tela do jogo estiver ativa
            if (this.IsActive)
            {
                // Se chegou ao final chama a tela de final
                if (this.isEndGame)
                {
                    LoadingScreen.Load(this.ScreenManager, true, null, null, new CongratulationsScreen());
                }
                // Se for Game Over chama a tela correspondente
                else if (this.level.Player.IsGameOver)
                {
                    this.ScreenManager.AddScreen(new GameOverScreen(), this.ControllingPlayer);
                }
                // Se o personagem cumpriu o objetivo, carrega a próxima fase
                else if (this.level.Completed)
                {
                    LoadingScreen.Load(this.ScreenManager, true, this.ControllingPlayer, null, this);
                }

                // Atualizando a fase corrente
                this.level.Update(gameTime);

                // Atribuindo nova posição para a câmera de acordo com a posição do player
                this.camera.Position = this.level.Player.Position;
            }
        }

        /// <summary>
        /// Recebe a entrada do teclado
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Pega a posição de jogador ativa
            int playerIndex = (int)ControllingPlayer.Value;

            // Recebe estado do teclado e GamePad
            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // Desconectado?
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            // Se o game for pausado
            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                // Adiciona o menu de 'Pause'
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            // Senão... verificar se vai reiniciar a fase
            else if (input.IsReload(this.ControllingPlayer))
            {
                this.ReloadCurrentLevel();
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Desenha o game
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            this.ScreenManager.GraphicsDevice.SetRenderTarget(this.render);
            this.ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            this.ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            // Desenha a fase
            this.level.Draw(gameTime, this.ScreenManager.SpriteBatch, this.ScreenManager.GraphicsDevice.Viewport, this.camera.Transformation());

            // render target
            this.ScreenManager.GraphicsDevice.SetRenderTarget(null);

            this.ScreenManager.GraphicsDevice.Clear(Color.White);
            this.ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, postProcessingEffect);
            this.ScreenManager.SpriteBatch.Draw(render, Vector2.Zero, Color.White);
            this.ScreenManager.SpriteBatch.End();

            // Se o game está em transição On ou Off
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #endregion
    }
}
