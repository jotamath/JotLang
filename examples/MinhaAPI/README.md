# 🚀 MinhaAPI

Uma API moderna construída com JotLang e .NET 9.0.

## 📋 Pré-requisitos

- .NET 9.0 SDK
- SQL Server (ou outro banco de dados compatível)
- Visual Studio 2022 ou VS Code

## 🚀 Começando

1. Clone o repositório
2. Restaure as dependências:
```bash
dotnet restore
```

3. Configure a string de conexão no arquivo `appsettings.json`

4. Execute as migrações do banco de dados:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

5. Execute a aplicação:
```bash
dotnet run
```

## 📚 Documentação da API

A documentação da API está disponível em:
- Swagger UI: `https://localhost:5001/swagger`
- OpenAPI: `https://localhost:5001/swagger/v1/swagger.json`

## 🔧 Configuração

### 🔐 JWT

Configure as chaves JWT no arquivo `appsettings.json`:
```json
"Jwt": {
    "Key": "sua-chave-secreta-aqui",
    "Issuer": "MinhaAPI",
    "Audience": "MinhaAPI-Client"
}
```

### 🌐 CORS

Configure as origens permitidas no arquivo `appsettings.json`:
```json
"Cors": {
    "AllowedOrigins": [
        "http://localhost:3000",
        "http://localhost:4200"
    ]
}
```

## 📦 Estrutura do Projeto

```
MinhaAPI/
├── src/
│   ├── Program.jt
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   └── Data/
├── tests/
├── docs/
├── appsettings.json
└── MinhaAPI.csproj
```

## 🛠️ Desenvolvimento

### 🔄 Compilação
```bash
dotnet build
```

### 🧪 Testes
```bash
dotnet test
```

### 📦 Publicação
```bash
dotnet publish -c Release
```

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está licenciado sob a licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

---

<div align="center">
  <sub>Feito com ❤️ usando JotLang</sub>
</div>
