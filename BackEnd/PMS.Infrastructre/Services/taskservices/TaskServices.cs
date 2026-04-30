using PMS.Application.DTO.Task;
using PMS.Application.Interfaces.Repositories;
using PMS.Application.Interfaces.Services;
using PMS.Domain.Entities;
using PMS.Infrastructre.Services.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Infrastructre.Services.taskservices
{
    public class TaskServices : ITaskService
    {
        private readonly Irepsitory<TaskItem> _taskRepo;
        private readonly Irepsitory<TaskTag> _taskTagRepo;
        private readonly Irepsitory<Category> _category;
        private readonly Irepsitory<ScheduleTask> _scheduleTaskRepo;
        private readonly IunitOfWork _uow;


        public TaskServices(
           Irepsitory<TaskItem> taskRepo,
           Irepsitory<Category> category,
           Irepsitory<TaskTag> taskTagRepo,
           Irepsitory<ScheduleTask> scheduleTaskRepo,
           IunitOfWork uow)
        {
            _taskRepo = taskRepo;
            _taskTagRepo = taskTagRepo;
            _category = category;
            _scheduleTaskRepo = scheduleTaskRepo;
            _uow = uow;
        }

        public async Task<bool> ChangeStatusAsync(int taskId, string status,int userid)
        {
            var task = await _taskRepo.FindOneAsync(t=>t.Id==taskId&&t.UserId==userid);

            if (task == null) return false;

            task.Status = status;

            await _taskRepo.UpdateAsync(task);
            await _uow.SaveChangesAsync();

            return true;
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
        {
            if (dto.CategoryId != null)
            {
            var category= await _category.ExistsAsync(id=>id.Id==dto.CategoryId&&id.UserId==dto.UserId);
            if (category ==false)
                dto.CategoryId = null;
            }
           

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Date = dto.Date,
                Time = dto.Time,
                Priority = dto.Priority,
                Status = "Pending",
                UserId = dto.UserId,
                CategoryId = dto.CategoryId
            };

            await _taskRepo.AddAsync(task);
            await _uow.SaveChangesAsync();

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Date = task.Date,
                Time = task.Time,
                Priority = task.Priority,
                Status = task.Status
            };
        }

        public async Task<bool> DeleteAsync(int taskId,int userId)
        {

           return await  _taskRepo.DeleteWhereAsync(t=>t.Id==taskId&&t.UserId==userId) >0;

        }

        public async Task<List<TaskDto>> FilterAsync(int userId, int? categoryId, int? tagId, DateTime? from, DateTime? to)
        {
            return await _taskRepo.FindAsyncAdvanced(
           t =>
               t.UserId == userId &&
               (categoryId == null || t.CategoryId == categoryId) &&
               (tagId == null ||t.TaskTags.Any(tt => tt.TagId == tagId)) &&
               (from == null || t.Date >= from) &&
               (to == null || t.Date <= to),

           t => new TaskDto
           {
               Id = t.Id,
               Title = t.Title,
               Description = t.Description,
               Date = t.Date,
               Time = t.Time,
               Priority = t.Priority,
               Status = t.Status
           });
        }

        public async Task<TaskDto?> GetByIdAsync(int taskid,int userId)
        {
            var data = await _taskRepo.FindAsyncAdvanced(
             t => t.Id == taskid&&t.UserId==userId,
             t => new TaskDto
             {
                 Id = t.Id,
                 Title = t.Title,
                 Description = t.Description,
                 Date = t.Date,
                 Time = t.Time,
                 Priority = t.Priority,
                 Status = t.Status
             });

            return data.FirstOrDefault();
        }

        public async Task<List<TaskDto>> GetByUserAsync(int userId)
        {
              return await _taskRepo.FindAsyncAdvanced(
                t => t.UserId == userId,
                t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Date = t.Date,
                    Time = t.Time,
                    Priority = t.Priority,
                    Status = t.Status
                });
        }
   
        public async Task<List<TaskDto>> SearchAsync(int userId, string keyword)
        {
            return await _taskRepo.FindAsyncAdvanced(
            t =>
                t.UserId == userId &&
                (t.Title.Contains(keyword) ||
                 t.Description!.Contains(keyword)),

            t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Date = t.Date,
                Time = t.Time,
                Priority = t.Priority,
                Status = t.Status
            });
        }

        public async Task<bool> UpdateAsync(UpdateTaskDto dto)
        {
            var task = await _taskRepo.FindOneAsync(t=>t.Id==dto.Id&&t.UserId==dto.UserId);
            if (task == null) return false;

            if (dto.CategoryId != null)
            {
           var category = await _category.ExistsAsync(id => id.Id == dto.CategoryId&&id.UserId==dto.UserId);
              if (category == false)
                dto.CategoryId = null;
            }

           

            if (dto.Title != null) task.Title = dto.Title;
            if (dto.Description != null) task.Description = dto.Description;
            if (dto.Date != null) task.Date = dto.Date;
            if (dto.Time != null) task.Time = dto.Time;
            if (dto.Priority != null) task.Priority = dto.Priority;
            if (dto.Status != null) task.Status = dto.Status;
            if (dto.CategoryId != null) task.CategoryId = dto.CategoryId;

            await _taskRepo.UpdateAsync(task);
            await _uow.SaveChangesAsync();

            return true;
        }

        public async Task<TaskDeleteCheckResult> CheckBeforeDeleteTaskAsync(int taskId,int userid)
        {
           
            var exists = await _taskRepo.ExistsAsync(t => t.Id == taskId&&t.UserId==userid);
            if (!exists)
                return new TaskDeleteCheckResult
                {
                    CanDeleteDirectly = false,
                    Message = "Task not found"
                };

            
            var hasSchedule = await _scheduleTaskRepo
                .ExistsAsync(st => st.TaskId == taskId&&st.Task.UserId==userid);//&& st.Task.UserId == userid


            if (!hasSchedule)
                return new TaskDeleteCheckResult
                {
                    CanDeleteDirectly = true
                };

           
            return new TaskDeleteCheckResult
            {
                CanDeleteDirectly = false,
                HasScheduleConflict = true,
                Message = "This task is linked to a schedule",

                Options = new List<string>
        {
            "ReplaceTask",     
            "ReplanSchedule",  
            "ClearSlot",      
            "Cancel"           
        }
            };
        }


       // await ReplaceTaskAsync(oldTaskId, userId, old => newTaskId);
       // await ReplaceTaskAsync(oldTaskId, userId, old => null);
        public async Task<bool> ReplaceTaskAsync(int oldTaskId,int userid,Func<int?,int?>taskresolver)
        {

            var newTaskId = taskresolver(oldTaskId);
           
            if (newTaskId != null)
            {
                if (oldTaskId == newTaskId)
                    return true;

                var isValidTask = await _taskRepo.ExistsAsync(t => t.Id == newTaskId && t.UserId == userid && t.Status != "Completed");
                if (!isValidTask)
                    return false;

            }
                var scheduleTasks = await _scheduleTaskRepo.FindAsync(st => st.TaskId == oldTaskId);
                     if (!scheduleTasks.Any())
                       {return true;}
            
            foreach (var item in scheduleTasks)
            {
                item.TaskId = newTaskId;
               // await _scheduleTaskRepo.UpdateAsync(item);
            }
            await _uow.SaveChangesAsync();
            return true;

            //this task is replaced in all schedules m,w,d because i want delete it
        }
    }


}