using API.Models;
using API.Util;
using BAL.DbOperation;
using DAL.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("User")]
    public class UserController : ApiController
    {
        [NonAction]
        public bool IsUserValid(string email, string password, out string statusMessage)
        {
            bool isValid = false;
            statusMessage = "User is either locked or Inactive to perform any action";
            UserCRUD userCRUD = new UserCRUD();
            var user = userCRUD.GetUser(email, password);
            if (user!=null)
            {
                if (user.is_active==true)
                {
                    if ((user.is_locked!=true))
                    {
                        statusMessage = string.Empty;
                        isValid = true;
                    }
                }
            }
            else
            {
                statusMessage = HttpStatusCode.Unauthorized.ToString();
            }

            return isValid;
        }

        [Route("Authenticate")]
        public AuthenticeResponse Authenticate(AuthenticateCR req)
        {
            AuthenticeResponse resp = new AuthenticeResponse();
            try
            {
                if (ValidateRequest.Authenticate(req))
                {
                    UserCRUD userCRUD = new UserCRUD();
                    string statusMessage = string.Empty;
                    if (IsUserValid(req.EmailID, req.Password, out statusMessage))
                    {
                        resp.Token = JWTService.GenerateToken(req.EmailID);
                        resp.StatusCode = HttpStatusCode.OK;
                        resp.StatusMessage = HttpStatusCode.OK.ToString();
                    }
                    else
                    {
                        resp.StatusCode = HttpStatusCode.Unauthorized;
                        resp.StatusMessage = statusMessage;
                    }
                }
                else
                {
                    resp.StatusCode = HttpStatusCode.BadRequest;
                    resp.StatusMessage = HttpStatusCode.BadRequest.ToString();
                }
            }
            catch (Exception es)
            {
                resp.StatusCode = HttpStatusCode.InternalServerError;
                resp.StatusMessage = HttpStatusCode.InternalServerError.ToString();
            }
            return resp;
        }

        [Route("AddUser")]
        public BaseResponse AddUser(UserCR req)
        {
            BaseResponse resp = new BaseResponse();
            try
            {
                if (ValidateRequest.AddUser(req))
                {
                    UserCRUD userCRUD = new UserCRUD();
                    if (!userCRUD.IsUserEmailExists(req.User.email_id))
                    {
                        req.User.last_update = DateTime.Now;
                        if (userCRUD.AddUser(req.User) > 0)
                        {
                            resp.StatusCode = HttpStatusCode.OK;
                            resp.StatusMessage = HttpStatusCode.OK.ToString();
                        }
                        else
                        {
                            resp.StatusCode = HttpStatusCode.Ambiguous;
                            resp.StatusMessage = HttpStatusCode.Ambiguous.ToString();
                        }
                    }
                    else
                    {
                        resp.StatusCode = HttpStatusCode.Conflict;
                        resp.StatusMessage = HttpStatusCode.Conflict.ToString();
                    }
                }
                else
                {
                    resp.StatusCode = HttpStatusCode.BadRequest;
                    resp.StatusMessage = HttpStatusCode.BadRequest.ToString();
                }
            }
            catch (Exception es)
            {
                resp.StatusCode = HttpStatusCode.InternalServerError;
                resp.StatusMessage = HttpStatusCode.InternalServerError.ToString();
            }
            return resp;
        }

        [Route("UpdateUser")]
        public BaseResponse UpdateUser(UserCR req)
        {
            BaseResponse resp = new BaseResponse();
            try
            {
                if (ValidateRequest.UpdateUser(req))
                {
                    UserCRUD userCRUD = new UserCRUD();
                    var email = JWTService.ValidateToken(req.Token);
                    if (!string.IsNullOrEmpty(email) && email == req.User.email_id)
                    {
                        req.User.last_update = DateTime.Now;
                        if (userCRUD.UpdateUser(req.User) > 0)
                        {
                            resp.StatusCode = HttpStatusCode.OK;
                            resp.StatusMessage = HttpStatusCode.OK.ToString();
                        }
                        else
                        {
                            resp.StatusCode = HttpStatusCode.Ambiguous;
                            resp.StatusMessage = HttpStatusCode.Ambiguous.ToString();
                        }
                    }
                    else
                    {
                        resp.StatusCode = HttpStatusCode.Unauthorized;
                        resp.StatusMessage = HttpStatusCode.Unauthorized.ToString();
                    }
                }
                else
                {
                    resp.StatusCode = HttpStatusCode.BadRequest;
                    resp.StatusMessage = HttpStatusCode.BadRequest.ToString();

                }
            }
            catch (Exception es)
            {
                resp.StatusCode = HttpStatusCode.InternalServerError;
                resp.StatusMessage = HttpStatusCode.InternalServerError.ToString();
            }
            return resp;
        }


        [Route("Profile")]
        public UserResponse Profile(BaseResquest req)
        {
            UserResponse resp = new UserResponse();
            try
            {
                if (ValidateRequest.Profile(req))
                {
                    UserCRUD userCRUD = new UserCRUD();
                    var email = JWTService.ValidateToken(req.Token);
                    if (!string.IsNullOrEmpty(email))
                    {
                        var user = userCRUD.GetUser(email);
                        if (user != null)
                        {
                            resp.User = user;
                            resp.User.password =null;
                            resp.StatusCode = HttpStatusCode.OK;
                            resp.StatusMessage = HttpStatusCode.OK.ToString();
                        }
                        else
                        {
                            resp.StatusCode = HttpStatusCode.NotFound;
                            resp.StatusMessage = HttpStatusCode.NotFound.ToString();

                        }
                    }
                    else
                    {
                        resp.StatusCode = HttpStatusCode.Unauthorized;
                        resp.StatusMessage = HttpStatusCode.Unauthorized.ToString();
                    }
                }
                else
                {
                    resp.StatusCode = HttpStatusCode.BadRequest;
                    resp.StatusMessage = HttpStatusCode.BadRequest.ToString();

                }
            }
            catch (Exception es)
            {
                resp.StatusCode = HttpStatusCode.InternalServerError;
                resp.StatusMessage = HttpStatusCode.InternalServerError.ToString();
            }
            return resp;
        }
    }
}
