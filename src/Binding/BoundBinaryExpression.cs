using System;

namespace Compiler
{
    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
        public override Type Type => Left.Type;

        public BoundExpression Left { get; }
        public BoundBinaryOperatorKind BinaryOperatorKind { get; }
        public BoundExpression Right { get; }


        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperatorKind binaryOperatorKind, BoundExpression right)
        {
            Left = left;
            BinaryOperatorKind = binaryOperatorKind;
            Right = right;
        }
    }
}