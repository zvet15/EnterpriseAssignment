using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 

namespace WebApplication1.Models
{
    public class Items
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        [Required]
        public int ItemTypeId { get; set; }


        [Index("IX_ITEM", 1, IsUnique = true)] 
        public string SellerId { get; set; } 
        public virtual ApplicationUser Seller { get; set; }

        [Required (ErrorMessage ="Quantity field is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Only possitive number allowed")]
        [Index("IX_ITEM", 2,IsUnique = true)]
        public int Quantity { get; set; }

        [Required]
        [Index("IX_ITEM",3,IsUnique = true)]
        public int QualityId { get; set; }

        [Required(ErrorMessage ="Price field is required")]
        [Range(1,float.MaxValue,ErrorMessage ="Only possitive number allowed")]
        [Index("IX_ITEM",4,IsUnique = true)]
        public float Price { get; set; }

        [Required]
        [Index("IX_Date", 5)]
        public DateTime Date { get; set; }

        public virtual ItemTypes ItemTypes { get; set; }
        public virtual Quality Quality { get; set; } 

    }
}