# 🚀 Instalação e Configuração da JotLang

## 📋 Pré-requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) ou superior
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) ou [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/downloads)

## 📥 Instalação

### 🔄 Clonando o Repositório
```bash
git clone https://github.com/seu-usuario/JotLang.git
cd JotLang
```

### 📦 Restaurando Dependências
```bash
dotnet restore
```

### 🏗️ Compilando o Projeto
```bash
dotnet build
```

## ⚙️ Configuração

### 📝 Configuração do Ambiente

1. Crie um arquivo `.env` na raiz do projeto:
```env
JOTLANG_ENV=development
JOTLANG_LOG_LEVEL=debug
```

2. Configure as variáveis de ambiente no seu sistema:
```bash
# Windows (PowerShell)
$env:JOTLANG_ENV="development"
$env:JOTLANG_LOG_LEVEL="debug"

# Linux/Mac
export JOTLANG_ENV="development"
export JOTLANG_LOG_LEVEL="debug"
```

### 🔧 Configuração do Projeto

1. Abra o arquivo `appsettings.json`:
```json
{
  "JotLang": {
    "Environment": "development",
    "LogLevel": "debug",
    "Features": {
      "WebSockets": true,
      "Caching": true,
      "Validation": true
    }
  }
}
```

2. Personalize as configurações conforme necessário.

## 🎯 Primeiros Passos

### 📝 Criando um Novo Projeto
```bash
dotnet new jotlang -n MeuProjeto
cd MeuProjeto
```

### 🔄 Executando o Projeto
```bash
dotnet run
```

### 📦 Adicionando Dependências
```bash
dotnet add package JotLang.Extensions
dotnet add package JotLang.Web
```

## 🔍 Verificando a Instalação

### 📝 Teste Básico
Crie um arquivo `test.jt`:
```jot
module Teste {
    fn Main() {
        print("Hello, JotLang!")
    }
}
```

Execute o teste:
```bash
dotnet run test.jt
```

## 🛠️ Ferramentas de Desenvolvimento

### 📝 Extensões Recomendadas para VS Code
- JotLang Language Support
- JotLang Debugger
- JotLang Formatter

### 🔧 Configuração do Editor
1. Instale as extensões recomendadas
2. Configure o formatter:
```json
{
  "editor.formatOnSave": true,
  "jotlang.format.enable": true
}
```

## 🔄 Atualização

### 📦 Atualizando o Compilador
```bash
dotnet tool update -g JotLang.Compiler
```

### 🔄 Atualizando as Dependências
```bash
dotnet restore
dotnet update
```

## 🎯 Solução de Problemas

### 📝 Problemas Comuns

1. **Erro de Compilação**
   - Verifique se todas as dependências estão instaladas
   - Execute `dotnet clean` e `dotnet build`

2. **Erro de Runtime**
   - Verifique as configurações do ambiente
   - Confira os logs de erro

3. **Problemas de Performance**
   - Verifique a configuração de cache
   - Otimize as queries do banco de dados

### 🔍 Logs e Depuração

1. Habilite logs detalhados:
```bash
dotnet run --log-level debug
```

2. Use o depurador:
```bash
dotnet run --debug
```

## 📚 Recursos Adicionais

- [Documentação Oficial](https://jotlang.dev/docs)
- [Exemplos de Código](https://github.com/jotlang/examples)
- [Fórum da Comunidade](https://forum.jotlang.dev)

---

<div align="center">
  <sub>🤝 Precisa de ajuda? Abra uma issue no GitHub!</sub>
</div> 