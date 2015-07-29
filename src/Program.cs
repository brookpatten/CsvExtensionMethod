using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvExtenstion
{
    class Program
    {
        /// <summary>
        /// Test program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
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

            Console.WriteLine(csv);
            Console.ReadLine();
        }
    }

    public class Person
    {
        /// <summary>
        /// test model
        /// </summary>
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
    }

    public static class CsvExtension
    {
        public static CsvExport<T> ToCsv<T>(this IEnumerable<T> items, string nullReplacement=null)
        {
            return new CsvExport<T>(items, nullReplacement);
        }
    }

    internal class CsvColumn<T>
    {
        public string Name { get; set; }
        public Func<T, object> Value { get; set; }
    }

    public class CsvExport<T>
    {
        private string _nullReplacement = null;
        private IEnumerable<T> Items { get; set; }
        private IList<CsvColumn<T>> Columns { get; set; }

        internal CsvExport(IEnumerable<T> items,string nullReplacement=null)
        {
            _nullReplacement = nullReplacement;
            Items = items;
            Columns = new List<CsvColumn<T>>();
        }

        public CsvExport<T> Column(string name, Func<T, object> value)
        {
            Columns.Add(new CsvColumn<T>() { Name = name, Value = value });
            return this;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in Columns)
            {
                sb.Append(c.Name);
                if (Columns.Last() == c)
                {
                    sb.Append(System.Environment.NewLine);
                }
                else
                {
                    sb.Append(",");
                }
            }

            foreach (var item in Items)
            {
                foreach (var c in Columns)
                {
                    string value = null;
                    try
                    {
                        value = c.Value(item).ToString();
                    }
                    catch(NullReferenceException)
                    {
                        if(_nullReplacement==null)
                        {
                            throw;
                        }
                        else
                        {
                            value = _nullReplacement;
                        }
                    }

                    sb.Append(value);
                    if (Columns.Last() == c)
                    {
                        sb.Append(System.Environment.NewLine);
                    }
                    else
                    {
                        sb.Append(",");
                    }
                }
            }

            return sb.ToString();
        }
    }
}
