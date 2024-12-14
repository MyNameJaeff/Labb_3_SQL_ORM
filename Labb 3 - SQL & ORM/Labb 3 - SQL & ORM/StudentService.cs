// StudentService.cs
using Labb_3___SQL___ORM;
using Spectre.Console;

namespace Labb_3_SQL_ORM.Services;

public static class StudentService
{
    public static async Task GetStudents()
    {
        try
        {
            var getData = new GetData();

            var firstOrLast = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold underline rgb(190,40,0)]Student Sorting[/]")
                    .AddChoices("Get by Firstname", "Get by Lastname", "[red]Back[/]")
            );

            var ascOrDesc = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold underline rgb(190,40,0)]Order[/]")
                    .AddChoices("Ascending", "Descending", "[red]Back[/]")
            );

            if (firstOrLast == "[red]Back[/]" || ascOrDesc == "[red]Back[/]")
                return;

            var students = await getData.GetStudents(firstOrLast, ascOrDesc);

            if (students?.Any() == true)
            {
                foreach (var student in students)
                {
                    Console.WriteLine($"Name: {student.Person.FirstName} {student.Person.LastName}, Role: {student.Person.Role}");
                    Console.WriteLine("--------------");
                }
            }
            else
            {
                Console.WriteLine("No students found.");
            }

            Console.WriteLine("\nPress any key to return");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
