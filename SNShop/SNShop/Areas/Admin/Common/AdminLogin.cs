using SNShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SNShop.Areas.Admin.Common
{
    public class AdminLogin
    {
        public long UserID { set; get; }
        public string UserName { set; get; }
        public string Email { set; get; }
        public string Image { set; get; }
        public string Roles { set; get; }
    }
}