using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PRM.Domain.BaseCore;
using PRM.Domain.BaseCore.Enums;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore.Dtos;

namespace PRM.Infrastructure.Persistence.MySQL.BaseCore
{
    public interface IReadOnlyRepository<TEntity> : IReadOnlyPersistenceGateway<TEntity> where TEntity : FullAuditedEntity
    {
    }
    
    public class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : FullAuditedEntity
    {
        public Task<PersistenceResponse<TEntity>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PersistenceResponse<List<TEntity>>> GetByIds(List<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<PersistenceResponse<GetAllResponse<TEntity>>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
    
    public interface IRepository<TEntity> : IManipulationPersistenceGateway<TEntity> where TEntity : FullAuditedEntity
    {
    }

    public class Repository<TEntity> : IRepository<TEntity> 
        where TEntity : FullAuditedEntity
    {

        private readonly IReadOnlyRepository<TEntity> _readOnlyRepository;

        public Repository(IReadOnlyRepository<TEntity> readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task<PersistenceResponse<TEntity>> GetById(Guid id)
        {
            return await _readOnlyRepository.GetById(id);
        }

        public async Task<PersistenceResponse<List<TEntity>>> GetByIds(List<Guid> ids)
        {
            return await _readOnlyRepository.GetByIds(ids);
        }

        public async Task<PersistenceResponse<GetAllResponse<TEntity>>> GetAll()
        {
            return await _readOnlyRepository.GetAll();
        }

        public Task<PersistenceResponse<TEntity>> Create(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<PersistenceResponse<TEntity>> Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<PersistenceResponse<DeletionResponses>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
    
}