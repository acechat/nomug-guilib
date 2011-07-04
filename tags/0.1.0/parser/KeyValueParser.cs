using System.Collections.Generic;

namespace GUILib
{
	/// <summary>
	/// KeyValueParser pre-parses a single GUI-Stylesheet and adds the key-value pairs to the given StyleDictionary.
	/// </summary>
	/// <author>Matthias Stock (mstock)</author>
	/// <author>$LastChangedBy: marshen $</author>
	/// <version>$Rev: 273 $, $Date: 2011-07-02 19:01:52 +0200 (Sa, 02 Jul 2011) $</version>
	public class KeyValueParser
	{
		#region Constructor
		/// <summary>
		/// Creates a new KeyValueParser instance with a content string and a StyleDictionary.
		/// </summary>
		/// <param name="content">The content of the GUI-Stylesheet.</param>
		/// <param name="dictionary">The StyleDict for saving the key-value pairs.</param>
		public KeyValueParser(string content, StyleDictionary dictionary, string filename)
		{
			this.curPosition = 0;

			this.currentIdent = "";
			this.currentAttribute = "";
			this.currentValue = "";
			this.currentStyle = "normal";

			this.fileName = filename;

			this.errorsDuringParsing = false;

			//this.content = content;
			this.styleDictionary = dictionary;

			/*char[] delimiters = new char[] { ' ', '\n', '\r', '\t', ';' };
			symbols = content.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);*/
			this.parseSymbolList(content);
		}

		#endregion

		#region Variables

		private StyleDictionary styleDictionary;
		private string[] symbols;

		//private string content = "";
		private int curPosition;

		private string fileName;

		private string currentIdent;
		private string currentAttribute;
		private string currentValue;
		private string currentStyle;

		private bool errorsDuringParsing;

		#endregion

		#region Properties

		/// <summary>
		/// Getter fot the internal error variable.
		/// </summary>
		public bool ParseErrors
		{
			get
			{
				return errorsDuringParsing;
			}
		}

		#endregion

		#region Grammar Structures
		/// <summary>
		/// Represents a single parsed Token.
		/// </summary>
		public struct Token
		{
			public TokenType type;
			public string content;
			
			/// <summary>
			/// String representation of the Token structure. 
			/// </summary>
			/// <returns>
			/// Token as string in "<tokentype> : <content of token>" format.
			/// </returns>
			public override string ToString()
			{
				return type + " : " + content;
			}

		}
		/// <summary>
		/// The type of a Token.
		/// </summary>
		public enum TokenType
		{
			STRING,
			OPENINGBRACKET,
			CLOSINGBRACKET,
			COLON,
			SEMICOLON,
			EOF,
		}

		#endregion

		#region Public Methods
		/// <summary>
		/// Check for unread symbols.
		/// </summary>
		/// <returns>True if there is unread text in content. False otherwise.</returns>
		public bool HasNewSymbols()
		{
			return (this.curPosition < this.symbols.Length);
		}

