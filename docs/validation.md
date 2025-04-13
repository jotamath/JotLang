# Validação

A linguagem Jot oferece um sistema robusto e intuitivo de validação de dados.

## Atributos de Validação

### Validações Básicas

```jot
@dto UsuarioDTO {
    prop nome: string @required
    prop email: string @email
    prop idade: int @min(18) @max(120)
    prop senha: string @min(8) @pattern("^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$")
}
```

### Tipos de Validação Disponíveis

- `@required`: Campo obrigatório
- `@min(value)`: Valor mínimo
- `@max(value)`: Valor máximo
- `@email`: Formato de email válido
- `@pattern(regex)`: Padrão regex personalizado
- `@length(min, max)`: Comprimento da string
- `@range(min, max)`: Intervalo numérico
- `@url`: URL válida
- `@creditcard`: Número de cartão de crédito válido
- `@phone`: Número de telefone válido

## Validação Customizada

```jot
@validator SenhaValidador {
    fn Validate(senha: string) => bool {
        // Lógica de validação customizada
        return senha.Length >= 8 && 
               senha.Any(c => char.IsUpper(c)) &&
               senha.Any(c => char.IsDigit(c))
    }
}

@dto UsuarioDTO {
    prop senha: string @validator(SenhaValidador)
}
```

## Validação em Tempo de Execução

```jot
fn ValidarUsuario(usuario: UsuarioDTO) => bool {
    if (!usuario.nome.IsValid()) {
        print("Nome inválido")
        return false
    }
    
    if (!usuario.email.IsValid()) {
        print("Email inválido")
        return false
    }
    
    return true
}
```

## Mensagens de Erro Personalizadas

```jot
@dto ProdutoDTO {
    prop nome: string @required("O nome do produto é obrigatório")
    prop preco: decimal @min(0, "O preço deve ser maior que zero")
    prop quantidade: int @min(1, "A quantidade deve ser pelo menos 1")
}
```

## Validação de Coleções

```jot
@dto PedidoDTO {
    prop itens: List<ItemPedidoDTO> @min(1, "O pedido deve ter pelo menos um item")
    prop produtos: List<string> @unique("Produtos devem ser únicos")
}
```

## Validação Condicional

```jot
@dto InscricaoDTO {
    prop tipo: string
    prop cpf: string @requiredIf(tipo == "PF")
    prop cnpj: string @requiredIf(tipo == "PJ")
}
```

## Boas Práticas

1. Use validações apropriadas para cada tipo de dado
2. Forneça mensagens de erro claras e úteis
3. Combine validações básicas com customizadas quando necessário
4. Valide dados tanto no cliente quanto no servidor
5. Mantenha as validações consistentes em toda a aplicação 