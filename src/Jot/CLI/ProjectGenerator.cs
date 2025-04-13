using System;
using System.IO;
using Jot.Templates;

namespace Jot.CLI
{
    public class ProjectGenerator
    {
        public static void GenerateApiProject(string projectName)
        {
            try
            {
                // Criar diretório do projeto
                Directory.CreateDirectory(projectName);
                Directory.CreateDirectory(Path.Combine(projectName, "src"));
                Directory.CreateDirectory(Path.Combine(projectName, "src", "Controllers"));
                Directory.CreateDirectory(Path.Combine(projectName, "src", "Models"));
                Directory.CreateDirectory(Path.Combine(projectName, "src", "Services"));
                Directory.CreateDirectory(Path.Combine(projectName, "src", "Data"));
                Directory.CreateDirectory(Path.Combine(projectName, "tests"));
                Directory.CreateDirectory(Path.Combine(projectName, "docs"));

                // Criar arquivos usando o template
                File.WriteAllText(Path.Combine(projectName, $"{projectName}.csproj"), ApiTemplate.GetCsprojContent(projectName));
                File.WriteAllText(Path.Combine(projectName, "appsettings.json"), ApiTemplate.GetAppSettingsContent(projectName));
                File.WriteAllText(Path.Combine(projectName, "src", "Program.jt"), ApiTemplate.GetProgramContent(projectName));
                File.WriteAllText(Path.Combine(projectName, "README.md"), ApiTemplate.GetReadmeContent(projectName));

                Console.WriteLine($"Projeto API '{projectName}' criado com sucesso!");
                Console.WriteLine($"Para começar, entre no diretório do projeto:");
                Console.WriteLine($"cd {projectName}");
                Console.WriteLine($"E execute:");
                Console.WriteLine($"jot run src/Program.jt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar o projeto: {ex.Message}");
            }
        }
    }
} 