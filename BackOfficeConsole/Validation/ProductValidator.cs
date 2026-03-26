using Entities.Enums;

namespace BackOfficeConsole.Validation;

public class ProductValidator
{
    public static bool TryGetName(out string name)
    {
        while (true)
        {

            Console.Write("Namn: ");
            name = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Fel! Namn får inte vara tomt");
            }
            else
            {
                return true;
            }
        }
    }
    public static bool TryGetDescription(out string description)
    {
        while (true)
        {
            Console.Write("Beskrivning: ");
            description = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Fel! Beskrivning får inte vara tomt");
            }
            else
            {
                return true;
            }
        }
        
    }

    public static bool TryGetPrice(out decimal price)
    {
        
        while (true)
        {
            Console.Write("Pris: ");
            if (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0)
            {
            Console.WriteLine("Fel: Ange ett giltigt pris (t.ex. 29.90).");
            }
            else
            {
            return true;
            }
        }
        
        
    }

    public static bool TryGetSection(out Section section)
    {
        Console.Write("Avdelning:");
        foreach (var s in Enum.GetValues<Section>())
        {
            Console.WriteLine($"  {(int)s}. {s}");
        }
        
            while (true)
            {

            Console.Write("Välj: ");
            if (!int.TryParse(Console.ReadLine(), out int val) || !Enum.IsDefined(typeof(Section), val))
            {
                Console.WriteLine("Fel: Ogiltigt val.");
            }
            else
            {
                section = (Section)val;
                return true;
            }
            }
            
        
    }

    public static bool TryGetShelfLocation(out ShelfLocation shelfLocation)
    {
        Console.Write("Hyllplats:");
        foreach (var s in Enum.GetValues<ShelfLocation>())
        {
            Console.WriteLine($"  {(int)s}. {s}");
        }
        
        while (true)
        {
            Console.Write("Välj: ");
            if (!int.TryParse(Console.ReadLine(), out int val) || !Enum.IsDefined(typeof(ShelfLocation), val))
            {
                Console.WriteLine("Fel: Ogiltig hyllplats.");
            }
            else
            {
                shelfLocation = (ShelfLocation)val;
                return true;

            }
        }
    }
}
