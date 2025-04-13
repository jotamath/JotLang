# 📝 Sintaxe do JotLang

## Estrutura Básica

### Namespace
```jot
namespace MeuNamespace {
    // código aqui
}
```

### Classe
```jot
class MinhaClasse {
    // propriedades e métodos aqui
}
```

### Função
```jot
fn MinhaFuncao() => string {
    return "Olá, mundo!"
}
```

## Propriedades

### Propriedade Simples
```jot
prop Nome => string = "João"
```

### Propriedade com Getter/Setter
```jot
prop Idade => int {
    get => _idade
    set => _idade = value
}
```

## Decoradores

### HTTP
```jot
@httpget("/api/usuarios")
fn ListarUsuarios() => List<Usuario> {
    // código aqui
}
```

### CRUD
```jot
@crud("Usuarios")
class UsuarioController {
    // métodos CRUD gerados automaticamente
}
```

### DTO
```jot
@dto
class UsuarioDTO {
    prop Nome => string
    prop Email => string
}
```

## Estruturas de Controle

### If/Else
```jot
if (condicao) {
    // código se verdadeiro
} else {
    // código se falso
}
```

### For
```jot
for (i in 0..10) {
    // código aqui
}
```

### While
```jot
while (condicao) {
    // código aqui
}
```

## Pattern Matching
```jot
pmatching (valor) {
    case 1 => "Um"
    case 2 => "Dois"
    default => "Outro"
}
```

## Coleções

### List
```jot
var lista = List<int> { 1, 2, 3 }
```

### Dictionary
```jot
var dicionario = Dictionary<string, int> {
    "um" => 1,
    "dois" => 2
}
```

## Tratamento de Erros

### Try/Catch
```jot
try {
    // código que pode gerar erro
} catch (ex) {
    // tratamento do erro
}
```

## Comentários

### Comentário de Linha
```jot
// Este é um comentário de linha
```

### Comentário de Bloco
```jot
/*
Este é um comentário
de múltiplas linhas
*/
```

## Exemplos Práticos

### API REST
```jot
@crud("Produtos")
class ProdutoController {
    @httpget("/api/produtos")
    fn Listar() => List<Produto> {
        return _produtos
    }
}
```

### DTO com Validação
```jot
@dto
class ProdutoDTO {
    prop Nome => string
    prop Preco => decimal
    prop Estoque => int
}
```

### Pattern Matching Avançado
```jot
fn ProcessarResposta(resposta) => string {
    pmatching (resposta) {
        case { Status: 200, Data: data } => data
        case { Status: 404 } => "Não encontrado"
        case { Status: 500 } => "Erro interno"
        default => "Resposta inválida"
    }
}
``` 