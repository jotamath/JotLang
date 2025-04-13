using System;
using System.IO;
using Jot;
using Jot.CLI;

namespace JotLang {
    public class Program {
        public static int Main(string[] args) {
            try {
                if (args.Length > 0) {
                    var sourceFile = args[^1];
                    if (File.Exists(sourceFile)) {
                        var sourceCode = File.ReadAllText(sourceFile);
                        var csharpCode = Transpiler.Transpile(sourceCode);
                        Console.WriteLine(csharpCode);
                    }
                }
                
                return CommandLineInterface.Run(args);
            }
            catch (Exception ex) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro: {ex.Message}");
                Console.ResetColor();
                return 1;
            }
        }
    }
} 