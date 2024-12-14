using Spectre.Console;

namespace Labb_3___SQL___ORM
{
    internal class ClassService
    {
        public static async Task GetStudentsOfClass()
        {
            try
            {
                var getData = new GetData();
                List<string> classes = await getData.GetClasses();
                classes.Add(classes.Count + 1 + ". [red]Back[/]");
                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold underline rgb(190,40,0)]Registered Classes[/]")
                        .AddChoices(classes)
                );

                if (selection == "[red]Back[/]")
                    return;

                var students = await getData.GetStudentsByClass(selection);
                if (students?.Any() == true)
                {
                    Console.WriteLine($"Selected Class: {selection}");
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

                Console.WriteLine("\nPress anything to continue");
                Console.ReadKey();
            }
            catch
            {
                throw;
            }
        }
    }
}
