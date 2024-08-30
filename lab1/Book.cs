using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public struct Book : IComparable<Book>
    {
        public string Name { get; set; }
        public int Year { get; set; }

        public static Book Parse(string str)
        {
            var data = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return new Book { Name = data[0], Year = int.Parse(data[1]) };
        }

        public int CompareTo(Book other)
        {
            var nameCompare = Name.CompareTo(other.Name);
            return nameCompare != 0 ? nameCompare : Year.CompareTo(other.Year);
        }

        public override string ToString() => $"{Name} {Year}";
    }
}
