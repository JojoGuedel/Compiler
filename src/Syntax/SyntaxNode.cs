using System.Collections.Generic;

namespace Solver_01
{
    abstract class SyntaxNode 
    {
        public abstract SyntaxKind Kind  { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}