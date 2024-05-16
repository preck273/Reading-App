using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace bookAppApi.Entities
{
    public partial class Users
    {
        public Users()
        {

        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Level { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
    }
}