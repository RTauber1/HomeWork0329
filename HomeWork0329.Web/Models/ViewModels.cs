using HomeWork0329.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork0329.Web.Models
{
    public class IndexViewModel
    {
        public List<Posts> Posts { get; set; }
    }
    public class MyAccountViewModel
    {
        public List<Posts> UsersPosts { get; set; }
    }
}
