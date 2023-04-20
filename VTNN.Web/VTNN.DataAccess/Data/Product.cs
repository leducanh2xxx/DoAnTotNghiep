namespace VTNN.DataAccess.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(150)]
        public string ProductName { get; set; }

        [StringLength(100)]
        public string Source { get; set; }

        [StringLength(100)]
        public string Weight { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        [Column(TypeName = "xml")]
        public string ListImage { get; set; }
        [Required(ErrorMessage = "Mô tả không được để trống")]
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,###}")]
        [Required(ErrorMessage = "Giá không được để trống")]
        public decimal? Price { get; set; }

        public decimal? PromotionPrice { get; set; }
        [Required(ErrorMessage = "Số lượng sản phẩm không được để trống")]
        public int? Amount { get; set; }

        public DateTime? Hot { get; set; }

        public int? ViewCount { get; set; }

        public DateTime? Created_At { get; set; }

        public DateTime? Updated_At { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
