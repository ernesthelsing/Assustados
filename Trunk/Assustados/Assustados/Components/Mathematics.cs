using System;
using Microsoft.Xna.Framework;

namespace Assustados.Components
{
    /// <summary>
    /// Métodos para verificar estruturas
    /// </summary>
    public static class Mathematics
    {
        #region Methods

        /// <summary>
        /// Calcula a intersecção entre duas estruturas retangulares
        /// </summary>
        /// <returns>Devolve a direção correta para empurrar objetos, mas se não se cruzarem devolve (0,0)</returns>
        public static Vector2 GetIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            // Calcula tamanhos
            float halfWidthA = rectA.Width / 2;
            float halfHeightA = rectA.Height / 2;
            float halfWidthB = rectB.Width / 2;
            float halfHeightB = rectB.Height / 2;

            // Calcula posição central
            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // Calcula distância atual e miníma para não se cruzarem
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // Se não colidir, retorna (0,0)
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
            {
                return Vector2.Zero;
            }

            // Calcula e retorna profundidade da intersecção
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;

            return new Vector2(depthX, depthY);
        }

        /// <summary>
        /// Obtém posição inferior-central de um retângulo
        /// </summary>
        public static Vector2 GetBottomCenter(this Rectangle rect)
        {
            return new Vector2(rect.X + rect.Width / 2, rect.Bottom);
        }

        /// <summary>
        /// Calcula a distância entre dois pontos
        /// </summary>
        /// <param name="vvP1">vvP1  Ponto 1</param>
        /// <param name="vvP2">vvP2  Ponto 2</param>
        /// <returns>A distância entre os dois pontos dados</returns>
        public static float GetDistance2Points(Vector2 vvP1, Vector2 vvP2)
        {
            float fDeltaX = vvP2.X - vvP1.X;
            float fDeltaY = vvP2.Y - vvP1.Y;

            //return ((float)Math.Sqrt(fDeltaX * fDeltaX + fDeltaY * fDeltaY));
            return MathHelper.Distance(fDeltaX, fDeltaY);
        }

        /// <summary>
        /// Função que normaliza o volume de som de acordo com a distância entre dois pontos.
        /// </summary>
        /// <param name="fDist">fDist Distância entre os dois pontos em pixels</param>
        /// <returns> fVolume Novo volume entre 0 e 1 conforme tabela</returns>
        public static float GetVolumeFromDistance(float fDist)
        {
            float fVolume;

            // Valores passados são arbitrários
            fVolume = Mathematics.NormalizeValue(100.0f, 300.0f, fDist);

            // Inverte o valor, pois quero que quanto mais próximo, mais alto o volume
            return 1 - fVolume;
        }

        /// <summary>
        /// Função que normaliza um valor dentro de uma faixa
        /// </summary>
        /// <param name="fMin">fMin  Valor mínimo da faixa</param>
        /// <param name="fMax">Valor máximo da faixa</param>
        /// <param name="fValor">Valor a ser normalizado dentro da faixa</param>
        /// <returns>fNormalizado  Valor entre 0 e 1</returns>
        private static float NormalizeValue(float fMin, float fMax, float fValor)
        {
            // Diferença entre mínimo e máximo
            float fDelta = fMax - fMin;
            float fDeltaValor = fMax - fValor;

            if (fValor >= fMax)
                return 1.0f;
            else if (fValor <= fMin)
                return 0.0f;
            else // Diferença entre máximo e o valor desejado
                return (1 - (fDeltaValor / fDelta));
        }

        #endregion
    }
}