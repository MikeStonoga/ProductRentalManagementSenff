﻿using System;
using System.Collections.Generic;
using PRM.Domain.BaseCore;
 using PRM.Domain.BaseCore.Enums;
 using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
 using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore.Dtos;
 using PRM.InterfaceAdapters.Presenters.BaseCore.Dtos;
 using PRM.UseCases.BaseCore;

namespace PRM.InterfaceAdapters.Presenters.BaseCore
{
    public interface IBaseReadOnlyPresenter<TEntity, TEntityView> 
        where TEntity : FullAuditedEntity
        where TEntityView : FullAuditedEntityView<TEntity>
    {
        PresenterResult<TEntityView> GetView(UseCaseResult<TEntity> useCaseResult);
        PresenterResult<List<TEntityView>> GetViews(UseCaseResult<List<TEntity>> useCaseResult);
        PresenterResult<GetAllViewsResponse<TEntity, TEntityView>> GetAllViews(UseCaseResult<GetAllResponse<TEntity>> useCaseResult);
    }

    public interface IBaseManipulationPresenter<TEntity, TEntityView> : IBaseReadOnlyPresenter<TEntity, TEntityView> 
        where TEntity : FullAuditedEntity
        where TEntityView : FullAuditedEntityView<TEntity>
    {
        PresenterResult<DeletionResponses> GetDeletionView(UseCaseResult<DeletionResponses> useCaseResult);
    }

    public abstract class BaseReadOnlyPresenter<TEntity, TEntityView> : IBaseReadOnlyPresenter<TEntity, TEntityView> 
        where TEntity : FullAuditedEntity
        where TEntityView : FullAuditedEntityView<TEntity>
    {
        public PresenterResult<TEntityView> GetView(UseCaseResult<TEntity> useCaseResult)
        {
            return new PresenterResult<TEntityView>
            {
                Message = useCaseResult.Message,
                Success = useCaseResult.Success,
                ErrorCodeName = useCaseResult.ErrorCodeName,
                View = Activator.CreateInstance(typeof(TEntityView), useCaseResult.Result) as TEntityView
            };
        }

        public PresenterResult<List<TEntityView>> GetViews(UseCaseResult<List<TEntity>> useCaseResult)
        {
            List<TEntityView> views = new List<TEntityView>();
            
            foreach (var result in useCaseResult.Result)
            {
                views.Add(Activator.CreateInstance(typeof(TEntityView), result) as TEntityView);
            }
            
            return new PresenterResult<List<TEntityView>>
            {
                Message = useCaseResult.Message,
                Success = useCaseResult.Success,
                ErrorCodeName = useCaseResult.ErrorCodeName,
                View = views
                
            };
        }

        public PresenterResult<GetAllViewsResponse<TEntity, TEntityView>> GetAllViews(UseCaseResult<GetAllResponse<TEntity>> useCaseResult)
        {
            List<TEntityView> views = new List<TEntityView>();

            foreach (var result in useCaseResult.Result.Items)
            {
                views.Add(Activator.CreateInstance(typeof(TEntityView), result) as TEntityView);
            }

            return new PresenterResult<GetAllViewsResponse<TEntity, TEntityView>>
            {
                Message = useCaseResult.Message,
                Success = useCaseResult.Success,
                ErrorCodeName = useCaseResult.ErrorCodeName,
                View = new GetAllViewsResponse<TEntity, TEntityView>
                {
                    Items = views,
                    TotalCount = views.Count
                }
            };
        }
    }    
    
    public abstract class BaseManipulationPresenter<TEntity, TEntityView, TIEntityReadOnlyPresenter> : BaseReadOnlyPresenter<TEntity, TEntityView>, IBaseManipulationPresenter<TEntity, TEntityView> 
        where TEntity : FullAuditedEntity
        where TEntityView : FullAuditedEntityView<TEntity>
        where TIEntityReadOnlyPresenter : IBaseReadOnlyPresenter<TEntity, TEntityView>
    {
        protected readonly TIEntityReadOnlyPresenter ReadOnlyPresenter;

        protected BaseManipulationPresenter(TIEntityReadOnlyPresenter readOnlyPresenter)
        {
            ReadOnlyPresenter = readOnlyPresenter;
        }

        public PresenterResult<DeletionResponses> GetDeletionView(UseCaseResult<DeletionResponses> useCaseResult)
        {
            return new PresenterResult<DeletionResponses>
            {
                Message = useCaseResult.Message,
                Success = useCaseResult.Success,
                ErrorCodeName = useCaseResult.ErrorCodeName,
                View = useCaseResult.Result
            };
        }
    }
}