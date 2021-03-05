using System.Collections.Generic;

namespace Compiler
{
    internal sealed class Parser
    {
        private int _Position;

        private SyntaxToken _CurrentToken { get => _PeekToken(0); }
        private SyntaxToken _PeekToken(int offset) => _Position + offset < _Tokens.Count? _Tokens[_Position + offset] : _Tokens[_Tokens.Count - 1];
        private SyntaxToken _MatchToken(SyntaxKind kind) 
        {
            if (_CurrentToken.Kind == kind) return _NextToken();

            _Diagnostics.Add(new DiagnosticMessage($"[Error] unexpected token <{_CurrentToken.Kind}>, expected <{kind}>"));
            return new SyntaxToken(kind, null, null);
        }
        private SyntaxToken _NextToken()
        {
            SyntaxToken current = _CurrentToken;
            _Position++;
            return current;
        }

        private List<DiagnosticMessage> _Diagnostics;
        public IEnumerable<DiagnosticMessage> Diagnostics => _Diagnostics;

        private List<SyntaxToken> _Tokens;
        private Lexer _Lexer;

        public Parser(string strInput) 
        {
            _Lexer = new Lexer(strInput);
            _Tokens = _Lexer.LexString();

            _Diagnostics = new List<DiagnosticMessage>();
            _Diagnostics.AddRange(_Lexer.Diagnostics);

            //foreach(SyntaxToken token in _Tokens) Console.WriteLine($"{token.Kind}({token.ClearText}): {token.Value}");
        }

        public SyntaxTree Parse()
        {
            ExpressionSyntax expression = _ParseExpression();
            SyntaxToken endOfFileToken = _MatchToken(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(_Diagnostics, expression, endOfFileToken);
        }

        private ExpressionSyntax _ParseExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;
            int unaryOperatorPrecedence = _CurrentToken.Kind.GetUnaryOperatorPrecedence();

            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                SyntaxToken operatorToken = _NextToken();
                ExpressionSyntax right = _ParseExpression(unaryOperatorPrecedence);
                left = new UnaryExpressionSyntax(operatorToken, right);
            }
            else left = ParsePrimaryExpression();

            while (true)
            {
                int precedence = _CurrentToken.Kind.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence) break;

                SyntaxToken operatorToken = _NextToken();
                ExpressionSyntax right = _ParseExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            switch (_CurrentToken.Kind)
            {
                case SyntaxKind.OpenBracketToken:
                    SyntaxToken openBracketToken = _NextToken();
                    ExpressionSyntax expression = _ParseExpression();
                    SyntaxToken closeBracketToken = _MatchToken(SyntaxKind.CloseBracketToken);
                    return new ParenthesizedExpressionSyntax(openBracketToken, expression, closeBracketToken);

                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                    bool value = _CurrentToken.Kind == SyntaxKind.TrueKeyword;
                    return new LiteralExpressionSyntax(_NextToken(), value);

                default:
                    SyntaxToken numberToken = _MatchToken(SyntaxKind.NumberToken);
                    return new LiteralExpressionSyntax(numberToken);
            }
        }
    }
}