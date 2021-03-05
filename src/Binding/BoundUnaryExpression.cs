using System;

namespace Compiler
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => Right.Type;

        public BoundUnaryOperatorKind UnaryOperatorKind { get; }
        public BoundExpression Right { get; }


        public BoundUnaryExpression(BoundUnaryOperatorKind unaryOperatorKind, BoundExpression right)
        {
            UnaryOperatorKind = unaryOperatorKind;
            Right = right;
        }
    }
}