namespace Homework.Entities.Configuration
{
    public class RoleSettings
    {
        public IEnumerable<string> Roles { get; set; }
        public string AdminEmail { get; set; }
        public string AdminRole { get; set; }
    }
}
