using PRM.Domain.BaseCore;

namespace PRM.Domain.Users
{
    public class User : AuthenticablePerson
    {
        public string GovernmentRegistrationDocumentCode { get; set; }
    }
}