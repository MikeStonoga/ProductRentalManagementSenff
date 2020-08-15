﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
 using System.Linq.Expressions;
 using System.Threading.Tasks;
 using Microsoft.AspNetCore.Mvc;
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

    public abstract class BaseReadOnlyController<TEntity, TEntityOutput, TIEntityUseCaseReadOnlyInteractor> : ControllerBase, IBaseReadOnlyController<TEntity, TEntityOutput> 
        where TEntity : FullAuditedEntity
        where TEntityOutput : TEntity, new()
        where TIEntityUseCaseReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<TEntity>
    {

        protected readonly TIEntityUseCaseReadOnlyInteractor UseCaseReadOnlyInteractor;

        protected BaseReadOnlyController(TIEntityUseCaseReadOnlyInteractor useCaseReadOnlyInteractor)
        {
            UseCaseReadOnlyInteractor = useCaseReadOnlyInteractor;
        }

        public virtual async Task<ApiResponse<TEntityOutput>> GetById(Guid id)
        {
            var useCaseResult = await UseCaseReadOnlyInteractor.GetById(id);
            var wasSuccessfullyExecuted = useCaseResult.Success;
            if (!wasSuccessfullyExecuted) return ApiResponses.FailureResponse(new TEntityOutput(), useCaseResult.Message);

            var entityOutput = Activator.CreateInstance(typeof(TEntityOutput), useCaseResult.Result) as TEntityOutput;
            return ApiResponses.SuccessfullyExecutedResponse(entityOutput);
        }


        public virtual async Task<ApiResponse<List<TEntityOutput>>> GetByIds(List<Guid> ids)
        {
            var useCaseResult = await UseCaseReadOnlyInteractor.GetByIds(ids);
            var wasSuccessfullyExecuted = useCaseResult.Success;

            var entityOutputs = useCaseResult.Result.Select(result => Activator.CreateInstance(typeof(TEntityOutput), result) as TEntityOutput).ToList();

            return wasSuccessfullyExecuted
                ? ApiResponses.SuccessfullyExecutedResponse(entityOutputs)
                : ApiResponses.FailureResponse(entityOutputs, useCaseResult.Message);
        }

        public virtual async Task<ApiResponse<GetAllResponse<TEntity, TEntityOutput>>> GetAll()
        {
            return await GetAll(null);
        }
        
        protected async Task<ApiResponse<GetAllResponse<TEntity, TEntityOutput>>> GetAll(Expression<Func<TEntity, object>> includePredicate = null)
        {
            var useCaseResult = await UseCaseReadOnlyInteractor.GetAll(includePredicate);
            var wasSuccessfullyExecuted = useCaseResult.Success;
            if (!wasSuccessfullyExecuted)
            {
                var failureResult = new GetAllResponse<TEntity, TEntityOutput>
                {
                    Items = new List<TEntityOutput>(),
                    TotalCount = 0
                };
                return ApiResponses.FailureResponse(failureResult, useCaseResult.Message);
            }
            
            var entityOutputs = useCaseResult.Result.Items.Select(result => Activator.CreateInstance(typeof(TEntityOutput), result) as TEntityOutput).ToList();
            
            var getAllOutput = new GetAllResponse<TEntity, TEntityOutput>
            {
                Items = entityOutputs,
                TotalCount = useCaseResult.Result.TotalCount
            };
            
            return ApiResponses.SuccessfullyExecutedResponse(getAllOutput);
        }
        
        protected async Task<ApiResponse<TOutput>> GetUseCaseExecutionResponse<TUseCaseRequirement, TUseCaseResult, TInput, TOutput>(Func<TUseCaseRequirement, Task<UseCaseResult<TUseCaseResult>>> useCase, TInput input)
            where TInput : TUseCaseRequirement 
            where TOutput : class, TUseCaseResult, new()
        {
            var useCaseResponse = await useCase(input);
            if (!useCaseResponse.Success) return ApiResponses.FailureResponse<TOutput>(useCaseResponse.Message);
            
            var output = Activator.CreateInstance(typeof(TOutput), useCaseResponse.Result) as TOutput;
            return ApiResponses.SuccessfullyExecutedResponse(output, useCaseResponse.Message);
        }
    }

    public abstract class BaseManipulationController<TEntity, TEntityInput, TEntityOutput, TIEntityUseCaseManipulationInteractor, TIEntityReadOnlyController> : BaseReadOnlyController<TEntity, TEntityOutput, TIEntityUseCaseManipulationInteractor>,  IBaseManipulationController<TEntity, TEntityInput, TEntityOutput>
        where TEntity : FullAuditedEntity
        where TEntityOutput : TEntity, new()
        where TEntityInput : TEntity, IAmManipulationInput<TEntity>
        where TIEntityUseCaseManipulationInteractor : IBaseUseCaseManipulationInteractor<TEntity>
        where TIEntityReadOnlyController : IBaseReadOnlyController<TEntity, TEntityOutput>
    {
        protected readonly TIEntityUseCaseManipulationInteractor UseCaseInteractor;
        protected readonly TIEntityReadOnlyController ReadOnlyController;

        protected BaseManipulationController(TIEntityUseCaseManipulationInteractor useCaseInteractor, TIEntityReadOnlyController readOnlyController) : base(useCaseInteractor)
        {
            UseCaseInteractor = useCaseInteractor;
            ReadOnlyController = readOnlyController;
        }
        
        public virtual async Task<ApiResponse<TEntityOutput>> Create(TEntityInput input)
        {
            var entity = input.MapToEntity();
            
            entity.Id = input.Id;
            entity.Code = input.Code;
            entity.Name = input.Name;
            entity.CreationTime = input.CreationTime;
            entity.CreatorId = input.CreatorId;
            
            var useCaseResult = await UseCaseInteractor.Create(entity);
            
            var wasSuccessfullyExecuted = useCaseResult.Success;
            if (!wasSuccessfullyExecuted) return ApiResponses.FailureResponse(new TEntityOutput(), useCaseResult.Message);

            var entityOutput = Activator.CreateInstance(typeof(TEntityOutput), useCaseResult.Result) as TEntityOutput;
            return ApiResponses.SuccessfullyExecutedResponse(entityOutput);
        }

        public virtual async Task<ApiResponse<TEntityOutput>> Update(TEntityInput input)
        {
            var entity = input.MapToEntity();
            
            entity.Id = input.Id;
            entity.Code = input.Code;
            entity.Name = input.Name;
            entity.CreationTime = input.CreationTime;
            entity.CreatorId = input.CreatorId;
            entity.LastModificationTime = input.LastModificationTime;
            entity.LastModifierId = input.LastModifierId;
            
            var useCaseResult = await UseCaseInteractor.Update(entity);
            
            var wasSuccessfullyExecuted = useCaseResult.Success;
            if (!wasSuccessfullyExecuted) return ApiResponses.FailureResponse(new TEntityOutput(), useCaseResult.Message);

            var entityOutput = Activator.CreateInstance(typeof(TEntityOutput), useCaseResult.Result) as TEntityOutput;
            return ApiResponses.SuccessfullyExecutedResponse(entityOutput);
        }

        public virtual async Task<ApiResponse<DeletionResponses>> Delete(Guid id)
        {
            var useCaseResult = await UseCaseInteractor.Delete(id);
            return !useCaseResult.Success ? DeletionResponses.DeletionFailure.GetFailureResult(useCaseResult.Result) : DeletionResponses.DeleteSuccessfully.GetSuccessResult(useCaseResult.Result);
        }
    }
}