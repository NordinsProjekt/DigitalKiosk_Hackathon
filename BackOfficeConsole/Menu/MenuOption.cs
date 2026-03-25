namespace BackOfficeConsole.Menu;

public class MenuOption
{
    public string Title { get; }
    public Func<Task> Action { get; }

    public MenuOption(string title, Func<Task> action)
    {
        Title = title;
        Action = action;
    }
}