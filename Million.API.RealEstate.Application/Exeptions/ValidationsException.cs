using FluentValidation.Results;

namespace Million.API.RealEstate.Application.Exeptions
{
    public class ValidationsException : ApplicationException
    {
        public List<string> Errors { get; }

        public ValidationsException(ValidationResult validationResult)
        {
            Errors = [];
            foreach (var error in validationResult.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
        }

        public override string ToString()
        {
            return $"ValidationsException: {string.Join(", ", Errors)}";
        }
    }
}
