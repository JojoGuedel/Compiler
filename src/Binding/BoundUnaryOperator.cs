using System;

namespace Compiler
{
    internal sealed class BoundUnaryOperator
    {
        public SyntaxKind SyntaxKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type RightType { get; }
        public Type ResultType { get; }

        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type rightType) : this(syntaxKind, kind, rightType, rightType)
        {
        }

        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type rightType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            RightType = rightType;
            ResultType = resultType;
        }

        private static BoundUnaryOperator[] _operators = 
        {
            new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.BangTokenToken, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),
        };

        public static BoundUnaryOperator Bind(SyntaxKind syntaxKind, Type rightType)
        {
            foreach (BoundUnaryOperator boundUnaryOperator in _operators) 
            {
                if (boundUnaryOperator.SyntaxKind == syntaxKind && boundUnaryOperator.RightType == rightType) return boundUnaryOperator;
            }

            return null;
        }
    }
}