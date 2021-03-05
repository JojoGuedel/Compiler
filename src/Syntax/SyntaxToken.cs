using System.Collections.Generic;
using System.Linq;

namespace Compiler
{
    public sealed class SyntaxToken : SyntaxNode
    {
        public override SyntaxKind Kind { get; }
        public override IEnumerable<SyntaxNode> GetChildren() => Enumerable.Empty<SyntaxNode>();

        public object Value { get; }
        public string ClearText { get; }

        public SyntaxToken(SyntaxKind kind, object value, string clearText) 

        {
            Kind = kind;
            Value = value;
            ClearText = clearText;
        }
    }
}