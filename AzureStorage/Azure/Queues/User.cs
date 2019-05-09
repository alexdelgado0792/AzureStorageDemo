using System;

namespace Azure.Queues
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Guid TenantId { get; set; }
        public bool IsActive { get; set; }
    }
}
