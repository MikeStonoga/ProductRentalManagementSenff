﻿using System;
 using System.Threading.Tasks;
 using PRM.InterfaceAdapters.Controllers.BaseCore.Enums;
 using PRM.UseCases.BaseCore;

 namespace PRM.InterfaceAdapters.Controllers.BaseCore.Extensions
{
    public static class ApiResponses
    {
        public static async Task<ApiResponse<TOutput>> GetUseCaseInteractorResponse<TUseCaseRequirement, TUseCaseResult, TInput, TOutput>(Func<TUseCaseRequirement, Task<UseCaseResult<TUseCaseResult>>> useCase, TInput input)
            where TInput : TUseCaseRequirement 
            where TOutput : class, TUseCaseResult, new()
        {
            var useCaseResponse = await useCase(input);
            if (!useCaseResponse.Success) return FailureResponse<TOutput>(useCaseResponse.Message);
            
            var output = Activator.CreateInstance(typeof(TOutput), useCaseResponse.Result) as TOutput;
            return SuccessfullyExecutedResponse(output, useCaseResponse.Message);
        }
        
        public static ApiResponse<TResult> SuccessfullyExecutedResponse<TResult>(TResult result, string message = "")
        {
            return ExecutionStatus.ExecutedSuccessfully.GetSuccessResult(result, message);
        }
        
        public static ApiResponse<TResult> FailureResponse<TResult>(TResult result, string message = "")
        {
            return ExecutionStatus.Failure.GetFailureResult(result, message);
        }
        
        public static ApiResponse<TResult> FailureResponse<TResult>(string message = "") where TResult : new()
        {
            var result = new TResult();
            return ExecutionStatus.Failure.GetFailureResult(result, message);
        }

        public static ApiResponse<TResult> GetSuccessResult<TEnum, TResult>(this TEnum resultEnum, TResult result, string message = "") 
            where TEnum : Enum 
        {
            return resultEnum.GetResponse(true, result, message);
        }
        
        public static ApiResponse<TResult> GetFailureResult<TEnum, TResult>(this TEnum resultEnum, TResult result, string message = "") 
            where TEnum : Enum 
        {
            return resultEnum.GetResponse(false, result, message);
        }
        
        
        public static ApiResponse<TResult> GetResponse<TEnum, TResult>(this TEnum resultEnum, bool isSuccessResult, TResult response, string message = "") 
            where TEnum : Enum
        {
            if (string.IsNullOrEmpty(message))
            {
                message = resultEnum.ToString();
            }
            
            return new ApiResponse<TResult>
            {
                Success = isSuccessResult,
                Message = message,
                ErrorCodeName = resultEnum.ToString(),
                Response = response
            };
        }
    }
}