using System;
using System.Collections.Generic;
using JotLang.Lexer;

namespace JotLang.Parser
{
    public abstract class Expr
    {
        public abstract T Accept<T>(IExprVisitor<T> visitor);
    }

    public abstract class Stmt
    {
        public abstract T Accept<T>(IStmtVisitor<T> visitor);
    }

    public interface IExprVisitor<T>
    {
        T VisitBinaryExpr(Binary expr);
        T VisitGroupingExpr(Grouping expr);
        T VisitLiteralExpr(Literal expr);
        T VisitUnaryExpr(Unary expr);
        T VisitVariableExpr(Variable expr);
        T VisitAssignExpr(Assign expr);
        T VisitCallExpr(Call expr);
        T VisitGetExpr(Get expr);
        T VisitSetExpr(Set expr);
        T VisitThisExpr(This expr);
        T VisitNewExpr(New expr);
    }

    public interface IStmtVisitor<T>
    {
        T VisitBlockStmt(Block stmt);
        T VisitClassStmt(Class stmt);
        T VisitExpressionStmt(Expression stmt);
        T VisitFunctionStmt(Function stmt);
        T VisitIfStmt(If stmt);
        T VisitPropertyStmt(Property stmt);
        T VisitReturnStmt(Return stmt);
        T VisitVarStmt(Var stmt);
        T VisitWhileStmt(While stmt);
    }

    public class Binary : Expr
    {
        public readonly Expr Left;
        public readonly Token Operator;
        public readonly Expr Right;

        public Binary(Expr left, Token @operator, Expr right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }
    }

    public class Grouping : Expr
    {
        public readonly Expr Expression;

