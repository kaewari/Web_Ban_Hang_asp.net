using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SNShop.Models;

namespace SNShop.DAO
{
    public class UserDao
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }
        public int CheckUser(string password, string email)
        {
            password = Encode.GetMD5(password);
            var result = db.Users.SingleOrDefault(x => x.Email == email);
            if (result == null)
                return 0;
            else
            {
                if (result.PasswordHash != password)
                    return -1;
                else
                {
                    return 1;
                }
            }
        }
        public User GetUserByEmail(string email)
        {
            return db.Users.SingleOrDefault(s => s.Email == email);
        }
        public int GetUserById(int Id)
        {
            return db.Customers.SingleOrDefault(s => s.UserID == Id).Id;
        }
        public Role GetRoleByRoleName(string roleName)
        {
            var result = db.Roles.SingleOrDefault(s => s.Name == roleName);
            return result;
        }
        public Role GetRoleById(int id)
        {
            var result = db.UserRoles.First(s=>s.UserId == id).Role;
            return result;
        }
    }
}