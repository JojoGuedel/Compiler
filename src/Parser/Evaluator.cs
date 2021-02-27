using System;

namespace Compiler
{
    class Evaluator
    {
        private ExpressionSyntax _Root { get; }

        public Evaluator(ExpressionSyntax root)
        {
            _Root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(_Root);
        }

        private int EvaluateExpression(ExpressionSyntax node)
        {
            if (node is NumberExpressionSyntax n) return (int)n.NumberToken.Value;
            if (node is BinaryExpressionSyntax b)
            {
                int left = EvaluateExpression(b.Left);
                int right = EvaluateExpression(b.Right);

                switch (b.OperatorToken.Kind)
                {
                    case SyntaxKind.PlusToken: return left + right;
                    case SyntaxKind.MinusToken: return left - right;
                    case SyntaxKind.StarToken: return left * right;
                    case SyntaxKind.SlashToken: return left / right;
                    default: throw new Exception($"Unexpected binary operatorkind {b.OperatorToken.Kind}");
                }
            }
            
            if (node is ParenthesizedExpressionSyntax p) return EvaluateExpression(p.Expression);

            throw new Exception($"Unexprected node {node.Kind}");
        }
    }
}