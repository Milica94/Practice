using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TAP.Models
{
    public partial class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        [MaxLength(10), Required]
        public string Status { get; set; }
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
