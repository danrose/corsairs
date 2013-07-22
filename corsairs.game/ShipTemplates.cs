using corsairs.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.game
{
    public static class ShipTemplates
    {
        private static List<ShipTemplate> templates = new List<ShipTemplate>();

        public static IEnumerable<ShipTemplate> AllTemplates
        {
            get
            {
                return templates.ToList();
            }
        }

        public static ShipTemplate GetById(string id)
        {
            return templates.First(x => x.Id == id);
        }

        public static void Init()
        {
            // get templates
        }
    }
}
