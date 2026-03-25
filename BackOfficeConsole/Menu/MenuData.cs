using System;
using System.Collections.Generic;
using System.Text;

namespace BackOfficeConsole.Menu;

public class MenuData
{
    private readonly List<MenuOption> _options;

    public MenuData(List<MenuOption> options)
    {
        _options = options;
    }

    public async Task Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== BackOffice ===\n");

            for (int i = 0; i < _options.Count; i++)
                Console.WriteLine($"  {i + 1}. {_options[i].Title}");

            Console.Write("\nVälj: ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out int val) && val >= 1 && val <= _options.Count)
                await _options[val - 1].Action();  // <-- här
            else
                Console.WriteLine("Ogiltigt val, tryck enter...");

            Console.ReadKey();
        }
    }
}