namespace BackOfficeConsole.Menu;

internal class NavigationMenu
{
    private readonly string[] _options;
    private readonly Func<int, Task> _executeChoice;

    public NavigationMenu(string[] options, Func<int, Task> executeChoice)
    {
        _options = options;
        _executeChoice = executeChoice;
    }

    public async Task Run()
    {
        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();
            PrintMenu(_options, selectedIndex);

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Escape) break;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex - 1 + _options.Length) % _options.Length;
                    break;

                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex + 1) % _options.Length;
                    break;

                case ConsoleKey.Enter:
                    Console.Clear();
                    await _executeChoice(selectedIndex);
                    WaitForUserConfirmation();
                    break;
            }
        }
    }

    private void PrintMenu(string[] options, int selectedIndex)
    {
        for (var i = 0; i < options.Length; i++)
        {
            if (i == selectedIndex)
                Console.WriteLine($"  > {options[i]}");
            else
                Console.WriteLine($"    {options[i]}");
        }
    }

    private void WaitForUserConfirmation()
    {
        Console.WriteLine("\n:: Press any key to continue ... ::");
        Console.ReadKey(true);
    }
}