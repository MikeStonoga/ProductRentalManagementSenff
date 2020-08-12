﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PRM.Domain.BaseCore;
 using PRM.Domain.BaseCore.Enums;
 using PRM.InterfaceAdapters.Controllers.BaseCore;
using PRM.InterfaceAdapters.Presenters.BaseCore;
 using PRM.InterfaceAdapters.Presenters.BaseCore.Dtos;
 using PRM.UseCases.BaseCore;

namespace PRM.Infrastructure.ApplicationDelivery.WebApiHost.BaseCore
{
    public interface IBaseReadOnlyWebController<TEntity, TEntityView> : IBaseReadOnlyController<TEntity, TEntityView>
        where TEntity : FullAuditedEntity
        where TEntityView : FullAuditedEntityView<TEntity>
    {
    }
    
    [ApiController]
    [Route("[controller]/[action]")]
    public abstract class BaseReadOnlyWebController<TEntity, TEntityView, TIEntityReadOnlyPresenter, TIEntityUseCaseReadOnlyInteractor> : BaseReadOnlyController<TEntity, TEntityView, TIEntityReadOnlyPresenter, TIEntityUseCaseReadOnlyInteractor>
        where TEntity : FullAuditedEntity
        where TEntityView : FullAuditedEntityView<TEntity>
        where TIEntityReadOnlyPresenter : IBaseReadOnlyPresenter<TEntity, TEntityView>
        where TIEntityUseCaseReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<TEntity>
    {
        
        public BaseReadOnlyWebController(TIEntityUseCaseReadOnlyInteractor baseConsoleUseCaseReadOnlyInteractor, TIEntityReadOnlyPresenter baseConsoleManipulationReadOnlyReadOnlyPresenter) : base(baseConsoleUseCaseReadOnlyInteractor, baseConsoleManipulationReadOnlyReadOnlyPresenter)
        {
        }

        [HttpGet("{id}")]
        public new async Task<ApiResponse<TEntityView>> GetById([FromQuery] Guid id)
        {
            return await base.GetById(id);
        }
        
        [HttpPost]
        public new async Task<ApiResponse<List<TEntityView>>> GetByIds([FromBody] List<Guid> ids)
        {
            return await base.GetByIds(ids);
        }

        [HttpGet]
        public new async Task<ApiResponse<GetAllViewsResponse<TEntity, TEntityView>>> GetAll()
        {
            return await base.GetAll();
        }

   
    }
    
    public interface IBaseManipulationWebController<TEntity, TEntityManipulationInput, TEntityView> :  IBaseManipulationController<TEntity, TEntityManipulationInput, TEntityView>
        where TEntity : FullAuditedEntity
        where TEntityView : FullAuditedEntityView<TEntity>
        where TEntityManipulationInput : TEntity, IAmManipulationInput<TEntity>
    {
    }

    [ApiController]
    [Route("[controller]/[action]")]
    public abstract class BaseManipulationWebController<TEntity, TEntityInput, TEntityManipulationInput, TEntityView, TIEntityManipulationPresenter, TIEntityUseCaseManipulationInteractor, TIEntityReadOnlyController> : BaseManipulationController<TEntity, TEntityManipulationInput, TEntityView, TIEntityManipulationPresenter, TIEntityUseCaseManipulationInteractor, TIEntityReadOnlyController>, IBaseManipulationWebController<TEntity, TEntityManipulationInput, TEntityView>
        where TEntity : FullAuditedEntity
        where TEntityView : FullAuditedEntityView<TEntity>
        where TEntityManipulationInput : TEntity, IAmManipulationInput<TEntity>
        where TEntityInput : TEntityView, IAmWebManipulationInput<TEntity, TEntityManipulationInput>, new()
        where TIEntityManipulationPresenter : IBaseManipulationPresenter<TEntity, TEntityView>
        where TIEntityUseCaseManipulationInteractor : IBaseUseCaseManipulationInteractor<TEntity>
        where TIEntityReadOnlyController : IBaseReadOnlyWebController<TEntity, TEntityView>
    {
        public BaseManipulationWebController(TIEntityUseCaseManipulationInteractor baseConsoleUseCaseManipulationInteractor, TIEntityManipulationPresenter baseConsoleManipulationPresenter, TIEntityReadOnlyController readOnlyController) : base(baseConsoleUseCaseManipulationInteractor, baseConsoleManipulationPresenter, readOnlyController)
        {
        }

        [HttpPost]
        public async Task<ApiResponse<TEntityView>> Create([FromBody] TEntityInput input)
        {
            var createInput = input.MapToManipulationInput();
            createInput.Id = Guid.NewGuid();
            createInput.Name = input.Name;
            createInput.Code = input.Code;
            
            return await base.Create(createInput);
        }

        [HttpPut]
        public async Task<ApiResponse<TEntityView>> Update([FromBody] TEntityInput input)
        {
            var updateInput = input.MapToManipulationInput();
            updateInput.Id = Guid.Parse(input.Id);
            updateInput.Name = input.Name;
            updateInput.Code = input.Code;
            
            return await base.Update(updateInput);
        }

        [HttpDelete("{id}")]
        public new async Task<ApiResponse<DeletionResponses>> Delete([FromQuery] Guid id)
        {
            return await base.Delete(id);
        }
    }
}