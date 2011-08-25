
namespace Assustados
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal do game.
        /// </summary>
        static void Main(string[] args)
        {
            using (AssustadosGame game = new AssustadosGame())
            {
                game.Run();
            }
        }
    }
}

