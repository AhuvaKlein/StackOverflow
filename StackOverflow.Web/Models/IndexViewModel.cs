using StackOverflow.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackOverflow.Web.Models
{
    public class IndexViewModel
    {
        public Question Question { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}
