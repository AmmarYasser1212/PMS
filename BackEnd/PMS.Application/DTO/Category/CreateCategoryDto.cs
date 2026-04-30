using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTO.Category
{
    public class CreateCategoryDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
    }
}
