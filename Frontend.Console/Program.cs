using Backend;

Console.WriteLine("Function Evaluator");
Console.WriteLine("Escribe una expresión matemática que desees");
Console.WriteLine("Escribe 'salir' para cerrar la aplicación.\n");

while (true)
{
    Console.Write("Ingresa la operación: ");
    string input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input) || input.ToLower() == "salir")
    {
        break;
    }

    try
    {
        double resultado = ExpressionEvaluator.Evalute(input);
        Console.WriteLine($"-> Resultado: {resultado}\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"-> Error: Expresión inválida o mal escrita. ({ex.Message})\n");
    }
}