using System;

namespace Compiler
{
    internal class Evaluator
    {
        private BoundExpression _Root { get; }

        public Evaluator(BoundExpression root)
        {
            _Root = root;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_Root);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            if (node is BoundLiteralExpression n) return n.Value;
            if (node is BoundUnaryExpression u) 
            {
                int right = (int) EvaluateExpression(u.Right);

                switch(u.UnaryOperatorKind)
                {
                    case BoundUnaryOperatorKind.Identity: return right;
                    case BoundUnaryOperatorKind.Negation: return -right;
                    default: throw new Exception($"Unexpected unary operatorkind {u.UnaryOperatorKind}");
                }
            }
            if (node is BoundBinaryExpression b)
            {
                int left = (int) EvaluateExpression(b.Left);
                int right = (int) EvaluateExpression(b.Right);

                switch (b.BinaryOperatorKind)
                {
                    case BoundBinaryOperatorKind.Addition: return left + right;
                    case BoundBinaryOperatorKind.Subtraction: return left - right;
                    case BoundBinaryOperatorKind.Multiplication: return left * right;
                    case BoundBinaryOperatorKind.Division: return left / right;
                    default: throw new Exception($"Unexpected binary operatorkind {b.BinaryOperatorKind}");
                }
            }

            throw new Exception($"Unexprected node {node.Kind}");
        }
    }
}