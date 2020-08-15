using System.Threading.Tasks;
using PRM.UseCases.BaseCore;

namespace PRM.UseCases.Rents.FinishRents
{
    public interface IFinishRent : IBaseUseCase<FinishRentRequirement, FinishRentResult>
    {
        
    }
    
    public class FinishRent : BaseUseCase<FinishRentRequirement, FinishRentResult>, IFinishRent
    {
        public override async Task<UseCaseResult<FinishRentResult>> Execute(FinishRentRequirement rentProductsRequirement)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class FinishRentResult
    {
    }

    public class FinishRentRequirement
    {
    }
}