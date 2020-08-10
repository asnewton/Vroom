using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using vroom.Extentions;

namespace vroom.Models
{
    public class Bike
    {
        public int Id { get; set; }
        public Make Make { get; set; }
        [ForeignKey("Make")]
        [RegularExpression("^[1-9]*$", ErrorMessage ="Please select value")]
        public int MakeID { get; set; }
        public Model Model { get; set; }
        [ForeignKey("Model")]
        [RegularExpression("^[1-9]*$", ErrorMessage = "Please select value")]
        public int ModelID { get; set; }

        [Required]
        [YearRangeTillDate(2000, ErrorMessage ="Invalid Year")]
        public int Year { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage ="Provide Milage")]
        public int Milage { get; set; }
        public string Features { get; set; }
        [Required(ErrorMessage ="Provide seller name")]
        public string SellerName { get; set; }
        [EmailAddress]
        public string SellerEmail { get; set; }
        [Required(ErrorMessage = "Provide seller phone number")]
        public string SellerPhone { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public int Price { get; set; }
        [Required]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "Please select value")]
        public string Currency { get; set; }
        public string ImagePath { get; set; }
    }
}
