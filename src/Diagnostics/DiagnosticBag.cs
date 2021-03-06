using System;
using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
    public sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Report (TextSpan textSpan, string message)
        {
            Diagnostic diagnostic = new Diagnostic(textSpan, message);
            _diagnostics.Add(diagnostic);
        }

        public void Report(object textSpan, string v)
        {
            throw new NotImplementedException();
        }

        public void ReportInvalidNumber(TextSpan textSpan, string tokenClearText, Type type)
        {
            string message = $"[Error] the number '{textSpan}' cannot be represented by an {type}";
            Report(textSpan, message);
        }

        public void ReportInvalidChar(TextSpan textSpan, char character)
        {
            string message = $"[Error] invalid char: '{character}'";
            Report(textSpan, message);
        }

        public void ReportUnexpectedToken(SyntaxToken snytaxToken, SyntaxKind expectedKind)
        {
            string message = $"[Error] unexpected token <{snytaxToken.Kind}>, expected <{expectedKind}>";
            Report(snytaxToken.TextSpan, message);
        }

        public void ReportUndefinedUnaryOperator(SyntaxToken operatorToken, Type type)
        {
            string message = $"[Error] Unary operator '{operatorToken.ClearText}' is not defined for type {type}";
            Report(operatorToken.TextSpan, message);
        }

        public void ReportUndefinedBinaryOperator(SyntaxToken operatorToken, Type leftType, Type rightType)
        {
            string message = $"[Error] Binary operator '{operatorToken.ClearText}' is not defined for type {leftType}, {rightType}";
            Report(operatorToken.TextSpan, message);
        }

        public void AddRange(DiagnosticBag diagnostics)
        {
            _diagnostics.AddRange(diagnostics);
        }
    }
}