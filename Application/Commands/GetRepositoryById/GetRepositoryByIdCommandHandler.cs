using Application.Commands.GetRepositoriesByFilters;
using Application.Commands.Results;
using Application.OutputPorts;
using ErrorOr;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.GetRepositoryById
{
    public record GetRepositoryByIdCommand(Guid Id) : IRequest<ErrorOr<GithubRepoWithStringLanguage?>>
    {
        public ErrorOr<bool> Validate()
        {
            var validator = new GetRepositoryByIdCommandValidator().Validate(this);
            if (!validator.IsValid)
            {
                return validator.Errors.Select(error => Error.Validation(error.ErrorCode, error.ErrorMessage)).ToList();
            }
            return true;
        }
    }
    public class GetRepositoryByIdCommandValidator : AbstractValidator<GetRepositoryByIdCommand>
    {
        public GetRepositoryByIdCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotEqual(Guid.Empty);
        }
    }
    public class GetRepositoryByIdCommandHandler(IGithubRepoRepository githubRepoRepository) : IRequestHandler<GetRepositoryByIdCommand, ErrorOr<GithubRepoWithStringLanguage?>>
    {
        private readonly IGithubRepoRepository _githubRepoRepository = githubRepoRepository;

        public async Task<ErrorOr<GithubRepoWithStringLanguage?>> Handle(GetRepositoryByIdCommand request, CancellationToken cancellationToken)
        {
            var validator = request.Validate();
            if (validator.IsError)
                return validator.Errors;

            var result = await _githubRepoRepository.GetByIdAsync(request.Id);
            return result;
        }
    }
}
