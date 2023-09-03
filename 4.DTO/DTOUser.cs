namespace _4.DTO
{
    public class DTOUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DTORole Role { get; set; }
    }
}
