﻿using System.Threading.Tasks;

namespace PRM.UseCases.BaseCore
{
    public interface IBaseUseCase<TRequirement, TResult>
    {
        Task<UseCaseResult<TResult>> Execute(TRequirement requirement);
    }

    public abstract class BaseUseCase<TRequirement, TResult> : IBaseUseCase<TRequirement, TResult>
    {
        public abstract Task<UseCaseResult<TResult>> Execute(TRequirement requirement);
    }
}