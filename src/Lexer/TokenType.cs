namespace JotLang.Lexer
{
    public enum TokenType
    {
        // Tokens de um caractere
        LEFT_PAREN, RIGHT_PAREN,
        LEFT_BRACE, RIGHT_BRACE,
        COMMA, DOT, MINUS, PLUS,
        SEMICOLON, SLASH, STAR,
        COLON, AT,

        // Tokens de um ou dois caracteres
        BANG, BANG_EQUAL,
        EQUAL, EQUAL_EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,
        NOT,

        // Literais
        IDENTIFIER,
        STRING, STRING_LITERAL,
        NUMBER,
        INTEGER_LITERAL,
        FLOAT_LITERAL,
        BOOL_LITERAL,
        TRUE, FALSE, NULL,

        // Tipos
        INT, FLOAT, BOOL, VOID,

        // Palavras-chave
        AND, CLASS, ELSE, FN,
        FOR, IF, OR, PRINT,
        RETURN, SUPER, THIS,
        VAR, WHILE, PROP, NEW,

        // Especiais
        ERROR,
        EOF
    }
} 
