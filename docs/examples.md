# 🎯 Exemplos Práticos JotLang

## 📝 Básico

### 👋 Hello World
```jot
fn main() => void {
    print("Olá, Mundo!")
}
```

### 🔢 Calculadora
```jot
fn soma(a: int, b: int) => int {
    return a + b
}

fn main() => void {
    let resultado = soma(5, 3)
    print("5 + 3 = {resultado}")
}
```

## 📦 CRUD

### 👤 Usuários
```jot
@crud("users")
class User {
    prop id: int
    prop name: string
    prop email: string
    prop age: int
}

@httpget("/api/users")
fn ListUsers() => List<User> {
    return User.all()
}

@httppost("/api/users")
fn CreateUser(@body user: User) => User {
    return user.save()
}

@httpput("/api/users/{id}")
fn UpdateUser(id: int, @body user: User) => User {
    return User.update(id, user)
}

@httpdelete("/api/users/{id}")
fn DeleteUser(id: int) => void {
    User.delete(id)
}
```

## 🌐 Web

### 📡 API REST
```jot
@httpget("/api/products")
fn ListProducts() => List<Product> {
    return Product.all()
}

@httppost("/api/products")
fn CreateProduct(@body product: Product) => Product {
    return product.save()
}

@httpget("/api/products/{id}")
fn GetProduct(id: int) => Product {
    return Product.find(id)
}
```

### 💬 Chat WebSocket
```jot
@websocket("/chat")
fn Chat(ws: WebSocket) {
    ws.on("message", (msg) => {
        let response = {
            user: ws.user,
            message: msg,
            timestamp: now()
        }
        ws.broadcast(response)
    })
}
```

## 💾 Database

### 📊 Queries
```jot
fn GetActiveUsers() => List<User> {
    return query<User>("""
        SELECT * FROM users 
        WHERE status = 'active' 
        ORDER BY created_at DESC
    """)
}

fn UpdateUserStatus(id: int, status: string) => void {
    execute("""
        UPDATE users 
        SET status = {status} 
        WHERE id = {id}
    """)
}
```

## 🔄 Padrões

### 🏭 Factory
```jot
@factory
class UserFactory {
    fn create(name: string, email: string) => User {
        return User {
            name: name,
            email: email,
            created_at: now()
        }
    }
}
```

### 🔄 Observer
```jot
@observable
class EventBus {
    fn on(event: string, handler: (any) => void) => void {
        // Implementação
    }
    
    fn emit(event: string, data: any) => void {
        // Implementação
    }
}
```

## 📊 Coleções

### 🔢 Listas
```jot
fn ProcessNumbers() => void {
    let numbers = list<int>()
    numbers.add(1)
    numbers.add(2)
    numbers.add(3)
    
    let doubled = numbers.map(n => n * 2)
    let even = numbers.filter(n => n % 2 == 0)
    let sum = numbers.reduce((acc, n) => acc + n, 0)
}
```

### 📝 Dicionários
```jot
fn ProcessConfig() => void {
    let config = dict<string,any>()
    config["host"] = "localhost"
    config["port"] = 8080
    config["debug"] = true
    
    for (key, value) in config {
        print("{key}: {value}")
    }
}
```

## 🔒 Segurança

### 🔐 Autenticação
```jot
@auth
@httpget("/api/profile")
fn GetProfile() => User {
    return current_user()
}

@role("admin")
@httpget("/api/admin/dashboard")
fn GetDashboard() => Dashboard {
    return Dashboard.load()
}
```

### 🔑 Autorização
```jot
@permission("read:users")
@httpget("/api/users")
fn ListUsers() => List<User> {
    return User.all()
}

@permission("write:users")
@httppost("/api/users")
fn CreateUser(@body user: User) => User {
    return user.save()
}
```

## 📦 Cache

### 🔄 Cache com TTL
```jot
@cache(ttl: 3600)
@httpget("/api/weather/{city}")
fn GetWeather(city: string) => Weather {
    return WeatherService.get(city)
}

@cachekey("user:{id}")
@httpget("/api/users/{id}")
fn GetUser(id: int) => User {
    return User.find(id)
}
```

## 📋 Validação

### ✅ Validação de Dados
```jot
@validate
class UserDTO {
    @required
    @minlength(3)
    @maxlength(100)
    prop name: string
    
    @required
    @email
    prop email: string
    
    @required
    @min(18)
    prop age: int
}
```

---

<div align="center">
  <sub>🎯 Experimente estes exemplos e crie seus próprios!</sub>
</div> 