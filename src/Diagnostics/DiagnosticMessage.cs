using System;

namespace Compiler
{
    public sealed class DiagnosticMessage
    {
        private string _Message { get; }

        public DiagnosticMessage(string message)
        {
            _Message = message;
        }

        public void Print()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(_Message);
            Console.ResetColor();
            ;
        }
    }
}