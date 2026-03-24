using System.Text.Json;
using ToySql.Lexer;
using ToySql.Parser;


//REPL here
Console.WriteLine("TOYSQL 0.1");
Console.WriteLine("Type 'exit' to quit.");
while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        continue;

    if (input.Trim().ToLower() == "exit")
        break;

    try
    {
        // 1. Lex
        var tokenizer = new Tokenizer(input);

        // 2. Parse
        var parser = new Parser(tokenizer);
        var stmt_ast = parser.Parse();

        // 3. Execute
        //var result = Executor.Execute(stmt);  // TODO

        // 4. Print result
        //if (result is not null)
        //    Console.WriteLine(result);

        var options = new JsonSerializerOptions { WriteIndented = true };
        // Records automatically print their nested structure
        Console.WriteLine(JsonSerializer.Serialize(stmt_ast, options));
        // Output: SelectStatement { Columns = [name], Table = users, Where = BinaryExpression { Left = id, Op = =, Right = 5 } }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: {ex.Message}");
        Console.ResetColor();
    }
};