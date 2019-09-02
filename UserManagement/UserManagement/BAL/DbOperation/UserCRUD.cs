using DAL.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DbOperation
{
    public class UserCRUD
    {
        Entities db;

        public UserCRUD()
        {
            db = new Entities();
            db.Database.Connection.Open();
        }

        ~UserCRUD()
        {
            db.Database.Connection.Close();
        }
        public user GetUser(string emailID)
        {
            var user = db.users.Where(x => x.email_id == emailID).FirstOrDefault();
            return user;
        }
        public user GetUser(string emailAddress, string password)
        {
            var user = db.users.Where(x => x.email_id.ToLower().Equals(emailAddress.ToLower()) && x.password.ToLower().Equals(password.ToLower())).FirstOrDefault();
            return user;
        }
        public bool IsUserValid(string emailAddress, string password)
        {
            var status = db.users.Any(x => x.email_id.ToLower().Equals(emailAddress.ToLower()) && x.password.ToLower().Equals(password.ToLower()));
            return status;
        }


        public bool IsUserEmailExists(string emailAddress)
        {
            var status = db.users.Any(x => x.email_id.ToLower().Equals(emailAddress.ToLower()));
            return status;
        }
        public bool IsUserActive(string emailAddress)
        {
            var status = db.users.Any(x => x.email_id.ToLower().Equals(emailAddress.ToLower()) && x.is_active == true);
            return status;
        }
        public bool IsUserLocked(string emailAddress)
        {
            var status = db.users.Any(x => x.email_id.ToLower().Equals(emailAddress.ToLower()) && x.is_locked == true);
            return status;
        }

        public int AddUser(user user)
        {
            user.is_active = true;
            user.is_locked = false;
            db.Entry(user).State = System.Data.Entity.EntityState.Added;
            db.users.Add(user);
            return db.SaveChanges();
        }
        public int UpdateUser(user user)
        {
            var tempUser = GetUser(user.email_id);

            if (!string.IsNullOrEmpty(user.email_id))
                tempUser.email_id = user.email_id;
            if (!string.IsNullOrEmpty(user.first_name))
                tempUser.first_name = user.first_name;
            if (!string.IsNullOrEmpty(user.last_name))
                tempUser.last_name = user.last_name;
            if (!string.IsNullOrEmpty(user.password))
                tempUser.password = user.password;
            if (user.is_active != null)
                tempUser.is_active = user.is_active;
            if (user.is_locked != null)
                tempUser.is_locked = user.is_locked;

            return db.SaveChanges();
        }

    }
}
