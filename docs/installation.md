# 📥 Instalação do JotLang

## Requisitos

- .NET 9.0 SDK ou superior
- Visual Studio 2022 ou VS Code
- Git (opcional, para clonar o repositório)

## Instalação via NuGet

1. Adicione o pacote JotLang ao seu projeto:

```bash
dotnet add package JotLang
```

2. Ou instale globalmente como uma ferramenta:

```bash
dotnet tool install -g jotlang
```

## Instalação Manual

1. Clone o repositório:

```bash
git clone https://github.com/jotamath/JotLang.git
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

4. Execute os testes:

```bash
dotnet test
```

## Verificando a Instalação

Para verificar se a instalação foi bem-sucedida:

```bash
jot --version
```

Você deve ver a versão 1.5.0 ou superior.

## Atualização

Para atualizar para a versão mais recente:

```bash
dotnet tool update -g jotlang
```

## Solução de Problemas

### Erro: "Comando não encontrado"
- Verifique se o .NET SDK está instalado
- Adicione o diretório de ferramentas ao PATH
- Execute `dotnet tool list -g` para verificar a instalação

### Erro: "Versão incompatível"
- Atualize o .NET SDK
- Verifique a versão do runtime
- Limpe o cache do NuGet

## Próximos Passos

- [Primeiros Passos](getting-started.md)
- [Sintaxe](syntax.md)
- [Templates](templates.md) 