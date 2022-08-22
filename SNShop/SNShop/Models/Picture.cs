using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SNShop.Models
{
    public class Picture
    {
        public dynamic Id { get; set; }
        public bool is_silhouette { get; set; }
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }
}