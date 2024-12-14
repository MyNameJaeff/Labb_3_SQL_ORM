// DatabaseService.cs
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace Labb_3_SQL_ORM.Services;

public static class DatabaseService
{
    public static async Task<SqlConnection> GetDatabaseConnectionAsync()
    {
        try
        {
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=ER-modellering;Trusted_Connection=True;";
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            AnsiConsole.MarkupLine("[green]Successfully connected to the database![/]");
            return connection;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Failed to connect to the database:[/] {ex.Message}");
            throw;
        }
    }
}
