using FluentValidation;

namespace Application.Worlds.Commands.CreateWorld
{
    public class CreateWorldCommandValidator : AbstractValidator<CreateWorldComand>
    {
        public CreateWorldCommandValidator()
        {
            RuleFor(v => v.createWorldDto.HasLife)
                .Equal(true);

            RuleFor(v => v.createWorldDto.Name)
                .NotEmpty();
        }

    }
}