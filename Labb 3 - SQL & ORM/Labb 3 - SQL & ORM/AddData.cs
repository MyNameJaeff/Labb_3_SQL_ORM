using Labb_3___SQL___ORM.Data;
using Labb_3___SQL___ORM.Models;

namespace Labb_3___SQL___ORM
{
    internal class AddData
    {
        public async Task AddStudent()
        {
            string firstName, lastName, personalNumber;
            int classId;

            while (true)
            {
                Console.Write("Enter First Name: ");
                firstName = Console.ReadLine();

                Console.Write("Enter Last Name: ");
                lastName = Console.ReadLine();

                while (true)
                {
                    Console.Write("Enter Personal Number (digits only): ");
                    personalNumber = Console.ReadLine();

                    if (long.TryParse(personalNumber, out _))
                        break;

                    Console.WriteLine("Invalid Personal Number. It should contain only digits.");
                }

                while (true)
                {
                    Console.Write("Enter Class ID: ");
                    if (int.TryParse(Console.ReadLine(), out classId))
                        break;

                    Console.WriteLine("Invalid Class ID. Please enter a valid integer.");
                }

                using var context = new ERModelleringContext();

                var person = new Person
                {
                    FirstName = firstName,
                    LastName = lastName,
                    PersonalNumber = personalNumber,
                    Role = "Student"
                };

                context.People.Add(person);
                await context.SaveChangesAsync();

                var student = new Student
                {
                    PersonId = person.PersonId,
                    ClassId = classId
                };

                context.Students.Add(student);
                await context.SaveChangesAsync();

                Console.WriteLine($"Student {firstName} {lastName} added successfully.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
        }

        public async Task AddStaff()
        {
            string firstName, lastName, personalNumber, role;

            while (true)
            {
                Console.Write("Enter First Name: ");
                firstName = Console.ReadLine();

                Console.Write("Enter Last Name: ");
                lastName = Console.ReadLine();

                while (true)
                {
                    Console.Write("Enter Personal Number (digits only): ");
                    personalNumber = Console.ReadLine();

                    if (long.TryParse(personalNumber, out _))
                        break;

                    Console.WriteLine("Invalid Personal Number. It should contain only digits.");
                }

                while (true)
                {
                    Console.Write("Enter Role (Teacher/Administration/etc.): ");
                    role = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(role))
                        break;

                    Console.WriteLine("Role cannot be empty.");
                }

                using var context = new ERModelleringContext();

                var person = new Person
                {
                    FirstName = firstName,
                    LastName = lastName,
                    PersonalNumber = personalNumber,
                    Role = role
                };

                context.People.Add(person);
                await context.SaveChangesAsync();

                Console.WriteLine($"Staff member {firstName} {lastName} added successfully as {role}.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
        }
    }
}