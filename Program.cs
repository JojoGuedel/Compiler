using System;
using System.Collections.Generic;

namespace Solver_01
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputString;
            Lexer lexer = new Lexer(LexerMode.INFORMATICAL);
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            while (true)
            {
                Console.Clear();

                inputString = Console.ReadLine();
                tokens = lexer.LexString("test");
                
                foreach(SyntaxToken token in tokens) Console.WriteLine($"{token.Kind}({token.ClearText}): {token.Value}");
                Console.ReadKey(true);
            }
        }
    }

    class SyntaxToken
    {
        public SyntaxKind Kind { get; }
        public object Value { get; }
        public string ClearText { get; }

        public SyntaxToken(SyntaxKind kind, object value, string clearText) 

        {
            Kind = kind;
            Value = value;
            ClearText = clearText;
        }
    }

    class Lexer
    {
        private LexerMode _LexerMode;

        private int _Position;
        private string _StrInput;

        private List<SyntaxToken> _TokenList;
        private SyntaxKind _TokenKind;
        private int _TokenPosition;
        private object _TokenValue;
        private string _TokenClearText;

        private char _CurrentChar { get => _Position < _StrInput.Length ? _StrInput[_Position] : '\0'; }

        private char _Next 
        { 
            get 
            {
                char current = _CurrentChar;
                _Position++;
                return current;
            }
        }

        public Lexer(LexerMode lexerMode)
        {
            _LexerMode = lexerMode;
            _TokenList = new List<SyntaxToken>();
        }

        public List<SyntaxToken> LexString(string strInput)
        {
            _StrInput = strInput;
            _Position = 0;
            _TokenValue = null;

            _TokenList.Clear();

            while (_TokenList.Count != 0? _TokenList[_TokenList.Count - 1].Kind != SyntaxKind.EndOfFileToken : true) _TokenList.Add(_Lex());
            return _TokenList;
        }

        private SyntaxToken _Lex()
        {
            switch (_CurrentChar)
            {
                case '\0':
                    _LexEndOfFile(); break;
                case ' ':
                case '\n':
                case '\r':
                case '\t':
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    _LexNumber(); break;
                case '_':
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                    _LexLetter(); break;

                case '+':
                    _LexPlus(); break;
                case '-':
                    _LexMinus(); break; 
                case '*':
                    _LexStar(); break;
                case '/':
                    _LexSlash(); break;

                case '(':
                    _LexOpenBracket(); break;
                case ')':
                    _LexClosedBracket(); break;
                case '[':
                    _LexOpenAngleBracket(); break;
                case ']':
                    _LexCloseAngleBracket(); break;
                case '{':
                    _LexOpenCurlyBracket(); break;
                case '}':
                    _LexCloseCurlyBracket(); break;
                
                default:
                    _LexInvalidChar(); break;
            }

            return new SyntaxToken(_TokenKind, _TokenValue, _TokenClearText);
        }

        private void _LexWhitespace()
        {
            _TokenPosition = _Position;

            while (char.IsWhiteSpace(_Next));

            _TokenKind = SyntaxKind.WhitespaceToken;
            _TokenClearText = _StrInput.Substring(_TokenPosition, _Position - _TokenPosition);
            _TokenValue = null;
        }


        private void _LexNumber()
        {
            _TokenPosition = _Position;

            while (char.IsDigit(_Next));

            _TokenKind = SyntaxKind.NumberToken;
            _TokenClearText = _StrInput.Substring(_TokenPosition, _Position - _TokenPosition);
            int.TryParse(_TokenClearText, out int _TokenValue);
        }

        private void _LexLetter()
        {
            _TokenPosition = _Position;

            while (char.IsLetter(_Next));

            _TokenKind = SyntaxKind.NameToken;
            _TokenClearText = _StrInput.Substring(_TokenPosition, _Position - _TokenPosition - 1);
            _TokenValue = null;
        }

        private void _LexPlus()
        {
            _TokenPosition = _Position;
            _TokenKind = SyntaxKind.PlusToken;
            _TokenClearText = _Next.ToString();
            _TokenValue = null;
        }

        private void _LexMinus()
        {
            _TokenPosition = _Position;
            _TokenKind = SyntaxKind.MinusToken;
            _TokenClearText = _Next.ToString();
            _TokenValue = null;
        }

        private void _LexStar()
        {
            _TokenPosition = _Position;
            _TokenKind = SyntaxKind.StarToken;
            _TokenClearText = _Next.ToString();
            _TokenValue = null;
        }

        private void _LexSlash()
        {
            _TokenPosition = _Position;
            _TokenKind = SyntaxKind.SlashToken;
            _TokenClearText = _Next.ToString();
            _TokenValue = null;
        }

        private void _LexOpenBracket()
        {
            _TokenPosition = _Position;
            _TokenKind = SyntaxKind.OpenBracketToken;
            _TokenClearText = _Next.ToString();
            _TokenValue = null;
        }

        private void _LexClosedBracket()
        {
            _TokenPosition = _Position;
            _TokenKind = SyntaxKind.CloseBracketToken;
            _TokenClearText = _Next.ToString();
            _TokenValue = null;
        }

        private void _LexOpenAngleBracket()
        {
            _TokenPosition = _Position;
            _TokenKind = SyntaxKind.OpenAngleBracketToken;
            _TokenClearText = _Next.ToString();
            _TokenValue = null;
        }

        private void _LexCloseAngleBracket()
        {
            _TokenPosition = _Position;
            _TokenKind = SyntaxKind.CloseAngleBracketToken;
            _TokenClearText = _Next.ToString();
            _TokenValue = null;
        }

        private void _LexOpenCurlyBracket()
        {
            _TokenPosition = _Position;
            _TokenKind = SyntaxKind.OpenCurlyBracketToken;
            _TokenClearText = _Next.ToString();
            _TokenValue = null;
        }

        private void _LexCloseCurlyBracket()
        {
            _TokenPosition = _Position;
            _TokenKind = SyntaxKind.CloseCurlyBracketToken;
            _TokenClearText = _Next.ToString();
            _TokenValue = null;
        }

        private void _LexInvalidChar()
        {
            _TokenPosition = _Position;
            _TokenKind = SyntaxKind.InvalidCharToken;
            _TokenClearText = _Next.ToString();
            _TokenValue = null;
        }

        private void _LexEndOfFile()
        {
            _TokenPosition = _Position;
            _TokenKind = SyntaxKind.EndOfFileToken;
            _TokenClearText = _Next.ToString();
            _TokenValue = null;
        }
    }

    enum LexerMode
    {
        INFORMATICAL,
        MATHEMATICAL
    }

    enum SyntaxKind
    {
        NumberToken,
        WhitespaceToken,
        InvalidCharToken,
        CloseCurlyBracketToken,
        OpenCurlyBracketToken,
        CloseAngleBracketToken,
        OpenAngleBracketToken,
        CloseBracketToken,
        OpenBracketToken,
        SlashToken,
        StarToken,
        MinusToken,
        PlusToken,
        EndOfFileToken,
        NameToken,
    }
}