using PRM.InterfaceAdapters.Controllers.BaseCore;

namespace PRM.Infrastructure.Authentication.Users.Dtos
{
    public class UserInput : User, IAmManipulationInput<User>
    {
        public User MapToEntity()
        {
            return new User
            {
                Email = Email,
                Login = Login,
                Name = Name,
                Password = Password,
                Role = Role,
                PersonImage = PersonImage,
            };
        }
    }
}