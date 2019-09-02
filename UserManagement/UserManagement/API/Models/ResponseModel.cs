using DAL.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

namespace API.Models
{
    [DataContract]
    public class BaseResponse
    {
        [DataMember(Name = "status_code")]
        public HttpStatusCode StatusCode { get; set; }
        [DataMember(Name = "status_message")]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public class AuthenticeResponse : BaseResponse
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }
    }
    [DataContract]
    public class UserResponse : BaseResponse
    {
        [DataMember(Name = "user")]
        public user User { get; set; }
    }
}