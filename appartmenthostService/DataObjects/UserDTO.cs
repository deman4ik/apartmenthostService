using System;

namespace appartmenthostService.DataObjects
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public string ContactEmail { get; set; }
        public string ContactKind { get; set; }
        public string Description { get; set; }
        public string PictureId { get; set; }
    }
}