using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTO.Task
{
    public class TaskDto
    {

        public int Id { get; set; }
        public string? Title { get; set; } = null!;
        public string? Description { get; set; }

        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }

        public string Priority { get; set; } = null!;
        public string Status { get; set; } = null!;


    }
}
