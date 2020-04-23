namespace Focus.Service.Identity.Application.Dto
{
    public class NewUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string OrganizationId { get; set; }
        public string UserRole { get; set; }
    }
}