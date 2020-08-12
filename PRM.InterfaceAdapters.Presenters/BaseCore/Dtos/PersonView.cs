using PRM.Domain.BaseCore;

namespace PRM.InterfaceAdapters.Presenters.BaseCore.Dtos
{
    public interface IPersonView : IFullAuditedEntityView
    {
        public byte[] PersonImage { get; set; }
        public string Email { get; set; }
    }
    public abstract class PersonView<TEntity> : FullAuditedEntityView<TEntity> where TEntity : Person
    {
        public byte[] PersonImage { get; set; }
        public string Email { get; set; }

        public PersonView()
        {
            
        }
        
        protected PersonView(TEntity entity) : base(entity)
        {
            PersonImage = entity.PersonImage;
            Email = entity.Email;
        }
    }

    public interface IAuthenticablePersonView : IPersonView
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
    
    public abstract class AuthenticablePersonView<TEntity> : PersonView<TEntity> where TEntity : AuthenticablePerson
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public AuthenticablePersonView()
        {
            
        }
        protected AuthenticablePersonView(TEntity entity) : base(entity)
        {
            Login = entity.Login;
            Password = entity.Password;
        }
    }
}