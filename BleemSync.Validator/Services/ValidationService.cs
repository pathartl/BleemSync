using BleemSync.Validator.Validators;
using System;
using System.Collections.Generic;
using System.IO;

namespace BleemSync.Validator.Services
{
    public class ValidationService
    {
        private readonly IEnumerable<IValidator> _validators;
        
        public ValidationService(string letter)
        {
            _validators = new List<IValidator>
            {
                new DriveFormatValidator(letter),
                new DriveNameValidator(letter),
                new BleemSystemValidator(letter),
                new GamesValidator(letter)
            };

            var drive = new DriveInfo(letter);
            if (!drive.IsReady) throw new DriveNotFoundException($"Drive '{letter}' not ready.");
        }

        public void Validate()
        {
            foreach (var validator in _validators)
            {
                ProcessValidator(validator);
            }
        }

        private static void ProcessValidator(IValidator validator, int depth = 0)
        {
            validator.Validate();
            ReportValidation(validator, depth);

            depth +=4;

            foreach (var child in validator.Validators)
            {
                ProcessValidator(child, depth);
            }
        }

        private static void ReportValidation(IValidator validator, int depth = 0)
        {
            var left = "".PadLeft(depth, ' ');

            Console.Write($"{left}{"".PadRight(6, '=')} {validator.Title}: ");

            WriteStatus(validator);

            foreach (var message in validator.Messages)
            {
                Console.WriteLine($"{left}{message}");
            }

            Console.WriteLine($"{left}".PadRight(100, '-'));
        }

        private static void WriteStatus(IValidator validator)
        {
            switch (validator.Status)
            {
                case ValidationStatus.Pending:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case ValidationStatus.Pass:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case ValidationStatus.Warn:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case ValidationStatus.Fail:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            Console.Write($"{validator.Status}\r\n");
            Console.ResetColor();
        }
    }
}
