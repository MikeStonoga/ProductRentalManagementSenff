using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRM.Domain.BaseCore.Enums;
using PRM.Infrastructure.Authentication.Users.Dtos;
using PRM.Infrastructure.Authentication.Users.Enums;
using PRM.Infrastructure.Authentication.Users.UseCases;
using PRM.InterfaceAdapters.Controllers.BaseCore;
using PRM.InterfaceAdapters.Controllers.BaseCore.Extensions;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;

namespace PRM.Infrastructure.Authentication.Users.Controllers
{
    public interface IUserReadOnlyController : IBaseReadOnlyController<User, UserOutput>
    {
        
    }
    public interface IUserController : IBaseManipulationController<User, UserInput, UserOutput>
    {
    }

    public class UserReadOnlyController : BaseReadOnlyController<User, UserOutput, IUserUseCasesReadOnlyInteractor>, IUserReadOnlyController
    {
        public UserReadOnlyController(IUserUseCasesReadOnlyInteractor useCaseReadOnlyInteractor) : base(useCaseReadOnlyInteractor)
        {
        }
    }
    
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class UserController : BaseManipulationController<User, UserInput, UserOutput, IUserUseCasesManipulationInteractor, IUserReadOnlyController>, IUserController
    {
        private readonly IReadOnlyPersistenceGateway<User> _users;
        public UserController(IUserUseCasesManipulationInteractor useCaseInteractor, IUserReadOnlyController readOnlyController, IReadOnlyPersistenceGateway<User> users) : base(useCaseInteractor, readOnlyController)
        {
            _users = users;
        }
        
        [HttpGet("{id}")]
        public override async Task<ApiResponse<UserOutput>> GetById([FromQuery] Guid id)
        {
            return await base.GetById(id);
        }
        
        [HttpPost]
        public override async Task<ApiResponse<List<UserOutput>>> GetByIds([FromBody] List<Guid> ids)
        {
            return await base.GetByIds(ids);
        }

        [HttpGet]
        public override async Task<ApiResponse<GetAllResponse<User, UserOutput>>> GetAll()
        {
            return await base.GetAll(null);
        }
        
        [HttpPost]
        public new async Task<ApiResponse<UserOutput>> Create([FromBody] UserInput input)
        {
            var isPasswordConfirmed = input.Password == input.PasswordConfirmation;
            if (!isPasswordConfirmed)
            {
                return ApiResponses.Failure<UserOutput>("Passwords doesnt matches");
            }
            
            input.Id = Guid.NewGuid();
            var userId = User.Claims.ToList()[2];
            input.CreatorId = Guid.Parse(userId.Value);
            input.Role = UserRoles.NonAdmin;
            return await base.Create(input);
        }

        [HttpPut]
        public new async Task<ApiResponse<UserOutput>> Update([FromBody] UserInput entityToUpdate)
        {
            var user = await _users.GetById(entityToUpdate.Id);
            if (!user.Success) return ApiResponses.Failure<UserOutput>("User doesnt exists");

            var isUpdatingPassword = entityToUpdate.Password != user.Response.Password;
            if (isUpdatingPassword)
            {
                var isPasswordConfirmed = entityToUpdate.Password == entityToUpdate.PasswordConfirmation;
                if (!isPasswordConfirmed)
                {
                    return ApiResponses.Failure<UserOutput>("Passwords doesnt matches");
                }
            }
            
            var userId = User.Claims.ToList()[2];
            entityToUpdate.LastModifierId = Guid.Parse(userId.Value);
            return await base.Update(entityToUpdate);
        }

        [HttpDelete("{id}")]
        public new async Task<ApiResponse<DeletionResponses>> Delete([FromQuery] Guid id)
        {
            return await base.Delete(id);
        }
    }
}