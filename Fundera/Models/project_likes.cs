namespace testProject2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class project_likes
    {
        public int proj_id { get; set; }

        public int user_id { get; set; }

        public int id { get; set; }

        public virtual project project { get; set; }

        public virtual user user { get; set; }
    }
}
