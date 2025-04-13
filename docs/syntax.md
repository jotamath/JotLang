# 📖 Guia de Sintaxe da JotLang

## 📝 Estrutura Básica

### 📦 Módulos
```jot
module MeuApp {
    // Seu código aqui
}
```

### 🏗️ Classes
```jot
class Usuario {
    // Propriedades e métodos
}
```

## 🔤 Tipos e Variáveis

### 📊 Tipos Básicos
- `int` - Números inteiros
- `float` - Números decimais
- `string` - Texto
- `bool` - Valores booleanos
- `void` - Sem retorno

### 📦 Coleções
- `List<T>` - Lista de elementos
- `Dictionary<K,V>` - Dicionário chave-valor
- `Set<T>` - Conjunto de elementos únicos

### 📝 Declaração de Variáveis
```jot
var nome: string = "João"  // Variável mutável
cst PI: float = 3.14      // Constante
```

## 🔄 Funções

### 📝 Sintaxe Básica
```jot
fn Soma(a: int, b: int) => int {
    return a + b
}
```

### 🎯 Funções Lambda
```jot
var quadrado = (x: int) => x * x
```

## 🏗️ Classes e Objetos

### 📦 Propriedades
```jot
class Pessoa {
    prop nome: string
    prop idade: int
    prop endereco: Endereco
}
```

### 🔄 Métodos
```jot
class Calculadora {
    fn Soma(a: int, b: int) => int {
        return a + b
    }
}
```

## 🎨 Decoradores

### 📝 Sintaxe
```jot
@decorator
class MinhaClasse {
    @outroDecorator
    fn MeuMetodo() {
        // Código aqui
    }
}
```

### 🎯 Decoradores Comuns
- `@api` - Marca uma classe como controlador de API
- `@route("caminho")` - Define a rota da API
- `@httpget`, `@httppost`, etc. - Define métodos HTTP
- `@authorize` - Requer autenticação
- `@validate` - Validação de dados

## 🔄 Estruturas de Controle

### 🔄 Loops
```jot
// For
for (var i = 0; i < 10; i++) {
    print(i)
}

// Foreach
foreach item in lista {
    print(item)
}

// While
while (condicao) {
    // Código aqui
}
```

### 🔍 Condicionais
```jot
if (condicao) {
    // Código aqui
} else if (outraCondicao) {
    // Código aqui
} else {
    // Código aqui
}
```

## 📦 Módulos e Importações

### 📝 Importação
```jot
import System
import System.Collections.Generic
import MeuApp.Models
```

## 🎯 Pattern Matching

### 📝 Sintaxe
```jot
match valor {
    case 1 => print("Um")
    case 2 => print("Dois")
    case _ => print("Outro")
}
```

## 🔄 Tratamento de Erros

### 📝 Try-Catch
```jot
try {
    // Código que pode gerar erro
} catch (ex: Exception) {
    print($"Erro: {ex.Message}")
} finally {
    // Código que sempre executa
}
```

## 📦 CRUD e APIs

### 📝 Exemplo de CRUD
```jot
@api
@route("api/usuarios")
class UsuarioController {
    @httpget
    fn GetAll() => List<Usuario> {
        return db.Usuarios.ToList()
    }

    @httppost
    fn Create(usuario: Usuario) => Usuario {
        db.Usuarios.Add(usuario)
        db.SaveChanges()
        return usuario
    }
}
```

## 🎯 DTOs e Validação

### 📝 Exemplo de DTO
```jot
@dto UsuarioDTO {
    prop id: guid
    prop nome: string @required @min(3)
    prop email: string @required @email
    prop idade: int @min(18)
}
```

## 🔄 WebSockets

### 📝 Exemplo de Hub
```jot
@websocket("/chat")
class ChatHub {
    fn OnConnect(usuario: string) {
        print($"Usuário {usuario} conectou")
    }

    fn OnMessage(usuario: string, mensagem: string) {
        BroadcastMessage(usuario, mensagem)
    }
}
```

## 📦 Cache

### 📝 Exemplo de Cache
```jot
@cache(ttl: 3600)
fn GetDados() => List<Dado> {
    return db.Dados.ToList()
}
```

## 🔄 Middleware

### 📝 Exemplo de Middleware
```jot
@middleware
class LoggingMiddleware {
    fn Invoke(context: HttpContext, next: RequestDelegate) {
        print($"Requisição: {context.Request.Path}")
        next(context)
    }
}
```

## 🎯 Melhores Práticas

1. 📝 Use nomes descritivos para variáveis e funções
2. 🔄 Mantenha funções pequenas e focadas
3. 📦 Use tipos fortes sempre que possível
4. 🎨 Aproveite os decoradores para funcionalidades comuns
5. 🔍 Faça validação de dados em DTOs
6. 📝 Documente seu código com comentários claros
7. 🔄 Use pattern matching para lógica complexa
8. 📦 Mantenha a consistência no estilo de código
9. 🎯 Teste seu código regularmente
10. 🔄 Use tratamento de erros apropriado

---

<div align="center">
  <sub>📚 Documentação em constante evolução - Contribua! 🤝</sub>
</div> 