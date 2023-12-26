using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using Restaurants;
using ReservationManagers;

namespace RestaurantTables
{
    public class RestaurantTable
    {
        private List<DateTime> basedate;


        public RestaurantTable()
        {
            basedate = new List<DateTime>();
        }

        public bool Book(DateTime d)
        {
            try
            {
                if (basedate.Contains(d))
                {
                    return false;
                }
                basedate.Add(d);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Book");
                return false;
            }
        }
        public bool IsBooked(DateTime d)
        {
            return basedate.Contains(d);
        }
    }
}
