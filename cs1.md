# Végállapot

```csharp
using System;
using System.Collections.Generic;

namespace HelloCSharp
{
    public class Person
    {
        public string Name { get; private set; }

        public DateTime DateOfBirth { get; }

        public Guid Id { get; } = Guid.NewGuid();

        public int Age => DateTime.Now.Subtract(DateOfBirth).Days / 365;

        public Person(string name, DateTime dateOfBirth)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
        }

        public override string ToString() => $"{Name} ({Age}) [ID: {Id}]";
    }

    public class Student : Person
    {
        public string Neptun { get; set; }

        public Student(string name, DateTime dateOfBirth, string neptun)
            : base(name, dateOfBirth)
        {
            Neptun = neptun;
        }

        public override string ToString() => $"{base.ToString()} Neptun: {Neptun}";
    }


    class Program
    {
        static void Main(string[] args)
        {
            List<Person> people = new List<Person>();
            people.Add(new Person("Horváth Aladár", new DateTime(1991, 06, 10)));
            people.Add(new Person("Kovács István", new DateTime(1994, 04, 22)));
            people.Add(new Person("Kovács Géza", new DateTime(1998, 03, 16)));

            people.Add(new Student("Nagy Sándor", new DateTime(1995, 11, 23), "RHSSDR"));
            people.Add(new Student("Nagy Sándor", new DateTime(1994, 7, 3), "HSSWG4"));
            people.Add(new Student("Horváth Géza", new DateTime(1994, 7, 3), "ASYF2K"));

            foreach (Person person in people)
                Console.WriteLine(person);

            Console.ReadLine();
        }
    }
}
```
