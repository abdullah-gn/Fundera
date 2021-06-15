namespace testProject2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        [StringLength(50)]
        public string address { get; set; }

        public int? phone { get; set; }

        [StringLength(50)]
        public string password { get; set; }

        [StringLength(50)]
        public string img { get; set; }

        public int? age { get; set; }

        [Column(TypeName = "date")]
        public DateTime? graduation_year { get; set; }

        public int? roleid { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment> comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<project_likes> project_likes { get; set; }
    }
}
