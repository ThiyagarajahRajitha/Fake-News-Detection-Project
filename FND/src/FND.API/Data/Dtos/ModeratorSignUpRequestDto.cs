namespace FND.API.Data.Dtos
{
    public class ModeratorSignUpRequestDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid InviteCode { get; set; }
        public string Password { get; set;}
    }
}
