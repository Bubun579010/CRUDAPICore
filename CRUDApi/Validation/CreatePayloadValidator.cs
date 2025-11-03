using CRUDApi.Models.Payload;
using FluentValidation;

namespace CRUDApi.Validation
{
    public class CreatePayloadValidator : AbstractValidator<CreatePayload>
    {
        public CreatePayloadValidator()
        {
            // Name validation
            RuleFor(D => D.Name).NotEmpty().WithMessage("Name is required.")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");
            //.Matches("^[A-Za-z0-9]{2-50}$").WithMessage("Name does not contain special characters.");

            // Description validation
            RuleFor(D => D.Description).NotEmpty().WithMessage("Description is required.")
                .Length(10, 200).WithMessage("Description must be between 10 and 200 characters.");
            //.Matches("^[A-Za-z0-9]{10-200}$").WithMessage("Description does not contain special characters.");
        }
    }
}
