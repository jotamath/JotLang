---
layout: default
title: Referência da Linguagem
nav_order: 3
has_children: true
---

# Referência da Linguagem Jot

## Índice

1. [Sintaxe Básica](#sintaxe-básica)
2. [Tipos](#tipos)
3. [Variáveis](#variáveis)
4. [Funções](#funções)
5. [Estruturas de Controle](#estruturas-de-controle)
6. [Classes e Objetos](#classes-e-objetos)
7. [Atributos](#atributos)
8. [Operadores](#operadores)
9. [Expressões](#expressões)
10. [Biblioteca Padrão](#biblioteca-padrão)

## Sintaxe Básica

### Estrutura do Programa

```jot
// Comentário de linha
/* Comentário
   de bloco */

// Importações
import System
import System.Collections.Generic

// Namespace
namespace MeuApp {
    // Código aqui
}
```

### Blocos de Código

```jot
fn Exemplo() {
    // Bloco de código
    {
        // Sub-bloco
    }
}
```

## Tipos

### Tipos Primitivos

- `bool`: Valores booleanos
- `int`: Números inteiros
- `float`: Números de ponto flutuante
- `string`: Texto
- `guid`: Identificador único
- `datetime`: Data e hora
- `decimal`: Números decimais precisos

### Tipos Compostos

- `List<T>`: Lista de elementos
- `Dictionary<K,V>`: Dicionário chave-valor
- `Array<T>`: Array de elementos
- `Tuple<T1,T2,...>`: Tupla de valores

## Variáveis

### Declaração

```jot
// Com tipo explícito
var nome: string = "João"

// Com inferência de tipo
var idade = 25

// Constante
const PI = 3.14159
```

### Escopo

```jot
var global = 10

fn Exemplo() {
    var local = 20
    {
        var bloco = 30
    }
    // bloco não está acessível aqui
}
```

## Funções

### Declaração

```jot
// Função sem retorno
fn Saudacao(nome: string) {
    print("Olá, " + nome)
}

// Função com retorno
fn Soma(a: int, b: int) => int {
    return a + b
}

// Função com parâmetros opcionais
fn Mensagem(texto: string, destaque: bool = false) {
    if (destaque) {
        print("*** " + texto + " ***")
    } else {
        print(texto)
    }
}
```

### Funções Lambda

```jot
var quadrado = (x: int) => x * x
var soma = (a: int, b: int) => a + b
```

## Estruturas de Controle

### If/Else

```jot
if (condicao) {
    // código
} else if (outraCondicao) {
    // código
} else {
    // código
}
```

### Loops

```jot
// For
for (var i = 0; i < 10; i++) {
    // código
}

// While
while (condicao) {
    // código
}

// Do-While
do {
    // código
} while (condicao)

// ForEach
foreach (var item in lista) {
    // código
}
```

### Switch

```jot
switch (valor) {
    case 1:
        // código
        break
    case 2:
        // código
        break
    default:
        // código
}
```

## Classes e Objetos

### Classes

```jot
class Pessoa {
    prop nome: string
    prop idade: int
    
    fn constructor(nome: string, idade: int) {
        this.nome = nome
        this.idade = idade
    }
    
    fn Saudacao() => string {
        return "Olá, eu sou " + this.nome
    }
}
```

### DTOs

```jot
@dto UsuarioDTO {
    prop id: guid
    prop nome: string
    prop email: string
}
```

### Entidades

```jot
@entity Produto {
    prop id: guid
    prop nome: string
    prop preco: decimal
    prop categoria: Categoria @manyToOne
}
```

## Atributos

### Atributos de Validação

```jot
@dto UsuarioDTO {
    prop nome: string @required
    prop email: string @email
    prop idade: int @min(18)
}
```

### Atributos HTTP

```jot
@httpGet("/api/usuarios")
@httpPost("/api/usuarios")
@httpPut("/api/usuarios/{id}")
@httpDelete("/api/usuarios/{id}")
```

### Atributos de Autorização

```jot
@authorize
@authorize(roles: ["Admin"])
@authorize(permissions: ["CreateUser"])
```

## Operadores

### Aritméticos

- `+`: Adição
- `-`: Subtração
- `*`: Multiplicação
- `/`: Divisão
- `%`: Módulo
- `++`: Incremento
- `--`: Decremento

### Relacionais

- `==`: Igual
- `!=`: Diferente
- `>`: Maior
- `<`: Menor
- `>=`: Maior ou igual
- `<=`: Menor ou igual

### Lógicos

- `&&`: E
- `||`: OU
- `!`: Não

### Atribuição

- `=`: Atribuição simples
- `+=`: Adição e atribuição
- `-=`: Subtração e atribuição
- `*=`: Multiplicação e atribuição
- `/=`: Divisão e atribuição

## Expressões

### Expressões de Coalescência

```jot
var valor = valorPadrao ?? outroValor
```

### Expressões de Condição

```jot
var resultado = condicao ? valorVerdadeiro : valorFalso
```

### Expressões de Desconstrutor

```jot
var (nome, idade) = pessoa
```

## Biblioteca Padrão

### Console

```jot
print("Texto")
print("Valor: " + valor.ToString())
```

### String

```jot
var texto = "Olá"
var maiusculo = texto.ToUpper()
var tamanho = texto.Length
```

### DateTime

```jot
var agora = DateTime.Now
var data = DateTime.Parse("2024-01-01")
```

### Collections

```jot
var lista = new List<int>()
lista.Add(1)
lista.Remove(1)
var existe = lista.Contains(1)
```

### LINQ

```jot
var numeros = new List<int> { 1, 2, 3, 4, 5 }
var pares = numeros.Where(n => n % 2 == 0)
var soma = numeros.Sum()
var media = numeros.Average()
```

## Tópicos

### Sintaxe Básica
- [Imports e Modules](./basics/imports-modules.md)
- [Classes e Funções](./basics/classes-functions.md)
- [Tipos de Dados](./basics/data-types.md)
- [Variáveis e Constantes](./basics/variables-constants.md)

### Funcionalidades Avançadas
- [CRUD Automático](./advanced/crud.md)
- [DTOs](./advanced/dtos.md)
- [Pattern Matching](./advanced/pattern-matching.md)
- [Funções Lambda](./advanced/lambda-functions.md)

### APIs e Web
- [Controladores](./web/controllers.md)
- [Rotas e Endpoints](./web/routing.md)
- [Middleware](./web/middleware.md)
- [Autenticação](./web/auth.md)

### Ferramentas
- [CLI](./tools/cli.md)
- [Compilador](./tools/compiler.md)
- [Depuração](./tools/debugging.md)
- [Testes](./tools/testing.md)

## Exemplos

Cada tópico inclui exemplos práticos e casos de uso comuns. Para exemplos mais completos, consulte a seção [Exemplos](../examples/index.md). 