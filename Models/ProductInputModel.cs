using System;
using System.ComponentModel.DataAnnotations;

namespace PROG6212_New_POE.Models
{
    public class ProductInputModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime? ProductionDate { get; set; }
    }
} 