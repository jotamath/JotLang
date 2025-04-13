# 🎨 Templates do JotLang

## 📋 Visão Geral

Os templates JotLang são estruturas pré-configuradas para diferentes tipos de projetos, incluindo todas as dependências e configurações necessárias.

## 🚀 Como Usar

Para criar um novo projeto usando um template:

```bash
jot new [tipo] [nome]
```

Exemplo:
```bash
jot new api MinhaAPI
```

## 📦 Templates Disponíveis

### API Templates

#### API REST
```bash
jot new api NomeDaAPI
```
Cria uma API REST completa com:
- Entity Framework Core
- Autenticação JWT
- Swagger/OpenAPI
- CORS configurado
- Logging
- Cache

#### WebSocket
```bash
jot new websocket NomeDoChat
```
Cria uma aplicação WebSocket com:
- Chat em tempo real
- Autenticação
- Rooms
- Mensagens privadas

#### Microservice
```bash
jot new microservice NomeDoServico
```
Cria uma estrutura para microsserviços com:
- API Gateway
- Service Discovery
- Circuit Breaker
- Load Balancing

#### CRUD
```bash
jot new crud NomeDoCrud
```
Cria um CRUD completo com:
- Validação
- Cache
- Paginação
- Filtros
- Ordenação

### Web Templates

#### MVC
```bash
jot new mvc NomeDoMvc
```
Cria uma aplicação MVC com:
- Razor Pages
- Layouts
- Partial Views
- Tag Helpers

#### Blazor
```bash
jot new blazor NomeDoBlazor
```
Cria uma aplicação Blazor com:
- Server-side
- Componentes
- JavaScript Interop
- SignalR

#### SPA
```bash
jot new spa NomeDoSpa
```
Cria uma aplicação SPA com:
- API REST
- Frontend (React/Angular)
- Autenticação
- Roteamento

### Database Templates

#### SQL Server
```bash
jot new sql NomeDoProjeto
```
Configura o projeto com:
- SQL Server
- Entity Framework
- Migrations
- Stored Procedures

#### PostgreSQL
```bash
jot new postgres NomeDoProjeto
```
Configura o projeto com:
- PostgreSQL
- Entity Framework
- Migrations
- JSONB

#### MongoDB
```bash
jot new mongodb NomeDoProjeto
```
Configura o projeto com:
- MongoDB
- Driver oficial
- Aggregations
- GridFS

### Security Templates

#### Autenticação
```bash
jot new auth NomeDoProjeto
```
Configura o projeto com:
- JWT
- Refresh Tokens
- Roles
- Policies

#### OAuth
```bash
jot new oauth NomeDoProjeto
```
Configura o projeto com:
- OAuth 2.0
- OpenID Connect
- Social Login
- Token Management

#### Identity
```bash
jot new identity NomeDoProjeto
```
Configura o projeto com:
- ASP.NET Identity
- 2FA
- Password Reset
- Email Confirmation

## 🔧 Personalização

Cada template pode ser personalizado usando opções adicionais:

```bash
jot new api NomeDaAPI --db sqlserver --auth jwt --swagger
```

Opções disponíveis:
- `--db`: Tipo de banco de dados (sqlserver, postgres, mongodb)
- `--auth`: Tipo de autenticação (jwt, oauth, identity)
- `--swagger`: Incluir Swagger
- `--cache`: Incluir cache
- `--logging`: Configurar logging
- `--cors`: Configurar CORS

## 📚 Exemplos

### API REST com PostgreSQL e JWT
```bash
jot new api MinhaAPI --db postgres --auth jwt
```

### MVC com SQL Server e Identity
```bash
jot new mvc MinhaWeb --db sqlserver --auth identity
```

### Microservice com MongoDB
```bash
jot new microservice MeuServico --db mongodb
```

## 🤝 Contribuindo

Você pode contribuir criando novos templates ou melhorando os existentes. Para isso:

1. Fork o repositório
2. Crie uma branch para seu template
3. Implemente o template
4. Adicione a documentação
5. Abra um Pull Request

---

<div align="center">
  <sub>Feito com ❤️ pela comunidade JotLang</sub>
</div>

## API REST

