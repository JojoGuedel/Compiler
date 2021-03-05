using System;

namespace Compiler
{
    internal abstract class BoundExpression : BoundNode
    {
        public abstract Type Type { get; }
    }
}