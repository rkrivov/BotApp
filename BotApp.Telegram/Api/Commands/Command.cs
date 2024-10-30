using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Telegram.Api.Commands
{
    internal class Command : ICommand
    {
        private readonly string _name;
        private readonly string _description;
        private readonly List<string> _arguments = new();

        public string Name => _name;
        public string Description => _description;
        public List<string> Arguments => _arguments;

        private Command(string command)
        {
            _name = command;
            _description = string.Empty;
        }
        private Command(string command, string description)
        {
            _name = command;
            _description = description;
        }
        private Command(string command, List<string> arguments)
        {
            _name = command;
            _description = string.Empty;
            _arguments.AddRange(arguments);
        }
        private Command(string command, string description, List<string> arguments)
        {
            _name = command;
            _description = description;
            _arguments.AddRange(arguments);
        }

        public static ICommand? Parse(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;    
            if (!text.StartsWith('/')) return null;

            var entites = text.Split(' ').ToList();

            var command = entites[0];
            entites.RemoveAt(0);

            if (entites.Count == 0)
            {
                return new Command(command);
            }

            return new Command(command, entites);
        }
    }
}
