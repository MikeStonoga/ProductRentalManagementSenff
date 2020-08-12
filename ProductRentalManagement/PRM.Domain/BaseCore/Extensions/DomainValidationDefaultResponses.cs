using PRM.Domain.BaseCore.Dtos;

namespace PRM.Domain.BaseCore.Extensions
{
    public static class DomainValidationsExtensions
    {
        public static DomainResponseDto<TResult> GetSuccessResponse<TResult>(this TResult result, string message)
        {
            return new DomainResponseDto<TResult>
            {
                Success = true,
                Message = message,
                Result = result
            };
        }
        
        public static DomainResponseDto<TResult> GetFailureResponse<TResult>(this TResult result, string message)
        {
            return new DomainResponseDto<TResult>
            {
                Success = false,
                Message = message,
                Result = result

            };
        }
    }
}