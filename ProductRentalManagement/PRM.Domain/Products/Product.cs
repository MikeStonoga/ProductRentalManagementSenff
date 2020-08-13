﻿using System;
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
            
            var isRewardRenting = rentRequirement.StartDate < DateTime.Now.AddMinutes(-1);
            if (isRewardRenting) return DomainValidationsExtensions.GetFailureResponse<RentResult>("CannotRentReward");
            
            var rent = new Rent(rentRequirement);
            rent.CreationTime = DateTime.Now;
            
            Rents ??= new List<Rent>();
            Rents.Add(rent);
            
            Status = RentStatus.Unavailable;
            return new RentResult(rent, Description).GetSuccessResponse("Rented");
        }
        
        public DomainResponseDto<RentFinishedDto> FinishProductRent(Guid rentId)
        {
            var rentToFinish = Rents.Find(rent => rentId == rent.Id);
            
            var rentFounded = rentToFinish != null;
            if (!rentFounded) return new RentFinishedDto().GetFailureResponse("RentNotFound");
            
            var finishedRent = rentToFinish.FinishRent();
            Status = RentStatus.Available;
            return finishedRent.GetSuccessResponse("RentFinished");
        }
        
        public decimal CalculateProductRentPrice(RentRequirement rentRequirement)
        {
            return new Rent(rentRequirement).PriceWithoutLateFee;
        }
    }
}