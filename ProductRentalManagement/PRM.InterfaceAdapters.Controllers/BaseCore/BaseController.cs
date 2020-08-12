﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PRM.Domain.BaseCore;
using PRM.Domain.BaseCore.Enums;
using PRM.InterfaceAdapters.Controllers.BaseCore.Extensions;
using PRM.InterfaceAdapters.Presenters.BaseCore;
using PRM.InterfaceAdapters.Presenters.BaseCore.Dtos;
using PRM.UseCases.BaseCore;

namespace PRM.InterfaceAdapters.Controllers.BaseCore
{
    public interface IBaseReadOnlyController<TEntity, TEntityView>
        where TEntity : FullAuditedEntity
        where TEntityView : FullAuditedEntityView<TEntity>
    {
        Task<ApiResponse<TEntityView>> GetById(Guid id);
        Task<ApiResponse<List<TEntityView>>> GetByIds(List<Guid> ids);
        Task<ApiResponse<GetAllViewsResponse<TEntity, TEntityView>>> GetAll();
    }
    
    public interface IBaseManipulationController<TEntity, TEntityManipulationInput, TEntityView> : IBaseReadOnlyController<TEntity, TEntityView> 
        where TEntity : FullAuditedEntity
        where TEntityView : FullAuditedEntityView<TEntity>
    {
        Task<ApiResponse<TEntityView>> Create(TEntityManipulationInput input);
        Task<ApiResponse<TEntityView>> Update(TEntityManipulationInput input);
        Task<ApiResponse<DeletionResponses>> Delete(Guid id);
    }

    public abstract class BaseReadOnlyController<TEntity, TEntityView, TIEntityReadOnlyPresenter, TIEntityUseCaseReadOnlyInteractor> : IBaseReadOnlyController<TEntity, TEntityView> 
        where TEntity : FullAuditedEntity
        where TEntityView : FullAuditedEntityView<TEntity>
        where TIEntityReadOnlyPresenter : IBaseReadOnlyPresenter<TEntity, TEntityView>
        where TIEntityUseCaseReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<TEntity>
    {

        private readonly TIEntityUseCaseReadOnlyInteractor _baseConsoleUseCaseManipulationReadOnlyInteractor;
        private readonly TIEntityReadOnlyPresenter _baseConsoleManipulationReadOnlyReadOnlyPresenter;

        protected BaseReadOnlyController(TIEntityUseCaseReadOnlyInteractor baseConsoleUseCaseManipulationReadOnlyInteractor, TIEntityReadOnlyPresenter baseConsoleManipulationReadOnlyReadOnlyPresenter)
        {
            _baseConsoleUseCaseManipulationReadOnlyInteractor = baseConsoleUseCaseManipulationReadOnlyInteractor;
            _baseConsoleManipulationReadOnlyReadOnlyPresenter = baseConsoleManipulationReadOnlyReadOnlyPresenter;
        }

        public async Task<ApiResponse<TEntityView>> GetById(Guid id)
        {
            var useCaseResult = await _baseConsoleUseCaseManipulationReadOnlyInteractor.GetById(id);
            var presenterResponse = _baseConsoleManipulationReadOnlyReadOnlyPresenter.GetView(useCaseResult);
            return GetApiResponse(presenterResponse);
        }
        
        protected ApiResponse<TResult> GetApiResponse<TResult>(PresenterResult<TResult> presenterResponse)
        {
            var wasSuccessfullyExecuted = presenterResponse.Success;
            
            return wasSuccessfullyExecuted
                ? ApiResponses.SuccessfullyExecutedResponse(presenterResponse.View)
                : ApiResponses.FailureResponse(presenterResponse.View, presenterResponse.Message);
        }

        public async Task<ApiResponse<List<TEntityView>>> GetByIds(List<Guid> ids)
        {
            var useCaseResult = await _baseConsoleUseCaseManipulationReadOnlyInteractor.GetByIds(ids);
            var presenterResponse = _baseConsoleManipulationReadOnlyReadOnlyPresenter.GetViews(useCaseResult);
            return GetApiResponse(presenterResponse);
        }

        public async Task<ApiResponse<GetAllViewsResponse<TEntity, TEntityView>>> GetAll()
        {
            var useCaseResult = await _baseConsoleUseCaseManipulationReadOnlyInteractor.GetAll();
            var presenterResponse = _baseConsoleManipulationReadOnlyReadOnlyPresenter.GetAllViews(useCaseResult);
            return GetApiResponse(presenterResponse);
        }
    }

