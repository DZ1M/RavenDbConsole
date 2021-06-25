using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenDbConsole.Models
{
    public class Product
    {
        public string Id { get; set; } // pode ser qualquer coisa o id
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
