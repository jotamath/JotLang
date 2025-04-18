using System;
using System.Collections.Generic;
using JotLang.Lexer;
using JotLang.Parser;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace JotLang.Interpreter
{
    public class JotRuntimeException : Exception
    {
        public Token Token { get; }
        public int Line => Token.Line;

        public JotRuntimeException(Token token, string message) : base($"Erro na linha {token.Line}: {message}")
        {
            Token = token;
        }
    }

    public class Environment
    {
        public Environment? Enclosing { get; private set; }
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();
        private readonly ILogger? _logger;
        private readonly Dictionary<string, Token> _types = new Dictionary<string, Token>();

        public Environment(ILogger? logger = null)
        {
            Enclosing = null;
            _logger = logger;
        }

        public Environment(Environment? enclosing, ILogger? logger = null)
        {
            Enclosing = enclosing;
            _logger = logger;
        }

        public void Define(string name, object? value, Token? type = null)
        {
            _logger?.LogDebug($"Definindo variável '{name}' com valor: {value ?? "null"} e tipo: {type?.Lexeme ?? "any"}");
            _values[name] = value ?? new object();
            if (type != null)
            {
                _types[name] = type;
            }
        }

        public object Get(Token name)
        {
            if (_values.ContainsKey(name.Lexeme))
            {
                var value = _values[name.Lexeme];
                _logger?.LogDebug($"Obtendo valor da variável '{name.Lexeme}': {value ?? "null"}");
                return value ?? new object();
            }

            if (Enclosing != null)
            {
                return Enclosing.Get(name);
            }

            throw new JotRuntimeException(name, $"Variável não definida '{name.Lexeme}'.");
        }

        public void Assign(Token name, object value)
        {
            if (_values.ContainsKey(name.Lexeme))
            {
                _logger?.LogDebug($"Atribuindo valor à variável '{name.Lexeme}': {value ?? "null"}");
                _values[name.Lexeme] = value;
                return;
            }

            if (Enclosing != null)
            {
                Enclosing.Assign(name, value);
                return;
            }

            throw new JotRuntimeException(name, $"Variável não definida '{name.Lexeme}'.");
        }

        public Token? GetType(Token name)
        {
            if (_types.ContainsKey(name.Lexeme))
            {
                return _types[name.Lexeme];
            }

            if (Enclosing != null)
            {
                return Enclosing.GetType(name);
            }

            return null;
        }
    }

    public class JotFunction
    {
        public Function? Declaration { get; }
        public Environment? Closure { get; }
        private readonly ILogger? _logger;

        public JotFunction(Function? declaration, Environment? closure, ILogger? logger = null)
        {
            Declaration = declaration;
            Closure = closure;
            _logger = logger;
        }

        public virtual object? Call(JotInterpreter interpreter, List<object> arguments)
        {
            if (Declaration == null || Closure == null) return null;

            var environment = new Environment(Closure, _logger);

            // Verificar tipos dos argumentos
            for (int i = 0; i < Declaration.Parameters.Count; i++)
            {
                if (!interpreter.CheckType(arguments[i], Declaration.ParameterTypes[i]))
                {
                    throw new JotRuntimeException(Declaration.Parameters[i],
                        $"Tipo incompatível para o parâmetro '{Declaration.Parameters[i].Lexeme}'. " +
                        $"Esperado '{Declaration.ParameterTypes[i].Lexeme}' mas recebeu valor do tipo '{arguments[i]?.GetType().Name ?? "null"}'.");
                }
                environment.Define(Declaration.Parameters[i].Lexeme, arguments[i], Declaration.ParameterTypes[i]);
            }

            try
            {
                interpreter.ExecuteBlock(Declaration.Body, environment);
            }
            catch (Return returnValue)
            {
                // Verificar tipo do retorno
                if (!interpreter.CheckType(returnValue.Value, Declaration.ReturnType))
                {
                    throw new JotRuntimeException(Declaration.ReturnType,
                        $"Tipo de retorno incompatível. Esperado '{Declaration.ReturnType.Lexeme}' mas retornou valor do tipo '{returnValue.Value?.GetType().Name ?? "null"}'.");
                }
                return returnValue.Value;
            }

            // Se não houver retorno explícito e o tipo de retorno for void, tudo bem
            if (Declaration.ReturnType.Type == TokenType.VOID)
            {
                return null;
            }

            throw new JotRuntimeException(Declaration.ReturnType,
                $"Função deve retornar um valor do tipo '{Declaration.ReturnType.Lexeme}'.");
        }

        public override string ToString() => $"<fn {Declaration?.Name.Lexeme ?? "anonymous"}>";
    }

    public class Return : Exception
    {
        public readonly object Value;

        public Return(object value)
        {
            Value = value;
        }
    }

    public class JotClass
    {
        public string Name { get; }
        public Dictionary<string, JotFunction> Methods { get; private set; }
        public Dictionary<string, object> Properties { get; private set; }

        public JotClass(string name)
        {
            Name = name;
            Methods = new Dictionary<string, JotFunction>();
            Properties = new Dictionary<string, object>();
        }

        public void SetMethods(Dictionary<string, JotFunction> methods)
        {
            Methods = methods;
        }

        public void SetProperties(Dictionary<string, object> properties)
        {
            Properties = properties;
        }

        public JotInstance CreateInstance()
        {
            return new JotInstance(this);
        }

        public JotFunction? FindMethod(string name)
        {
            return Methods.TryGetValue(name, out var method) ? method : null;
        }

        public override string ToString() => Name;
    }

    public class JotInstance
    {
        private readonly JotClass _class;
        private readonly Dictionary<string, object> _fields;
        private readonly ILogger? _logger;

        public JotClass Class => _class;

        public JotInstance(JotClass @class, ILogger? logger = null)
        {
            _class = @class ?? throw new ArgumentNullException(nameof(@class));
            _fields = new Dictionary<string, object>();
            _logger = logger;

            // Inicializa as propriedades da classe
            foreach (var prop in _class.Properties)
            {
                _fields[prop.Key] = prop.Value;
            }
        }

        public object Get(Token name)
        {
            _logger?.LogDebug($"Tentando acessar propriedade '{name.Lexeme}' da instância de {_class.Name}");

            if (_fields.TryGetValue(name.Lexeme, out var value))
            {
                return value;
            }

            var method = _class.FindMethod(name.Lexeme);
            if (method != null)
            {
                return method;
            }

            throw new JotRuntimeException(name, $"Propriedade ou método '{name.Lexeme}' não encontrado na classe {_class.Name}.");
        }

        public void Set(Token name, object value)
        {
            _logger?.LogDebug($"Definindo propriedade '{name.Lexeme}' da instância de {_class.Name} com valor: {value}");
            _fields[name.Lexeme] = value;
        }

        public override string ToString() => $"{_class.Name} instance";
    }

    public class JotInterpreter : IExprVisitor<object>, IStmtVisitor<object>
    {
        public readonly Environment Globals;
        private Environment _environment;
        private readonly ILogger<JotInterpreter>? _logger;

        public JotInterpreter()
        {
            _logger = null;
            Globals = new Environment(_logger);
            _environment = Globals;

            // Adicionar funções nativas
            Globals.Define("print", new NativePrint(_logger));
        }

        public JotInterpreter(ILogger<JotInterpreter> logger)
        {
            _logger = logger;
            Globals = new Environment(_logger);
            _environment = Globals;

            // Adicionar funções nativas
            Globals.Define("print", new NativePrint(_logger));
        }

        public async Task InterpretAsync(string code)
        {
            try
            {
                _logger?.LogInformation("Iniciando interpretação do código");
                
                _logger?.LogDebug("Criando lexer...");
                var lexer = new JotLexer(code);
                
                _logger?.LogDebug("Gerando tokens...");
                var tokens = lexer.ScanTokens();
                if (tokens == null)
                {
                    throw new Exception("Falha ao gerar tokens");
                }
                _logger?.LogDebug($"Tokens gerados: {tokens.Count}");

                _logger?.LogDebug("Criando parser...");
                var parser = new JotParser(tokens);
                
                _logger?.LogDebug("Parseando código...");
                var statements = parser.Parse();
                if (statements == null)
                {
                    throw new Exception("Falha ao parsear código");
                }
                _logger?.LogDebug($"Statements parseados: {statements.Count}");

                foreach (var statement in statements)
                {
                    if (statement == null) continue;
                    
                    _logger?.LogDebug($"Executando statement do tipo: {statement.GetType().Name}");
                    try
                    {
                        Execute(statement);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError($"Erro ao executar statement {statement.GetType().Name}: {ex.Message}");
                        _logger?.LogError($"Stack trace: {ex.StackTrace}");
                        throw;
                    }
                }

                _logger?.LogInformation("Interpretação concluída com sucesso");
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Erro ao interpretar código: {ex.Message}");
                _logger?.LogError($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    _logger?.LogError($"Inner exception: {ex.InnerException.Message}");
                    _logger?.LogError($"Inner stack trace: {ex.InnerException.StackTrace}");
                }
                throw;
            }
        }

        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (var statement in statements)
                {
                    Execute(statement);
                }
            }
            catch (JotRuntimeException error)
            {
                // TODO: Implementar sistema de erro mais robusto
                Console.WriteLine($"Erro em tempo de execução: {error.Message}");
            }
        }

        private void Execute(Stmt stmt)
        {
            try
            {
                _logger?.LogDebug($"Executando statement: {stmt.GetType().Name}");
                stmt.Accept(this);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Erro ao executar statement {stmt.GetType().Name}: {ex.Message}");
                _logger?.LogError($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public void ExecuteBlock(List<Stmt> statements, Environment environment)
        {
            var previous = _environment;
            try
            {
                _environment = environment;

                foreach (var statement in statements)
                {
                    try 
                    {
                        Execute(statement);
                    }
                    catch (Exception ex) when (!(ex is Return))
                    {
                        throw new JotRuntimeException(
                            new Token(TokenType.ERROR, "", null, statement is Expression expr ? 
                                (expr.ExpressionValue as Variable)?.Name.Line ?? 0 : 0),
                            $"Erro ao executar statement: {ex.Message}");
                    }
                }
            }
            finally
            {
                _environment = previous;
            }
        }

        public object VisitBinaryExpr(Binary expr)
        {
            var left = Evaluate(expr.Left);
            var right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.PLUS:
                    if (left is double && right is double)
                        return (double)left + (double)right;
                    if (left is string && right is string)
                        return (string)left + (string)right;
                    throw new JotRuntimeException(expr.Operator, "Operandos devem ser dois números ou duas strings.");
                case TokenType.MINUS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left - (double)right;
                case TokenType.STAR:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left * (double)right;
                case TokenType.SLASH:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left / (double)right;
                case TokenType.GREATER:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left <= (double)right;
                case TokenType.EQUAL_EQUAL:
                    return IsEqual(left, right);
                case TokenType.BANG_EQUAL:
                    return !IsEqual(left, right);
            }

            return null;
        }

        public object VisitGroupingExpr(Grouping expr)
        {
            return Evaluate(expr.Expression);
        }

        public object VisitLiteralExpr(Literal expr)
        {
            return expr.Value;
        }

        public object VisitUnaryExpr(Unary expr)
        {
            var right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.MINUS:
                    CheckNumberOperand(expr.Operator, right);
                    return -(double)right;
                case TokenType.BANG:
                    return !IsTruthy(right);
            }

            return null;
        }

        public object VisitVariableExpr(Variable expr)
        {
            return _environment.Get(expr.Name);
        }

        public object VisitAssignExpr(Assign expr)
        {
            var value = Evaluate(expr.Value);
            
            // Obter o tipo da variável do ambiente
            var varType = _environment.GetType(expr.Name);
            if (varType != null && !CheckType(value, varType))
            {
                throw new JotRuntimeException(expr.Name, 
                    $"Tipo incompatível na atribuição. Esperado '{varType.Lexeme}' mas recebeu valor do tipo '{value?.GetType().Name ?? "null"}'.");
            }

            _environment.Assign(expr.Name, value);
            return value;
        }

        public object VisitCallExpr(Call expr)
        {
            _logger?.LogDebug($"Iniciando chamada de função/método");
            
            var callee = Evaluate(expr.Callee);
            if (callee == null)
            {
                throw new JotRuntimeException(expr.Paren, "Tentativa de chamar uma função/método nulo.");
            }

            _logger?.LogDebug($"Tipo do callee: {callee.GetType().Name}");

            var arguments = new List<object>();
            foreach (var argument in expr.Arguments)
            {
                var value = Evaluate(argument);
                if (value == null)
                {
                    throw new JotRuntimeException(expr.Paren, "Argumento nulo não é permitido.");
                }
                arguments.Add(value);
            }

            if (callee is JotFunction function)
            {
                if (function is NativePrint)
                {
                    return function.Call(this, arguments) ?? throw new JotRuntimeException(expr.Paren, "Função retornou null.");
                }

                if (function.Declaration == null)
                {
                    throw new JotRuntimeException(expr.Paren, "Função inválida: declaração é nula.");
                }

                if (arguments.Count != function.Declaration.Parameters.Count)
                {
                    throw new JotRuntimeException(expr.Paren,
                        $"Esperado {function.Declaration.Parameters.Count} argumentos mas recebeu {arguments.Count}.");
                }

                return function.Call(this, arguments) ?? throw new JotRuntimeException(expr.Paren, "Função retornou null.");
            }

            throw new JotRuntimeException(expr.Paren, $"Não é possível chamar objeto do tipo {callee.GetType().Name}.");
        }

        public object VisitGetExpr(Get expr)
        {
            var obj = Evaluate(expr.Object);
            
            _logger?.LogDebug($"Acessando propriedade '{expr.Name.Lexeme}' do objeto tipo {obj?.GetType().Name ?? "null"}");

            if (obj == null)
            {
                throw new JotRuntimeException(expr.Name, "Não é possível acessar propriedades de null.");
            }

            if (obj is JotInstance instance)
            {
                return instance.Get(expr.Name);
            }

            throw new JotRuntimeException(expr.Name, "Só é possível acessar propriedades de instâncias de classes.");
        }

        public object VisitSetExpr(Set expr)
        {
            var obj = Evaluate(expr.Object);
            
            _logger?.LogDebug($"Definindo propriedade '{expr.Name.Lexeme}' do objeto tipo {obj?.GetType().Name ?? "null"}");

            if (obj == null)
            {
                throw new JotRuntimeException(expr.Name, "Não é possível definir propriedades em null.");
            }

            if (!(obj is JotInstance instance))
            {
                throw new JotRuntimeException(expr.Name, "Só é possível definir propriedades em instâncias de classes.");
            }

            var value = Evaluate(expr.Value);
            instance.Set(expr.Name, value ?? throw new JotRuntimeException(expr.Name, "Não é possível atribuir null."));
            return value;
        }

        public object VisitThisExpr(This expr)
        {
            return _environment.Get(expr.Keyword);
        }

        public object VisitBlockStmt(Block stmt)
        {
            ExecuteBlock(stmt.Statements, new Environment(_environment));
            return null;
        }

        public object VisitClassStmt(Class stmt)
        {
            _logger?.LogDebug($"Definindo classe: {stmt.Name.Lexeme}");
            
            _environment.Define(stmt.Name.Lexeme, null);

            var methods = new Dictionary<string, JotFunction>();
            foreach (var method in stmt.Methods)
            {
                _logger?.LogDebug($"Registrando método: {method.Name.Lexeme}");
                var function = new JotFunction(method, _environment, _logger);
                methods[method.Name.Lexeme] = function;
            }

            var properties = new Dictionary<string, object>();
            foreach (var property in stmt.Properties)
            {
                _logger?.LogDebug($"Registrando propriedade: {property.Name.Lexeme}");
                properties[property.Name.Lexeme] = null;
            }

            var @class = new JotClass(stmt.Name.Lexeme);
            @class.SetMethods(methods);
            @class.SetProperties(properties);

            _environment.Assign(stmt.Name, @class);
            return null;
        }

        public object VisitExpressionStmt(Expression stmt)
        {
            Evaluate(stmt.ExpressionValue);
            return null;
        }

        public object VisitFunctionStmt(Function stmt)
        {
            var function = new JotFunction(stmt, _environment, _logger);
            _environment.Define(stmt.Name.Lexeme, function);
            return null;
        }

        public object VisitIfStmt(If stmt)
        {
            if (IsTruthy(Evaluate(stmt.Condition)))
            {
                Execute(stmt.ThenBranch);
            }
            else if (stmt.ElseBranch != null)
            {
                Execute(stmt.ElseBranch);
            }
            return null;
        }

        public object VisitPropertyStmt(Property stmt)
        {
            return null;
        }

        public object VisitReturnStmt(Parser.Return stmt)
        {
            object value = null;
            if (stmt.Value != null)
            {
                value = Evaluate(stmt.Value);
            }

            throw new Return(value);
        }

        public object VisitVarStmt(Var stmt)
        {
            object value = null;
            if (stmt.Initializer != null)
            {
                value = Evaluate(stmt.Initializer);
                
                // Verificar tipo
                if (!CheckType(value, stmt.Type))
                {
                    throw new JotRuntimeException(stmt.Name, 
                        $"Tipo incompatível. Esperado '{stmt.Type.Lexeme}' mas recebeu valor do tipo '{value?.GetType().Name ?? "null"}'.");
                }
            }

            _environment.Define(stmt.Name.Lexeme, value);
            return null;
        }

        public object VisitWhileStmt(While stmt)
        {
            while (IsTruthy(Evaluate(stmt.Condition)))
            {
                Execute(stmt.Body);
            }
            return null;
        }

        public object VisitNewExpr(New expr)
        {
            _logger?.LogDebug($"Criando nova instância da classe {expr.Name.Lexeme}");

            try
            {
                var value = _environment.Get(expr.Name);
                if (!(value is JotClass @class))
                {
                    throw new JotRuntimeException(expr.Name, $"'{expr.Name.Lexeme}' não é uma classe.");
                }

                var instance = @class.CreateInstance();
                
                // Avaliar argumentos antes de chamar o construtor
                var arguments = new List<object>();
                foreach (var argument in expr.Arguments)
                {
                    var argValue = Evaluate(argument);
                    if (argValue == null)
                    {
                        throw new JotRuntimeException(expr.Name, "Argumento do construtor não pode ser nulo.");
                    }
                    arguments.Add(argValue);
                }

                // Se houver um construtor, chamá-lo com os argumentos avaliados
                var constructor = @class.FindMethod("constructor");
                if (constructor != null)
                {
                    _logger?.LogDebug("Chamando construtor com argumentos");
                    constructor.Call(this, arguments);
                }
                else if (expr.Arguments.Count > 0)
                {
                    throw new JotRuntimeException(expr.Name, 
                        "Tentativa de chamar construtor com argumentos em uma classe sem construtor.");
                }

                return instance;
            }
            catch (Exception ex) when (!(ex is JotRuntimeException))
            {
                _logger?.LogError($"Erro ao criar instância de {expr.Name.Lexeme}: {ex.Message}");
                _logger?.LogError($"Stack trace: {ex.StackTrace}");
                throw new JotRuntimeException(expr.Name, $"Erro ao criar instância: {ex.Message}");
            }
        }

        private object? Evaluate(Expr? expr)
        {
            try
            {
                if (expr == null)
                {
                    _logger?.LogWarning("Tentativa de avaliar expressão nula");
                    return null;
                }

                _logger?.LogDebug($"Avaliando expressão do tipo: {expr.GetType().Name}");
                var result = expr.Accept(this);
                _logger?.LogDebug($"Resultado da avaliação: {result ?? "null"}");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Erro ao avaliar expressão {expr.GetType().Name}: {ex.Message}");
                _logger?.LogError($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        private bool IsTruthy(object? obj)
        {
            if (obj == null) return false;
            if (obj is bool b) return b;
            return true;
        }

        private bool IsEqual(object? a, object? b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;
            return a.Equals(b);
        }

        private void CheckNumberOperand(Token @operator, object? operand)
        {
            if (operand is double) return;
            throw new JotRuntimeException(@operator, "Operando deve ser um número.");
        }

        private void CheckNumberOperands(Token @operator, object? left, object? right)
        {
            if (left is double && right is double) return;
            throw new JotRuntimeException(@operator, "Operandos devem ser números.");
        }

        public bool CheckType(object? value, Token typeToken)
        {
            if (value == null) return true; // null é válido para qualquer tipo

            switch (typeToken.Type)
            {
                case TokenType.INT:
                    return value is int;
                case TokenType.FLOAT:
                    return value is float || value is double;
                case TokenType.BOOL:
                    return value is bool;
                case TokenType.STRING:
                case TokenType.IDENTIFIER when typeToken.Lexeme.ToLower() == "string":
                    return value is string;
                case TokenType.IDENTIFIER: // Para tipos de classe
                    if (value is JotInstance instance)
                    {
                        return instance.Class.Name == typeToken.Lexeme;
                    }
                    return false;
                default:
                    return false;
            }
        }
    }

    public class NativePrint : JotFunction
    {
        private readonly ILogger? _logger;

        public NativePrint(ILogger? logger = null) : base(null, null)
        {
            _logger = logger;
        }

        public override object Call(JotInterpreter interpreter, List<object> arguments)
        {
            try
            {
                if (arguments == null || arguments.Count == 0)
                {
                    _logger?.LogWarning("Chamada de print sem argumentos");
                    return null;
                }

                var value = arguments[0];
                _logger?.LogDebug($"Imprimindo valor: {value ?? "null"}");
                Console.WriteLine(value);
                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Erro ao executar print: {ex.Message}");
                _logger?.LogError($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public override string ToString() => "<native fn: print>";
    }
} 
