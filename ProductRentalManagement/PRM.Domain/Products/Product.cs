using System;
using System.Collections.Generic;
using PRM.Domain.BaseCore;
using PRM.Domain.BaseCore.Dtos;
using PRM.Domain.BaseCore.Extensions;
using PRM.Domain.Products.Rents;
using PRM.Domain.Products.Rents.Dtos;
using PRM.Domain.Products.Rents.Enums;

namespace PRM.Domain.Products
{
    public class Product : FullAuditedEntity
    {
        public string Description { get; set; }
        public List<Rent> Rents { get; set; }
        public RentStatus Status { get; set; }
        public bool IsAvailable => Status == RentStatus.Available;

        public DomainResponseDto<RentResult> RentProduct(RentRequirement rentRequirement)
        {
            if (!IsAvailable) return DomainValidationsExtensions.GetFailureResponse<RentResult>("AlreadyRentedProduct");
            
            var isBackwardRenting = rentRequirement.StartDate < DateTime.Now.AddMinutes(-1);
            if (isBackwardRenting) return DomainValidationsExtensions.GetFailureResponse<RentResult>("CannotRentReward");

            var rent = new Rent(rentRequirement);
            
            Rents ??= new List<Rent>();
            Rents.Add(rent);
            
            Status = RentStatus.Unavailable;
            return new RentResult(rent, Description).GetSuccessResponse("Rented");
        }
        
        public DomainResponseDto<RentFinishedResult> FinishProductRent(FinishRentRequirement finishRentRequirement)
        {
            var rentToFinish = Rents.Find(rent => finishRentRequirement.RentId == rent.Id);

            var rentFounded = rentToFinish != null;
            if (!rentFounded) return new RentFinishedResult().GetFailureResponse("RentNotFound");

            if (finishRentRequirement.DamageFee != 0M)
            {
                rentToFinish.WasProductDamaged = true;
                rentToFinish.DamageFee = finishRentRequirement.DamageFee;
            }
            
            var finishedRent = rentToFinish.FinishRent();
            Status = RentStatus.Available;
            return finishedRent.GetSuccessResponse("RentFinished");
        }
        
        public decimal CalculateProductRentPrice(RentRequirement rentRequirement)
        {
            return new Rent(rentRequirement).PriceWithoutFees;
        }
    }

    public class FinishRentRequirement
    {
        public Guid RentId { get; set; }
        public decimal DamageFee { get; set; }
    }
}