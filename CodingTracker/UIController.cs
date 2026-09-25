using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System.ComponentModel;

namespace CodingTracker
{
    internal class UIController
    {
        internal enum MenuOption
        {
            ViewSessions,
            AddSession,
            DeleteSession,
            ExitApplication
        }

        private Dictionary<string, MenuOption> options = new()
        {
            ["View Sessions"] = MenuOption.ViewSessions,
            ["Add Sessions"] = MenuOption.AddSession,
            ["Delete Sessions"] = MenuOption.DeleteSession,
            ["Exit Application"] = MenuOption.ExitApplication
        };

        private static IConfiguration _config = null!;

        public UIController(IConfiguration config)
        {
            _config = config;
        }

        internal void MainMenu()
        {
            bool exitApp = false;
            while (!exitApp)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new Panel("Coding Tracker"));
                MenuOption option = GetUserSelection();
                switch (option)
                {
                    case MenuOption.ViewSessions:
                        ViewSessions();
                        break;
                    case MenuOption.AddSession:
                        AddSession();
                        break;
                    case MenuOption.DeleteSession:
                        DeleteSession();
                        break;
                    case MenuOption.ExitApplication:
                        exitApp = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private MenuOption GetUserSelection()
        {
            bool optionSelected = false;
            MenuOption option = default;
            while (!optionSelected)
            {
                string selected = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Main Menu")
                    .AddChoices(options.Keys)
                );
                optionSelected = options.TryGetValue(selected, out option);
            }
            return option;
        }

        private void ViewSessions()
        {
            var sessions = DatabaseManager.GetSessions();
            Table table = new Table().AddColumns("Id", "Project", "Language", "Start time", "End time", "Duration");
            foreach(CodingSession session in sessions)
            {
                var duration = session.Duration;
                table.AddRow(session.Id.ToString(), session.Project, session.Language, session.StartTime.ToString(), session.EndTime?.ToString() ?? "In progress", $"{(int)duration.TotalHours}h {duration.Minutes}m {duration.Seconds}s");
            }
            Panel menu = new Panel(table).Header("Coding Sessions").Expand();
            menu.Height = Console.WindowHeight - 1;
            AnsiConsole.Write(menu);
            string action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .AddChoices("[blue]Go back[/]")
            );
        }

        private void AddSession()
        {

        }

        private void DeleteSession()
        {

        }

    }
}
