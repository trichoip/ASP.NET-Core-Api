using asp.net_core_empty_5._0.Models;
using System.Linq;

namespace asp.net_core_empty_5._0.Data
{
    public class DbInitializer
    {

        public static void Initialize(ETransportationSystemContext context)
        {

            if (context.Cars.Any())
            {
                return;   // DB has been seeded
            }

            // add info car
            //context.SaveChanges();
        }
    }
}
