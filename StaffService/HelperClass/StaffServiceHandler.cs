using StaffService.Context;
using StaffService.DTOs;
using StaffService.Models;

namespace StaffService.HelperClass
{
    public class StaffServiceHandler
    {
        private readonly StaffDbContext _context;
        private readonly MessagePublisher _publisher;

        public StaffServiceHandler(StaffDbContext context, MessagePublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task RegisterStaffAsync(StaffRegistrationDto dto)
        {
            var staff = new Staff
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();

            _publisher.SendMessage(dto);
        }
    }
}
