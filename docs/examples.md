---
layout: default
title: Exemplos - JotLang
---

# 📝 Exemplos Práticos

Aqui estão alguns exemplos práticos de como usar o JotLang em diferentes cenários.

## 🏗️ API REST

```jot
namespace Api {
    @httpget("/users")
    fn GetUsers() => List<User> = {
        return db.Users.ToList()
    }

    @httppost("/users")
    fn CreateUser(@dto UserDto user) => User = {
        var newUser = new User {
            Name = user.Name,
            Email = user.Email
        }
        db.Users.Add(newUser)
        db.SaveChanges()
        return newUser
    }
}
```

## 💾 CRUD com Banco de Dados

```jot
namespace Data {
    @crud
    class Product {
        prop Id: int
        prop Name: string
        prop Price: decimal
        prop Stock: int
    }

    @crud
    class Order {
        prop Id: int
        prop CustomerId: int
        prop Products: List<Product>
        prop Total: decimal
    }
}
```

## 🔍 Pattern Matching

```jot
fn ProcessPayment(payment: Payment) => string = {
    pmatching payment {
        case CreditCard(card) => "Processando cartão de crédito"
        case Pix(key) => "Processando PIX"
        case Boleto(code) => "Gerando boleto"
        case _ => "Método de pagamento inválido"
    }
}
```

## 🔄 WebSocket Chat

```jot
namespace Chat {
    @websocket("/chat")
    class ChatHub {
        fn OnMessage(message: string) => void = {
            Clients.All.SendMessage(message)
        }

        fn OnJoin(user: string) => void = {
            Clients.All.SendMessage($"{user} entrou no chat")
        }
    }
}
```

## 🛡️ Autenticação e Autorização

```jot
namespace Auth {
    @httpget("/profile")
    @authorize
    fn GetProfile() => UserProfile = {
        var userId = User.GetId()
        return db.Profiles.Find(userId)
    }

    @httppost("/login")
    fn Login(@dto LoginDto credentials) => AuthResult = {
        var user = db.Users.FirstOrDefault(u => u.Email == credentials.Email)
        if (user == null || !VerifyPassword(user, credentials.Password)) {
            return new AuthResult { Success = false }
        }
        
        var token = GenerateJwtToken(user)
        return new AuthResult { 
            Success = true,
            Token = token
        }
    }
}
```

## 📊 Cache com TTL

```jot
namespace Cache {
    @cache(ttl: "5m")
    fn GetCachedData(key: string) => string = {
        return ExpensiveOperation(key)
    }

    @cache(ttl: "1h")
    fn GetUserData(userId: int) => UserData = {
        return db.Users.Find(userId)
    }
}
```

## 🔄 Background Jobs

```jot
namespace Jobs {
    @background(every: "1h")
    fn CleanupOldData() => void = {
        var oldData = db.Data.Where(d => d.CreatedAt < DateTime.Now.AddDays(-30))
        db.Data.RemoveRange(oldData)
        db.SaveChanges()
    }

    @background(every: "5m")
    fn CheckSystemHealth() => void = {
        var health = new SystemHealth {
            Memory = GetMemoryUsage(),
            CPU = GetCPUUsage(),
            Disk = GetDiskSpace()
        }
        NotifyHealthStatus(health)
    }
}
```

## 🧪 Testes Unitários

```jot
namespace Tests {
    class CalculatorTests {
        fn TestAddition() => void = {
            var calc = new Calculator()
            var result = calc.Add(2, 3)
            Assert.Equal(5, result)
        }

        fn TestDivision() => void = {
            var calc = new Calculator()
            Assert.Throws<DivideByZeroException>(() => calc.Divide(10, 0))
        }
    }
}
```

## 📦 Publicando um Pacote

```jot
namespace MyPackage {
    @package("meu-pacote", version: "1.0.0")
    class MyLibrary {
        prop Config: string
        prop IsEnabled: bool

        fn Initialize() => void = {
            // Inicialização do pacote
        }
    }
}
```

---

<div align="center">
  <sub>🎯 Experimente estes exemplos e crie seus próprios!</sub>
</div> 