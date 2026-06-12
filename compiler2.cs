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
  //Change Value to string
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
    string current_string = current_char.ToString();
    char? next_char = null;
    try
    {
      next_char = text[Pos + 1];
      // Console.WriteLine($"Current char: {current_char}, next_char: {next_char} ");
    }
    catch (System.Exception)
    {
      Console.WriteLine("There is no next_char index");
    }

    //Si el current_char is digito: ->
    if (char.IsDigit(current_char))
    {
      if (next_char is char n && char.IsDigit(n))
      {
        string multi_digit = current_char.ToString() + next_char.ToString();
        Pos += multi_digit.Length;
        return new Token(Integer, multi_digit);
      }

      Pos += 1;
      return new Token(Integer, current_string);
    }
    //Si el current_char is "+": ->
    if (current_char == '+')
    {
      Pos += 1;
      return new Token(Plus, current_string);
    }
    if (current_char == '-')
    {
      Pos += 1;
      return new Token(Minus, current_string);
    }

    error();
    return null;
  }


  public void eat(TokenType token_type)
  {
    Console.WriteLine($"Eat(): BEFORE IF>> {Current_Token.Type}, {Current_Token.Value}");
    //If curren_state type (Integer, Plus, EOF) == token_type (Integer, Plus, EOF)
    if (Current_Token.Type == token_type)
    {
      //Current_Token changes to the next token
      Current_Token = get_next_token();
      Console.WriteLine($"Eat(): INSIDE IF AFTER get_next_token>> {Current_Token.Type}, {Current_Token.Value}");
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

