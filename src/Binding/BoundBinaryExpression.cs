using System;

namespace Compiler
{
    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
        public override Type Type => Left.Type;

        public BoundExpression Left { get; }
        public BoundBinaryOperator BinaryOperator { get; }
        public BoundExpression Right { get; }


        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperator binaryOperator, BoundExpression right)
        {
            Left = left;
            BinaryOperator = binaryOperator;
            Right = right;
        }
    }
}