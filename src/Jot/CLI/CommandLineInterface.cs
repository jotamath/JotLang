using System;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Runtime.Loader;
using System.Text;

namespace Jot.CLI {
    public static class CommandLineInterface {
        public static int Run(string[] args) {
            if (args.Length == 0) {
                ShowHelp();
                return 1;
            }

            var command = args[0].ToLower();
            var remainingArgs = args[1..];

            try {
                return command switch {
                    "build" => BuildCommand(remainingArgs),
                    "run" => RunCommand(remainingArgs),
                    "new" => NewCommand(remainingArgs),
                    "help" or "--help" or "-h" => ShowHelp(),
                    _ => UnknownCommand(command)
                };
            }
            catch (Exception ex) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"Erro: {ex.Message}");
                Console.ResetColor();
                return 1;
            }
        }

        private static int BuildCommand(string[] args) {
            if (args.Length == 0) {
                throw new ArgumentException("É necessário especificar um arquivo .jt para compilar");
            }

            var sourceFile = args[0];
            if (!File.Exists(sourceFile)) {
                throw new FileNotFoundException($"Arquivo não encontrado: {sourceFile}");
            }

            var sourceCode = File.ReadAllText(sourceFile);
            var csharpCode = Transpiler.Transpile(sourceCode);
            
            var outputFile = Path.ChangeExtension(sourceFile, ".cs");
            File.WriteAllText(outputFile, csharpCode);
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Arquivo compilado com sucesso: {outputFile}");
            Console.ResetColor();
            
            return 0;
        }

        private static int RunCommand(string[] args) {
            if (args.Length == 0) {
                throw new ArgumentException("É necessário especificar um arquivo .jt para executar");
            }

            var sourceFile = args[0];
            if (!File.Exists(sourceFile)) {
                throw new FileNotFoundException($"Arquivo não encontrado: {sourceFile}");
            }

            // Lê e transpila o código Jot
            var sourceCode = File.ReadAllText(sourceFile);
            var csharpCode = Transpiler.Transpile(sourceCode);

            // Adiciona as referências necessárias
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location), "System.Runtime.dll")),
                MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location), "System.Collections.dll")),
                MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location), "netstandard.dll")),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.ControllerBase).Assembly.Location)
            };

            // Adiciona o código necessário para executar o Main
            var mainCode = new StringBuilder();
            mainCode.AppendLine("using System;");
            mainCode.AppendLine("using System.Collections.Generic;");
            mainCode.AppendLine("using Microsoft.AspNetCore.Mvc;");
            mainCode.AppendLine();
            mainCode.AppendLine(csharpCode);
            mainCode.AppendLine();
            mainCode.AppendLine("public class Program {");
            mainCode.AppendLine("    public static void Main(string[] args) {");
            mainCode.AppendLine("        try {");
            mainCode.AppendLine("            var program = new Program();");
            mainCode.AppendLine("            program.Main();");
            mainCode.AppendLine("        } catch (Exception ex) {");
            mainCode.AppendLine("            Console.WriteLine($\"Erro: {ex.Message}\");");
            mainCode.AppendLine("        }");
            mainCode.AppendLine("    }");
            mainCode.AppendLine("}");

            // Compila o código C#
            var compilation = CSharpCompilation.Create(
                Path.GetFileNameWithoutExtension(sourceFile),
                new[] { CSharpSyntaxTree.ParseText(mainCode.ToString()) },
                references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication)
            );

            // Gera o assembly em memória
            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success) {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var diagnostic in result.Diagnostics) {
                    Console.Error.WriteLine(diagnostic);
                }
                Console.ResetColor();
                return 1;
            }

            // Carrega e executa o assembly
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
            var entryPoint = assembly.EntryPoint;

            if (entryPoint != null) {
                try {
                    entryPoint.Invoke(null, new object[] { Array.Empty<string>() });
                    return 0;
                }
                catch (Exception ex) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine($"Erro ao executar: {ex.Message}");
                    Console.ResetColor();
                    return 1;
                }
            }
            else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Não foi possível encontrar o ponto de entrada (Main)");
                Console.ResetColor();
                return 1;
            }
        }

        private static int NewCommand(string[] args) {
            if (args.Length < 2) {
                throw new ArgumentException("Uso: jot new <tipo> <nome>");
            }

            var tipo = args[0].ToLower();
            var nome = args[1];

            switch (tipo) {
                case "api":
                    return CreateApiProject(nome);
                default:
                    throw new ArgumentException($"Tipo de projeto desconhecido: {tipo}");
            }
        }

        private static int CreateApiProject(string nome) {
            var projectDir = Path.Combine(Directory.GetCurrentDirectory(), nome);
            Directory.CreateDirectory(projectDir);

            // Criar estrutura de diretórios
            Directory.CreateDirectory(Path.Combine(projectDir, "src"));
            Directory.CreateDirectory(Path.Combine(projectDir, "tests"));
            Directory.CreateDirectory(Path.Combine(projectDir, "docs"));

            // Criar arquivo principal
            var mainFile = Path.Combine(projectDir, "src", "Program.jt");
            File.WriteAllText(mainFile, GetApiTemplate(nome));

            // Criar README
            var readmeFile = Path.Combine(projectDir, "README.md");
            File.WriteAllText(readmeFile, GetReadmeTemplate(nome));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Projeto API '{nome}' criado com sucesso!");
            Console.WriteLine($"Para começar, entre no diretório do projeto:");
            Console.WriteLine($"cd {nome}");
            Console.WriteLine("E execute:");
            Console.WriteLine("jot run src/Program.jt");
            Console.ResetColor();

            return 0;
        }

        private static string GetApiTemplate(string nome) {
            return @$"import System
import Microsoft.AspNetCore.Mvc
import Microsoft.Extensions.DependencyInjection

module {nome} {{
    @crud User {{
        prop int Id
        prop string Name
        prop string Email
        prop DateTime CreatedAt
    }}

    @dto UserDto {{
        prop string Name
        prop string Email
    }}

    class Program {{
        fn Main() {{
            var builder = WebApplication.CreateBuilder()
            
            // Configurar serviços
            builder.Services.AddControllers()
            builder.Services.AddEndpointsApiExplorer()
            builder.Services.AddSwaggerGen()
            
            var app = builder.Build()
            
            // Configurar pipeline
            app.UseSwagger()
            app.UseSwaggerUI()
            app.UseHttpsRedirection()
            app.UseAuthorization()
            app.MapControllers()
            
            app.Run()
        }}
    }}
}}";
        }

        private static string GetReadmeTemplate(string nome) {
            return @$"# {nome}

API gerada com Jot Lang.

## Estrutura do Projeto

```
{nome}/
├── src/           # Código fonte
├── tests/         # Testes
└── docs/          # Documentação
```

## Começando

1. Execute o projeto:
   ```
   jot run src/Program.jt
   ```

2. Acesse a documentação Swagger:
   http://localhost:5000/swagger

## Endpoints Disponíveis

- GET /api/users - Lista todos os usuários
- GET /api/users/{{id}} - Obtém um usuário pelo ID
- POST /api/users - Cria um novo usuário
- PUT /api/users/{{id}} - Atualiza um usuário
- DELETE /api/users/{{id}} - Remove um usuário
";
        }

        private static int ShowHelp() {
            Console.WriteLine(@"Uso: jot <comando> [argumentos]

Comandos disponíveis:
  build <arquivo.jt>     Compila um arquivo Jot para C#
  run <arquivo.jt>       Compila e executa um arquivo Jot
  new api <nome>         Cria um novo projeto de API
  help                   Mostra esta ajuda

Exemplos:
  jot build app.jt
  jot run app.jt
  jot new api MinhaApi");
            
            return 0;
        }

        private static int UnknownCommand(string command) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"Comando desconhecido: {command}");
            Console.ResetColor();
            ShowHelp();
            return 1;
        }
    }
} 