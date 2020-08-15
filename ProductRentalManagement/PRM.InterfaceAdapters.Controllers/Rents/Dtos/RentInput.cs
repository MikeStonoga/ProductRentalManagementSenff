using PRM.Domain.Rents;
using PRM.InterfaceAdapters.Controllers.BaseCore;

namespace PRM.InterfaceAdapters.Controllers.Rents.Dtos
{
    public class RentInput : Rent, IAmManipulationInput<Rent>
    {
        public Rent MapToEntity()
        {
            return new Rent
            {
                Id = Id,
                Code = Code,
                Name = Name,
                CreationTime = CreationTime,
                CreatorId = CreatorId,
                DeleterId = DeleterId,
                DeletionTime = DeletionTime,
                IsDeleted = IsDeleted,
                LastModificationTime = LastModificationTime,
                LastModifierId = LastModifierId
            };
        }
    }
}