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
            BoundUnaryOperatorKind? boundOperatorKind = _BindUnaryOperatorKind(syntax.OperatorToken.Kind, boundRight.Type);

            if (boundOperatorKind == null)
            {
                _Diagnostics.Add(new DiagnosticMessage($"Unary operator '{syntax.OperatorToken.ClearText}' is not defined for type {boundRight.Type}"));
                return boundRight;
            }

            return new BoundUnaryExpression(boundOperatorKind.Value, boundRight);
        }

        private BoundUnaryOperatorKind? _BindUnaryOperatorKind(SyntaxKind kind, Type type)
        {
            if (type != typeof(int)) return null;
            Console.WriteLine(type.ToString());

            switch(kind)
            {
                case SyntaxKind.PlusToken:
                    return BoundUnaryOperatorKind.Identity;
                case SyntaxKind.MinusToken:
                    return BoundUnaryOperatorKind.Negation;
                default:
                    throw new Exception($"Unexpected unary operator {kind}");
            }
        }

        private BoundExpression _BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            BoundExpression boundLeft = BindExpression(syntax.Left);
            BoundExpression boundRight = BindExpression(syntax.Right);
            BoundBinaryOperatorKind? boundOperatorToken = BindBinaryOperatorKind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);
            
            if (boundOperatorToken == null)
            {
                _Diagnostics.Add(new DiagnosticMessage($"Binary operator '{syntax.OperatorToken.ClearText}' is not defined for type {boundLeft.Type}, {boundRight.Type}"));
                return boundRight;
            }
            
            return new BoundBinaryExpression(boundLeft, boundOperatorToken.Value, boundRight);
        }

        private BoundBinaryOperatorKind? BindBinaryOperatorKind(SyntaxKind kind, Type leftType, Type rightType)
        {
            if (leftType != typeof(int) || rightType != typeof(int)) return null;

            switch(kind)
            {
                case SyntaxKind.PlusToken:
                    return BoundBinaryOperatorKind.Addition;
                case SyntaxKind.MinusToken:
                    return BoundBinaryOperatorKind.Subtraction;
                case SyntaxKind.StarToken:
                    return BoundBinaryOperatorKind.Multiplication;
                case SyntaxKind.SlashToken:
                    return BoundBinaryOperatorKind.Division;
                default:
                    throw new Exception($"Unexpected binary operator {kind}");
            }
        }

        
        private BoundExpression _BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }
    }
}