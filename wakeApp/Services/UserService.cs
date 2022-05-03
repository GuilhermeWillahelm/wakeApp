using System.Security.Claims;
using wakeApp.Models;

namespace wakeApp.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _context;

        public UserService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public int GetUserId()
        {
            var user =  _context.HttpContext.User.Claims.Where(u => u.Type == ClaimTypes.NameIdentifier).Select(x => x.Value);
            int idUser = int.Parse(user.Last());

            return idUser;
        }

        public string GetUserName()
        {
            var user = _context.HttpContext.User.Claims.Where(u => u.Type == ClaimTypes.Name).Select(x => x.Value);
            return user.Last();
        }
    }
}
