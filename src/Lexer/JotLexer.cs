using System;
using System.Collections.Generic;
using System.Text;

namespace JotLang.Lexer
{
    public class JotLexerException : Exception
    {
        public int Line { get; }
        public string Character { get; }

        public JotLexerException(int line, string character, string message) 
            : base($"Erro na linha {line}: {message} - Caractere: '{character}'")
        {
            Line = line;
            Character = character;
        }
    }

    public class Token
    {
        public TokenType Type { get; }
        public string Lexeme { get; }
        public object Literal { get; }
        public int Line { get; }

        public Token(TokenType type, string lexeme, object literal, int line)
        {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
            Line = line;
        }

        public override string ToString() => $"{Type} {Lexeme} {Literal}";
    }

    public class JotLexer
    {
        private readonly string _source;
        private readonly List<Token> _tokens = new List<Token>();
        private int _start = 0;
        private int _current = 0;
        private int _line = 1;

        private static readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>
        {
            { "and", TokenType.AND },
            { "class", TokenType.CLASS },
            { "else", TokenType.ELSE },
            { "false", TokenType.FALSE },
            { "fn", TokenType.FN },
            { "for", TokenType.FOR },
            { "if", TokenType.IF },
            { "null", TokenType.NULL },
            { "or", TokenType.OR },
            { "print", TokenType.PRINT },
            { "return", TokenType.RETURN },
            { "super", TokenType.SUPER },
            { "this", TokenType.THIS },
            { "true", TokenType.TRUE },
            { "var", TokenType.VAR },
            { "while", TokenType.WHILE },
            { "new", TokenType.NEW },
            { "int", TokenType.INT },
            { "float", TokenType.FLOAT },
            { "bool", TokenType.BOOL },
            { "void", TokenType.VOID },
            { "prop", TokenType.PROP }
        };

        public JotLexer(string source)
        {
            _source = source;
        }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                try
                {
                    _start = _current;
                    ScanToken();
                }
                catch (Exception ex)
                {
                    if (ex is JotLexerException)
                        throw;
                    throw new JotLexerException(_line, _current < _source.Length ? _source[_current].ToString() : "EOF", 
                        "Erro ao analisar token: " + ex.Message);
                }
            }

            _tokens.Add(new Token(TokenType.EOF, "", null, _line));
            return _tokens;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '+': AddToken(TokenType.PLUS); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case ':': AddToken(TokenType.COLON); break;
                case '*': AddToken(TokenType.STAR); break;
                case '@': AddToken(TokenType.AT); break;
                case '!': AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.NOT); break;
                case '=': AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                case '&': if (Match('&')) AddToken(TokenType.AND); break;
                case '|': if (Match('|')) AddToken(TokenType.OR); break;
                case '/':
                    if (Match('/'))
                    {
                        // Comentário de linha única
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else if (Match('*'))
                    {
                        MultiLineComment();
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // Ignora espaços em branco
                    break;
                case '\n':
                    _line++;
                    break;
                case '"': String(); break;
                default:
                    if (IsDigit(c))
                    {
                        Number();
                    }
                    else if (IsAlpha(c))
                    {
                        Identifier();
                    }
                    else
                    {
                        throw new JotLexerException(_line, c.ToString(), 
                            "Caractere não suportado");
                    }
                    break;
            }
        }

        private void MultiLineComment()
        {
            while (!IsAtEnd())
            {
                if (Peek() == '*' && PeekNext() == '/')
                {
                    Advance(); // Consome *
                    Advance(); // Consome /
                    return;
                }
                else if (Peek() == '\n')
                {
                    _line++;
                }
                Advance();
            }

            throw new JotLexerException(_line, "*/", 
                "Comentário multilinha não foi fechado");
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek())) Advance();

            string text = _source.Substring(_start, _current - _start);
            TokenType type = _keywords.ContainsKey(text) ? _keywords[text] : TokenType.IDENTIFIER;

            if (type == TokenType.BOOL && (text == "true" || text == "false"))
            {
                AddToken(TokenType.BOOL_LITERAL, bool.Parse(text));
            }
            else
            {
                AddToken(type);
            }
        }

        private void Number()
        {
            while (IsDigit(Peek())) Advance();

            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Advance();
                while (IsDigit(Peek())) Advance();
                AddToken(TokenType.FLOAT_LITERAL, float.Parse(_source.Substring(_start, _current - _start)));
            }
            else
            {
                AddToken(TokenType.INTEGER_LITERAL, int.Parse(_source.Substring(_start, _current - _start)));
            }
        }

        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') _line++;
                Advance();
            }

            if (IsAtEnd())
            {
                throw new Exception($"String não terminada na linha {_line}");
            }

            Advance(); // Fecha as aspas

            string value = _source.Substring(_start + 1, _current - _start - 2);
            AddToken(TokenType.STRING_LITERAL, value);
        }

        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (_source[_current] != expected) return false;

            _current++;
            return true;
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[_current];
        }

        private char PeekNext()
        {
            if (_current + 1 >= _source.Length) return '\0';
            return _source[_current + 1];
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                    c == '_';
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private char Advance()
        {
            return _source[_current++];
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object literal)
        {
            string text = _source.Substring(_start, _current - _start);
            _tokens.Add(new Token(type, text, literal, _line));
        }

        private bool IsAtEnd()
        {
            return _current >= _source.Length;
        }
    }
} 
