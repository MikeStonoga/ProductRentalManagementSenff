﻿using PRM.Domain.BaseCore;
using PRM.InterfaceAdapters.Controllers.BaseCore;

namespace PRM.Infrastructure.ApplicationDelivery.WebApiHost.BaseCore
{
    public interface IAmWebManipulationInput<TEntity, TManipulationInput> 
        where TEntity : FullAuditedEntity
        where TManipulationInput : IAmManipulationInput<TEntity>
    {
        TManipulationInput MapToManipulationInput();
    }
}