using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core
{
    /// <summary>
    /// Represents a class of ship, e.g. galleon/rowboat
    /// </summary>
    public class ShipTemplate
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public int MaxMasts { get; private set; }
    }
}
