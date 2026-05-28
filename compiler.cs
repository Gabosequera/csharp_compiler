using static TokenType;

public enum TokenType
{
  Integer,
  Plus,
  EOF
}


public class Token
{
  public TokenType Type;
  public char? Value;

  public Token(TokenType type, char? value)
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

  public Interpreter(string text) //Constructor parameters
 {
    Text = text; //Saves the Constructor parameter into the string text
    Pos = 0;
    Current_Token = null;
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

  public Token get_next_token()
  {
    string text = Text;

    if (Pos > text.Length - 1)
    {
      return new Token(EOF, null);
    }

    //Current_chart = el caracter en la posicion que estemos
    char current_char = text[Pos];
    
    //Si el current_char is digito: ->
    if (char.IsDigit(current_char))
    {
      Token token = new Token(Integer, current_char);
      Pos += 1;
      return token;
    }
    //Si el current_char is "+": ->
if (current_char == '+')
    {
      Token token = new Token(Plus, current_char);
      Pos += 1;
      return token;
    }

    error();
    return null;
  }

  public void eat(TokenType token_type)
  {
    //If curren_state type (Integer, Plus, EOF) == token_type (Integer, Plus, EOF)
    if (Current_Token.Type == token_type)
    {
      //Current_Token changes to the next token
      Current_Token = get_next_token();
    }
    else
    {
      Console.WriteLine("Linea 100, Public void eat, else loop");
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
    eat(Plus);

    Token right = Current_Token;
    eat(Integer);

    int? int_left = left.Value - '0';
    int? int_right = right.Value - '0';
    int? result = int_left + int_right;
    return result;
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
