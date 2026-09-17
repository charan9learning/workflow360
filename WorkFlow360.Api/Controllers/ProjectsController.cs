using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkFlow360.Api.Contracts.Projects;
using WorkFlow360.Application.Projects.CreateProject;
using WorkFlow360.Application.Projects.DeleteProject;
using WorkFlow360.Application.Projects.GetProjectById;
using WorkFlow360.Application.Projects.GetProjects;
using WorkFlow360.Application.Projects.UpdateProject;

namespace WorkFlow360.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ProjectsController : ApiController
    {
        private readonly ISender _sender;

        public ProjectsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create( CreateProjectRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateProjectCommand( request.Name, request.Description);

            var result = await _sender.Send( command, cancellationToken);

            if (result.IsFailure)
            {
                return Problem(result.Error);
            }
            
            return CreatedAtAction( nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById( Guid id, CancellationToken cancellationToken)
        {
            var query = new GetProjectByIdQuery(id);

            var result = await _sender.Send( query, cancellationToken);

            if (result.IsFailure)
            {
                return Problem(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll( [FromQuery] GetProjectsRequest request, CancellationToken cancellationToken)
        {
            var query = new GetProjectsQuery(
                request.Search,
                request.Page,
                request.PageSize,
                request.SortBy,
                request.SortDirection);

            var result = await _sender.Send( query, cancellationToken);

            if (result.IsFailure)
            {
                return Problem(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update( Guid id, UpdateProjectRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateProjectCommand( id, request.Name, request.Description);

            var result = await _sender.Send( command, cancellationToken);

            if (result.IsFailure)
            {
                return Problem(result.Error);
            }

            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete( Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteProjectCommand(id);

            var result = await _sender.Send( command, cancellationToken);

            if (result.IsFailure)
            {
                return Problem(result.Error);
            }

            return NoContent();
        }
    }
}