### Controller Básico
```jot
@crud("Entidade")
class EntidadeController {
    @httpget("/api/entidades")
    fn Listar() => List<Entidade> {
        return _entidades
    }

    @httpget("/api/entidades/{id}")
    fn Obter(id: int) => Entidade {
        return _entidades.FirstOrDefault(e => e.Id == id)
    }

    @httppost("/api/entidades")
    fn Criar(entidade: Entidade) => Entidade {
        _entidades.Add(entidade)
        return entidade
    }

    @httpput("/api/entidades/{id}")
    fn Atualizar(id: int, entidade: Entidade) => Entidade {
        var index = _entidades.FindIndex(e => e.Id == id)
        if (index != -1) {
            _entidades[index] = entidade
        }
        return entidade
    }

    @httpdelete("/api/entidades/{id}")
    fn Excluir(id: int) => void {
        _entidades.RemoveAll(e => e.Id == id)
    }
}
```

### DTO
```jot
@dto
class EntidadeDTO {
    prop Id => int
    prop Nome => string
    prop Descricao => string
    prop DataCriacao => DateTime
}
```

## WebSocket

### Servidor
```jot
class ChatServer {
    prop _clients => List<WebSocket> = new()

    fn Iniciar() => void {
        var server = new WebSocketServer("ws://localhost:8080")
        server.OnConnect += (client) => {
            _clients.Add(client)
            Broadcast("Novo usuário conectado")
        }
        server.OnMessage += (client, message) => {
            Broadcast(message)
        }
        server.OnDisconnect += (client) => {
            _clients.Remove(client)
            Broadcast("Usuário desconectado")
        }
    }

    fn Broadcast(message: string) => void {
        foreach (client in _clients) {
            client.Send(message)
        }
    }
}
```

### Cliente
```jot
class ChatClient {
    prop _socket => WebSocket

    fn Conectar() => void {
        _socket = new WebSocket("ws://localhost:8080")
        _socket.OnMessage += (message) => {
            Console.WriteLine(message)
        }
    }

    fn EnviarMensagem(mensagem: string) => void {
        _socket.Send(mensagem)
    }
}
```

## Cache

### Cache com TTL
```jot
class CacheService {
    prop _cache => Dictionary<string, CacheItem> = new()
    prop _ttl => TimeSpan = TimeSpan.FromMinutes(30)

    fn Obter<T>(chave: string) => T {
        if (_cache.ContainsKey(chave)) {
            var item = _cache[chave]
            if (item.DataExpiracao > DateTime.Now) {
                return item.Valor as T
            }
            _cache.Remove(chave)
        }
        return default
    }

    fn Definir<T>(chave: string, valor: T) => void {
        _cache[chave] = new CacheItem {
            Valor = valor,
            DataExpiracao = DateTime.Now.Add(_ttl)
        }
    }
}

class CacheItem {
    prop Valor => object
    prop DataExpiracao => DateTime
}
```

## Autenticação

### JWT
```jot
class AuthService {
    prop _secret => string = "chave-secreta"
    prop _issuer => string = "meu-app"

    fn GerarToken(usuario: Usuario) => string {
        var tokenHandler = new JwtSecurityTokenHandler()
        var key = Encoding.ASCII.GetBytes(_secret)
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _issuer,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        }
        var token = tokenHandler.CreateToken(tokenDescriptor)
        return tokenHandler.WriteToken(token)
    }

    fn ValidarToken(token: string) => ClaimsPrincipal {
        var tokenHandler = new JwtSecurityTokenHandler()
        var key = Encoding.ASCII.GetBytes(_secret)
        var tokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }
        return tokenHandler.ValidateToken(token, tokenValidationParameters, out _)
    }
}
```

## Validação

### Validador de DTO
```jot
class Validador<T> {
    prop _erros => List<string> = new()

    fn Validar(objeto: T) => bool {
        _erros.Clear()
        var propriedades = typeof(T).GetProperties()
        foreach (prop in propriedades) {
            var valor = prop.GetValue(objeto)
            if (valor == null) {
                _erros.Add($"{prop.Name} é obrigatório")
            }
        }
        return _erros.Count == 0
    }

    fn ObterErros() => List<string> {
        return _erros
    }
}
```

## Logging

### Serviço de Log
```jot
class LogService {
    prop _logger => ILogger

    fn LogInfo(mensagem: string) => void {
        _logger.LogInformation(mensagem)
    }

    fn LogErro(mensagem: string, excecao: Exception) => void {
        _logger.LogError(excecao, mensagem)
    }

    fn LogAviso(mensagem: string) => void {
        _logger.LogWarning(mensagem)
    }
}
``` 