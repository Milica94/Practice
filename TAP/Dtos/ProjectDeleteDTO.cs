using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAP.Dtos
{
    public class ProjectDeleteDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public int ClientId { get; set; }
    }
}
