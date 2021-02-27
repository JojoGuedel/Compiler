using System.Collections.Generic;

namespace Compiler
{
    abstract class SyntaxNode 
    {
        public abstract SyntaxKind Kind  { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}