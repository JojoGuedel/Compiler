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
                object right = EvaluateExpression(u.Right);

                switch(u.UnaryOperator.Kind)
                {
                    case BoundUnaryOperatorKind.Identity:
                        return (int) right;
                    case BoundUnaryOperatorKind.Negation: 
                        return  -(int) right;
                    case BoundUnaryOperatorKind.LogicalNegation: 
                        return  ! (bool) right;
                    default: throw new Exception($"Unexpected unary operatorkind {u.UnaryOperator}");
                }
            }
            if (node is BoundBinaryExpression b)
            {
                object left = EvaluateExpression(b.Left);
                object right = EvaluateExpression(b.Right);

                switch (b.BinaryOperator.Kind)
                {
                    case BoundBinaryOperatorKind.Addition: 
                        return (int)left + (int)right;
                    case BoundBinaryOperatorKind.Subtraction: 
                        return (int)left - (int)right;
                    case BoundBinaryOperatorKind.Multiplication: 
                        return (int)left * (int)right;
                    case BoundBinaryOperatorKind.Division: 
                        return (int)left / (int)right;
                    case BoundBinaryOperatorKind.LogicalAnd:
                        return (bool)left && (bool) right;
                    case BoundBinaryOperatorKind.LogicalOr:
                        return (bool)left || (bool) right;
                    case BoundBinaryOperatorKind.Equals:
                        return Equals(left, right);
                    case BoundBinaryOperatorKind.NotEquals:
                        return !Equals(left, right);
                    default: throw new Exception($"Unexpected binary operatorkind {b.BinaryOperator}");
                }
            }

            throw new Exception($"Unexprected node {node.Kind}");
        }
    }
}