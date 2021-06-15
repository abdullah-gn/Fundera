namespace testProject2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("user")]
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            comments = new HashSet<comment>();
            project_likes = new HashSet<project_likes>();
        }

        public int id { get; set; }

        [Remote("emailChecker", "User", ErrorMessage = "Email Is Exits")]
        [StringLength(50)]
        [Required(ErrorMessage = "Email Is Requird")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", ErrorMessage = "Invalid Email")]
        public string email { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        [StringLength(50)]
        public string address { get; set; }
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone Number Must Be 11 Number")]
        public string phone { get; set; }

        [StringLength(50)]
        public string password { get; set; }
        [DataType(DataType.Password)]
        [NotMapped]
        [System.ComponentModel.DataAnnotations.Compare("password", ErrorMessage = "Password ddint Mathch")]
        public string confirmPassword { get; set; }

        [StringLength(50)]
        public string img { get; set; }


        public int? age { get; set; }


        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Enter Graduation Year Like 2017 , 2018 ..... ")]
        public string graduation_year { get; set; }

        public int? role_id { get; set; }

        public bool? blocked { get; set; }

        public string aboutMe { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment> comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<project_likes> project_likes { get; set; }

        public virtual role role { get; set; }

        public virtual login login { get; set; }
    }
}
