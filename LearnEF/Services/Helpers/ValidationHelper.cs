using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
    //validattion cho model, neu model khong hop le thi throw exception, neu hop le thi khong lam gi ca
    public class ValidationHelper
    {
        internal static void ModelValidation(object model)
        {
            //validation model
            ValidationContext validationContext = new ValidationContext(model);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(model, validationContext, validationResults))
            {
                //throw new ArgumentException("Invalid person data.", nameof(personAddRequest));
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
