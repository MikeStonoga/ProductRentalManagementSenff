namespace PRM.Domain.BaseCore
{
    public abstract class Person : FullAuditedEntity
    {
        public byte[] PersonImage { get; set; }
        public string Email { get; set; }
    }

    public abstract class AuthenticablePerson : Person
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}