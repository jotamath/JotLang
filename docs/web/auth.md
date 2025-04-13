# Autenticação

A linguagem Jot oferece um sistema robusto e flexível de autenticação e autorização.

## Configuração Básica

```jot
@auth {
    prop jwtSecret: string = env("JWT_SECRET")
    prop jwtExpiration: int = 3600 // 1 hora
    prop refreshTokenExpiration: int = 604800 // 7 dias
}
```

## Autenticação JWT

### Login

```jot
@httpPost("/api/auth/login")
fn Login(credentials: LoginDTO) => AuthResponseDTO {
    var usuario = db.Usuarios
        .First(u => u.Email == credentials.email)
    
    if (!VerifyPassword(credentials.senha, usuario.SenhaHash)) {
        throw new UnauthorizedException("Credenciais inválidas")
    }
    
    var token = GenerateJwtToken(usuario)
    var refreshToken = GenerateRefreshToken(usuario)
    
    return new AuthResponseDTO {
        token = token,
        refreshToken = refreshToken,
        usuario = new UsuarioDTO(usuario)
    }
}
```

### Refresh Token

```jot
@httpPost("/api/auth/refresh")
fn RefreshToken(refreshToken: string) => AuthResponseDTO {
    var tokenData = ValidateRefreshToken(refreshToken)
    var usuario = db.Usuarios.Find(tokenData.UserId)
    
    var newToken = GenerateJwtToken(usuario)
    var newRefreshToken = GenerateRefreshToken(usuario)
    
    return new AuthResponseDTO {
        token = newToken,
        refreshToken = newRefreshToken,
        usuario = new UsuarioDTO(usuario)
    }
}
```

## Autorização

### Roles e Permissões

```jot
@entity Usuario {
    prop id: guid
    prop email: string
    prop senha: string
    prop roles: List<Role> @manyToMany
}

@entity Role {
    prop id: guid
    prop nome: string
    prop permissoes: List<Permissao> @manyToMany
}

@entity Permissao {
    prop id: guid
    prop nome: string
    prop descricao: string
}
```

### Middleware de Autenticação

```jot
@middleware AuthMiddleware {
    fn Invoke(context: HttpContext) {
        var token = context.Request.Headers["Authorization"]
            .ToString()
            .Replace("Bearer ", "")
        
        if (string.IsNullOrEmpty(token)) {
            context.Response.StatusCode = 401
            return
        }
        
        try {
            var claims = ValidateJwtToken(token)
            context.Items["User"] = claims
        } catch {
            context.Response.StatusCode = 401
        }
    }
}
```

### Atributos de Autorização

```jot
@httpGet("/api/admin/usuarios")
@authorize(roles: ["Admin"])
fn GetUsuarios() => List<UsuarioDTO> {
    return db.Usuarios
        .Select(u => new UsuarioDTO(u))
        .ToList()
}

@httpPost("/api/produtos")
@authorize(permissions: ["CreateProduct"])
fn CreateProduto(produto: ProdutoDTO) => ProdutoDTO {
    // Implementação
}
```

## Segurança

### Senhas

```jot
fn HashPassword(senha: string) => string {
    return BCrypt.HashPassword(senha)
}

fn VerifyPassword(senha: string, hash: string) => bool {
    return BCrypt.Verify(senha, hash)
}
```

### Rate Limiting

```jot
@rateLimit(requests: 100, period: 60) // 100 requisições por minuto
@httpPost("/api/auth/login")
fn Login(credentials: LoginDTO) => AuthResponseDTO {
    // Implementação
}
```

## Boas Práticas

1. **Segurança**
   - Use HTTPS em produção
   - Implemente rate limiting
   - Armazene senhas com hash seguro
   - Use tokens JWT com expiração adequada

2. **Tokens**
   - Implemente refresh tokens
   - Use blacklist para tokens revogados
   - Configure expiração apropriada

3. **Autorização**
   - Use roles e permissões granulares
   - Implemente middleware de autenticação
   - Valide tokens em todas as rotas protegidas

4. **Manutenção**
   - Mantenha logs de autenticação
   - Implemente monitoramento
   - Tenha planos de revogação de tokens

## Exemplos de Uso

### Protegendo Rotas

```jot
@httpGet("/api/perfil")
@authorize
fn GetPerfil() => PerfilDTO {
    var userId = GetCurrentUserId()
    var usuario = db.Usuarios.Find(userId)
    return new PerfilDTO(usuario)
}
```

### Verificando Permissões

```jot
fn VerificarPermissao(usuario: Usuario, permissao: string) => bool {
    return usuario.Roles
        .SelectMany(r => r.Permissoes)
        .Any(p => p.Nome == permissao)
}
``` 