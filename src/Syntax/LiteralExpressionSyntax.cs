using System.Collections.Generic;

namespace Compiler
{
    public sealed class LiteralExpressionSyntax : ExpressionSyntax
    {
        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;
        public override IEnumerable<SyntaxNode> GetChildren() { yield return LiteralToken; }

        public SyntaxToken LiteralToken { get; }

        public LiteralExpressionSyntax(SyntaxToken numberToken)
        {
            LiteralToken = numberToken;
        }
    }
}