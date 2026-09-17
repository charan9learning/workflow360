using Microsoft.EntityFrameworkCore;
using WorkFlow360.Application.Common.Interface;
using WorkFlow360.Application.Common.Messaging;
using WorkFlow360.Application.Common.Results;

namespace WorkFlow360.Application.Projects.UpdateProject
{
    public sealed class UpdateProjectCommandHandler : ICommandHandler<UpdateProjectCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        public UpdateProjectCommandHandler( IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {

            var project = await _dbContext.Projects .FirstOrDefaultAsync( x => x.Id == request.Id, cancellationToken);


            if (project is null)
            {
                return Result.Failure( Error.NotFound( "Project.NotFound", "Project was not found."));
            }

            project.Update( request.Name, request.Description);

            await _dbContext.SaveChangesAsync( cancellationToken);

            return Result.Success();
        }
    }
}
