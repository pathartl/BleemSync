using System.Collections.Generic;

namespace BleemSync.Validator.Validators
{
    public abstract class Validator : IValidator
    {
        public abstract string Title { get; }
        protected string DirPath { get; }
        public ValidationStatus Status { get; protected set; }
        public List<string> Messages { get; }
        public List<IValidator> Validators { get; }

        protected Validator(string path)
        {
            DirPath = path;
            Status = ValidationStatus.Pending;
            Messages = new List<string>();
            Validators = new List<IValidator>();
        }

        public abstract void Validate();
    }
}