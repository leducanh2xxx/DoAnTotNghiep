namespace VTNN.DataAccess.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Orders = new HashSet<Order>();
        }
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Email không được để trống!")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
        [StringLength(11, ErrorMessage = "Số điện thoại không đúng định dạng")]
        [MaxLength(11, ErrorMessage = "Số điện thoại không đúng định dạng")]
        [Required(ErrorMessage = "Số điện thoại không được để trống!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        [StringLength(200)]
        [DataType(DataType.Password)]
        [UIHint("Password")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự.")]
        public string Password { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Mật khẩu nhập lại không được để trống!")]
        [Compare("password", ErrorMessage = "Mật khẩu không khớp.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự.")]
        public string ConfirmPassword { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Tên không được để trống!")]
        public string FullName { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Địa chỉ không được để trống!")]
        public string Address { get; set; }
        [UIHint("Role")]
        public int? Role { get; set; }
        [UIHint("Active")]
        public bool Is_Active { get; set; }

        public DateTime? Created_At { get; set; }

        public DateTime? Updated_At { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
