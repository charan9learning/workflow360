using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WorkFlow360.Application.Projects.CreateProject;
using WorkFlow360.Application.Projects.DeleteProject;
using WorkFlow360.Application.Projects.GetProjectById;
using WorkFlow360.Application.Projects.GetProjects;
using WorkFlow360.Application.Projects.UpdateProject;

namespace WorkFlow360.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddScoped<CreateProjectCommandHandler>();
            services.AddScoped<GetProjectsQueryHandler>();
            services.AddScoped<UpdateProjectCommandHandler>();
            services.AddScoped<DeleteProjectCommandHandler>();
            services.AddScoped<GetProjectByIdQueryHandler>();

            return services;
        }
    }
}
