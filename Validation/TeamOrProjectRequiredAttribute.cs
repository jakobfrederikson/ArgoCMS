using ArgoCMS.Models;
using System.ComponentModel.DataAnnotations;

namespace ArgoCMS.Validation
{
    public class TeamOrProjectRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (Job)validationContext.ObjectInstance;

            if (model.TeamID == null && model.ProjectID == null)
            {
                return new ValidationResult("You must select either a team or a project (or both).");
            }

            return ValidationResult.Success;
        }
    }
}
