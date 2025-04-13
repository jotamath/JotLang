# Relacionamentos

A linguagem Jot oferece um sistema poderoso para definir e gerenciar relacionamentos entre entidades.

## Tipos de Relacionamentos

### 1:1 (Um para Um)

```jot
@entity Usuario {
    prop id: guid
    prop nome: string
    prop perfil: Perfil @oneToOne
}

@entity Perfil {
    prop id: guid
    prop bio: string
    prop usuario: Usuario @oneToOne
}
```

### 1:N (Um para Muitos)

```jot
@entity Cliente {
    prop id: guid
    prop nome: string
    prop pedidos: List<Pedido> @oneToMany
}

@entity Pedido {
    prop id: guid
    prop data: datetime
    prop cliente: Cliente @manyToOne
}
```

### N:N (Muitos para Muitos)

```jot
@entity Aluno {
    prop id: guid
    prop nome: string
    prop cursos: List<Curso> @manyToMany
}

@entity Curso {
    prop id: guid
    prop nome: string
    prop alunos: List<Aluno> @manyToMany
}
```

## Configurações de Relacionamento

### Cascata

```jot
@entity Usuario {
    prop id: guid
    prop posts: List<Post> @oneToMany(cascade: true)
}

@entity Post {
    prop id: guid
    prop titulo: string
    prop usuario: Usuario @manyToOne
}
```

### Chaves Estrangeiras

```jot
@entity Pedido {
    prop id: guid
    prop clienteId: guid @foreignKey("Cliente")
    prop cliente: Cliente @manyToOne
}
```

## Relacionamentos com DTOs

```jot
@dto PedidoDTO {
    prop id: guid
    prop cliente: ClienteDTO @include
    prop itens: List<ItemPedidoDTO> @include
}

@dto ClienteDTO {
    prop id: guid
    prop nome: string
    prop pedidos: List<PedidoDTO> @exclude
}
```

## Consultas com Relacionamentos

```jot
fn GetClienteComPedidos(id: guid) => ClienteDTO {
    return db.Clientes
        .Include(c => c.Pedidos)
        .Include(c => c.Pedidos.Itens)
        .First(c => c.Id == id)
}
```

## Boas Práticas

1. **Nomenclatura**
   - Use nomes descritivos para as propriedades de relacionamento
   - Mantenha consistência na nomenclatura

2. **Configuração**
   - Configure cascata apenas quando necessário
   - Use lazy loading para relacionamentos grandes
   - Configure índices apropriadamente

3. **Performance**
   - Evite relacionamentos circulares
   - Use Include/Exclude em DTOs para controlar o carregamento
   - Configure índices para chaves estrangeiras

4. **Manutenção**
   - Documente relacionamentos complexos
   - Mantenha a consistência dos dados
   - Use transações quando necessário

## Exemplos de Uso

### Criando Relacionamentos

```jot
fn CriarPedido(clienteId: guid, itens: List<ItemPedidoDTO>) => Pedido {
    var cliente = db.Clientes.Find(clienteId)
    var pedido = new Pedido {
        Cliente = cliente,
        Itens = itens.Select(i => new ItemPedido(i)).ToList()
    }
    db.Pedidos.Add(pedido)
    db.SaveChanges()
    return pedido
}
```

### Consultando com Relacionamentos

```jot
fn GetPedidosPorCliente(clienteId: guid) => List<PedidoDTO> {
    return db.Pedidos
        .Include(p => p.Itens)
        .Where(p => p.ClienteId == clienteId)
        .Select(p => new PedidoDTO(p))
        .ToList()
}
``` 