		/// <summary>
		/// Gets the next Token from the content.
		/// </summary>
		/// <returns>Next Token from content.</returns>
		public Token NextSymbol()
		{
			Token read;

			if (!HasNewSymbols())
			{
				read.type = TokenType.EOF;
				read.content = "end of file";

				return read;
			}

			switch (symbols[this.curPosition])
			{
				case ":":
					read.type = TokenType.COLON;
					read.content = ":";
					break;
				case "{":
					read.type = TokenType.OPENINGBRACKET;
					read.content = "{";
					break;
				case "}":
					read.type = TokenType.CLOSINGBRACKET;
					read.content = "}";
					break;
				default:
					// A little bit trickier to get quotation mark encapsuled strings in one token.
					read.type = TokenType.STRING;
					if (symbols[this.curPosition].StartsWith("\""))
					{
						if (symbols[this.curPosition].EndsWith("\""))
						{
							//one word string
							read.content = symbols[this.curPosition].Substring(1, symbols[this.curPosition].Length - 2);
							this.curPosition++;
							return read;
						}
						read.content = symbols[this.curPosition].Substring(1) + " ";
						this.curPosition++;
						if (this.curPosition >= symbols.Length)
						{
							read.type = TokenType.EOF;
							read.content = "end of file";
							return read;
						}
						while (!symbols[this.curPosition].EndsWith("\"") && this.curPosition < symbols.Length)
						{
							read.content += symbols[this.curPosition] + " ";
							this.curPosition++;
							//Debug.Log(read.content);
						}
						read.content += symbols[this.curPosition].Substring(0, symbols[this.curPosition].Length - 1);
					}
					else
					{
						read.content = symbols[this.curPosition];
					}
					break;
			}

			this.curPosition++;

			return read;
		}
		/// <summary>
		/// Takes the generated symbol array and starts the syntax checking and StyleDictionary filling.
		/// </summary>
		/// <exception cref="System.Exception">Thrown if a parsing error occurs.</exception>
		public void Parse()
		{
			Token curRead;

			this.curPosition = 0; // Just to be sure that we're at the beginning of the Tokenlist
			curRead = NextSymbol();
			this.ebnfFile(curRead);

			if (errorsDuringParsing)
			{
				this.error("There were errors during Stylesheet parsing!");
				throw new System.Exception("There were errors during Stylesheet parsing!");
			}

		}

		#endregion

		#region Private Methods
		/// <summary>
		/// Converts the string read into an easier accessible SymbolList. 
		/// </summary>
		/// <param name="content">
		/// Content of a Stylesheet as a string.
		/// </param>
		private void parseSymbolList(string content)
		{
			List<string> symList = new List<string>();

			string buffer = "";
			string curSym = "";

			this.curPosition = 0; // Just to be sure

			while (this.curPosition < content.Length)
			{
				curSym = content.Substring(this.curPosition, 1);

				if (curSym.Equals("\r") || curSym.Equals("\n") || curSym.Equals(";") || curSym.Equals("\t") || curSym.Equals(" "))
				{
					if (!buffer.Equals("")) symList.Add(buffer);
					buffer = "";
				}
				else if (curSym.Equals("{") || curSym.Equals("}") || curSym.Equals(":"))
				{
					if (!buffer.Equals("")) symList.Add(buffer);
					buffer = "";
					symList.Add(curSym);
				}
				else if (curSym.Equals("/"))
				{
					this.curPosition += 1;
					curSym = content.Substring(this.curPosition, 1);
					if (curSym.Equals("/"))
					{
						if (!buffer.Equals("")) symList.Add(buffer);
						buffer = "";
						while (!curSym.Equals("\n") && this.curPosition < content.Length - 1)
						{
							//skip all symbols between "//" and line-end resp. end-of-file.
							this.curPosition += 1;
							curSym = content.Substring(this.curPosition, 1);
						}
					}
					else
					{
						this.curPosition -= 1;
						buffer = buffer + "/";
					}
				}
				else
				{
					buffer = buffer + curSym;
				}

				this.curPosition += 1;
			}

			if (!buffer.Equals("")) symList.Add(buffer);
			this.symbols = symList.ToArray();
			curPosition = 0;
		}

		/// <summary>
		/// Reports an error and sets internal error variable to true.
		/// </summary>
		/// <param name="text">The text that will be shown.</param>
		private void error(string text)
		{
			this.errorsDuringParsing = true;
			//Debug.LogError(text);
			OverlayManager.Instance.LogError("[StyleParser] " + text + "\nError occured near '" + this.currentIdent + "' in '" + this.fileName + "'");
		}

		/// <summary>
		/// Left recursive implementation of the EBNF file rule.
		/// </summary>
		/// <param name="symbol">The latest symbol which has been read.</param>
		private void ebnfFile(Token symbol)
		{
			Token newsymbol = symbol;
			while (newsymbol.type == TokenType.STRING)
			{
				this.currentIdent = newsymbol.content;
				this.ebnfClause(newsymbol);
				newsymbol = this.NextSymbol();
			}
		}

