using static TokenType;

public enum TokenType
{
  Integer,
  Plus,
  Minus,
  Space,
  EOF
}

public class Token
{
  public TokenType Type;
  public string? Value;

  public Token(TokenType type, string value)
  {
    Type = type;
    Value = value;
  }
}

public class Interpreter
{ //Access this: this.text
  private string Text; //Declare text for the whole class
  private int Pos;
  private Token? Current_Token;
  private char Current_char;
  private bool eof;

  public Interpreter(string text) //Constructor parameters
  {
    Text = text; //Saves the Constructor parameter into the string text
    Pos = 0;
    Current_Token = null;
    //Current_chart = el caracter en la posicion que estemos
    Current_char = Text[Pos]; //Por default 0, primer caracter del input
    eof = false;

  }

  public class PrintError
  {
    public Token? Current_Token;
    public TokenType? Token_Type;

    public PrintError(Token? current_token, TokenType? token_type)
    {
      Current_Token = current_token;
      Token_Type = token_type;

      if (Current_Token == null)
      {
        Console.WriteLine("Current_Token == null. Invalid operation");
        throw new InvalidOperationException("PrintError Current_Token is null. Invalid Operation");
      }
    }

    public void print_error(Token Current_Token, TokenType? token_type = null)
    {
      Console.WriteLine($"Current: {Current_Token.Type} {((int)Current_Token.Type)}");
      if (Token_Type != null)
      {
        Console.WriteLine($"Token type: {token_type} {((int)token_type)}");
      }
    } 
  }

  public void error()
  {
    throw new InvalidOperationException("Invalid operation"); 
  }

  public void advance()
  {
    //Abstract the "advance pos pointer to set the current_char" logic into a function
    Pos += 1;
    if (Pos > Text.Length - 1)
    {
      eof = true;
      // Current_char = TokenType.EOF; //Indicates the end of the input string
    }
    else
    {
      Current_char = Text[Pos];
    }
  }

  public void skip_whitespace()
  {
    // while self.current_char is not None and self.current_char.isspace():
    while (eof != true && char.IsWhiteSpace(Current_char))
    {
      advance();
    }
  }

  public string integer()
  {
    string result = "";
    //while (Current_char is not null && char?.IsDigit(Current_char))
    while (eof != true && char.IsDigit(Current_char))
    {
      result += Current_char;
      advance();
    }
    return result;
  }

  public Token get_next_token()
  {
    while (eof != true)
    {
      if (char.IsWhiteSpace(Current_char))
      {
        skip_whitespace();
        continue;
      }
      if (char.IsDigit(Current_char))
      {
        return new Token(Integer, integer());
      }
      
      if (Current_char == '+')
      {
        advance();
        return new Token(Plus, Current_char.ToString());
      }
      if (Current_char == '-')
      {
        advance();
        return new Token(Minus, Current_char.ToString());
      }
      error();
    }
    // Console.WriteLine("Next token se esta quedando atrapado fuera del while con return EOF, null");
    return new Token(EOF, null);
  }


  public void eat(TokenType token_type)
  {
    // Console.WriteLine($"Eat(): BEFORE IF>> {Current_Token.Type}, {Current_Token.Value}");
    //If curren_state type (Integer, Plus, EOF) == token_type (Integer, Plus, EOF)
    if (Current_Token.Type == token_type)
    {
      // Console.WriteLine($"Eat(): INSIDE IF BEFORE>> {Current_Token.Type}, {Current_Token.Value}");
      //Current_Token changes to the next token
      Current_Token = get_next_token();
      // Console.WriteLine($"Eat(): INSIDE IF AFTER get_next_token>> {Current_Token.Type}, {Current_Token.Value}");
    }
    else
    {
      Console.WriteLine("Linea 118, Public void eat, else loop");
      new PrintError(Current_Token, token_type).print_error(Current_Token, token_type);
      error();
    }
  }

  public int? expr()
  {
    // new PrintError(Current_Token, null).print_error(Current_Token, null);
    Current_Token = get_next_token();
    // new PrintError(Current_Token, null).print_error(Current_Token, null);

    Token left = Current_Token;
    eat(Integer);

    Token op = Current_Token;
    eat(Current_Token.Type);

    Token right = Current_Token;
    eat(Integer);

    //Now Value is a string not a char
    // int? int_left = left.Value - '0';
    // int? int_right = right.Value - '0';
    // int? result = left.IntValue + right.IntValue;

    //Converts string to int
    int? int_left = int.Parse(left.Value);
    int? int_right = int.Parse(right.Value);

    Console.WriteLine($"Expr>> op: {op.Type}");
    if (op.Type == TokenType.Plus)
    {
      return int_left + int_right;
    }
    if (op.Type == TokenType.Minus)
    {
      return int_left - int_right;
    }
    error();
    return null;
  }
}

public class Program
{
  public static void Main()
  {
    while (true)
      try
      {
        Console.WriteLine(">calc> ");
        string? text = Console.ReadLine();
        if (string.IsNullOrEmpty(text))
          continue;

        Interpreter interpreter = new Interpreter(text);
        int result = interpreter.expr() ?? 0;
        Console.WriteLine(result);
      }

      catch (Exception e)
      {
        Console.WriteLine("Invalido " + e);
        break;
      }
  }  
}

