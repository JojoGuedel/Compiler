using System.Collections.Generic;
using System.Linq;

namespace Compiler
{
    public sealed class SyntaxToken : SyntaxNode
    {
        public override SyntaxKind Kind { get; }
        public override IEnumerable<SyntaxNode> GetChildren() => Enumerable.Empty<SyntaxNode>();

        public int Position { get; }
        public object Value { get; }
        public string ClearText { get; }
        public TextSpan TextSpan => new TextSpan(Position, ClearText.Length);

        public SyntaxToken(int position, SyntaxKind kind, object value, string clearText) 

        {
            Position = position;
            Kind = kind;
            Value = value;
            ClearText = clearText;
        }
    }
}