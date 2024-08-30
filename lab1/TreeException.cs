using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class TreeException : Exception
    {
        public TreeException(string message) : base(message) { }
    }
}
