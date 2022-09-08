using Assessment.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Assessment.Controllers
{
    public class BaseController : ApiController
    {
        internal IHttpActionResult GetErrorResult(ServiceResponse result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.IsSuccess)
            {
                if (!string.IsNullOrEmpty(result.Message))
                {
                    ModelState.AddModelError("", result.Message);
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}