using System.Collections.Generic;

namespace BleemSync.Validator.Validators
{
    public interface IValidator
    {
        string Title { get; }
        ValidationStatus Status { get; }
        List<IValidator> Validators { get; }
        List<string> Messages { get; }
        void Validate();
    }
}