        public Grouping(Expr expression)
        {
            Expression = expression;
        }

        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitGroupingExpr(this);
        }
    }

    public class Literal : Expr
    {
        public readonly object Value;

        public Literal(object value)
        {
            Value = value;
        }

        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }
    }

    public class Unary : Expr
    {
        public readonly Token Operator;
        public readonly Expr Right;

        public Unary(Token @operator, Expr right)
        {
            Operator = @operator;
            Right = right;
        }

        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    }

    public class Variable : Expr
    {
        public readonly Token Name;

        public Variable(Token name)
        {
            Name = name;
        }

        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitVariableExpr(this);
        }
    }

    public class Assign : Expr
    {
        public readonly Token Name;
        public readonly Expr Value;

        public Assign(Token name, Expr value)
        {
            Name = name;
            Value = value;
        }

        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitAssignExpr(this);
        }
    }

    public class Call : Expr
    {
        public readonly Expr Callee;
        public readonly Token Paren;
        public readonly List<Expr> Arguments;

        public Call(Expr callee, Token paren, List<Expr> arguments)
        {
            Callee = callee;
            Paren = paren;
            Arguments = arguments;
        }

        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitCallExpr(this);
        }
    }

    public class Get : Expr
    {
        public readonly Expr Object;
        public readonly Token Name;

        public Get(Expr obj, Token name)
        {
            Object = obj;
            Name = name;
        }

        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitGetExpr(this);
        }
    }

    public class Set : Expr
    {
        public readonly Expr Object;
        public readonly Token Name;
        public readonly Expr Value;

        public Set(Expr obj, Token name, Expr value)
        {
            Object = obj;
            Name = name;
            Value = value;
        }

        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitSetExpr(this);
        }
    }

    public class This : Expr
    {
        public readonly Token Keyword;

        public This(Token keyword)
        {
            Keyword = keyword;
        }

        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitThisExpr(this);
        }
    }

    public class New : Expr
    {
        public readonly Token Name;
        public readonly List<Expr> Arguments;

        public New(Token name, List<Expr> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitNewExpr(this);
        }
    }

    public class Block : Stmt
    {
        public readonly List<Stmt> Statements;

        public Block(List<Stmt> statements)
        {
            Statements = statements;
        }

        public override T Accept<T>(IStmtVisitor<T> visitor)
        {
            return visitor.VisitBlockStmt(this);
        }
    }

    public class Class : Stmt
    {
        public readonly Token Name;
        public readonly List<Property> Properties;
        public readonly List<Function> Methods;

        public Class(Token name, List<Property> properties, List<Function> methods)
        {
            Name = name;
            Properties = properties;
            Methods = methods;
        }

        public override T Accept<T>(IStmtVisitor<T> visitor)
        {
            return visitor.VisitClassStmt(this);
        }
    }

    public class Expression : Stmt
    {
        public readonly Expr ExpressionValue;

        public Expression(Expr expression)
        {
            ExpressionValue = expression;
        }

        public override T Accept<T>(IStmtVisitor<T> visitor)
        {
            return visitor.VisitExpressionStmt(this);
        }
    }

    public class Function : Stmt
    {
        public readonly Token Name;
        public readonly List<Token> Parameters;
        public readonly List<Token> ParameterTypes;
        public readonly Token ReturnType;
        public readonly List<Stmt> Body;

        public Function(Token name, List<Token> parameters, List<Token> parameterTypes, Token returnType, List<Stmt> body)
        {
            Name = name;
            Parameters = parameters;
            ParameterTypes = parameterTypes;
            ReturnType = returnType;
            Body = body;
        }

        public override T Accept<T>(IStmtVisitor<T> visitor)
        {
            return visitor.VisitFunctionStmt(this);
        }
    }

    public class If : Stmt
    {
        public readonly Expr Condition;
        public readonly Stmt ThenBranch;
        public readonly Stmt ElseBranch;

        public If(Expr condition, Stmt thenBranch, Stmt elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }

        public override T Accept<T>(IStmtVisitor<T> visitor)
        {
            return visitor.VisitIfStmt(this);
        }
    }

    public class Property : Stmt
    {
        public readonly Token Type;
        public readonly Token Name;

        public Property(Token type, Token name)
        {
            Type = type;
            Name = name;
        }

        public override T Accept<T>(IStmtVisitor<T> visitor)
        {
            return visitor.VisitPropertyStmt(this);
        }
    }

    public class Return : Stmt
    {
        public readonly Token Keyword;
        public readonly Expr Value;

        public Return(Token keyword, Expr value)
        {
            Keyword = keyword;
            Value = value;
        }

        public override T Accept<T>(IStmtVisitor<T> visitor)
        {
            return visitor.VisitReturnStmt(this);
        }
    }

    public class Var : Stmt
    {
        public readonly Token Name;
        public readonly Token Type;
        public readonly Expr Initializer;

        public Var(Token name, Token type, Expr initializer)
        {
            Name = name;
            Type = type;
            Initializer = initializer;
        }

        public override T Accept<T>(IStmtVisitor<T> visitor)
        {
            return visitor.VisitVarStmt(this);
        }
    }

    public class While : Stmt
    {
        public readonly Expr Condition;
        public readonly Stmt Body;

        public While(Expr condition, Stmt body)
        {
            Condition = condition;
            Body = body;
        }

        public override T Accept<T>(IStmtVisitor<T> visitor)
        {
            return visitor.VisitWhileStmt(this);
        }
    }

    public class JotParser
    {
        private readonly List<Token> _tokens;
        private int _current = 0;

        public JotParser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public List<Stmt> Parse()
        {
            var statements = new List<Stmt>();
            while (!IsAtEnd())
            {
                statements.Add(Declaration());
            }
            return statements;
        }

        private Stmt Declaration()
        {
            try
            {
                if (Match(TokenType.CLASS)) return ClassDeclaration();
                if (Match(TokenType.FN)) return Function();
                if (Match(TokenType.VAR)) return VarDeclaration();
                if (Match(TokenType.PROP)) return PropertyDeclaration();

                return Statement();
            }
            catch (Exception)
            {
                Synchronize();
                return null;
            }
        }

        private Stmt ClassDeclaration()
        {
            var name = Consume(TokenType.IDENTIFIER, "Esperado nome da classe.");
            Consume(TokenType.LEFT_BRACE, "Esperado '{' antes do corpo da classe.");

            var properties = new List<Property>();
            var methods = new List<Function>();

            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                if (Match(TokenType.PROP))
                {
                    properties.Add((Property)PropertyDeclaration());
                }
                else if (Match(TokenType.FN))
                {
                    methods.Add((Function)Function());
                }
            }

            Consume(TokenType.RIGHT_BRACE, "Esperado '}' após o corpo da classe.");
            return new Class(name, properties, methods);
        }

        private Stmt PropertyDeclaration()
        {
            var type = Consume(TokenType.IDENTIFIER, "Esperado tipo da propriedade.");
            var name = Consume(TokenType.IDENTIFIER, "Esperado nome da propriedade.");
            return new Property(type, name);
        }

        private Stmt VarDeclaration()
        {
            var name = Consume(TokenType.IDENTIFIER, "Esperado nome da variável.");
            Consume(TokenType.COLON, "Esperado ':' após nome da variável.");
            var type = Consume(TokenType.IDENTIFIER, "Esperado tipo da variável.");

            Expr initializer = null;
            if (Match(TokenType.EQUAL))
            {
                initializer = Expression();
            }

            return new Var(name, type, initializer);
        }

        private Stmt Function()
        {
            var name = Consume(TokenType.IDENTIFIER, "Esperado nome da função.");
            Consume(TokenType.LEFT_PAREN, "Esperado '(' após nome da função.");

            var parameters = new List<Token>();
            var parameterTypes = new List<Token>();

            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    parameters.Add(Consume(TokenType.IDENTIFIER, "Esperado nome do parâmetro."));
                    Consume(TokenType.COLON, "Esperado ':' após nome do parâmetro.");
                    parameterTypes.Add(Consume(TokenType.IDENTIFIER, "Esperado tipo do parâmetro."));
                } while (Match(TokenType.COMMA));
            }

            Consume(TokenType.RIGHT_PAREN, "Esperado ')' após parâmetros.");
            Consume(TokenType.COLON, "Esperado ':' após parâmetros.");
            var returnType = Consume(TokenType.IDENTIFIER, "Esperado tipo de retorno da função.");

            Consume(TokenType.LEFT_BRACE, "Esperado '{' antes do corpo da função.");
            var body = Block();
            return new Function(name, parameters, parameterTypes, returnType, body);
        }

        private List<Stmt> Block()
        {
            var statements = new List<Stmt>();

            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                statements.Add(Declaration());
            }

            Consume(TokenType.RIGHT_BRACE, "Esperado '}' após bloco.");
            return statements;
        }

        private Stmt Statement()
        {
            if (Match(TokenType.IF)) return IfStatement();
            if (Match(TokenType.RETURN)) return ReturnStatement();
            if (Match(TokenType.WHILE)) return WhileStatement();
            if (Match(TokenType.LEFT_BRACE)) return new Block(Block());

            return ExpressionStatement();
        }

        private Stmt IfStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Esperado '(' após 'if'.");
            var condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Esperado ')' após condição do if.");

            var thenBranch = Statement();
            Stmt elseBranch = null;
            if (Match(TokenType.ELSE))
            {
                elseBranch = Statement();
            }

            return new If(condition, thenBranch, elseBranch);
        }

        private Stmt ReturnStatement()
        {
            var keyword = Previous();
            Expr value = null;
            if (!Check(TokenType.SEMICOLON))
            {
                value = Expression();
            }

            return new Return(keyword, value);
        }

        private Stmt WhileStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Esperado '(' após 'while'.");
            var condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Esperado ')' após condição do while.");
            var body = Statement();

            return new While(condition, body);
        }

        private Stmt ExpressionStatement()
        {
            var expr = Expression();
            return new Expression(expr);
        }

        private Expr Expression()
        {
            return Assignment();
        }

        private Expr Assignment()
        {
            var expr = Or();

            if (Match(TokenType.EQUAL))
            {
                var equals = Previous();
                var value = Assignment();

                if (expr is Variable variable)
                {
                    return new Assign(variable.Name, value);
                }
                else if (expr is Get get)
                {
                    return new Set(get.Object, get.Name, value);
                }

                throw Error(equals, "Alvo de atribuição inválido.");
            }

            return expr;
        }

        private Expr Or()
        {
            var expr = And();

            while (Match(TokenType.OR))
            {
                var op = Previous();
                var right = And();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr And()
        {
            var expr = Equality();

            while (Match(TokenType.AND))
            {
                var op = Previous();
                var right = Equality();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Equality()
        {
            var expr = Comparison();

            while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                var op = Previous();
                var right = Comparison();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Comparison()
        {
            var expr = Term();

            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                var op = Previous();
                var right = Term();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Term()
        {
            var expr = Factor();

            while (Match(TokenType.MINUS, TokenType.PLUS))
            {
                var op = Previous();
                var right = Factor();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Factor()
        {
            var expr = Unary();

            while (Match(TokenType.SLASH, TokenType.STAR))
            {
                var op = Previous();
                var right = Unary();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Unary()
        {
            if (Match(TokenType.BANG, TokenType.MINUS))
            {
                var op = Previous();
                var right = Unary();
                return new Unary(op, right);
            }

            return Call();
        }

        private Expr Call()
        {
            var expr = Primary();

            while (true)
            {
                if (Match(TokenType.LEFT_PAREN))
                {
                    expr = FinishCall(expr);
                }
                else if (Match(TokenType.DOT))
                {
                    var name = Consume(TokenType.IDENTIFIER, "Esperado nome da propriedade após '.'.");
                    expr = new Get(expr, name);
                }
                else
                {
                    break;
                }
            }

            return expr;
        }

        private Expr FinishCall(Expr callee)
        {
            var arguments = new List<Expr>();
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    arguments.Add(Expression());
                } while (Match(TokenType.COMMA));
            }

            var paren = Consume(TokenType.RIGHT_PAREN, "Esperado ')' após argumentos.");

            return new Call(callee, paren, arguments);
        }

        private Expr Primary()
        {
            if (Match(TokenType.FALSE)) return new Literal(false);
            if (Match(TokenType.TRUE)) return new Literal(true);
            if (Match(TokenType.NULL)) return new Literal(null);
            if (Match(TokenType.INTEGER_LITERAL)) return new Literal(Previous().Literal);
            if (Match(TokenType.FLOAT_LITERAL)) return new Literal(Previous().Literal);
            if (Match(TokenType.STRING_LITERAL)) return new Literal(Previous().Literal);
            if (Match(TokenType.THIS)) return new This(Previous());
            if (Match(TokenType.NEW))
            {
                var name = Consume(TokenType.IDENTIFIER, "Esperado nome da classe após 'new'.");
                var arguments = new List<Expr>();

                if (Match(TokenType.LEFT_PAREN))
                {
                    if (!Check(TokenType.RIGHT_PAREN))
                    {
                        do
                        {
                            arguments.Add(Expression());
                        } while (Match(TokenType.COMMA));
                    }

                    Consume(TokenType.RIGHT_PAREN, "Esperado ')' após argumentos do construtor.");
                }

                return new New(name, arguments);
            }
            if (Match(TokenType.IDENTIFIER)) return new Variable(Previous());

            if (Match(TokenType.LEFT_PAREN))
            {
                var expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Esperado ')' após expressão.");
                return new Grouping(expr);
            }

            throw Error(Peek(), "Esperado expressão.");
        }

        private bool Match(params TokenType[] types)
        {
            foreach (var type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            throw Error(Peek(), message);
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) _current++;
            return Previous();
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        private Token Peek()
        {
            return _tokens[_current];
        }

        private Token Previous()
        {
            return _tokens[_current - 1];
        }

        private Exception Error(Token token, string message)
        {
            // TODO: Implementar sistema de erro mais robusto
            return new Exception($"Erro na linha {token.Line}: {message}");
        }

        private void Synchronize()
        {
            Advance();

            while (!IsAtEnd())
            {
                if (Previous().Type == TokenType.SEMICOLON) return;

                switch (Peek().Type)
                {
                    case TokenType.CLASS:
                    case TokenType.FN:
                    case TokenType.VAR:
                    case TokenType.FOR:
                    case TokenType.IF:
                    case TokenType.WHILE:
                    case TokenType.RETURN:
                        return;
                }

                Advance();
            }
        }
    }
} 
