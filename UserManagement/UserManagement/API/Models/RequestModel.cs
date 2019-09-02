using DAL.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace API.Models
{
    [DataContract]
    public class BaseResquest
    {
        [DataMember(Name ="token")]
        public string Token { get; set; }
    }
    [DataContract]
    public class UserCR : BaseResquest
    {
        [DataMember(Name = "user")]
        public user User { get; set; }
    }
    [DataContract]
    public class AuthenticateCR : BaseResquest
    {
        [DataMember(Name = "email_id")]
        public string EmailID { get; set; }
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }


}