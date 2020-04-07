using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HouseListMVC.Models
{
    public class House
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Address { get; set; }

        [Required]
        [Range(1, 15000)]
        public int Number { get; set; }

        [Required]
        [Range(1, 150)]
        public int Age { get; set; }

        [Required]
        [Range(1, 3000)]
        public int Square { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Description { get; set; }
               
        [ForeignKey("AppUser")]
        public int AppUserId { get; set; }
    }
}
