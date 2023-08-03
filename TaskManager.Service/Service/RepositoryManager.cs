using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Domain;
using TaskManager.Data.Data;
using TaskManager.Service.Interface;

namespace TaskManager.Service.Service
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;

        private IUserAuthenticationRepository _userAuthenticationRepository;
        private UserManager<User> _userManager;
        private IMapper _mapper;
        private IConfiguration _configuration;
        private ITaskRepository _taskRepository;
        private IUserRoleManager _userRoleManager;
        private RoleManager<Role> _roleManager;

        public RepositoryManager(RepositoryContext repositoryContext, UserManager<User> userManager,
            IMapper mapper, IConfiguration configuration, RoleManager<Role> roleManager)
        {
            _repositoryContext = repositoryContext;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        public ITaskRepository Task
        {
            get
            {
                if (_taskRepository is null)
                    _taskRepository = new TaskRepository(_repositoryContext, _mapper, _userManager);
                return _taskRepository;
            }
        }
        public IUserAuthenticationRepository UserAuthentication
        {
            get
            {
                if (_userAuthenticationRepository is null)
                    _userAuthenticationRepository = new UserAuthenticationRepository(_userManager, _configuration, _mapper);
                return _userAuthenticationRepository;
            }
        }

        public IUserRoleManager RoleManager {
            get
            {
                if (_userRoleManager is null)
                    _userRoleManager = new UserRoleManager(_userManager, _configuration, _mapper, _roleManager);
                return _userRoleManager;
            }
        }

        public System.Threading.Tasks.Task SaveAsync() => _repositoryContext.SaveChangesAsync();
    }
}
