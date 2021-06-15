namespace testProject2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("comment")]
    public partial class comment
    {
        public int id { get; set; }

        [StringLength(50)]
        public string com_content { get; set; }

        public int? proj_id { get; set; }

        public int? user_id { get; set; }

        public DateTime? date { get; set; }

        public virtual project project { get; set; }

        public virtual user user { get; set; }
    }
}
