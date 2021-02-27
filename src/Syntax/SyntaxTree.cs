using System.Collections.Generic;
using System.Linq;

namespace Compiler
{
    sealed class SyntaxTree
    {
        public IReadOnlyList<DiagnosticMessage> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public SyntaxTree(IEnumerable<DiagnosticMessage> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;
        }
    }
}