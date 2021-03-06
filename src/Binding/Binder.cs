using System;
using System.Collections.Generic;

namespace Compiler
{
    internal sealed class Binder
    {
        private List<DiagnosticMessage> _Diagnostics = new List<DiagnosticMessage>();
        public IEnumerable<DiagnosticMessage> Diagnostics => _Diagnostics;

        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch(syntax.Kind)
            {
                case SyntaxKind.LiteralExpression:
                    return _BindLiteralExpression((LiteralExpressionSyntax)syntax);
                case SyntaxKind.UnaryExpression:
                    return _BindUnaryExpression((UnaryExpressionSyntax) syntax);
                case SyntaxKind.BinaryExpression:
                    return _BindBinaryExpression((BinaryExpressionSyntax) syntax);
                case SyntaxKind.ParenthesizedExpression:
                    return _BindParenthesizedExpression((ParenthesizedExpressionSyntax) syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        private BoundExpression _BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            object value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }

        private BoundExpression _BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            BoundExpression boundRight = BindExpression(syntax.Right);
            BoundUnaryOperator boundUnaryOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundRight.Type);

            if (boundUnaryOperator == null)
            {
                _Diagnostics.Add(new DiagnosticMessage($"Unary operator '{syntax.OperatorToken.ClearText}' is not defined for type {boundRight.Type}"));
                return boundRight;
            }

            return new BoundUnaryExpression(boundUnaryOperator, boundRight);
        }

        private BoundExpression _BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            BoundExpression boundLeft = BindExpression(syntax.Left);
            BoundExpression boundRight = BindExpression(syntax.Right);
            BoundBinaryOperator boundBinaryOperator =  BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);
            
            if (boundBinaryOperator == null)
            {
                _Diagnostics.Add(new DiagnosticMessage($"Binary operator '{syntax.OperatorToken.ClearText}' is not defined for type {boundLeft.Type}, {boundRight.Type}"));
                return boundRight;
            }
            
            return new BoundBinaryExpression(boundLeft, boundBinaryOperator, boundRight);
        }
        
        private BoundExpression _BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }
    }
}