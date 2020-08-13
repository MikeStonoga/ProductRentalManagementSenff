using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PRM.Domain.BaseCore;
using PRM.Domain.BaseCore.Enums;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore.Dtos;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore.Enums;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore.Extensions;

namespace PRM.Infrastructure.Persistence.MySQL.BaseCore
{
    public interface IReadOnlyRepository<TEntity> : IReadOnlyPersistenceGateway<TEntity> where TEntity : FullAuditedEntity
    {
    }
    
    public class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : FullAuditedEntity, new()
    {
        private readonly ICurrentDbContext _database;

        public ReadOnlyRepository(ICurrentDbContext database)
        {
            _database = database;
        }

        public async Task<PersistenceResponse<TEntity>> GetById(Guid id)
        {
            try
            {
                var entity = await _database.Context.Set<TEntity>().FindAsync(id);

                return entity.IsDeleted 
                    ? PersistenceResponseStatus.Success.GetFailureResponse<PersistenceResponseStatus, TEntity>("WasDeleted") 
                    : PersistenceResponseStatus.Success.GetSuccessResponse(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return PersistenceResponseStatus.PersistenceFailure.GetFailureResponse<PersistenceResponseStatus, TEntity>();
            }
        }

        public async Task<PersistenceResponse<List<TEntity>>> GetByIds(List<Guid> ids)
        {
            try
            {
                var entities = await _database.Context.Set<TEntity>()
                    .Where(e => ids.Contains(e.Id) && !e.IsDeleted)
                    .ToListAsync();;

                return PersistenceResponseStatus.Success.GetSuccessResponse(entities);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return PersistenceResponseStatus.PersistenceFailure.GetFailureResponse<PersistenceResponseStatus, List<TEntity>>();
            }
        }

        public async Task<PersistenceResponse<GetAllResponse<TEntity>>> GetAll()
        {
            try
            {
                var all = await _database.Context.Set<TEntity>().Where(e => !e.IsDeleted).ToListAsync();
                
                var getAllResponse = new GetAllResponse<TEntity>
                {
                    Items = all,
                    TotalCount = all.Count
                };

                return PersistenceResponseStatus.Success.GetSuccessResponse(getAllResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return PersistenceResponseStatus.PersistenceFailure.GetFailureResponse<PersistenceResponseStatus, GetAllResponse<TEntity>>();
            }
            
        }

        public async Task<PersistenceResponse<GetAllResponse<TEntity>>> GetAll(Expression<Func<TEntity, object>> includePredicate)
        {
            try
            {
                var all = includePredicate != null
                    ? await _database.Context.Set<TEntity>().Include(includePredicate).Where(e => !e.IsDeleted).ToListAsync()
                    : await _database.Context.Set<TEntity>().Where(e => !e.IsDeleted).ToListAsync();
                
                var getAllResponse = new GetAllResponse<TEntity>
                {
                    Items = all,
                    TotalCount = all.Count
                };

                return PersistenceResponseStatus.Success.GetSuccessResponse(getAllResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return PersistenceResponseStatus.PersistenceFailure.GetFailureResponse<PersistenceResponseStatus, GetAllResponse<TEntity>>();
            }
        }
        
        public async Task<PersistenceResponse<TEntity>> First(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> includePredicate = null)
        {
            try
            {
                var first = includePredicate != null 
                    ? await _database.Context.Set<TEntity>().Include(includePredicate).FirstAsync(predicate) 
                    : await _database.Context.Set<TEntity>().FirstAsync(predicate);
                
                return PersistenceResponseStatus.Success.GetSuccessResponse(first);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return PersistenceResponseStatus.PersistenceFailure.GetFailureResponse<PersistenceResponseStatus, TEntity>();
            }
        }
    }
    
    public interface IRepository<TEntity> :  IManipulationPersistenceGateway<TEntity> where TEntity : FullAuditedEntity
    {
    }

    public class Repository<TEntity> : IRepository<TEntity> 
        where TEntity : FullAuditedEntity, new()
    {

        private readonly IReadOnlyRepository<TEntity> _readOnlyRepository;
        private readonly ICurrentDbContext _database;
        public Repository(ICurrentDbContext database, IReadOnlyRepository<TEntity> readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _database = database;
        }

        public Repository(IReadOnlyRepository<TEntity> readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
        }
        

        public async Task<PersistenceResponse<TEntity>> Create(TEntity entity)
        {
            try
            {
                entity.CreationTime = DateTime.Now;
                await _database.Context.AddAsync(entity);
                await _database.Context.SaveChangesAsync();

                return PersistenceResponseStatus.Success.GetSuccessResponse(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return PersistenceResponseStatus.PersistenceFailure.GetFailureResponse<PersistenceResponseStatus, TEntity>();
            }
        }

        public async Task<PersistenceResponse<TEntity>> Update(TEntity entity)
        {
            try
            {
                entity.LastModificationTime = DateTime.Now;
                
                var entityToUpdate = await _database.Context.FindAsync<TEntity>(entity.Id);
                if (entityToUpdate.IsDeleted) return PersistenceResponseStatus.PersistenceFailure.GetFailureResponse<PersistenceResponseStatus, TEntity>("AlreadyWasDeleted");
                
                entityToUpdate = entity;
                await _database.Context.SaveChangesAsync();

                return PersistenceResponseStatus.Success.GetSuccessResponse(entityToUpdate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return PersistenceResponseStatus.PersistenceFailure.GetFailureResponse<PersistenceResponseStatus, TEntity>();
            }
        }

        public async Task<PersistenceResponse<DeletionResponses>> Delete(Guid id)
        {
            try
            {
                var entity = await _database.Context.FindAsync<TEntity>(id);

                if (entity.IsDeleted)
                {
                    return new PersistenceResponse<DeletionResponses>
                    {
                        Message = "AlreadyWasDeleted",
                        Success = false,
                        Response = DeletionResponses.DeletionFailure,
                        ErrorCodeName = DeletionResponses.DeletionFailure.ToString()
                    };
                }

                
                entity.DeletionTime = DateTime.Now;
                entity.IsDeleted = true;
                await _database.Context.SaveChangesAsync();

                return new PersistenceResponse<DeletionResponses>
                {
                    Message = "Success",
                    Success = true,
                    Response = DeletionResponses.DeleteSuccessfully,
                    ErrorCodeName = DeletionResponses.DeleteSuccessfully.ToString()
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
                return new PersistenceResponse<DeletionResponses>
                {
                    Message = "PersistenceFailure",
                    Success = false,
                    Response = DeletionResponses.DeletionFailure,
                    ErrorCodeName = DeletionResponses.DeletionFailure.ToString()
                };
            }
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
        
        public async Task<PersistenceResponse<GetAllResponse<TEntity>>> GetAll(Expression<Func<TEntity, object>> includePredicate)
        {
            return await _readOnlyRepository.GetAll(includePredicate);
        }

        public async Task<PersistenceResponse<TEntity>> First(Expression<Func<TEntity, bool>> predicate)
        {
            return await _readOnlyRepository.First(predicate);
        }

        public async Task<PersistenceResponse<TEntity>> First(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> includePredicate)
        {
            return await _readOnlyRepository.First(predicate, includePredicate);
        }
    }
}