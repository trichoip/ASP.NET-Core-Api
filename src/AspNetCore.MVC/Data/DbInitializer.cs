using AspNetCore.MVC.Models;
using System.Linq;

namespace AspNetCore.MVC.Data
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
