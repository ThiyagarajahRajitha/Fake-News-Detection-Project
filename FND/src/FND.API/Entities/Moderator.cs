namespace FND.API.Entities
{
    public class Moderator
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public Guid InviteCode { get; set; }
        public bool IsUpdated { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
