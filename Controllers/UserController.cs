using Assessment.Infrastructure.DataProvider;
using Assessment.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Assessment.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : BaseController
    {
        public UserController()
        {
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Add")]
        public  IHttpActionResult Add(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IUserDataProvider _userDataProvider = new UserDataProvider();
            var result = _userDataProvider.RegisterUser(model);

            if (!result.IsSuccess)
            {
                return GetErrorResult(result);
            }

            return Ok(result);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("Authenticate")]
        public IHttpActionResult Authenticate(AuthenticateRequest model)
        {
            IUserDataProvider _userDataProvider = new UserDataProvider();
            var response = _userDataProvider.Authenticate(model);
            if (!response.IsSuccess)
            {
                return GetErrorResult(response);
            }

            return Ok(response);
        }
    }
}