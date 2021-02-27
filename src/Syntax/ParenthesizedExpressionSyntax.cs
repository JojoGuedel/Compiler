using System.Collections.Generic;

namespace Compiler
{
    sealed class ParenthesizedExpressionSyntax : ExpressionSyntax
    {
        public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;
        public override IEnumerable<SyntaxNode> GetChildren() { yield return OpenBracketToken; 
                                                                yield return Expression; 
                                                                yield return CloseBracketToken;}

        public SyntaxToken OpenBracketToken { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken CloseBracketToken { get; }

        public ParenthesizedExpressionSyntax(SyntaxToken openBracketToken, ExpressionSyntax expression, SyntaxToken closeBracketToken)
        {
            OpenBracketToken = openBracketToken;
            Expression = expression;
            CloseBracketToken = closeBracketToken;
        }
    }
}