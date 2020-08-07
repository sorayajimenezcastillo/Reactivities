using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.User;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class UserController: BaseController
    {
        [AllowAnonymous] //this will override the policy so we can login without the key 
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(Login.Query query)
        {
            return await Mediator.Send(query);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(Register.Command command)
        {
            return await Mediator.Send(command);
        }
        
        [HttpGet]
        public async Task<ActionResult<User>> CurrentUser()
        {
            return await Mediator.Send(new CurrentUser.Query());
        }
    }
}