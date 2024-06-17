using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPupsekBot.Games
{
    internal class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public long Id { get; set; }
        public Product(string name, string desc, int price, long id)
        {
            Name = name;
            Description = desc;
            Price = price;
            Id = id;
        }
    }
}
