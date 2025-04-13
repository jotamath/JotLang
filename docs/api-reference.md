# 📚 Referência da API JotLang

## 📦 Módulos Principais

### 🔧 Core
```jot
module Core {
    // Funções básicas
    fn print(text: string) => void
    fn input(prompt: string) => string
    fn len(obj: any) => int
    fn type(obj: any) => string
}
```

### 🔄 Collections
```jot
module Collections {
    // Listas
    fn list<T>() => List<T>
    fn dict<K,V>() => Dictionary<K,V>
    fn set<T>() => Set<T>
    
    // Operações
    fn map<T,U>(list: List<T>, fn: (T) => U) => List<U>
    fn filter<T>(list: List<T>, fn: (T) => bool) => List<T>
    fn reduce<T,U>(list: List<T>, fn: (U,T) => U, initial: U) => U
}
```

### 🌐 Web
```jot
module Web {
    // HTTP
    @httpget("/api/users")
    fn GetUsers() => List<User>
    
    @httppost("/api/users")
    fn CreateUser(@body user: User) => User
    
    // WebSocket
    @websocket("/chat")
    fn Chat(ws: WebSocket) => void
}
```

### 💾 Database
```jot
module Database {
    // CRUD
    @crud("users")
    class User {
        prop id: int
        prop name: string
        prop email: string
    }
    
    // Queries
    fn query<T>(sql: string) => List<T>
    fn execute(sql: string) => int
}
```

## 🎯 Decorators

### 📝 HTTP
```jot
@httpget("/path")
@httppost("/path")
@httpput("/path")
@httpdelete("/path")
@httphead("/path")
@httppatch("/path")
```

### 🔒 Autenticação
```jot
@auth
@role("admin")
@permission("read")
```

### 📦 Cache
```jot
@cache(ttl: int)
@cachekey("prefix")
```

### 📋 Validação
```jot
@validate
@required
@minlength(5)
@maxlength(100)
@email
@pattern("regex")
```

## 🔄 Tipos de Dados

### 📊 Básicos
```jot
string
int
float
bool
date
time
datetime
```

### 📦 Coleções
```jot
List<T>
Dictionary<K,V>
Set<T>
Tuple<T1,T2,...>
```

### 🎯 Especiais
```jot
Option<T>
Result<T,E>
Future<T>
Stream<T>
```

## 🔧 Funções Utilitárias

### 📝 String
```jot
fn format(template: string, ...args: any) => string
fn split(str: string, delimiter: string) => List<string>
fn join(list: List<string>, delimiter: string) => string
fn trim(str: string) => string
```

### 📊 Math
```jot
fn min(a: number, b: number) => number
fn max(a: number, b: number) => number
fn abs(x: number) => number
fn round(x: number, decimals: int) => number
```

### 📅 DateTime
```jot
fn now() => datetime
fn today() => date
fn parseDate(str: string) => date
fn formatDate(date: date, format: string) => string
```

## 🔄 Padrões de Projeto

### 📦 Factory
```jot
@factory
class UserFactory {
    fn create(name: string, email: string) => User
}
```

### 🔄 Singleton
```jot
@singleton
class Config {
    prop settings: Dictionary<string,any>
}
```

### 📝 Observer
```jot
@observable
class EventEmitter {
    fn on(event: string, handler: (any) => void) => void
    fn emit(event: string, data: any) => void
}
```

## 🎯 Exemplos de Uso

### 📝 CRUD Completo
```jot
@crud("products")
class Product {
    prop id: int
    prop name: string
    prop price: float
    prop stock: int
}

@httpget("/api/products")
fn ListProducts() => List<Product> {
    return Product.all()
}

@httppost("/api/products")
fn CreateProduct(@body product: Product) => Product {
    return product.save()
}
```

### 🔄 WebSocket Chat
```jot
@websocket("/chat")
fn Chat(ws: WebSocket) {
    ws.on("message", (msg) => {
        ws.broadcast(msg)
    })
}
```

### 📦 Cache com TTL
```jot
@cache(ttl: 3600)
@httpget("/api/weather")
fn GetWeather(city: string) => Weather {
    return WeatherService.get(city)
}
```

---

<div align="center">
  <sub>📚 Consulte a documentação completa para mais detalhes!</sub>
</div> 