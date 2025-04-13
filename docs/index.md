---
layout: home
title: Início
nav_order: 1
---

# Jot Lang

Jot é uma linguagem de programação moderna baseada em C#, projetada para ser simples, expressiva e produtiva.

## Características Principais

- **Sintaxe Limpa**: Sintaxe inspirada em C#, mas com menos verbosidade
- **CRUD Automático**: Geração automática de APIs RESTful com `@crud`
- **DTOs Simplificados**: Criação de DTOs com `@dto`
- **Pattern Matching**: Suporte nativo a pattern matching com `pmatching`
- **Funções Expressivas**: Sintaxe moderna para funções com `fn` e `=>`
- **Coleções Genéricas**: Suporte a `list<T>` e `dict<K,V>`

## Começando

1. Instale o Jot Lang:
   ```bash
   dotnet tool install -g jot
   ```

2. Crie um novo projeto de API:
   ```bash
   jot new api MinhaApi
   ```

3. Execute o projeto:
   ```bash
   cd MinhaApi
   jot run src/Program.jt
   ```

## Exemplo Básico

```jot
import System

module MinhaApp {
    @crud User {
        prop int Id
        prop string Name
        prop string Email
    }

    @dto UserDto {
        prop string Name
        prop string Email
    }

    class Program {
        fn Main() {
            print("Hello, Jot!")
            
            var users = list<User>()
            foreach user in users {
                pmatching user {
                    case User u when u.Name == "Admin" => print($"Admin: {u.Email}")
                    case User u => print($"User: {u.Name}")
                }
            }
        }
    }
}
```

## Próximos Passos

- [Guia de Instalação](./docs/getting-started/installation.md)
- [Tutorial Básico](./docs/getting-started/tutorial.md)
- [Referência da Linguagem](./docs/reference/index.md)
- [Exemplos](./docs/examples/index.md) 