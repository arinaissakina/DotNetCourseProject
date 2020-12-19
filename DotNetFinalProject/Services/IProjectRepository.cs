using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DotNetFinalProject.Models;

namespace DotNetFinalProject.Services
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAll();
        Task<Project> GetOne(long? id);
        void AddProject(Project project);
        void EditProject(Project project);
        Task Save();
        void RemoveProject(Project project);
        bool Exists(long? id);
        Task<List<Project>> GetProjects(Expression<Func<Project, bool>> predicate);
    }
}