using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PRM.Domain.BaseCore.Enums;
using PRM.Infrastructure.Authentication.Users.Dtos;
using PRM.Infrastructure.Authentication.Users.UseCases;
using PRM.InterfaceAdapters.Controllers.BaseCore;

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
    [Route("[controller]/[action]")]
    public class UserController : BaseManipulationController<User, UserInput, UserOutput, IUserUseCasesManipulationInteractor, IUserReadOnlyController>, IUserController
    {
        public UserController(IUserUseCasesManipulationInteractor useCaseInteractor, IUserReadOnlyController readOnlyController) : base(useCaseInteractor, readOnlyController)
        {
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
            input.Id = Guid.NewGuid();
            return await base.Create(input);
        }

        [HttpPut]
        public new async Task<ApiResponse<UserOutput>> Update([FromBody] UserInput input)
        {
            return await base.Update(input);
        }

        [HttpDelete("{id}")]
        public new async Task<ApiResponse<DeletionResponses>> Delete([FromQuery] Guid id)
        {
            return await base.Delete(id);
        }
    }
}