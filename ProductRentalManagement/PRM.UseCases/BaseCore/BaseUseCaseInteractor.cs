﻿using System;
using System.Collections.Generic;
 using System.Linq.Expressions;
 using System.Threading.Tasks;
using PRM.Domain.BaseCore;
 using PRM.Domain.BaseCore.Enums;
 using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
 using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore.Dtos;
 using PRM.UseCases.BaseCore.Extensions;

namespace PRM.UseCases.BaseCore
{
    public interface IBaseUseCaseReadOnlyInteractor<TEntity>
        where TEntity : Entity
    {
        Task<UseCaseResult<TEntity>> GetById(Guid id);
        Task<UseCaseResult<List<TEntity>>> GetByIds(List<Guid> ids);
        Task<UseCaseResult<GetAllResponse<TEntity>>> GetAll(Expression<Func<TEntity, object>> includePredicate = null);
    }
    public interface IBaseUseCaseManipulationInteractor<TEntity> : IBaseUseCaseReadOnlyInteractor<TEntity>
        where TEntity : Entity
    {
        Task<UseCaseResult<TEntity>> Create(TEntity entity);
        Task<UseCaseResult<TEntity>> Update(TEntity entity);
        Task<UseCaseResult<DeletionResponses>> Delete(Guid id);
    }

    public class BaseUseCaseReadOnlyInteractor<TEntity> : IBaseUseCaseReadOnlyInteractor<TEntity>
        where TEntity : FullAuditedEntity
    {
        private readonly IReadOnlyPersistenceGateway<TEntity> _baseReadOnlyPersistenceGateway;

        public BaseUseCaseReadOnlyInteractor(IReadOnlyPersistenceGateway<TEntity> baseReadOnlyPersistenceGateway)
        {
            _baseReadOnlyPersistenceGateway = baseReadOnlyPersistenceGateway;
        }
        

        public virtual async Task<UseCaseResult<TEntity>> GetById(Guid id)
        {
            var persistenceResponse = await _baseReadOnlyPersistenceGateway.GetById(id);

            return GetUseCaseResult(persistenceResponse);
        }

        protected UseCaseResult<TResult> GetUseCaseResult<TResult>(PersistenceResponse<TResult> persistenceResponse)
        {
            var wasSuccessfullyExecuted = persistenceResponse.Success;
            
            return wasSuccessfullyExecuted
                ? UseCasesResponses.SuccessfullyExecutedResponse(persistenceResponse.Response)
                : UseCasesResponses.PersistenceErrorResponse(persistenceResponse.Response, persistenceResponse.Message);
        }

        public virtual async Task<UseCaseResult<List<TEntity>>> GetByIds(List<Guid> ids)
        {
            var persistenceResponse = await _baseReadOnlyPersistenceGateway.GetByIds(ids);
            return GetUseCaseResult(persistenceResponse);
        }

        public async Task<UseCaseResult<GetAllResponse<TEntity>>> GetAll(Expression<Func<TEntity, object>> includePredicate = null)
        {
            var persistenceResponse = await _baseReadOnlyPersistenceGateway.GetAll(includePredicate);
            return GetUseCaseResult(persistenceResponse);
        }
    }
    
    public class BaseUseCaseManipulationInteractor<TEntity, TIEntityUseCasesReadOnlyInteractor> : BaseUseCaseReadOnlyInteractor<TEntity>, IBaseUseCaseManipulationInteractor<TEntity>
        where TEntity : FullAuditedEntity
        where TIEntityUseCasesReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<TEntity>
    {
        protected readonly TIEntityUseCasesReadOnlyInteractor UseCasesReadOnlyInteractor;
        private readonly IManipulationPersistenceGateway<TEntity> _basePersistenceGateway;
        
        public BaseUseCaseManipulationInteractor(IManipulationPersistenceGateway<TEntity> basePersistenceGateway, TIEntityUseCasesReadOnlyInteractor useCasesReadOnlyInteractor) : base(basePersistenceGateway)
        {
            _basePersistenceGateway = basePersistenceGateway;
            UseCasesReadOnlyInteractor = useCasesReadOnlyInteractor;
        }
        
        
        public virtual async Task<UseCaseResult<TEntity>> Create(TEntity entity)
        {
            var persistenceResponse = await _basePersistenceGateway.Create(entity);
            return GetUseCaseResult(persistenceResponse);
        }

        public virtual async Task<UseCaseResult<TEntity>> Update(TEntity entity)
        {
            var persistenceResponse = await _basePersistenceGateway.Update(entity);
            return GetUseCaseResult(persistenceResponse);
        }

        public virtual async Task<UseCaseResult<DeletionResponses>> Delete(Guid id)
        {
            var persistenceResponse = await _basePersistenceGateway.Delete(id);
            return GetUseCaseResult(persistenceResponse);
        }
    }
}