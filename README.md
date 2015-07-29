# CsvExtensionMethod
Quick extension method to convert an IEnumerable<T> to a csv string

Example Usage

            var people = new List<Person>();
            people.Add(new Person() { FirstName = "Luke", LastName = "Skywalker", Age = 32 });
            people.Add(new Person() { FirstName = "Han", LastName = "Solo" });
            people.Add(new Person() { FirstName = "Leia", LastName = "Organa", Age = 3 });
            people.Add(new Person() { FirstName = "Wedge", LastName = "Antilles", Age = 2 });

            var csv = people.ToCsv("")
                .Column("First", f => f.FirstName)
                .Column("Last", f => f.LastName)
                .Column("Age", f => f.Age)
                .ToString();
