using System;
using RadgieDevelopmentTestProject;

namespace RadgieDevelopmentTestProject
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (RadgieDemo game = new RadgieDemo())
            {
                game.Run();
            }
        }
    }
#endif
}

