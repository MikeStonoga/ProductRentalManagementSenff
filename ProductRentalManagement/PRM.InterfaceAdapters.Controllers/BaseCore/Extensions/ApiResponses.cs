﻿﻿using System;
 using System.Linq;
 using System.Threading.Tasks;
 using Microsoft.Extensions.DependencyInjection;
 using PRM.Domain.BaseCore;
 using PRM.InterfaceAdapters.Controllers.BaseCore.Enums;
 using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore.Dtos;
 using PRM.UseCases.BaseCore;
 using PRM.UseCases.BaseCore.Extensions;

 namespace PRM.InterfaceAdapters.Controllers.BaseCore.Extensions
{
    public static class ApiResponses
    {
        public static async Task<ApiResponse<GetAllResponse<TEntity, TOutput>>> GetUseCaseInteractorResponse<TEntity, TOutput>(Func<Task<UseCaseResult<GetAllResponse<TEntity>>>> useCase)
            where TEntity : FullAuditedEntity, new()
            where TOutput : TEntity, new()
        {
            var useCaseResponse = await useCase();
            if (!useCaseResponse.Success) return FailureResponse<GetAllResponse<TEntity, TOutput>>(useCaseResponse.Message);
            
            var outputs = new GetAllResponse<TEntity, TOutput>();
            
            foreach (var output in useCaseResponse.Result.Items.Select(entity => Activator.CreateInstance(typeof(TOutput), entity) as TOutput))
            {
                outputs.Items.Add(output);
            }

            outputs.TotalCount = useCaseResponse.Result.TotalCount;
            return SuccessfullyExecutedResponse(outputs, useCaseResponse.Message);
        }
        
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