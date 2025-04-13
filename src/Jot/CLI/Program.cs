using System;

namespace Jot.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Uso: jot [comando] [argumentos]");
                Console.WriteLine("Comandos disponíveis:");
                Console.WriteLine("  new api [nome] - Cria um novo projeto de API");
                Console.WriteLine("  run [arquivo] - Executa um arquivo .jt");
                return;
            }

            string command = args[0].ToLower();

            switch (command)
            {
                case "new":
                    if (args.Length < 3)
                    {
                        Console.WriteLine("Uso: jot new [tipo] [nome]");
                        return;
                    }

                    string type = args[1].ToLower();
                    string name = args[2];

                    if (type == "api")
                    {
                        ProjectGenerator.GenerateApiProject(name);
                    }
                    else
                    {
                        Console.WriteLine($"Tipo de projeto desconhecido: {type}");
                    }
                    break;

                case "run":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Uso: jot run [arquivo]");
                        return;
                    }

                    string file = args[1];
                    if (!file.EndsWith(".jt"))
                    {
                        Console.WriteLine("O arquivo deve ter extensão .jt");
                        return;
                    }

                    try
                    {
                        string code = File.ReadAllText(file);
                        string csharpCode = Transpiler.Transpile(code);
                        // Aqui você pode adicionar a lógica para compilar e executar o código C#
                        Console.WriteLine("Código transpilado com sucesso!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao executar o arquivo: {ex.Message}");
                    }
                    break;

                default:
                    Console.WriteLine($"Comando desconhecido: {command}");
                    break;
            }
        }
    }
} 