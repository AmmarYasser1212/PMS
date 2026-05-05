using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTO.TimeEntry
{
    public class TimeEntryDto
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public bool IsPaused { get; set; }

        public int AccumulatedSeconds { get; set; }

        public DateTime StartedAt { get; set; }

        public int CurrentSeconds { get; set; }
    }
}
