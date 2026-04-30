using PMS.Application.DTO.User;
using PMS.Application.Interfaces.Repositories;
using PMS.Application.Interfaces.Services;
using PMS.Application.Interfaces;
using PMS.Domain.Entities;
using PMS.Infrastructre.Services.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Infrastructre.Services.userser
{
    public class UserServices :IUserServices
    {
        private readonly Irepsitory<User>_context;
        private readonly IunitOfWork _uow;

        public UserServices(Irepsitory<User> context, IunitOfWork uow) {
        
            _context = context;
            _uow = uow;
        }

        public async Task<int> CreateUserAsync(CreateUserDto dto)
        {
            var exists = await _context.ExistsAsync(u => u.Email == dto.Email);
            if (exists)
                throw new Exception("Email already exists");

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = dto.Password,
                Name = dto.Name,
                Avatar = dto.Avatar,
                CreatedAt = DateTime.UtcNow
            };

            await _context.AddAsync(user);
            await _uow.SaveChangesAsync();

            return user.Id;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {

            // we should first delete any categories and tags and tasks'user  cascade with categories and tags so we should remove tasks first

            var user = await _context.GetByIdAsync(id);

            if (user == null)
                return false;

             _context.Delete(user);

            await _uow.SaveChangesAsync();
            return true;

        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _context.GetAllAsync();

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name,
                Avatar = u.Avatar
            }).ToList();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _context.GetByIdAsync(id);

            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Avatar = user.Avatar
            };
        }

        public async Task <bool> UpdateUserAsync(UpdateUserDto dto)
        {
            var user = await _context.GetByIdAsync(dto.Id);
            if (user == null) return false;

            if (dto.Name != null)
                user.Name = dto.Name;

            if (dto.Avatar != null)
                user.Avatar = dto.Avatar;

          //  await _context.UpdateAsync(user);  entity من  طالما جبت db 

            await _uow.SaveChangesAsync();

            return true;
        }

    }
}
