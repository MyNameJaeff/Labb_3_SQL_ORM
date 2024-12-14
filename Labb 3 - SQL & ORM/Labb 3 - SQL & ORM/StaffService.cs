// StaffService.cs
using Labb_3___SQL___ORM;
using Spectre.Console;

namespace Labb_3_SQL_ORM.Services;

public static class StaffService
{
    public static async Task GetStaff()
    {
        try
        {
            var getData = new GetData();
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold underline rgb(190,40,0)]Staff Menu[/]")
                    .AddChoices("Get All Staff", "Get Teachers", "Get Administration", "[red]Back[/]")
            );

            string selectedRole = selection switch
            {
                "Get All Staff" => "All",
                "Get Teachers" => "Teachers",
                "Get Administration" => "Administration",
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(selectedRole))
                return;

            var staff = await getData.GetStaff(selectedRole);
            if (staff?.Any() == true)
            {
                foreach (var person in staff)
                {
                    Console.WriteLine($"Name: {person.FirstName} {person.LastName}, Role: {person.Role}");
                    Console.WriteLine("--------------");
                }
            }
            else
            {
                Console.WriteLine("No staff found.");
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
