using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTO.Task
{
    public class UpdateTaskDto
    {
        [Required(ErrorMessage ="ID is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserId {  get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }

        public string? Priority { get; set; }
        public string? Status { get; set; }

        public int? CategoryId { get; set; }
    }
}
