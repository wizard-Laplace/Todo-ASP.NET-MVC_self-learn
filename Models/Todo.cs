using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoApp.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Summary{ get; set; }
        public string Detail{ get; set; }
        public DateTime Limit{ get; set; }
        public bool Done { get; set; }
    }
}