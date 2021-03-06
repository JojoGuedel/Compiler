using System;

namespace Compiler
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => Right.Type;

        public BoundUnaryOperator UnaryOperator { get; }
        public BoundExpression Right { get; }


        public BoundUnaryExpression(BoundUnaryOperator unaryOperator, BoundExpression right)
        {
            UnaryOperator = unaryOperator;
            Right = right;
        }
    }
}