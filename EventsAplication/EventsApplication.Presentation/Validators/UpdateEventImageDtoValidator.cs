using EventsAplication.Presentation.Dto;
using FluentValidation;
using EventsApplication.Application.Events.Commands;

namespace EventsAplication.Presentation.Validators
{
    public class UpdateEventImageDtoValidator : AbstractValidator<UpdateEventImageDto>
    {
        public UpdateEventImageDtoValidator()
        {
            RuleFor(e => e.Image)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Image is required")
                .Must(i => Extensions.ImageExt
                        .Contains(Path.GetExtension(i.FileName).ToLower()))
                .WithMessage("Incorrect file extension");
        }
    }
}
