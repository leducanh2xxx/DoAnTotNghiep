namespace VTNN.DataAccess.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }

        [StringLength(250)]
        public string ProductName { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public DateTime? Created_At { get; set; }

        public DateTime? Updated_At { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
