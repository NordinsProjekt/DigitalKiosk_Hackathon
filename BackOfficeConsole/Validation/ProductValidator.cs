using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackOfficeConsole.Validation;

public class ProductValidator
{
    public static bool TryGetName(out string name)
    {
        Console.WriteLine("Namn: ");
        name = Console.ReadLine()!;

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Fel! Namn får inte vara tomt");
            return false;
        }
            return true;
    }
    public static bool TryGetDescription(out string description)
    {
        Console.WriteLine("Namn: ");
        description = Console.ReadLine()!;

        if (string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("Fel! Beskrivning får inte vara tomt");
            return false;
        }
            return true;
    }

    public static bool TryGetPrice(out decimal price)
    {
        Console.WriteLine("Pris: ");
        if (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0)
        {
            Console.WriteLine("Fel: Ange ett giltigt pris (t.ex. 29.90).");
            return false;
        }
        return true;
    }

    public static bool TryGetSection(out Section section)
    {
        Console.WriteLine("Avdelning:");
        foreach (var s in Enum.GetValues<Section>())
            Console.WriteLine($"  {(int)s}. {s}");
        Console.Write("Välj: ");
        if (!int.TryParse(Console.ReadLine(), out int val) || !Enum.IsDefined(typeof(Section), val))
        {
            Console.WriteLine("Fel: Ogiltigt val.");
            section = default;
            return false;
        }
        section = (Section)val;
        return true;
    }

    public static bool TryGetShelfLocation(out ShelfLocation shelfLocation)
    {
        Console.WriteLine("Hyllplats:");
        foreach (var s in Enum.GetValues<ShelfLocation>())
            Console.WriteLine($"  {(int)s}. {s}");
        Console.Write("Välj: ");
        if (!int.TryParse(Console.ReadLine(), out int val) || !Enum.IsDefined(typeof(ShelfLocation), val))
        {
            Console.WriteLine("Fel: Ogiltig hyllplats.");
            shelfLocation = default;
            return false;
        }
        shelfLocation = (ShelfLocation)val;
        return true;
    }
}
