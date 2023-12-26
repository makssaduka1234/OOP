using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantTables;
using Restaurants;
using Main;

namespace ReservationManagers
{
    public class ReservationManager
    {
        public List<Restaurant> res;

        public ReservationManager()
        {
            res = new List<Restaurant>();
        }
        public void AddRestaurant(string n, int t)
        {
            try
            {
                Restaurant restaurant = new Restaurant
                {
                    name = n,
                    table = Enumerable.Range(0, t).Select(_ => new RestaurantTable()).ToArray()
                };
                res.Add(restaurant);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error AddRestaurant");
            }
        }
        private void LoadRestaurantsFromFile(string fileP)
        {
            try
            {
                string[] ls = File.ReadAllLines(fileP);
                foreach (string l in ls)
                {
                    var parts = l.Split(',');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int tableCount))
                    {
                        AddRestaurant(parts[0], tableCount);
                    }
                    else
                    {
                        Console.WriteLine(l);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error LoadRestaurantsFromFile");
            }
        }
        public List<string> FindAllFreeTables(DateTime dt)
        {
            try
            {
                return res
                    .SelectMany(r =>
                        r.table
                            .Select((table, i) => new { RestaurantName = r.name, TableNumber = i + 1, IsBooked = table.IsBooked(dt) })
                    )
                    .Where(t => !t.IsBooked)
                    .Select(t => $"{t.RestaurantName} - Table {t.TableNumber}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error FindAllFreeTables");
                return new List<string>();
            }
        }


        public bool BookTable(string restaurantName, DateTime date, int tableNumber)
        {
            try
            {
                var restaurant = res.FirstOrDefault(r => r.name == restaurantName);

                if (restaurant != null && tableNumber >= 0 && tableNumber < restaurant.table.Length)
                {
                    return restaurant.table[tableNumber].Book(date);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error BookTable");
            }

            throw new Exception(null);
        }


        public void SortResByAvail(DateTime dt)
        {
            try
            {
                res = res
                    .OrderByDescending(r => CountAvailableTables(r, dt))
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error SortResByAvail");
            }
        }

        public int CountAvailableTables(Restaurant r, DateTime dt)
        {
            try
            {
                return r.table.Count(t => !t.IsBooked(dt));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error CountAvailableTables");
                return 0;
            }
        }

    }
}
