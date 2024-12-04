namespace POE.Models
{
    public class AdminUsers
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserNames { get; set; }
        public string UserPassword { get; set; }
    }
}
