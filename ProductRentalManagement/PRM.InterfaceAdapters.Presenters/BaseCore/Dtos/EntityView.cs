using PRM.Domain.BaseCore;
using PRM.InterfaceAdapters.Presenters.BaseCore.Extensions;

namespace PRM.InterfaceAdapters.Presenters.BaseCore.Dtos
{
    public interface IEntityView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public interface IFullAuditedEntityView : IEntityView
    {
        public string CreatorId { get; set; }
        public string CreationTime { get; set; }
        public string LastModifierId { get; set; }
        public string LastModificationTime { get; set; }
        public string DeleterId { get; set; }
        public string DeletionTime { get; set; }
    }
    
    public abstract class EntityView<TEntity> : IEntityView where TEntity : Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        

        public EntityView()
        {
            
        }

        public EntityView(TEntity entity)
        {
            Id = entity.Id.ToString().ToUpper();
            Code = entity.Code;
            Name = entity.Name;
        }
    }
    
    public abstract class FullAuditedEntityView<TEntity> : EntityView<TEntity>, IFullAuditedEntityView where TEntity : FullAuditedEntity
    {
        public string CreatorId { get; set; }
        public string CreationTime { get; set; }
        public string LastModifierId { get; set; }
        public string LastModificationTime { get; set; }
        public string DeleterId { get; set; }
        public string DeletionTime { get; set; }

        public FullAuditedEntityView()
        {
            
        }
        protected FullAuditedEntityView(TEntity entity) : base(entity)
        {
            CreationTime = entity.CreationTime.FormatDate();
            CreatorId = entity.CreatorId.FormatId();
            LastModificationTime = entity.LastModificationTime.FormatDate();
            LastModifierId = entity.LastModifierId.FormatId();
            DeletionTime = entity.DeletionTime.FormatDate();
            DeleterId = entity.DeleterId.FormatId();
        }
    }
}