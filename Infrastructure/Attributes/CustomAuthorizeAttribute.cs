using Assessment.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Web.Http.Filters;
using Assessment.Models.ViewModel;
using System.Net;

namespace Assessment.Infrastructure.Attributes
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (string.IsNullOrEmpty(SessionHelper.Token))
            {
                UnauthorizedUser(new ServiceResponse() { IsSuccess = false, Message = "The authorization token is missing" }, actionContext);
                return;
            }
            if (SessionHelper.UserId == 0)
            {
                UnauthorizedUser(new ServiceResponse() { IsSuccess = false, Message = "The authorization token is not valid!" }, actionContext);

                return;
            }

            base.OnActionExecuting(actionContext);
        }

        public static void UnauthorizedUser(ServiceResponse response, HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(
                HttpStatusCode.NotAcceptable,
                response,
                actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                );
        }

    }
}