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
                Compilation compilation = new Compilation(syntaxTree);
                EvaluationResult result = compilation.Evaluate();

                IReadOnlyList<Diagnostic> diagnostics = result.Diagnostics.ToArray();

                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    PrettyPrint(syntaxTree.Root);
                    Console.ResetColor();
                }

                if (!diagnostics.Any())
                    Console.WriteLine(result.Value);

                else
                    foreach (Diagnostic message in diagnostics) 
                    {
                        Console.WriteLine();
                        message.Print();
                        
                        Console.Write("    ");
                        Console.WriteLine(inputString);
                        Console.Write("    ");
                        for(int i = 0; i < message.Span.Start; i++) Console.Write(" ");
                        Console.WriteLine("^");
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