namespace TelerikMvcApp4
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Car
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }


        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }



    }


    public class Category {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<Car> Cars { get; set; }
    
    }
}
