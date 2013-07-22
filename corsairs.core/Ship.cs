using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core
{
    /// <summary>
    /// Represents an instance of a ship
    /// </summary>
    public class Ship
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public ShipTemplate Template { get; private set; }

        public Ship(string name, ShipTemplate template)
        {
            Id = Guid.NewGuid();
            Name = name;
            Template = template;
        }
    }
}
