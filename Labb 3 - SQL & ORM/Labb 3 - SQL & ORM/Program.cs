using Labb_3_SQL_ORM.Services;
using Spectre.Console;

namespace Labb_3_SQL_ORM;

internal class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            // Establish database connection
            using var sqlConnection = await DatabaseService.GetDatabaseConnectionAsync();

            // Start Main Menu Navigation
            await MenuService.MainMenu(sqlConnection);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Critical Error:[/] {ex.Message}");
        }
    }
}
