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
                Restaurant resturans = new Restaurant();
                resturans.name = n;
                resturans.table = new RestaurantTable[t];
                for (int i = 0; i < t; i++)
                {
                    resturans.table[i] = new RestaurantTable();
                }
                res.Add(resturans);
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
                List<string> free = new List<string>();
                foreach (var r in res)
                {
                    for (int i = 0; i < r.table.Length; i++)
                    {
                        if (!r.table[i].IsBooked(dt))
                        {
                            free.Add($"{r.name} - Table {i + 1}");
                        }
                    }
                }
                return free;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error FindAllFreeTables");
                return new List<string>();
            }
        }

        public bool BookTable(string rName, DateTime d, int tNumber)
        {
            foreach (var r in res)
            {
                if (r.name == rName)
                {
                    if (tNumber < 0 || tNumber >= r.table.Length)
                    {
                        throw new Exception(null);
                    }

                    return r.table[tNumber].Book(d);
                }
            }

            throw new Exception(null);
        }

        public void SortResByAvail(DateTime dt)
        {
            try
            {
                bool swapped;
                do
                {
                    swapped = false;
                    for (int i = 0; i < res.Count - 1; i++)
                    {
                        int avTc = CountAvailableTables(res[i], dt);
                        int avTn = CountAvailableTables(res[i + 1], dt);

                        if (avTc < avTn)
                        {
                            var temp = res[i];
                            res[i] = res[i + 1];
                            res[i + 1] = temp;
                            swapped = true;
                        }
                    }
                } while (swapped);
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
                int count = 0;
                foreach (var t in r.table)
                {
                    if (!t.IsBooked(dt))
                    {
                        count++;
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error CountAvailableTables");
                return 0;
            }
        }
    }
}
