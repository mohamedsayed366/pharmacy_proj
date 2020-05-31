using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;



namespace Pharma.Models
{
    public class Cart
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [ForeignKey("Product")]
        public int Product_id { get; set; }
        public DateTime Added_To { get; set; }
        public virtual Product Product { get; set; }

    }
}