﻿
using System;
using System.Collections.Generic;

public class TableReservationApp
{
    static void Main(string[] args)
    {
        ReservationManagerClass m = new ReservationManagerClass();
        m.AddRestaurantMethod("A", 10);
        m.AddRestaurantMethod("B", 5);

        Console.WriteLine(m.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
        Console.WriteLine(m.BookTable("A", new DateTime(2023, 12, 25), 3)); // False
    }
}

public class ReservationManagerClass
{
    public List<RestaurantClass> res;

    public ReservationManagerClass()
    {
        res = new List<RestaurantClass>();
    }
    public void AddRestaurantMethod(string n, int t)
    {
        try
        {
            RestaurantClass r = new RestaurantClass();
            r.n = n;
            r.t = new RestaurantTableClass[t];
            for (int i = 0; i < t; i++)
            {
                r.t[i] = new RestaurantTableClass();
            }
            res.Add(r);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }
    private void LoadRestaurantsFromFileMethod(string fileP)
    {
        try
        {
            string[] ls = File.ReadAllLines(fileP);
            foreach (string l in ls)
            {
                var parts = l.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int tableCount))
                {
                    AddRestaurantMethod(parts[0], tableCount);
                }
                else
                {
                    Console.WriteLine(l);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }
    public List<string> FindAllFreeTables(DateTime dt)
    {
        try
        { 
            List<string> free = new List<string>();
            foreach (var r in res)
            {
                for (int i = 0; i < r.t.Length; i++)
                {
                    if (!r.t[i].IsBooked(dt))
                    {
                        free.Add($"{r.n} - Table {i + 1}");
                    }
                }
            }
            return free;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return new List<string>();
        }
    }

    public bool BookTable(string rName, DateTime d, int tNumber)
    {
        foreach (var r in res)
        {
            if (r.n == rName)
            {
                if (tNumber < 0 || tNumber >= r.t.Length)
                {
                    throw new Exception(null);
                }

                return r.t[tNumber].Book(d);
            }
        }

        throw new Exception(null);
    }

    public void SortRestaurantsByAvailabilityForUsersMethod(DateTime dt)
    {
        try
        { 
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < res.Count - 1; i++)
                {
                    int avTc = CountAvailableTablesForRestaurantClassAndDateTimeMethod(res[i], dt); // available tables current
                    int avTn = CountAvailableTablesForRestaurantClassAndDateTimeMethod(res[i + 1], dt); // available tables next

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
            Console.WriteLine("Error");
        }
    }
    public int CountAvailableTablesForRestaurantClassAndDateTimeMethod(RestaurantClass r, DateTime dt)
    {
        try
        {
            int count = 0;
            foreach (var t in r.t)
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
            Console.WriteLine("Error");
            return 0;
        }
    }
}

public class RestaurantClass
{
    public string n; 
    public RestaurantTableClass[] t; 
}

public class RestaurantTableClass
{
    private List<DateTime> bd;


    public RestaurantTableClass()
    {
        bd = new List<DateTime>();
    }

    public bool Book(DateTime d)
    {
        try
        { 
            if (bd.Contains(d))
            {
                return false;
            }
            bd.Add(d);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return false;
        }
    }
    public bool IsBooked(DateTime d)
    {
        return bd.Contains(d);
    }
}
