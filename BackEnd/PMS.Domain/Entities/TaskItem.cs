using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string ?Description { get; set; }

        public DateTime? Date { get; set; }

        public TimeSpan? Time { get; set; }

        [Required, MaxLength(20)]
        public string Priority { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Status { get; set; }= null!;

        [Required]
        public int UserId { get; set; }

        public int? CategoryId { get; set; }

        public User User { get; set; }
        public Category? Category { get; set; }

        public ICollection<TaskTag>? TaskTags { get; set; }
        public ICollection<TimeTracking>? TimeTrackings { get; set; }
    }
}
