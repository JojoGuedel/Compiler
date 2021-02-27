using System.Collections.Generic;

namespace Solver_01
{
    class Parser
    {
        private int _Position;

        private SyntaxToken _CurrentToken { get => _PeekToken(0); }
        private SyntaxToken _PeekToken(int offset) => _Position + offset < _Tokens.Count? _Tokens[_Position + offset] : _Tokens[_Tokens.Count - 1];
        private SyntaxToken _Match(SyntaxKind kind) =>_CurrentToken.Kind == kind? NextToken() : new SyntaxToken(kind, null, null);

        private List<SyntaxToken> _Tokens;
        private Lexer _Lexer;

        public Parser(string strInput) 
        {
            _Lexer = new Lexer(strInput);
            _Tokens = _Lexer.LexString();

            //foreach(SyntaxToken token in _Tokens) Console.WriteLine($"{token.Kind}({token.ClearText}): {token.Value}");
        }

        private SyntaxToken NextToken()
        {
            SyntaxToken current = _CurrentToken;
            _Position++;
            return current;
        }

        public ExpressionSyntax Parse() 
        {
            ExpressionSyntax left = ParsePrimaryExpression();

            while (_CurrentToken.Kind == SyntaxKind.PlusToken
                || _CurrentToken.Kind == SyntaxKind.MinusToken)
            {
                SyntaxToken operatorToken = NextToken();
                ExpressionSyntax right = ParsePrimaryExpression();

                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            SyntaxToken numberToken = _Match(SyntaxKind.NumberToken);
            return new NumberExpressionSyntax(numberToken);
        }
    }
}