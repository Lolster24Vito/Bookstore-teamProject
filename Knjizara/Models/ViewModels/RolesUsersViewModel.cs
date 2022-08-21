using Knjizara.Models.Authentication;

namespace Knjizara.Models.ViewModels
{
    public class RolesUsersViewModel
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public List<String>? Roles { get; set; }
    }
}
