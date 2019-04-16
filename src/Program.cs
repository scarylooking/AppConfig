using System;

namespace AppConfig
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // 1. Add a reference to System.Configuration and System.Security

            var configuration = new ConfigurationHelper();

            configuration.WriteEncryptedValue("AlpacaFluffinessLevel", "Dangerously High");
            configuration.WriteClearTextValue("NumberOfAlpacas", "9000");

            var alpacaFluffinessLevel = configuration.ReadEncryptedValue("AlpacaFluffinessLevel");
            var numberOfAlpacas = configuration.ReadClearTextValue("NumberOfAlpacas");

            Console.WriteLine($"Decrypted Alpaca Fluffiness Level is '{alpacaFluffinessLevel}'");
            Console.WriteLine($"Clear Text Number of Alpacas is '{numberOfAlpacas}'");

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
