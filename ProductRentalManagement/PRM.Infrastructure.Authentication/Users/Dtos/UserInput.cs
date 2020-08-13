using PRM.InterfaceAdapters.Controllers.BaseCore;

namespace PRM.Infrastructure.Authentication.Users.Dtos
{
    public class UserInput : User, IAmManipulationInput<User>
    {
        public User MapToEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}