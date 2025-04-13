# Tutorial Básico

Este tutorial irá guiá-lo através dos conceitos básicos da linguagem Jot.

## Hello World

Crie um arquivo `hello.jt`:

```jot
fn main() {
    print("Hello, World!")
}
```

Execute o programa:
```bash
jot run hello.jt
```

## Variáveis e Tipos

```jot
fn main() {
    // Tipos básicos
    var nome: string = "João"
    var idade: int = 25
    var altura: float = 1.75
    var ativo: bool = true
    
    // Inferência de tipo
    var sobrenome = "Silva"
    
    // Constantes
    const PI = 3.14159
    
    print("Nome: " + nome)
    print("Idade: " + idade.ToString())
}
```

## Funções

```jot
fn Soma(a: int, b: int) => int {
    return a + b
}

fn main() {
    var resultado = Soma(5, 3)
    print("5 + 3 = " + resultado.ToString())
}
```

## Estruturas de Controle

### If/Else

```jot
fn VerificarIdade(idade: int) {
    if (idade >= 18) {
        print("Maior de idade")
    } else {
        print("Menor de idade")
    }
}
```

### Loops

```jot
fn main() {
    // For
    for (var i = 0; i < 5; i++) {
        print("Iteração: " + i.ToString())
    }
    
    // While
    var contador = 0
    while (contador < 3) {
        print("Contador: " + contador.ToString())
        contador++
    }
}
```

## DTOs e Validação

```jot
@dto UsuarioDTO {
    prop nome: string @required
    prop email: string @email
    prop idade: int @min(18)
}

fn main() {
    var usuario = new UsuarioDTO {
        nome = "Maria",
        email = "maria@email.com",
        idade = 25
    }
    
    if (usuario.IsValid()) {
        print("Usuário válido")
    }
}
```

## APIs HTTP

```jot
@httpGet("/api/usuarios")
fn GetUsuarios() => List<UsuarioDTO> {
    return db.Usuarios
        .Select(u => new UsuarioDTO(u))
        .ToList()
}

@httpPost("/api/usuarios")
fn CreateUsuario(usuario: UsuarioDTO) => UsuarioDTO {
    if (!usuario.IsValid()) {
        throw new ValidationException("Dados inválidos")
    }
    
    var novoUsuario = new Usuario(usuario)
    db.Usuarios.Add(novoUsuario)
    db.SaveChanges()
    
    return new UsuarioDTO(novoUsuario)
}
```

## Banco de Dados

```jot
@entity Usuario {
    prop id: guid
    prop nome: string
    prop email: string
    prop posts: List<Post> @oneToMany
}

@entity Post {
    prop id: guid
    prop titulo: string
    prop conteudo: string
    prop usuario: Usuario @manyToOne
}

fn main() {
    var usuario = new Usuario {
        nome = "João",
        email = "joao@email.com"
    }
    
    var post = new Post {
        titulo = "Meu primeiro post",
        conteudo = "Conteúdo do post",
        usuario = usuario
    }
    
    db.Usuarios.Add(usuario)
    db.Posts.Add(post)
    db.SaveChanges()
}
```

## Autenticação

```jot
@httpPost("/api/auth/login")
fn Login(credentials: LoginDTO) => AuthResponseDTO {
    var usuario = db.Usuarios
        .First(u => u.Email == credentials.email)
    
    if (!VerifyPassword(credentials.senha, usuario.SenhaHash)) {
        throw new UnauthorizedException("Credenciais inválidas")
    }
    
    var token = GenerateJwtToken(usuario)
    return new AuthResponseDTO {
        token = token,
        usuario = new UsuarioDTO(usuario)
    }
}
```

## Próximos Passos

- Explore os [Exemplos](../examples/index.md)
- Consulte a [Referência da Linguagem](../reference/index.md)
- Aprenda sobre [DTOs](../dtos.md)
- Estude [Validação](../validation.md)
- Entenda [Relacionamentos](../relationships.md)
- Veja [Autenticação](../web/auth.md) 