using System;
using System.Collections.Generic;
using System.Text;

namespace BackOfficeConsole.Validation;

public class CustomerValidation
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

    public static bool TryGetPersonalIdentityNumber(out string personalIdentity)
    {
        Console.WriteLine("Personnummer: ");
        personalIdentity = Console.ReadLine()!;

        if (string.IsNullOrWhiteSpace(personalIdentity))
        {
            Console.WriteLine("Fel! personnummer får inte vara tomt");
            return false;
        }
        return true;
    }
}
