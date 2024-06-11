using Application.Commands.Results;
using Application.OutputPorts;
using Domain.GithubRepo;
using ErrorOr;
using FluentValidation;
using MediatR;

namespace Application.Commands.GetRepositoriesByFilters
{
    public record GetRepositoriesByFiltersCommand(int Page, int Limit, string Language) : IRequest<ErrorOr<GetRepositoriesByFiltersResult>>
    {
        public ErrorOr<bool> Validate()
        {
            var validator = new GetRepositoriesByFiltersCommandValidator().Validate(this);
            if (!validator.IsValid)
            {
                return validator.Errors.Select(error => Error.Validation(error.ErrorCode, error.ErrorMessage)).ToList();
            }
            return true;
        }
    }

    public class GetRepositoriesByFiltersCommandValidator : AbstractValidator<GetRepositoriesByFiltersCommand>
    {
        public GetRepositoriesByFiltersCommandValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.Limit).GreaterThanOrEqualTo(1)
                                 .LessThanOrEqualTo(100);
            When(x => !string.IsNullOrEmpty(x.Language), () =>
            {
                RuleFor(x => x.Language).Must((type) => IsValidLanguage(type))
                                        .WithMessage("A linguagem fornecida não está na lista das linguagens");
            });
        }

        private static bool IsValidLanguage(string language) => Enum.GetNames(typeof(LanguageEnum)).Contains(language);
    }

    public class GetRepositoriesByFiltersCommandHandler(IGithubRepoRepository githubRepoRepository) : IRequestHandler<GetRepositoriesByFiltersCommand, ErrorOr<GetRepositoriesByFiltersResult>>
    {
        private readonly IGithubRepoRepository _githubRepoRepository = githubRepoRepository;

        public async Task<ErrorOr<GetRepositoriesByFiltersResult>> Handle(GetRepositoriesByFiltersCommand request, CancellationToken cancellationToken)
        {
            var validator = request.Validate();
            if (validator.IsError)
                return validator.Errors;

            var result = await _githubRepoRepository.GetAllPaginatedAsync(request.Page, request.Limit, request.Language);
            return result;
        }
    }
}
