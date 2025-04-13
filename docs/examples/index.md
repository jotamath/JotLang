# Exemplos

Esta seção contém exemplos práticos de uso da linguagem Jot.

## Índice

1. [API REST](./api-rest.md)
2. [Autenticação JWT](./auth-jwt.md)
3. [Banco de Dados](./database.md)
4. [Validação de Dados](./validation.md)
5. [WebSocket](./websocket.md)
6. [Testes](./testing.md)

## Exemplo Rápido

Aqui está um exemplo básico de uma API REST usando Jot:

```jot
@entity Produto {
    prop id: guid
    prop nome: string
    prop preco: decimal
    prop quantidade: int
}

@dto ProdutoDTO {
    prop id: guid
    prop nome: string @required
    prop preco: decimal @min(0)
    prop quantidade: int @min(0)
}

@httpGet("/api/produtos")
fn GetProdutos() => List<ProdutoDTO> {
    return db.Produtos
        .Select(p => new ProdutoDTO(p))
        .ToList()
}

@httpPost("/api/produtos")
fn CreateProduto(produto: ProdutoDTO) => ProdutoDTO {
    if (!produto.IsValid()) {
        throw new ValidationException("Dados inválidos")
    }
    
    var novoProduto = new Produto(produto)
    db.Produtos.Add(novoProduto)
    db.SaveChanges()
    
    return new ProdutoDTO(novoProduto)
}
```

## Exemplos por Categoria

### APIs e Web

- [API REST Completa](./api-rest.md)
- [Autenticação JWT](./auth-jwt.md)
- [WebSocket em Tempo Real](./websocket.md)
- [Upload de Arquivos](./file-upload.md)

### Banco de Dados

- [CRUD Básico](./database.md)
- [Relacionamentos](./relationships.md)
- [Migrations](./migrations.md)
- [Queries Complexas](./queries.md)

### Validação e Segurança

- [Validação de Dados](./validation.md)
- [Autorização com Roles](./authorization.md)
- [Rate Limiting](./rate-limiting.md)
- [Logging e Monitoramento](./logging.md)

### Testes

- [Testes Unitários](./unit-tests.md)
- [Testes de Integração](./integration-tests.md)
- [Testes de API](./api-tests.md)
- [Mocks e Stubs](./mocks.md)

## Como Executar os Exemplos

1. Clone o repositório de exemplos:
```bash
git clone https://github.com/seu-usuario/jot-examples.git
cd jot-examples
```

2. Instale as dependências:
```bash
dotnet restore
```

3. Execute o exemplo desejado:
```bash
dotnet run --project examples/api-rest
```

## Contribuindo

Sinta-se à vontade para contribuir com novos exemplos! Para isso:

1. Fork o repositório
2. Crie uma branch para seu exemplo
3. Adicione seu código e documentação
4. Envie um Pull Request

## Próximos Passos

- Explore os [exemplos detalhados](./api-rest.md)
- Consulte a [Referência da Linguagem](../reference/index.md)
- Siga o [Tutorial Básico](../getting-started/tutorial.md) 