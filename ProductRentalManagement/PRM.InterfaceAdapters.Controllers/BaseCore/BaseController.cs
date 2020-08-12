﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRM.Domain.BaseCore;
using PRM.Domain.BaseCore.Enums;
using PRM.InterfaceAdapters.Controllers.BaseCore.Extensions;
using PRM.UseCases.BaseCore;

namespace PRM.InterfaceAdapters.Controllers.BaseCore
{
    public interface IBaseReadOnlyController<TEntity, TEntityOutput>
        where TEntity : FullAuditedEntity
        where TEntityOutput : TEntity
    {
        Task<ApiResponse<TEntityOutput>> GetById(Guid id);
        Task<ApiResponse<List<TEntityOutput>>> GetByIds(List<Guid> ids);
        Task<ApiResponse<GetAllResponse<TEntity, TEntityOutput>>> GetAll();
        
    }
    
    public class GetAllResponse<TEntity, TEntityOutput> where TEntityOutput : TEntity where TEntity : FullAuditedEntity
    {
        public List<TEntityOutput> Items { get; set; }
        public int TotalCount { get; set; }
    }
    
    public interface IBaseManipulationController<TEntity, TEntityInput, TEntityOutput> : IBaseReadOnlyController<TEntity, TEntityOutput> 
        where TEntity : FullAuditedEntity
        where TEntityOutput : TEntity
    {
        Task<ApiResponse<TEntityOutput>> Create(TEntityInput input);
        Task<ApiResponse<TEntityOutput>> Update(TEntityInput input);
        Task<ApiResponse<DeletionResponses>> Delete(Guid id);
    }

    public abstract class BaseReadOnlyController<TEntity, TEntityOutput, TIEntityUseCaseReadOnlyInteractor> : IBaseReadOnlyController<TEntity, TEntityOutput> 
        where TEntity : FullAuditedEntity
        where TEntityOutput : TEntity, new()
        where TIEntityUseCaseReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<TEntity>
    {

        private readonly TIEntityUseCaseReadOnlyInteractor _baseConsoleUseCaseManipulationReadOnlyInteractor;

        protected BaseReadOnlyController(TIEntityUseCaseReadOnlyInteractor baseConsoleUseCaseManipulationReadOnlyInteractor)
        {
            _baseConsoleUseCaseManipulationReadOnlyInteractor = baseConsoleUseCaseManipulationReadOnlyInteractor;
        }

        public async Task<ApiResponse<TEntityOutput>> GetById(Guid id)
        {
            var useCaseResult = await _baseConsoleUseCaseManipulationReadOnlyInteractor.GetById(id);
            var wasSuccessfullyExecuted = useCaseResult.Success;
            if (!wasSuccessfullyExecuted) return ApiResponses.FailureResponse(new TEntityOutput(), useCaseResult.Message);

            var entityOutput = Activator.CreateInstance(typeof(TEntityOutput), useCaseResult.Result) as TEntityOutput;
            return ApiResponses.SuccessfullyExecutedResponse(entityOutput);
        }


        public async Task<ApiResponse<List<TEntityOutput>>> GetByIds(List<Guid> ids)
        {
            var useCaseResult = await _baseConsoleUseCaseManipulationReadOnlyInteractor.GetByIds(ids);
            var wasSuccessfullyExecuted = useCaseResult.Success;

            var entityOutputs = useCaseResult.Result.Select(result => Activator.CreateInstance(typeof(TEntityOutput), result) as TEntityOutput).ToList();

            return wasSuccessfullyExecuted
                ? ApiResponses.SuccessfullyExecutedResponse(entityOutputs)
                : ApiResponses.FailureResponse(entityOutputs, useCaseResult.Message);
        }

        public async Task<ApiResponse<GetAllResponse<TEntity, TEntityOutput>>> GetAll()
        {
            var useCaseResult = await _baseConsoleUseCaseManipulationReadOnlyInteractor.GetAll();
            var wasSuccessfullyExecuted = useCaseResult.Success;

            var entityOutputs = useCaseResult.Result.Items.Select(result => Activator.CreateInstance(typeof(TEntityOutput), result) as TEntityOutput).ToList();
            
            var getAllOutput = new GetAllResponse<TEntity, TEntityOutput>
            {
                Items = entityOutputs,
                TotalCount = useCaseResult.Result.TotalCount
            };
            
            return wasSuccessfullyExecuted
                ? ApiResponses.SuccessfullyExecutedResponse(getAllOutput)
                : ApiResponses.FailureResponse(getAllOutput, useCaseResult.Message);
        }
    }

    public abstract class BaseManipulationController<TEntity, TEntityInput, TEntityOutput, TIEntityUseCaseManipulationInteractor, TIEntityReadOnlyController> : BaseReadOnlyController<TEntity, TEntityOutput, TIEntityUseCaseManipulationInteractor>,  IBaseManipulationController<TEntity, TEntityInput, TEntityOutput>
        where TEntity : FullAuditedEntity
        where TEntityOutput : TEntity, new()
        where TEntityInput : TEntity, IAmManipulationInput<TEntity>
        where TIEntityUseCaseManipulationInteractor : IBaseUseCaseManipulationInteractor<TEntity>
        where TIEntityReadOnlyController : IBaseReadOnlyController<TEntity, TEntityOutput>
    {
        private readonly TIEntityUseCaseManipulationInteractor _baseConsoleUseCaseManipulationInteractor;
        protected readonly TIEntityReadOnlyController ReadOnlyController;

        protected BaseManipulationController(TIEntityUseCaseManipulationInteractor baseConsoleUseCaseManipulationInteractor, TIEntityReadOnlyController readOnlyController) : base(baseConsoleUseCaseManipulationInteractor)
        {
            _baseConsoleUseCaseManipulationInteractor = baseConsoleUseCaseManipulationInteractor;
            ReadOnlyController = readOnlyController;
        }
        
        public async Task<ApiResponse<TEntityOutput>> Create(TEntityInput input)
        {
            var entity = input.MapToEntity();
            
            entity.Id = input.Id;
            entity.Code = input.Code;
            entity.Name = input.Name;
            entity.CreationTime = input.CreationTime;
            entity.CreatorId = input.CreatorId;
            
            var useCaseResult = await _baseConsoleUseCaseManipulationInteractor.Create(entity);
            
            var wasSuccessfullyExecuted = useCaseResult.Success;
            if (!wasSuccessfullyExecuted) return ApiResponses.FailureResponse(new TEntityOutput(), useCaseResult.Message);

            var entityOutput = Activator.CreateInstance(typeof(TEntityOutput), useCaseResult.Result) as TEntityOutput;
            return ApiResponses.SuccessfullyExecutedResponse(entityOutput);
        }

        public async Task<ApiResponse<TEntityOutput>> Update(TEntityInput input)
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
            
            var wasSuccessfullyExecuted = useCaseResult.Success;
            if (!wasSuccessfullyExecuted) return ApiResponses.FailureResponse(new TEntityOutput(), useCaseResult.Message);

            var entityOutput = Activator.CreateInstance(typeof(TEntityOutput), useCaseResult.Result) as TEntityOutput;
            return ApiResponses.SuccessfullyExecutedResponse(entityOutput);
        }

        public async Task<ApiResponse<DeletionResponses>> Delete(Guid id)
        {
            var useCaseResult = await _baseConsoleUseCaseManipulationInteractor.Delete(id);
            return !useCaseResult.Success ? DeletionResponses.DeletionFailure.GetFailureResult(useCaseResult.Result) : DeletionResponses.DeleteSuccessfully.GetSuccessResult(useCaseResult.Result);
        }
    }
}