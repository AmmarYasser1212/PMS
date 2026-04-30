using PMS.Application.DTO.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<TaskDto> CreateAsync(CreateTaskDto dto);
        Task<bool> UpdateAsync(UpdateTaskDto dto);
        Task<bool> DeleteAsync(int id,int userId);

        Task<TaskDto?> GetByIdAsync(int taskid, int userId);
        Task<List<TaskDto>> GetByUserAsync(int userId);

        Task<bool> ChangeStatusAsync(int taskId, string status, int userid);

        Task<List<TaskDto>> FilterAsync(int userId, int? categoryId, int? tagId, DateTime? from, DateTime? to);

        Task<List<TaskDto>> SearchAsync(int userId, string keyword);
    }
}
