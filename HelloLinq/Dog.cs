using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static HelloLinq.Extensions.StringExtensions;

namespace HelloLinq
{
    public class Dog
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? Age => (int?)(-DateOfBirth?.Subtract(DateTime.Now))?.TotalDays / 365;

        public override string ToString() => $"{Name.TrimPad(20)} ({Age.TrimPad(2)}) [ID: {Id.TrimPad(3)}] <{SiblingIds.Count().TrimPad(2)} Siblings>";

        public IEnumerable<Dog> Siblings => SiblingIds?.Select(i => Repository[i]);

        private HashSet<int> SiblingIds { get; set; } = new HashSet<int>();

        public void SetSibling(Dog dog)
        {
            if (dog.Id == Id)
                return;
            SiblingIds.Add(dog.Id);
            dog.SiblingIds.Add(Id);
        }

        public void UnsetSibling(Dog dog)
        {
            SiblingIds.Remove(dog.Id);
            dog.SiblingIds.Remove(Id);
        }

        public static Dog Import(string text)
        {
            var tokens = text.Split(';');
            return new Dog
            {
                Name = tokens[0],
                Id = int.Parse(tokens[1]),
                DateOfBirth = DateTime.Parse(tokens[2]),
                SiblingIds = new HashSet<int>(tokens[3].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse))
            };
        }

        public static string Export(Dog dog)
            => string.Join(";", new[] {                                         // CSV összefűzése ';'-vel
                dog.Name,                                                       // név
                dog.Id.ToString(),                                              // ID
                dog.DateOfBirth?.ToString(),                                    // születési dátum
                string.Join(",", dog.SiblingIds ?? Enumerable.Empty<int>())    // testvér ID-k ','-vel elválasztva
            }.Select(s => s.Replace(";", "|")));                                // a stringek ';' karaktereinek lecserélése '|'-reű

        const string DogRepositoryFilePath = "dogs.csv";
        private static Lazy<Dictionary<int, Dog>> RepositoryHolder { get; }
            = new Lazy<Dictionary<int, Dog>>(() => File.ReadAllLines(DogRepositoryFilePath).Select(Import).ToDictionary(d => d.Id));

        public static Dictionary<int, Dog> Repository => RepositoryHolder.Value;
    }
}
