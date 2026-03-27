namespace BackOfficeConsole.Menu;

public abstract class BaseMenu
{
    protected abstract string Title { get; }
    protected abstract string[] Options { get; }
    protected abstract Task<bool> ExecuteChoiceAsync(int selectedIndex);

    public async Task Run()
    {
        int selectedIndex = 0;
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine($"=== {Title} ===\n");
            PrintMenu(selectedIndex);

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Escape) break;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex - 1 + Options.Length) % Options.Length;
                    break;

                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex + 1) % Options.Length;
                    break;

                case ConsoleKey.Enter:
                    Console.Clear();
                    running = await ExecuteChoiceAsync(selectedIndex);
                    if (running)
                        WaitForUserConfirmation();
                    break;
            }
        }
    }

    private void PrintMenu(int selectedIndex)
    {
        for (var i = 0; i < Options.Length; i++)
        {
            if (i == selectedIndex)
                Console.WriteLine($"  > {Options[i]}");
            else
                Console.WriteLine($"    {Options[i]}");
        }
    }

    private void WaitForUserConfirmation()
    {
        Console.WriteLine("\n:: Press any key to continue ... ::");
        Console.ReadKey(true);
    }
}