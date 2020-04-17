using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ItemTypes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemTypeId { get; set; }

        [Required(ErrorMessage ="Item Type name is required")]
        public string Name { get; set; }

        public string Image { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual Categories Categories { get; set; }

        public virtual ICollection<Items> Items { get; set; }
    }
}