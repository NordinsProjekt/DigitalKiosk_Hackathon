namespace BackOfficeConsole.Validation;

public class CustomerValidation
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

    public static bool TryGetPersonalIdentityNumber(out string personalIdentity)
    {
        while (true)
        {
            Console.Write("Personnummer (YYYYMMDD-XXXX): ");
            personalIdentity = Console.ReadLine()!;

        if (string.IsNullOrWhiteSpace(personalIdentity))
        {
            Console.WriteLine("Fel! personnummer får inte vara tomt");
        }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(personalIdentity, @"^\d{8}-\d{4}$"))
            {
                Console.WriteLine("Fel: Ange personnummer i format YYYYMMDD-XXXX.");
            }
                else
                {
                    return true;
                }
        }
    }
}
