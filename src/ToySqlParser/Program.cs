using System.Text.Json;
using ToySqlParser.Lexer;
using ToySqlParser.Parser;

var lexer = new Lexer("SELECT id, name AS emp_name FROM Employees");
var parser = new Parser(lexer);
var ast = parser.Parse();

var options = new JsonSerializerOptions { WriteIndented = true };
// Records automatically print their nested structure
Console.WriteLine(JsonSerializer.Serialize(ast,options)); 
// Output: SelectStatement { Columns = [name], Table = users, Where = BinaryExpression { Left = id, Op = =, Right = 5 } }