    public abstract class BaseManipulationController<TEntity, TEntityManipulationInput, TEntityView, TIEntityManipulationPresenter, TIEntityUseCaseManipulationInteractor, TIEntityReadOnlyController> : BaseReadOnlyController<TEntity, TEntityView, TIEntityManipulationPresenter, TIEntityUseCaseManipulationInteractor>,  IBaseManipulationController<TEntity, TEntityManipulationInput, TEntityView>
        where TEntity : FullAuditedEntity
        where TEntityView : FullAuditedEntityView<TEntity>
        where TEntityManipulationInput : TEntity, IAmManipulationInput<TEntity>
        where TIEntityManipulationPresenter : IBaseManipulationPresenter<TEntity, TEntityView>
        where TIEntityUseCaseManipulationInteractor : IBaseUseCaseManipulationInteractor<TEntity>
        where TIEntityReadOnlyController : IBaseReadOnlyController<TEntity, TEntityView>
    {
        private readonly TIEntityUseCaseManipulationInteractor _baseConsoleUseCaseManipulationInteractor;
        private readonly TIEntityManipulationPresenter _baseConsoleManipulationPresenter;
        protected readonly TIEntityReadOnlyController ReadOnlyController;

        protected BaseManipulationController(TIEntityUseCaseManipulationInteractor baseConsoleUseCaseManipulationInteractor, TIEntityManipulationPresenter baseConsoleManipulationPresenter, TIEntityReadOnlyController readOnlyController) : base(baseConsoleUseCaseManipulationInteractor, baseConsoleManipulationPresenter)
        {
            _baseConsoleUseCaseManipulationInteractor = baseConsoleUseCaseManipulationInteractor;
            _baseConsoleManipulationPresenter = baseConsoleManipulationPresenter;
            ReadOnlyController = readOnlyController;
        }
        
        public async Task<ApiResponse<TEntityView>> Create(TEntityManipulationInput input)
        {
            var entity = input.MapToEntity();
            
            entity.Id = input.Id;
            entity.Code = input.Code;
            entity.Name = input.Name;
            entity.CreationTime = input.CreationTime;
            entity.CreatorId = input.CreatorId;
            
            var useCaseResult = await _baseConsoleUseCaseManipulationInteractor.Create(entity);
            var presenterResponse = _baseConsoleManipulationPresenter.GetView(useCaseResult);
            return GetApiResponse(presenterResponse);
        }

        public async Task<ApiResponse<TEntityView>> Update(TEntityManipulationInput input)
        {
            var entity = input.MapToEntity();
            
            entity.Id = input.Id;
            entity.Code = input.Code;
            entity.Name = input.Name;
            entity.CreationTime = input.CreationTime;
            entity.CreatorId = input.CreatorId;
            entity.LastModificationTime = input.LastModificationTime;
            entity.LastModifierId = input.LastModifierId;
            
            var useCaseResult = await _baseConsoleUseCaseManipulationInteractor.Update(entity);
            var presenterResponse = _baseConsoleManipulationPresenter.GetView(useCaseResult);
            return GetApiResponse(presenterResponse);
        }

        public async Task<ApiResponse<DeletionResponses>> Delete(Guid id)
        {
            var useCaseResult = await _baseConsoleUseCaseManipulationInteractor.Delete(id);
            var presenterResponse = _baseConsoleManipulationPresenter.GetDeletionView(useCaseResult);
            return GetApiResponse(presenterResponse);
        }
    }
}