using System;

namespace Compiler
{
    class DiagnosticMessage
    {
        private string _Message { get; }

        public DiagnosticMessage(string message)
        {
            _Message = message;
        }

        public void Print()
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(_Message);
            Console.ForegroundColor = color;
        }
    }
}