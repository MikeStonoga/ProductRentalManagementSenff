﻿using System.Threading.Tasks;
using PRM.Domain.Products;
using PRM.Domain.Rents;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;

namespace PRM.UseCases.Rents.FinishRents
{
    public interface IFinishRent : IBaseUseCase<FinishRentRequirement, FinishRentResult>
    {
        
    }
    
    public class FinishRent : BaseUseCase<FinishRentRequirement, FinishRentResult>, IFinishRent
    {
        private readonly IManipulationPersistenceGateway<Rent> _rents;
        private readonly IManipulationPersistenceGateway<Product> _products;
        private readonly IManipulationPersistenceGateway<ProductRentalHistory> _productRentalHistories;

        public FinishRent(IManipulationPersistenceGateway<Rent> rents, IManipulationPersistenceGateway<Product> products, IManipulationPersistenceGateway<ProductRentalHistory> productRentalHistories)
        {
            _rents = rents;
            _products = products;
            _productRentalHistories = productRentalHistories;
        }

        public override async Task<UseCaseResult<FinishRentResult>> Execute(FinishRentRequirement requirement)
        {
            
            var rentToFinish = await _rents.GetById(requirement.RentId);
            if (!rentToFinish.Success) return UseCasesResponses.ExecutionFailureResponse<FinishRentResult>(rentToFinish.Message);

            var finishRentResponse = rentToFinish.Response.FinishRent(requirement.DamageFee, requirement.Discount);
            if (!finishRentResponse.Success) return UseCasesResponses.ExecutionFailureResponse<FinishRentResult>(finishRentResponse.Message);

            var productsToTurnAvailableIds = await _productRentalHistories.GetAllIds(history => history.RentId == requirement.RentId);
            var productsToTurnAvailable = await _products.GetByIds(productsToTurnAvailableIds.Response);
            
            if (!productsToTurnAvailableIds.Success) return UseCasesResponses.ExecutionFailureResponse<FinishRentResult>(productsToTurnAvailableIds.Message);
            
            // TODO: UnitOfWork
            foreach (var product in productsToTurnAvailable.Response)
            {
                product.MarkAsAvailable();
                await _products.Update(product);
                
            }

            await _rents.Update(finishRentResponse.Result);
            
            var finishRentResult = new FinishRentResult(finishRentResponse.Result.CurrentRentPaymentValue);
            return UseCasesResponses.SuccessfullyExecutedResponse(finishRentResult);
        }
    }
}