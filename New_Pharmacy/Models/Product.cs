using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_Pharmacy.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [DataType(DataType.Date)]
        public DateTime PubDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ExpDate { get; set; }
        public int Quantity { get; set; }
        [DataType(DataType.Currency)]
        public int Price { get; set; }
        public string? Image { get; set; }
        public int CatId { get; set; }

        public virtual Category? Cat { get; set; }
        [NotMapped]
        public IFormFile FileUplaod { get; set; }
    }
}
