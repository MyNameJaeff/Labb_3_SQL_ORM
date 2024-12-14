// MenuService.cs
using Labb_3___SQL___ORM;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace Labb_3_SQL_ORM.Services;

public static class MenuService
{
    public static async Task MainMenu(SqlConnection sqlConnection)
    {
        var getData = new GetData(); // Create an instance of GetData
        var addData = new AddData(); // Create an instance of AddData

        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"[bold underline rgb(190,40,0)]Database:[/] {sqlConnection.Database}");

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold underline rgb(190,40,0)]Main Menu[/]")
                    .AddChoices("Get Staff", "Get Students", "Get Students by Class", "Get grades (1 month)", "Get courses", "Add students", "Add Teachers", "[red]Exit[/]")
            );

            switch (selection)
            {
                case "Get Staff":
                    await StaffService.GetStaff();
                    break;

                case "Get Students":
                    await StudentService.GetStudents();
                    break;

                case "Get Students by Class":
                    await ClassService.GetStudentsOfClass();
                    break;

                case "Get grades (1 month)":
                    var grades = await getData.GetLastMonthsGrades(); // Use the instance to call the method

                    if (grades?.Any() == true)
                    {
                        foreach (var grade in grades)
                        {
                            var firstName = grade.Student?.Person?.FirstName ?? "N/A";
                            var lastName = grade.Student?.Person?.LastName ?? "N/A";
                            var gradeValue = grade.GradeValue ?? "N/A";
                            var courseName = grade.Course?.CourseName ?? "N/A";

                            Console.WriteLine($"Name: {firstName} {lastName}, Grade: {gradeValue}, Course: {courseName}");
                            Console.WriteLine("--------------");
                        }

                        Console.WriteLine("Press anything to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("No students found.");
                    }
                    break;

                case "Get courses":
                    var courses = await getData.GetCoursesAndGrades();
                    if (courses?.Any() == true)
                    {
                        foreach (var course in courses)
                        {
                            Console.WriteLine($"Course: {course.CourseName}, Teacher: {course.TeacherName}");
                            Console.WriteLine($"Average Grade: {course.AverageGrade:F2}, Max Grade: {course.MaxGrade:F2}, Min Grade: {course.MinGrade:F2}");
                            Console.WriteLine("--------------");
                        }
                        Console.WriteLine("Press anything to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("No courses found.");
                    }
                    break;

                case "Add students":
                    await addData.AddStudent();
                    break;

                case "Add Teachers":
                    await addData.AddStaff();
                    break;

                case "[red]Back[/]":
                    Console.Clear();
                    return;

                case "[red]Exit[/]":
                    AnsiConsole.MarkupLine("[green]Goodbye![/]");
                    await sqlConnection.DisposeAsync();
                    return;

                default:
                    AnsiConsole.MarkupLine("[red]Invalid option, please try again.[/]");
                    break;
            }
        }
    }
}
