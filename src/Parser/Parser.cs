using System.Collections.Generic;

namespace Compiler
{
    class Parser
    {
        private int _Position;

        private SyntaxToken _CurrentToken { get => _PeekToken(0); }
        private SyntaxToken _PeekToken(int offset) => _Position + offset < _Tokens.Count? _Tokens[_Position + offset] : _Tokens[_Tokens.Count - 1];
        private SyntaxToken _Match(SyntaxKind kind) 
        {
            if (_CurrentToken.Kind == kind) return NextToken();

            _Diagnostics.Add(new DiagnosticMessage($"[Error] unexpected token <{_CurrentToken.Kind}>, expected <{kind}>"));
            return new SyntaxToken(kind, null, null);
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

        private SyntaxToken NextToken()
        {
            SyntaxToken current = _CurrentToken;
            _Position++;
            return current;
        }

        public SyntaxTree Parse()
        {
            ExpressionSyntax expression = _ParseTerm();
            SyntaxToken endOfFileToken = _Match(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(_Diagnostics, expression, endOfFileToken);
        }

        private ExpressionSyntax _ParseTerm()
        {
            ExpressionSyntax left = _ParseFactor();

            while (_CurrentToken.Kind == SyntaxKind.PlusToken
                || _CurrentToken.Kind == SyntaxKind.MinusToken)
            {
                SyntaxToken operatorToken = NextToken();
                ExpressionSyntax right = _ParseFactor();

                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax _ParseFactor()
        {
            ExpressionSyntax left = ParsePrimaryExpression();

            while (_CurrentToken.Kind == SyntaxKind.StarToken
                || _CurrentToken.Kind == SyntaxKind.SlashToken)
            {
                SyntaxToken operatorToken = NextToken();
                ExpressionSyntax right = ParsePrimaryExpression();

                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            if (_CurrentToken.Kind == SyntaxKind.OpenBracketToken)
            {
                SyntaxToken openBracketToken = NextToken();
                ExpressionSyntax expression = _ParseTerm();
                SyntaxToken closeBracketToken = _Match(SyntaxKind.CloseBracketToken);
                return new ParenthesizedExpressionSyntax(openBracketToken, expression, closeBracketToken);
            }
            else
            {
                SyntaxToken numberToken = _Match(SyntaxKind.NumberToken);
                return new NumberExpressionSyntax(numberToken);
            }
        }
    }
}