		/// <summary>
		/// Left recursive implementation of the EBNF clause rule.
		/// </summary>
		/// <param name="symbol">The latest symbol which has been read.</param>
		private void ebnfClause(Token symbol)
		{
			//styleDictionary.Items.Add(symbol.content, new ElementStyleDict());
			this.styleDictionary.AddUnique(symbol.content, new ElementStyleDictionary());
			this.styleDictionary.Items[symbol.content].FileName = this.fileName;
			Token newsymbol = NextSymbol();
			if (newsymbol.type == TokenType.COLON)
			{
				newsymbol = NextSymbol();
				if (newsymbol.type == TokenType.STRING)
				{
					switch(newsymbol.content.ToLower())
					{
						case "active":
							this.currentStyle = "active";
							break;
						case "hover":
							this.currentStyle = "hover";
							break;
						case "focused":
							this.currentStyle = "focused";
							break;
						default:
							this.currentStyle = "normal";
							break;
					}
					newsymbol = this.NextSymbol();
					if (newsymbol.type == TokenType.OPENINGBRACKET)
					{
						newsymbol = this.NextSymbol();
						while (newsymbol.type == TokenType.STRING)
						{
							this.ebnfObjectDef(newsymbol);
							newsymbol = this.NextSymbol();
						}

					}
					else this.error("Parsing error: Expected '{' but '" + newsymbol.content + "' found.");
				}
				else this.error("Parsing error: Expected 'normal', 'active' or 'hover' but '" + newsymbol.content + "' found.");
			}
			else if (newsymbol.type == TokenType.OPENINGBRACKET)
			{
				this.currentStyle = "normal";
				newsymbol = NextSymbol();
				while (newsymbol.type == TokenType.STRING)
				{
					this.ebnfObjectDef(newsymbol);
					newsymbol = this.NextSymbol();
				}
			}
			else this.error("Parsing error: Expected ':' but '" + newsymbol.content + "' found.");
			//newsymbol = NextSymbol();
			if (!(newsymbol.type == TokenType.CLOSINGBRACKET)) this.error("Parsing error: Expected '}' but '" + newsymbol.content + "' found.");
		}

		/// <summary>
		/// Left recursive implementation of the EBNF objectdef rule.
		/// </summary>
		/// <param name="symbol">The latest symbol which has been read.</param>
		private void ebnfObjectDef(Token symbol)
		{
			if (symbol.type == TokenType.STRING)
			{
				this.currentAttribute = symbol.content;
				Token newsymbol = NextSymbol();
				if (newsymbol.type == TokenType.COLON)
				{
					newsymbol = NextSymbol();
					if (newsymbol.type == TokenType.STRING)
					{
						currentValue = newsymbol.content;
						// add key value pair to right style and Ident for later parsing to GUIStyle
						if (this.currentStyle.Equals("normal"))
							this.styleDictionary.Items[currentIdent].Normal[currentAttribute.ToLower()] = currentValue;
						else if (this.currentStyle.Equals("active"))
							this.styleDictionary.Items[currentIdent].Active[currentAttribute.ToLower()] = currentValue;
						else if (this.currentStyle.Equals("hover"))
							this.styleDictionary.Items[currentIdent].Hover[currentAttribute.ToLower()] = currentValue;
						else if (this.currentStyle.Equals("focused"))
							this.styleDictionary.Items[currentIdent].Focused[currentAttribute.ToLower()] = currentValue;
					}
					else this.error("Parsing error: Expected  Object-Value but '" + newsymbol.content + "' found.");
				}
				else this.error("Parsing error: Expected ':' but '" + newsymbol.content + "' found.");
			}
			else this.error("Parsing error: Expectet Object-Attribute but '" + symbol.content + "' found.");
		}

		#endregion
	}
}
