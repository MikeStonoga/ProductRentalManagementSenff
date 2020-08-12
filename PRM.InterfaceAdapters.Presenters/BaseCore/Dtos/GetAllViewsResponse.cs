using System.Collections.Generic;
using PRM.Domain.BaseCore;

namespace PRM.InterfaceAdapters.Presenters.BaseCore.Dtos
{
    public class GetAllViewsResponse<TEntity, TEntityView> where TEntityView : FullAuditedEntityView<TEntity> where TEntity : FullAuditedEntity
    {
        public List<TEntityView> Items { get; set; }
        public int TotalCount { get; set; }
    }
}