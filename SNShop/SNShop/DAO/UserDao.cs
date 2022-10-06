﻿using System.Linq;
using SNShop.Models;

namespace SNShop.DAO
{
    public class UserDao
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        public bool CheckEmail(string email)
        {
            var checkDuplicate = db.Users.FirstOrDefault(s => s.Email == email);
            if (checkDuplicate != null)
            {
                return true;
            }
            return false;
        }
        public bool CheckEmail(string email, int id)
        {
            var checkDuplicate = db.Users.FirstOrDefault(s => s.Id == id).Email;
            if (checkDuplicate == email)
            {
                return false;
            }
            return true;
        }
        public bool CheckUsername(string username, int id)
        {
            var checkDuplicate = db.Users.FirstOrDefault(s => s.Id == id).Username;
            if(checkDuplicate == username)
            {
                return false;
            }
            return true;
        }
        public int CheckCustomer(string password, string email)
        {
            password = Encode.GetMD5(password);
            var result = db.Users.SingleOrDefault(x => x.Email == email);
            if (result == null || result.PasswordHash != password)
                return 0;
            else
            {
                if (db.UserRoles.FirstOrDefault(s => s.UserId == result.Id).Role.Name == "Users")
                    return 1;
                return -1;
            }
        }
        public int CheckSales(string password, string email)
        {
            password = Encode.GetMD5(password);
            var result = db.Users.SingleOrDefault(x => x.Email == email);
            if (result == null || result.PasswordHash != password)
                return 0;
            else
            {
                if (db.UserRoles.FirstOrDefault(s => s.UserId == result.Id).Role.Name == "Members")
                    return 1;
                return -1;
            }
        }
        public int CheckAdmin(string password, string email)
        {
            password = Encode.GetMD5(password);
            var result = db.Users.SingleOrDefault(x => x.Email == email);
            if (result == null || result.PasswordHash != password)
                return 0;
            else
            {
                if (db.UserRoles.FirstOrDefault(s => s.UserId == result.Id).Role.Name == "Admin")
                    return 1;
                return -1;
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