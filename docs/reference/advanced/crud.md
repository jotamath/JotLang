---
layout: default
title: CRUD Automático
parent: Referência da Linguagem
nav_order: 1
---

# CRUD Automático

O Jot Lang oferece uma funcionalidade poderosa de geração automática de CRUD (Create, Read, Update, Delete) através do atributo `@crud`.

## Sintaxe Básica

```jot
@crud NomeEntidade {
    prop int Id
    prop string Nome
    // ... outras propriedades
}
```

## O que é Gerado?

Ao usar o atributo `@crud`, o Jot Lang gera automaticamente:

1. **Classe de Modelo**
   ```jot
   public class NomeEntidade {
       prop int Id
       // ... outras propriedades
   }
   ```

2. **Interface do Repositório**
   ```jot
   public interface INomeEntidadeRepository {
       fn GetAllAsync() => List<NomeEntidade>
       fn GetByIdAsync(int id) => NomeEntidade
       fn CreateAsync(NomeEntidade entity) => NomeEntidade
       fn UpdateAsync(int id, NomeEntidade entity) => NomeEntidade
       fn DeleteAsync(int id) => void
   }
   ```

3. **Implementação do Repositório**
   ```jot
   public class NomeEntidadeRepository : INomeEntidadeRepository {
       // Implementação completa dos métodos CRUD
   }
   ```

4. **Controller REST**
   ```jot
   @api
   @route("api/[controller]")
   class NomeEntidadeController {
       @httpget
       fn GetAll() => List<NomeEntidade>

       @httpget("{id}")
       fn GetById(int id) => NomeEntidade

       @httppost
       fn Create(NomeEntidade entity) => NomeEntidade

       @httpput("{id}")
       fn Update(int id, NomeEntidade entity) => NomeEntidade

       @httpdelete("{id}")
       fn Delete(int id) => void
   }
   ```

## Exemplo Completo

```jot
import System
import Microsoft.AspNetCore.Mvc

module MinhaApi {
    @crud User {
        prop int Id
        prop string Name
        prop string Email
        prop DateTime CreatedAt
    }

    @dto UserDto {
        prop string Name
        prop string Email
    }

    class Program {
        fn Main() {
            var builder = WebApplication.CreateBuilder()
            builder.Services.AddControllers()
            
            var app = builder.Build()
            app.MapControllers()
            app.Run()
        }
    }
}
```

## Endpoints Gerados

O exemplo acima gera os seguintes endpoints REST:

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/users` | Lista todos os usuários |
| GET | `/api/users/{id}` | Obtém um usuário pelo ID |
| POST | `/api/users` | Cria um novo usuário |
| PUT | `/api/users/{id}` | Atualiza um usuário |
| DELETE | `/api/users/{id}` | Remove um usuário |

## Personalização

### Adicionando Validação

```jot
@crud User {
    prop int Id
    
    @required
    @minlength(3)
    prop string Name
    
    @required
    @email
    prop string Email
}
```

### Configurando Relacionamentos

```jot
@crud Order {
    prop int Id
    prop int UserId
    
    @foreignkey("UserId")
    prop User User
    
    @onetomany
    prop list<OrderItem> Items
}
```

## Boas Práticas

1. **Nomeação**: Use nomes significativos no singular para entidades
2. **ID**: Sempre inclua uma propriedade `Id` do tipo `int` ou `Guid`
3. **DTOs**: Use DTOs para operações de criação e atualização
4. **Validação**: Adicione validações apropriadas para cada propriedade
5. **Documentação**: Documente endpoints personalizados

## Próximos Passos

- [DTOs](./dtos.md)
- [Validação](./validation.md)
- [Relacionamentos](./relationships.md)
- [Autenticação](../web/auth.md) 