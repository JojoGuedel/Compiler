using System;
using System.Collections.Generic;

namespace Compiler
{
    internal sealed class Lexer
    {
        private int _Position;
        private string _StrInput;

        private List<DiagnosticMessage> _Diagnostics;
        public IEnumerable<DiagnosticMessage> Diagnostics => _Diagnostics;

        private List<SyntaxToken> _TokenList;
        private SyntaxKind _TokenKind;
        private int _TokenPosition;
        private object _TokenValue;
        private string _TokenClearText;

        private char _PeekChar(int offset) => _Position + offset < _StrInput.Length ? _StrInput[_Position + offset] : '\0';
        private char _CurrentChar { get => _PeekChar(0); }

        public Lexer(string strInput)
        {
            _Diagnostics = new List<DiagnosticMessage>();
            _TokenList = new List<SyntaxToken>();
            _StrInput = strInput;
            _Position = 0;
        }

        public List<SyntaxToken> LexString()
        {
            _TokenList.Clear();
            _Position = 0;

            while (_TokenList.Count != 0? _TokenList[_TokenList.Count - 1].Kind != SyntaxKind.EndOfFileToken : true) 
            {
                SyntaxToken token = Lex();

                if (token.Kind != SyntaxKind.WhitespaceToken && token.Kind != SyntaxKind.InvalidCharToken) _TokenList.Add(token);
            }

            return _TokenList;
        }

        public SyntaxToken Lex()
        {
            _TokenPosition = _Position;
            _TokenValue = null;

            switch (_CurrentChar)
            {
                case '\0':
                    _LexEndOfFile(); break;
                case ' ':
                case '\n':
                case '\r':
                case '\t':
                    _LexWhitespace(); break;
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
                case '!':
                    _LexBangToken(); break;
                case '&':
                    _LexAmpersantToken(); break;
                case '|':
                    _LexPipeToken(); break;

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

            while (char.IsWhiteSpace(_CurrentChar)) _Next();

            _TokenKind = SyntaxKind.WhitespaceToken;
            _TokenClearText = _StrInput.Substring(_TokenPosition, _Position - _TokenPosition);
        }


        private void _LexNumber()
        {

            while (char.IsDigit(_CurrentChar)) _Next();

            _TokenKind = SyntaxKind.NumberToken;
            _TokenClearText = _StrInput.Substring(_TokenPosition, _Position - _TokenPosition);
            if (!int.TryParse(_TokenClearText, out int tokenValue)) _Diagnostics.Add(new DiagnosticMessage($"[Error] the number '{_TokenClearText}' cannot be represented by an Int32"));
            _TokenValue = tokenValue;
        }

        private void _LexLetter()
        {

            while (char.IsLetterOrDigit(_CurrentChar)) _Next();

            _TokenClearText = _StrInput.Substring(_TokenPosition, _Position - _TokenPosition);
            _TokenKind = SyntaxFacts.GetKeywordKind(_TokenClearText);
        }

        private void _LexPlus()
        {
            _TokenKind = SyntaxKind.PlusToken;
            _TokenClearText = _Next().ToString();
        }

        private void _LexMinus()
        {
            _TokenKind = SyntaxKind.MinusToken;
            _TokenClearText = _Next().ToString();
        }

        private void _LexStar()
        {
            _TokenKind = SyntaxKind.StarToken;
            _TokenClearText = _Next().ToString();
        }

        private void _LexSlash()
        {
            _TokenKind = SyntaxKind.SlashToken;
            _TokenClearText = _Next().ToString();
        }

        private void _LexBangToken()
        {
            _TokenKind = SyntaxKind.BangToken;
            _TokenClearText = _Next().ToString();
        }

        private void _LexAmpersantToken()
        {
            switch(_PeekChar(1))
            {
                case '&':
                    _Next(2);
                    _TokenKind = SyntaxKind.AmpersantAmpersantToken;
                    _TokenClearText = _StrInput.Substring(_TokenPosition, _Position - _TokenPosition);
                    break;
                default:
                    _LexInvalidChar(); break;
            }
        }

        private void _LexPipeToken()
        {
            switch(_PeekChar(1))
            {
                case '|':
                    _Next(2);
                    _TokenKind = SyntaxKind.PipePipeToken;
                    _TokenClearText = _StrInput.Substring(_TokenPosition, _Position - _TokenPosition);
                    break;
                default:
                    _LexInvalidChar(); break;
            }
        }

        private void _LexOpenBracket()
        {
            _TokenKind = SyntaxKind.OpenBracketToken;
            _TokenClearText = _Next().ToString();
        }

        private void _LexClosedBracket()
        {
            _TokenKind = SyntaxKind.CloseBracketToken;
            _TokenClearText = _Next().ToString();
        }

        private void _LexOpenAngleBracket()
        {
            _TokenKind = SyntaxKind.OpenAngleBracketToken;
            _TokenClearText = _Next().ToString();
        }

        private void _LexCloseAngleBracket()
        {
            _TokenKind = SyntaxKind.CloseAngleBracketToken;
            _TokenClearText = _Next().ToString();
        }

        private void _LexOpenCurlyBracket()
        {
            _TokenKind = SyntaxKind.OpenCurlyBracketToken;
            _TokenClearText = _Next().ToString();
        }

        private void _LexCloseCurlyBracket()
        {
            _TokenKind = SyntaxKind.CloseCurlyBracketToken;
            _TokenClearText = _Next().ToString();
        }

        private void _LexInvalidChar()
        {
            _Diagnostics.Add(new DiagnosticMessage($"[Error] invalid char: '{_CurrentChar}'"));

            _TokenKind = SyntaxKind.InvalidCharToken;
            _TokenClearText = _Next().ToString();
        }

        private void _LexEndOfFile()
        {
            _TokenKind = SyntaxKind.EndOfFileToken;
            _TokenClearText = _Next().ToString();
        }
        
        private char _Next(int offset = 1)
        {
            char currentChar = _CurrentChar;
            _Position += offset;
            return currentChar;
        }
    }
}