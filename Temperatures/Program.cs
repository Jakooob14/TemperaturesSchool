using System;
using System.Linq;
using System.Threading;
using Spectre.Console;

namespace Temperatures
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Menu();
        }

        public static void Menu(int[] input = null)
        {
            Console.Clear();

            string[] choices;
            if (input == null)
            {
                choices = new[] { "Calculate", "Exit" };
            }
            else
            {
                choices = new[] { "Calculate", "Show input", "Exit" };
            }
            
            // Ask if the user wants an avg, max or min
            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]What temperature do you want to be calculated?[/]")
                    .AddChoices(choices));

            switch (selection)
            {
                case "Calculate":
                {
                    CalculateCalendar();
                    break;
                }
                case "Show input":
                {
                    AnsiConsole.Markup("[blue]These are the numbers you entered:[/]");
                    Console.WriteLine();
                    foreach (var x in input)
                    {
                        AnsiConsole.Markup($"{x} ");
                    }
                    
                    Console.WriteLine();
                    AnsiConsole.Markup("Press anything to continue");
                    Console.ReadLine();
                    Menu(input);
                    break;
                }
                case "Exit":
                {
                    Environment.Exit(0);
                    break;
                }
            }
        }

        public static void CalculateCalendar()
        {
            DateTime now = DateTime.Now;
            int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            int[] temperatureInput = new int[daysInMonth];

            // Loop through daysInMonth
            for (int i = 1; i <= daysInMonth; i++)
            { 
                // Create a new calendar instance
                Calendar calendar = new Calendar(now.Year, now.Month);
                // Highlight the date from the loop i
                calendar.AddCalendarEvent(now.Year, now.Month, i);
                calendar.HighlightStyle(Style.Parse("yellow bold"));
                AnsiConsole.Write(calendar);
                // Ask for the temperature of the current date
                temperatureInput[i-1] = AnsiConsole.Ask<int>("[yellow]What was the temperature for this day? (°C)[/]");
                Console.Clear();
            }
            
            
            // Ask if the user wants an avg, max or min
            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]What temperature do you want to be calculated?[/]")
                    .AddChoices(new[] {
                        "Coldest", "Average", "Hottest" 
                    }));

            switch (selection)
            {
                case "Coldest":
                {
                    AnsiConsole.Markup($"[blue]The coldest temperature is: {temperatureInput.Min()}°C[/]");
                    break;
                }
                case "Average":
                {
                    AnsiConsole.Markup($"[yellow]The average temperature is: {Math.Round(temperatureInput.Average(), 2)}°C[/]");
                    break;
                }
                case "Hottest":
                {
                    AnsiConsole.Markup($"[red]The hottest temperature is: {temperatureInput.Max()}°C[/]");
                    break;
                }
            }

            Console.WriteLine();
            AnsiConsole.Markup("Press anything to continue");
            Console.ReadLine();
            Menu(temperatureInput);
        }
    }
}