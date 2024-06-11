using Application.Commands.GetRepositoriesByFilters;
using Application.Commands.GetRepositoryById;
using Application.Commands.Results;
using Application.Commands.UpdateAllData;
using Domain.GithubRepo;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [Route("")]
    public class GithubRepoController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("languages")]
        [SwaggerOperation(Summary = "Get languages", Description = "Get the five languages choosen to view the repositories")]
        [SwaggerResponse(StatusCodes.Status200OK, "The languages", typeof(string[]))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
        public IActionResult GetLanguagesAsync()
        {
            try
            {
                return Ok(Enum.GetNames(typeof(LanguageEnum)));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        [HttpPatch("repositories")]
        [SwaggerOperation(Summary = "Update repositories", Description = "Update repositories calling GitHub API and saving on Database the repositories of the five languages choosen")]
        [SwaggerResponse(StatusCodes.Status200OK, "The repositories", typeof(EmptyResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid parameters")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]

        public async Task<IActionResult> UpdateRepositoriesAsync()
        {
            try
            {
                var command = new UpdateAllDataCommand();
                var result = await _mediator.Send(command);
                return result.IsError ? BadRequest(result.Errors) : Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("repositories")]
        [SwaggerOperation(Summary = "Get repositories", Description = "Get the repositories saved on Database. The result has a default pagination but you can pass the parameters of page, limitPage and you can filter by specific language")]
        [SwaggerResponse(StatusCodes.Status200OK, "The repositories", typeof(GetRepositoriesByFiltersResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid parameters")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
        public async Task<IActionResult> GetRepositoriesAsync([FromQuery] int page = 1, [FromQuery] int limitPage = 10, [FromQuery] string language = "")
        {
            try
            {
                var result = await _mediator.Send(new GetRepositoriesByFiltersCommand(page, limitPage, language));
                return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("repositories/{id}")]
        [SwaggerOperation(Summary = "Get repository by id", Description = "Get the repository saved on Database by id")]
        [SwaggerResponse(StatusCodes.Status200OK, "The repository", typeof(GithubRepoWithStringLanguage))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid parameters")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Invalid parameters")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
        public async Task<IActionResult> GetRepositoryByIdAsync(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetRepositoryByIdCommand(id));
                if (result.IsError)
                    return BadRequest(result.Errors);
                if (result.Value is null)
                    return NotFound();
                return Ok(result.Value);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
