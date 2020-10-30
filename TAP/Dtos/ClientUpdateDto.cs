using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAP.Models;

namespace TAP.Dtos
{
    public class ClientUpdateDto
    {

        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public virtual Project Project { get; set; }


    }
}
