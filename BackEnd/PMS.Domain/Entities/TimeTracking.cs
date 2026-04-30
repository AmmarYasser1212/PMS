using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class TimeTracking
    {


        public int Id { get; set; }

        [Required]
        public int TaskId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? Duration { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; }

        public DateTime? PausedAt { get; set; }

        public int TotalPausedSeconds { get; set; } = 0;

        public TaskItem Task { get; set; }=new  TaskItem();
    }
}
