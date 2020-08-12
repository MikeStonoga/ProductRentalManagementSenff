using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRM.Domain.BaseCore;
using PRM.Domain.BaseCore.Enums;
using PRM.Infrastructure.Persistence.MySQL.EntityFrameworkCore;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore.Dtos;

namespace PRM.Infrastructure.Persistence.MySQL.BaseCore
{
    public interface IReadOnlyRepository<TEntity> : IReadOnlyPersistenceGateway<TEntity> where TEntity : FullAuditedEntity
    {
    }
    
    public class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : FullAuditedEntity
    {
        private readonly PrmDbContext _db;

        public ReadOnlyRepository(PrmDbContext db)
        {
            _db = db;
        }

        public async Task<PersistenceResponse<TEntity>> GetById(Guid id)
        {
            var entity = await _db.Set<TEntity>().FindAsync(id);
            
            return new PersistenceResponse<TEntity>
            {
                Success = true,
                ErrorCodeName = "Success",
                Message = "Success",
                Response = entity
            };
        }

        public async Task<PersistenceResponse<List<TEntity>>> GetByIds(List<Guid> ids)
        {
            var entities = await _db.Set<TEntity>()
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();;
            
            return new PersistenceResponse<List<TEntity>>
            {
                Success = true,
                ErrorCodeName = "Success",
                Message = "Success",
                Response = entities
            };
        }

        public async Task<PersistenceResponse<GetAllResponse<TEntity>>> GetAll()
        {
            var all = await _db.Set<TEntity>().ToListAsync();

            return new PersistenceResponse<GetAllResponse<TEntity>>
            {
                Success = true,
                ErrorCodeName = "Success",
                Message = "Success",
                Response = new GetAllResponse<TEntity>
                {
                    Items = all,
                    TotalCount = all.Count
                }
            };
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