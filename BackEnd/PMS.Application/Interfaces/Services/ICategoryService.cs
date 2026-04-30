using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Application.DTO.Category;
using PMS.Application.DTO.User;


namespace PMS.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<int> CreateAsync(CreateCategoryDto dto);
        Task<List<CategoryDto>> GetByUserAsync(int userId);
        Task<CategoryDto?> GetByIdAsync(int id,int userid);
        Task<bool> UpdateAsync(CategoryDto dto);
        Task<bool> DeleteAsync(int id, int userid);
    }
}
