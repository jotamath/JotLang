# DTOs (Data Transfer Objects)

Os DTOs na linguagem Jot são uma forma elegante e type-safe de definir objetos de transferência de dados.

## Sintaxe Básica

```jot
@dto NomeDTO {
    prop nome: string
    prop idade: int
    prop email: string
}
```

## Características

- **Type Safety**: Todos os campos devem ter tipos explícitos
- **Imutabilidade**: Por padrão, os DTOs são imutáveis
- **Validação**: Suporte integrado para validação de dados
- **Serialização**: Conversão automática para JSON

## Exemplos

### DTO Simples
```jot
@dto UsuarioDTO {
    prop id: guid
    prop nome: string
    prop email: string
    prop dataNascimento: datetime
}
```

### DTO com Validações
```jot
@dto ProdutoDTO {
    prop id: guid
    prop nome: string @min(3) @max(100)
    prop preco: decimal @min(0)
    prop quantidade: int @min(0)
    prop categoria: string @required
}
```

### DTO com Relacionamentos
```jot
@dto PedidoDTO {
    prop id: guid
    prop cliente: ClienteDTO
    prop itens: List<ItemPedidoDTO>
    prop total: decimal
    prop status: string
}
```

## Uso com APIs

```jot
@httpget("/api/usuarios/{id}")
fn GetUsuario(id: guid) => UsuarioDTO {
    // Implementação
}
```

## Boas Práticas

1. Mantenha os DTOs simples e focados
2. Use tipos apropriados para cada campo
3. Adicione validações quando necessário
4. Evite aninhamento excessivo
5. Documente campos complexos ou não óbvios

## Convenções de Nomenclatura

- Sufixo `DTO` para todos os DTOs
- Nomes em PascalCase
- Campos em camelCase
- Nomes descritivos e significativos 