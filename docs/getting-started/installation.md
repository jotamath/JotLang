---
layout: default
title: Instalação
parent: Começando
nav_order: 1
---

# Guia de Instalação

Este guia irá ajudá-lo a instalar e configurar o ambiente de desenvolvimento Jot.

## Pré-requisitos

- .NET 9.0 SDK ou superior
- Visual Studio 2022 ou VS Code
- Git (opcional, para clonar o repositório)

## Instalação via NuGet

### 1. Pacote Principal

```bash
dotnet add package JotLang
```

### 2. Ferramenta de Linha de Comando

```bash
dotnet tool install -g JotLang.Tool
```

Após a instalação, você pode verificar se a ferramenta foi instalada corretamente:

```bash
jot --version
```

## Instalação Manual

1. Clone o repositório:
```bash
git clone https://github.com/seu-usuario/JotLang.git
cd JotLang
```

2. Restaure as dependências:
```bash
dotnet restore
```

3. Compile o projeto:
```bash
dotnet build
```

## Configuração do Ambiente

### Visual Studio Code

1. Instale a extensão "Jot Language Support"
2. Configure o arquivo `settings.json`:
```json
{
    "jot.languageServer.path": "path/to/jot-language-server",
    "jot.format.enable": true
}
```

### Visual Studio 2022

1. Instale a extensão "Jot Language Tools"
2. Reinicie o Visual Studio

## Verificação da Instalação

Para verificar se a instalação foi bem-sucedida, execute:

```bash
jot --version
```

Você deve ver a versão do Jot instalada.

## Primeiro Projeto

1. Crie um novo projeto:
```bash
jot new meu-projeto
cd meu-projeto
```

2. Execute o projeto:
```bash
jot run
```

## Solução de Problemas

### Erro: Comando não encontrado
- Verifique se o .NET SDK está instalado corretamente
- Tente reinstalar a ferramenta global: `dotnet tool install -g JotLang.Tool`

### Erro: Falha na compilação
- Verifique se todas as dependências foram restauradas: `dotnet restore`
- Limpe a solução e reconstrua: `dotnet clean && dotnet build`

### Erro: Extensão não funciona
- Verifique a compatibilidade da versão
- Reinstale a extensão
- Limpe o cache do IDE

## Próximos Passos

- Siga o [Tutorial Básico](./tutorial.md)
- Explore os [Exemplos](../examples/index.md)
- Consulte a [Referência da Linguagem](../reference/index.md) 