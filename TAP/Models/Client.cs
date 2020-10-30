using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TAP.Models
{
    public partial class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual Project Project { get; set; }
    }
}
