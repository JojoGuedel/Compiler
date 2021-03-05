using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            bool showTree = false;
            string inputString;
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            while (true)
            {
                Console.Write("> ");
                /*
                inputString = "true";
                */
                inputString = Console.ReadLine();


                if (inputString == "#toggleTree")
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree? "Parser tree is now visible" : "Parser tree is now hidden");
                    continue;
                }
                else if (inputString == "#clear")
                {
                    Console.Clear();
                    continue;
                }

                if (string.IsNullOrWhiteSpace(inputString)) return;

                SyntaxTree syntaxTree = SyntaxTree.Parse(inputString);
                Binder binder = new Binder();
                BoundExpression boundExpression = binder.BindExpression(syntaxTree.Root);
                IReadOnlyList<DiagnosticMessage> diagnostics = syntaxTree.Diagnostics;
                diagnostics = diagnostics.Concat(binder.Diagnostics).ToArray();
                

                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    PrettyPrint(syntaxTree.Root);
                    Console.ResetColor();
                }

                if (!diagnostics.Any())
                {
                    Evaluator evaluator = new Evaluator(boundExpression);
                    object result = evaluator.Evaluate();
                    Console.WriteLine(result);
                }
                else
                {
                    foreach (DiagnosticMessage message in diagnostics) message.Print();
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