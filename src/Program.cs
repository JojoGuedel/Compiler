using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser;
            string inputString;
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            while (true)
            {
                Console.Write("> ");
                inputString = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputString)) return;

                parser = new Parser(inputString);
                SyntaxTree syntaxTree = parser.Parse();

                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                PrettyPrint(syntaxTree.Root);
                Console.ForegroundColor = color;

                if (syntaxTree.Diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    foreach (DiagnosticMessage message in parser.Diagnostics) message.Print();
                    Console.ForegroundColor = color;
                }
            }
        }

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            string marker = isLast ? "└──" : "├──";

            Console.Write(indent + marker + node.Kind);
            if (node is SyntaxToken t && t.Value != null) Console.Write(": " + t.Value);
            Console.WriteLine();

            indent += isLast? "    " : "│   ";
            SyntaxNode lastChild = node.GetChildren().LastOrDefault();
            foreach (var child in node.GetChildren()) PrettyPrint(child, indent, child == lastChild);
        }
    }
}