using PRM.Domain.BaseCore.Dtos;

namespace PRM.InterfaceAdapters.Presenters.BaseCore.Dtos
{
    public interface IPresenterResult<TView> : IResultInfoDto
    {
        public TView View { get; set; }   
    }
    
    public class PresenterResult<TView> : ResultInfoDto, IPresenterResult<TView>
    {
        public TView View { get; set; }
    }
}