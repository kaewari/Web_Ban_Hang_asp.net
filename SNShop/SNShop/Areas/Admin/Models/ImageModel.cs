﻿using System.Web;

namespace SNShop.Areas.Admin.Models
{
    public class ImageModel
    {
        public string Path { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}