using System.Collections.Generic;

namespace Compiler
{
    public sealed class UnaryExpressionSyntax : ExpressionSyntax
    {
        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
        public override IEnumerable<SyntaxNode> GetChildren() { yield return OperatorToken; 
                                                                yield return Right; }

        public SyntaxToken NumberToken { get; }
        public ExpressionSyntax Left { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Right { get; }

        public UnaryExpressionSyntax(SyntaxToken operatorToken, ExpressionSyntax right)
        {
            OperatorToken = operatorToken;
            Right = right;
        }
    }
}