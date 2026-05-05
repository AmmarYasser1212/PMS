using PMS.Application.DTO.TimeEntry;
using PMS.Application.Interfaces.Repositories;
using PMS.Application.Interfaces.Services;
using PMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Services.TimeTrackingServices
{
    public class TimeTrackingServices : ITimeTrackingService
    {

        private readonly Irepsitory<TimeTracking> _irepsitory;
        private readonly IunitOfWork _unitOfWork;

        public TimeTrackingServices(Irepsitory<TimeTracking> irepsitory, IunitOfWork unitOfWork)
        {
            _irepsitory = irepsitory;
            _unitOfWork = unitOfWork;
        }

        public async Task<TimeEntryDto?> GetActiveAsync(int userId)
        {
            var result = await _irepsitory
            .FindAsyncAdvanced(e =>
                e.UserId == userId &&
                e.EndedAt == null,
              e => new TimeEntryDto
              {
                  Id = e.Id,
                  TaskId = e.TaskId,
                  IsPaused = e.IsPaused,
                  AccumulatedSeconds = e.AccumulatedSeconds,
                  StartedAt = e.StartedAt,
                  CurrentSeconds = e.IsPaused
                    ? e.AccumulatedSeconds
                    : e.AccumulatedSeconds +
                      (int)(DateTime.UtcNow - e.StartedAt).TotalSeconds
              });
          return result.FirstOrDefault();
        }

        public async Task<TimeEntryDto> PauseAsync(int entryId, int userId)
        {


            var entry = await GetActiveEntry(entryId, userId);

            var elapsed = (int)(DateTime.UtcNow - entry.StartedAt).TotalSeconds;
            entry.AccumulatedSeconds += elapsed;
            entry.IsPaused = true;



            await _unitOfWork.SaveChangesAsync();

            return new TimeEntryDto
            {
                Id= entryId,
                TaskId = entry.TaskId,
                StartedAt = entry.StartedAt,
                AccumulatedSeconds = entry.AccumulatedSeconds,
                IsPaused = entry.IsPaused

            };

        }

        private async Task<TimeTracking> GetActiveEntry(int entryId, int userId)
        {
            return await _irepsitory.FindOneAsync(e =>
                       e.Id == entryId &&
                       e.UserId == userId &&
                       e.EndedAt == null &&
                       !e.IsPaused)
                   ?? throw new Exception("no timer'id runnin");
        }

        public async Task<TimeEntryDto> ResumeAsync(int entryId, int userId)
        {
            var entry = await _irepsitory.FindOneAsync(e =>
           e.Id == entryId &&
           e.UserId == userId &&
           !e.IsPaused 
           )
           ?? throw new Exception("all timers paused");

            entry.StartedAt = DateTime.UtcNow;

            entry.IsPaused = false;

            await _unitOfWork.SaveChangesAsync();

            return new TimeEntryDto
            {

                TaskId = entry.TaskId,
                StartedAt = entry.StartedAt,
                AccumulatedSeconds = entry.AccumulatedSeconds,
                IsPaused = entry.IsPaused

            };
        }

        public async Task<TimeEntryDto> StartAsync(int taskId, int userId)
        {
            var hasActive = await _irepsitory.ExistsAsync(e =>
            e.UserId == userId &&
            e.EndedAt == null &&
            !e.IsPaused);

            if (hasActive)
                throw new Exception("Already is running");

            var entry = new TimeTracking
            {
                
                TaskId = taskId,
                UserId = userId,
                StartedAt = DateTime.UtcNow,
                AccumulatedSeconds = 0,
                IsPaused = false
            };

            var dto = new TimeEntryDto
            {
                TaskId = taskId,
                StartedAt = DateTime.UtcNow,
                AccumulatedSeconds = 0,
                IsPaused = false
            };

            await _irepsitory.AddAsync(entry);
            await _unitOfWork.SaveChangesAsync();

            return dto;
        }

        public async Task<TimeEntryDto> StopAsync(int entryId, int userId)
        {
            var entry =  await _irepsitory.FindOneAsync(e =>
           e.Id == entryId &&
           e.UserId == userId &&
           e.EndedAt == null
           )
           ?? throw new Exception("all timers paused");

            if (!entry.IsPaused)
            {
                var elapsed = (int)(DateTime.UtcNow - entry.StartedAt).TotalSeconds;
                entry.AccumulatedSeconds += elapsed;
            }

            entry.EndedAt = DateTime.UtcNow;
            entry.IsPaused = false;

            await _irepsitory.UpdateAsync(entry);

            return new TimeEntryDto {

                Id = entry.Id, 
                TaskId = entry.TaskId,
                StartedAt = entry.StartedAt,
                AccumulatedSeconds = entry.AccumulatedSeconds,
                IsPaused = entry.IsPaused,

                CurrentSeconds = entry.IsPaused
                ? entry.AccumulatedSeconds
                : entry.AccumulatedSeconds +
                  (int)(DateTime.UtcNow - entry.StartedAt).TotalSeconds


            };
        }
    }
}
