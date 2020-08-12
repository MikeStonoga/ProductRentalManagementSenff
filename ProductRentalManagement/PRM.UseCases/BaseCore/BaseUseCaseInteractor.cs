﻿using System;
using System.Collections.Generic;
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
        Task<UseCaseResult<GetAllResponse<TEntity>>> GetAll();
    }
    public interface IBaseUseCaseManipulationInteractor<TEntity> : IBaseUseCaseReadOnlyInteractor<TEntity>
        where TEntity : Entity
    {
        Task<UseCaseResult<TEntity>> Create(TEntity entity);
        Task<UseCaseResult<TEntity>> Update(TEntity entity);
        Task<UseCaseResult<DeletionResponses>> Delete(Guid id);
    }

    public class BaseUseCaseReadOnlyInteractor<TEntity, TIEntityReadOnlyPersistenceGateway> : IBaseUseCaseReadOnlyInteractor<TEntity>
        where TEntity : FullAuditedEntity
        where TIEntityReadOnlyPersistenceGateway : IBaseReadOnlyPersistenceGateway<TEntity>
    {
        private readonly TIEntityReadOnlyPersistenceGateway _baseReadOnlyPersistenceGateway;

        public BaseUseCaseReadOnlyInteractor(TIEntityReadOnlyPersistenceGateway baseReadOnlyPersistenceGateway)
        {
            _baseReadOnlyPersistenceGateway = baseReadOnlyPersistenceGateway;
        }
        

        public async Task<UseCaseResult<TEntity>> GetById(Guid id)
        {
            var persistenceResponse = await _baseReadOnlyPersistenceGateway.GetById(id);

            return GetUseCaseResult(persistenceResponse);
        }

        protected UseCaseResult<TResult> GetUseCaseResult<TResult>(PersistenceResponse<TResult> persistenceResponse)
        {
            var wasSuccessfullyExecuted = persistenceResponse.Success;
            
            return wasSuccessfullyExecuted
                ? UseCasesResponses.UseCaseSuccessfullyExecutedResponse(persistenceResponse.Response)
                : UseCasesResponses.PersistenceErrorResponse(persistenceResponse.Response, persistenceResponse.Message);
        }

        public async Task<UseCaseResult<List<TEntity>>> GetByIds(List<Guid> ids)
        {
            var persistenceResponse = await _baseReadOnlyPersistenceGateway.GetByIds(ids);
            return GetUseCaseResult(persistenceResponse);
        }

        public async Task<UseCaseResult<GetAllResponse<TEntity>>> GetAll()
        {
            var persistenceResponse = await _baseReadOnlyPersistenceGateway.GetAll();
            return GetUseCaseResult(persistenceResponse);
        }
    }
    
    public class BaseUseCaseManipulationInteractor<TEntity, TIEntityUseCasesReadOnlyInteractor, TIEntityManipulationPersistenceGateway> : BaseUseCaseReadOnlyInteractor<TEntity, TIEntityManipulationPersistenceGateway>, IBaseUseCaseManipulationInteractor<TEntity>
        where TEntity : FullAuditedEntity
        where TIEntityManipulationPersistenceGateway : IBaseManipulationPersistenceGateway<TEntity>
        where TIEntityUseCasesReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<TEntity>
    {
        protected readonly TIEntityUseCasesReadOnlyInteractor UseCasesReadOnlyInteractor;
        private readonly TIEntityManipulationPersistenceGateway _basePersistenceGateway;
        
        public BaseUseCaseManipulationInteractor(TIEntityManipulationPersistenceGateway basePersistenceGateway, TIEntityUseCasesReadOnlyInteractor useCasesReadOnlyInteractor) : base(basePersistenceGateway)
        {
            _basePersistenceGateway = basePersistenceGateway;
            UseCasesReadOnlyInteractor = useCasesReadOnlyInteractor;
        }
        
        
        public async Task<UseCaseResult<TEntity>> Create(TEntity entity)
        {
            var persistenceResponse = await _basePersistenceGateway.Create(entity);
            return GetUseCaseResult(persistenceResponse);
        }

        public async Task<UseCaseResult<TEntity>> Update(TEntity entity)
        {
            var persistenceResponse = await _basePersistenceGateway.Update(entity);
            return GetUseCaseResult(persistenceResponse);
        }

        public async Task<UseCaseResult<DeletionResponses>> Delete(Guid id)
        {
            var persistenceResponse = await _basePersistenceGateway.Delete(id);
            return GetUseCaseResult(persistenceResponse);
        }
    }
}