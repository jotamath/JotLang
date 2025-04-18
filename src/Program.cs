using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace JotLang 
{
    public class Program 
    {
        private static readonly string Version = "1.0.0";

        public static async Task<int> Main(string[] args) 
        {
            if (args.Length == 0 || args[0] == "--help")
            {
                ShowHelp();
                return 1;
            }

            if (args[0] == "--version")
            {
                Console.WriteLine($"JotLang versão {Version}");
                return 0;
            }

            if (args[0] == "run" && args.Length > 1)
            {
                var file = args[1];
                var verbose = Array.IndexOf(args, "--verbose") >= 0 || Array.IndexOf(args, "-v") >= 0;

                try 
                {
                    if (!File.Exists(file))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Arquivo não encontrado: {file}");
                        Console.ResetColor();
                        return 1;
                    }

                    var interpreter = CreateInterpreter(verbose);
                    var sourceCode = await File.ReadAllTextAsync(file);
                    await interpreter.InterpretAsync(sourceCode);
                    return 0;
                }
                catch (Exception ex)
                {
                    HandleError(ex);
                    return 1;
                }
            }

            ShowHelp();
            return 1;
        }

        private static void ShowHelp()
        {
            Console.WriteLine("JotLang - Uma linguagem de programação simples e moderna");
            Console.WriteLine();
            Console.WriteLine("Uso:");
            Console.WriteLine("  jota run <arquivo>     Executa um arquivo fonte");
            Console.WriteLine("  jota --version         Mostra a versão do JotLang");
            Console.WriteLine("  jota --help            Mostra esta ajuda");
            Console.WriteLine();
            Console.WriteLine("Opções:");
            Console.WriteLine("  --verbose, -v          Ativa logs detalhados");
        }

        private static Interpreter.JotInterpreter CreateInterpreter(bool verbose)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(verbose ? LogLevel.Debug : LogLevel.Information);
            });
            var logger = loggerFactory.CreateLogger<Interpreter.JotInterpreter>();
            return new Interpreter.JotInterpreter(logger);
        }

        private static void HandleError(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Erro: {ex.Message}");
            Console.WriteLine($"Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            Console.ResetColor();
        }
    }
} 
