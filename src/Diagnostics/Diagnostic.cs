using System;

namespace Compiler
{
    public sealed class Diagnostic
    {
        public TextSpan Span { get; }
        public string Message { get; }

        public Diagnostic (TextSpan span, string message)
        {
            Span = span;
            Message = message;
        }

        public void Print()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Message);
            Console.ResetColor();
        }

        public override string ToString() => Message;
    }
}