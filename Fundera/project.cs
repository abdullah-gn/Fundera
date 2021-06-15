namespace testProject2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("project")]
    public partial class project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public project()
        {
            comments = new HashSet<comment>();
            project_likes = new HashSet<project_likes>();
        }

        public int id { get; set; }

        [StringLength(50)]
        public string title { get; set; }

        public string bref { get; set; }

        public string description { get; set; }

        public string prototype { get; set; }

        public int? cat_id { get; set; }

        public int? fund_id { get; set; }

        public DateTime? Proj_date { get; set; }

        public virtual catalog catalog { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment> comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<project_likes> project_likes { get; set; }
    }
}
