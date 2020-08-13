﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PRM.Domain.BaseCore;
using PRM.Domain.BaseCore.Enums;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore.Dtos;

namespace PRM.InterfaceAdapters.Gateways.Persistence.BaseCore
{
    public interface IReadOnlyPersistenceGateway<TEntity>
        where TEntity : FullAuditedEntity
    {
        Task<PersistenceResponse<TEntity>> GetById(Guid id);
        Task<PersistenceResponse<List<TEntity>>> GetByIds(List<Guid> ids);
        Task<PersistenceResponse<GetAllResponse<TEntity>>> GetAll(Expression<Func<TEntity, object>> includePredicate = null);
        Task<PersistenceResponse<TEntity>> First(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> includePredicate = null);
    }
    
    public interface IManipulationPersistenceGateway<TEntity> : IReadOnlyPersistenceGateway<TEntity>
        where TEntity : FullAuditedEntity
    {
        Task<PersistenceResponse<TEntity>> Create(TEntity entity);
        Task<PersistenceResponse<TEntity>> Update(TEntity entity);
        Task<PersistenceResponse<DeletionResponses>> Delete(Guid id);
    }
}