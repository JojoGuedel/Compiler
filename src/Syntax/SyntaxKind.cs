namespace Compiler
{
    public enum SyntaxKind
    {
        InvalidCharToken,
        EndOfFileToken,
        WhitespaceToken,
        NumberToken,
        NameToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenBracketToken,
        CloseBracketToken,
        OpenCurlyBracketToken,
        CloseCurlyBracketToken,
        OpenAngleBracketToken,
        CloseAngleBracketToken,

        LiteralExpression,
        BinaryExpression,
        ParenthesizedExpression,
        UnaryExpression,
    }
}