using System;

namespace HomeWork0329.Data
{
    public class Posts
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public bool IsUsers { get; set; }
        public int UserId { get; set; }
    }
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

}
