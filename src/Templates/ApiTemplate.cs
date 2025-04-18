using System;
using System.IO;
using System.Text;

namespace JotLang.Templates
{
    public static class ApiTemplate
    {
        public static string GetCsprojContent(string projectName) => $@"<Project Sdk=""Microsoft.NET.Sdk.Web"">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <InvariantGlobalization>true</InvariantGlobalization>
    <RootNamespace>{projectName}</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""Microsoft.AspNetCore.OpenApi"" Version=""9.0.3"" />
    <PackageReference Include=""Swashbuckle.AspNetCore"" Version=""6.5.0"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore"" Version=""9.0.3"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore.SqlServer"" Version=""9.0.3"" />
    <PackageReference Include=""Microsoft.Extensions.Caching.Memory"" Version=""9.0.3"" />
    <PackageReference Include=""Microsoft.Extensions.Configuration"" Version=""9.0.3"" />
    <PackageReference Include=""Microsoft.Extensions.DependencyInjection"" Version=""9.0.3"" />
    <PackageReference Include=""Microsoft.Extensions.Logging"" Version=""9.0.3"" />
  </ItemGroup>

  <ItemGroup>
    <None Update=""appsettings.json"">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Update=""appsettings.Development.json"">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>

</Project>";

        public static string GetAppSettingsContent(string projectName) => $@"{{
  ""Logging"": {{
    ""LogLevel"": {{
      ""Default"": ""Information"",
      ""Microsoft.AspNetCore"": ""Warning""
    }}
  }},
  ""AllowedHosts"": ""*"",
  ""ConnectionStrings"": {{
    ""DefaultConnection"": ""Server=localhost;Database={projectName};Trusted_Connection=True;TrustServerCertificate=True;""
  }},
  ""Jwt"": {{
    ""Key"": ""your-secret-key-here"",
    ""Issuer"": ""{projectName}"",
    ""Audience"": ""{projectName}-Client""
  }},
  ""Cors"": {{
    ""AllowedOrigins"": [
      ""http://localhost:3000"",
      ""http://localhost:4200""
    ]
  }}
}}";

        public static string GetProgramContent(string projectName) => $@"module {projectName}

import Microsoft.AspNetCore.Builder
import Microsoft.Extensions.DependencyInjection
import Microsoft.Extensions.Hosting
import Microsoft.Extensions.Configuration
import Microsoft.EntityFrameworkCore
import Microsoft.OpenApi.Models
import System.Text
import Microsoft.AspNetCore.Authentication.JwtBearer
import Microsoft.IdentityModel.Tokens

class Program {{
    fn Main(args: string[]) => void {{
        let builder = WebApplication.CreateBuilder(args)
        
        // Configuração de Serviços
        ConfigureServices(builder.Services, builder.Configuration)
        
        let app = builder.Build()
        
        // Configuração do Pipeline
        ConfigurePipeline(app)
        
        app.Run()
    }}
    
    fn ConfigureServices(services: IServiceCollection, config: IConfiguration) => void {{
        // Configuração do DbContext
        services.AddDbContext<ApplicationDbContext>(options => {{
            options.UseSqlServer(config.GetConnectionString(""DefaultConnection""))
        }})
        
        // Configuração do CORS
        services.AddCors(options => {{
            options.AddPolicy(""AllowSpecificOrigins"", policy => {{
                policy.WithOrigins(config.GetSection(""Cors:AllowedOrigins"").Get<string[]>())
                      .AllowAnyHeader()
                      .AllowAnyMethod()
            }})
        }})
        
        // Configuração do JWT
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {{
                    options.TokenValidationParameters = TokenValidationParameters {{
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config[""Jwt:Issuer""],
                        ValidAudience = config[""Jwt:Audience""],
                        IssuerSigningKey = SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config[""Jwt:Key""])
                        )
                    }}
                }})
        
        // Configuração do Swagger
        services.AddEndpointsApiExplorer()
        services.AddSwaggerGen(c => {{
            c.SwaggerDoc(""v1"", OpenApiInfo {{
                Title = ""{projectName}"",
                Version = ""v1""
            }})
        }})
        
        // Configuração de Controllers
        services.AddControllers()
    }}
    
    fn ConfigurePipeline(app: WebApplication) => void {{
        if (app.Environment.IsDevelopment()) {{
            app.UseSwagger()
            app.UseSwaggerUI()
        }}
        
        app.UseHttpsRedirection()
        app.UseCors(""AllowSpecificOrigins"")
        app.UseAuthentication()
        app.UseAuthorization()
        
        app.MapControllers()
    }}
}}

// Exemplo de Controller
@api
@route(""api/[controller]"")
class WeatherForecastController {{
    @httpget
    fn Get() => List<WeatherForecast> {{
        return [
            WeatherForecast {{
                Date = DateTime.Now,
                TemperatureC = 20,
                Summary = ""Ensolarado""
            }}
        ]
    }}
}}

class WeatherForecast {{
    prop Date: DateTime
    prop TemperatureC: int
    prop Summary: string
}}

// Exemplo de DbContext
class ApplicationDbContext : DbContext {{
    fn constructor(options: DbContextOptions<ApplicationDbContext>) : base(options) {{}}
    
    // Adicione seus DbSets aqui
    // prop Users: DbSet<User>
}}";

        public static string GetReadmeContent(string projectName) => $@"# 🚀 {projectName}

Uma API moderna construída com JotLang e .NET 9.0.

## 📋 Pré-requisitos

- .NET 9.0 SDK
- SQL Server (ou outro banco de dados compatível)
- Visual Studio 2022 ou VS Code

## 🚀 Começando

1. Clone o repositório
2. Restaure as dependências:
```bash
dotnet restore
```

3. Configure a string de conexão no arquivo `appsettings.json`

4. Execute as migrações do banco de dados:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

5. Execute a aplicação:
```bash
dotnet run
```

## 📚 Documentação da API

A documentação da API está disponível em:
- Swagger UI: `https://localhost:5001/swagger`
- OpenAPI: `https://localhost:5001/swagger/v1/swagger.json`

## 🔧 Configuração

### 🔐 JWT

Configure as chaves JWT no arquivo `appsettings.json`:
```json
""Jwt"": {{
    ""Key"": ""sua-chave-secreta-aqui"",
    ""Issuer"": ""{projectName}"",
    ""Audience"": ""{projectName}-Client""
}}
```

### 🌐 CORS

Configure as origens permitidas no arquivo `appsettings.json`:
```json
""Cors"": {{
    ""AllowedOrigins"": [
        ""http://localhost:3000"",
        ""http://localhost:4200""
    ]
}}
```

## 📦 Estrutura do Projeto

```
{projectName}/
├── src/
│   ├── Program.jt
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   └── Data/
├── tests/
├── docs/
├── appsettings.json
└── {projectName}.csproj
```

## 🛠️ Desenvolvimento

### 🔄 Compilação
```bash
dotnet build
```

### 🧪 Testes
```bash
dotnet test
```

### 📦 Publicação
```bash
dotnet publish -c Release
```

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está licenciado sob a licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

---

<div align=""center"">
  <sub>Feito com ❤️ usando JotLang</sub>
</div>";
    }
} 
