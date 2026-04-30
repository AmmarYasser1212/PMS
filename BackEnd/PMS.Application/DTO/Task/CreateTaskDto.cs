using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTO.Task
{
    public class CreateTaskDto
    {
        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Priority { get; set; } = null!;
        public int UserId { get; set; }
        public int? CategoryId { get; set; }
    }
}
