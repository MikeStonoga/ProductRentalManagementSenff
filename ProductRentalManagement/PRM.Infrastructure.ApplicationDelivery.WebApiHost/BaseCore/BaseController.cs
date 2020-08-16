﻿﻿using System;
using System.Collections.Generic;
 using System.Linq;
 using System.Threading.Tasks;
 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Mvc;
using PRM.Domain.BaseCore;
 using PRM.Domain.BaseCore.Enums;
 using PRM.InterfaceAdapters.Controllers.BaseCore;
 using PRM.UseCases.BaseCore;

namespace PRM.Infrastructure.ApplicationDelivery.WebApiHost.BaseCore
{
    public interface IBaseReadOnlyWebController<TEntity, TEntityOutput> : IBaseReadOnlyController<TEntity, TEntityOutput>
        where TEntity : FullAuditedEntity
        where TEntityOutput : TEntity, new()
    {
    }
    
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public abstract class BaseReadOnlyWebController<TEntity, TEntityOutput, TIEntityReadOnlyController, TIEntityUseCaseReadOnlyInteractor> : BaseReadOnlyController<TEntity, TEntityOutput, TIEntityUseCaseReadOnlyInteractor>, IBaseReadOnlyWebController<TEntity, TEntityOutput>
        where TEntity : FullAuditedEntity
        where TEntityOutput : TEntity, new()
        where TIEntityUseCaseReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<TEntity>
        where TIEntityReadOnlyController : IBaseReadOnlyController<TEntity, TEntityOutput>
    {

        protected TIEntityReadOnlyController ReadOnlyController; 
        
        public BaseReadOnlyWebController(TIEntityUseCaseReadOnlyInteractor useCaseReadOnlyInteractor, TIEntityReadOnlyController readOnlyController) : base(useCaseReadOnlyInteractor)
        {
            ReadOnlyController = readOnlyController;
        }
        
        [HttpGet("{id}")]
        public override async Task<ApiResponse<TEntityOutput>> GetById([FromQuery] Guid id)
        {
            return await base.GetById(id);
        }
        
        [HttpPost]
        public override async Task<ApiResponse<List<TEntityOutput>>> GetByIds([FromBody] List<Guid> ids)
        {
            return await base.GetByIds(ids);
        }

        [HttpGet]
        public override async Task<ApiResponse<GetAllResponse<TEntity, TEntityOutput>>> GetAll()
        {
            return await base.GetAll(null);
        }
        
   
    }
    
    public interface IBaseManipulationWebController<TEntity, TEntityInput, TEntityOutput> :  IBaseManipulationController<TEntity, TEntityInput, TEntityOutput>
        where TEntity : FullAuditedEntity
        where TEntityOutput : TEntity
        where TEntityInput : TEntity, IAmManipulationInput<TEntity>
    {
    }

    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public abstract class BaseManipulationWebController<TEntity, TEntityInput, TEntityOutput, TIEntityUseCaseManipulationInteractor, TIEntityManipulationController> : BaseManipulationController<TEntity, TEntityInput, TEntityOutput, TIEntityUseCaseManipulationInteractor, TIEntityManipulationController>, IBaseManipulationWebController<TEntity, TEntityInput, TEntityOutput>
        where TEntity : FullAuditedEntity
        where TEntityOutput : TEntity, new()
        where TEntityInput : TEntity, IAmManipulationInput<TEntity>, new()
        where TIEntityUseCaseManipulationInteractor : IBaseUseCaseManipulationInteractor<TEntity>
        where TIEntityManipulationController : IBaseManipulationController<TEntity, TEntityInput, TEntityOutput>
    {
        protected readonly TIEntityManipulationController ManipulationController;
        
        public BaseManipulationWebController(TIEntityUseCaseManipulationInteractor useCaseInteractor, TIEntityManipulationController manipulationController) : base(useCaseInteractor, manipulationController)
        {
            ManipulationController = manipulationController;
        }
        
        


        [HttpPost]
        public new async Task<ApiResponse<TEntityOutput>> Create([FromBody] TEntityInput input)
        {
            input.Id = Guid.NewGuid();
            input.CreationTime = DateTime.Now;
            var userId = User.Claims.ToList()[2];
            input.CreatorId = Guid.Parse(userId.Value);
            return await base.Create(input);
        }

        [HttpPut]
        public new async Task<ApiResponse<TEntityOutput>> Update([FromBody] TEntityInput input)
        {
            input.LastModificationTime = DateTime.Now;
            var userId = User.Claims.ToList()[2];
            input.LastModifierId = Guid.Parse(userId.Value);
            return await base.Update(input);
        }

        [HttpDelete("{id}")]
        public new async Task<ApiResponse<DeletionResponses>> Delete([FromQuery] Guid id)
        {
            return await base.Delete(id);
        }
    }
}