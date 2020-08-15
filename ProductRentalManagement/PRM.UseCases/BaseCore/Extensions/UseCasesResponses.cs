﻿﻿using System;
 using System.Threading.Tasks;
 using PRM.UseCases.BaseCore.Enums;

 namespace PRM.UseCases.BaseCore.Extensions
{
    public static class UseCasesResponses
    {
        
        public static async Task<UseCaseResult<TUseCaseResult>> GetUseCaseExecutionResponse<TUseCase, TUseCaseRequirement, TUseCaseResult>(TUseCase useCase, TUseCaseRequirement input)
            where TUseCase : IBaseUseCase<TUseCaseRequirement, TUseCaseResult> where TUseCaseResult : class, new()
        {
            var useCaseResponse = await useCase.Execute(input);
            if (!useCaseResponse.Success) return ExecutionFailureResponse<TUseCaseResult>(useCaseResponse.Message);
            
            var output = Activator.CreateInstance(typeof(TUseCaseResult), useCaseResponse.Result) as TUseCaseResult;
            return SuccessfullyExecutedResponse(output, useCaseResponse.Message);
        }
        
        public static UseCaseResult<TResult> SuccessfullyExecutedResponse<TResult>(TResult result, string message = "")
        {
            return UseCaseResults.UseCaseSuccessfullyExecuted.GetSuccessResult(result, message);
        }
        
        public static UseCaseResult<TResult> ExecutionFailureResponse<TResult>(string message = "") where TResult : new()
        {
            var result = new TResult();
            return UseCaseResults.UseCaseFailureExecution.GetFailureResult(result, message);
        }
        
        public static UseCaseResult<TResult> ValidationErrorResponse<TResult>(TResult result, string message = "")
        {
            return UseCaseResults.ValidationError.GetFailureResult(result, message);
        }
        
        public static UseCaseResult<TResult> ValidationErrorResponse<TResult>(string message = "") where TResult : new()
        {
            var result = new TResult();
            return UseCaseResults.PersistenceError.GetFailureResult(result, message);
        }
        
        public static UseCaseResult<TResult> PersistenceErrorResponse<TResult>(TResult result, string message = "")
        {
            return UseCaseResults.PersistenceError.GetFailureResult(result, message);
        }
        
        public static UseCaseResult<TResult> PersistenceErrorResponse<TResult>(string message = "") where TResult : new()
        {
            var result = new TResult();
            return UseCaseResults.PersistenceError.GetFailureResult(result, message);
        }

        public static UseCaseResult<TResult> GetSuccessResult<TEnum, TResult>(this TEnum resultEnum, TResult result, string message = "") 
            where TEnum : Enum 
        {
            return resultEnum.GetResult(true, result, message);
        }
        
        public static UseCaseResult<TResult> GetFailureResult<TEnum, TResult>(this TEnum resultEnum, TResult result, string message = "") 
            where TEnum : Enum 
        {
            return resultEnum.GetResult(false, result, message);
        }
        
        public static UseCaseResult<TResult> GetResult<TEnum, TResult>(this TEnum resultEnum, bool isSuccessResult, TResult result, string message = "") 
            where TEnum : Enum
        {
            if (string.IsNullOrEmpty(message))
            {
                message = resultEnum.ToString();
            }
            
            return new UseCaseResult<TResult>
            {
                Success = isSuccessResult,
                Message = message,
                ErrorCodeName = resultEnum.ToString(),
                Result = result
            };
        }
    }
}