using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PMS.Application.DTO.Category
{
    public class CategoryDto
    {
        [Required(ErrorMessage ="Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "User Id is required")]
        public int UserId {  get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;
    }
}
