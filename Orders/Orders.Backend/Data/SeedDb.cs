using Microsoft.EntityFrameworkCore;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Enums;

namespace Orders.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUsersUnitOfWork _usersUnitOfWork;

        public SeedDb(DataContext context, IUsersUnitOfWork usersUnitOfWork)
        {
            _context = context;
            _usersUnitOfWork = usersUnitOfWork;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoryAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Nehuen", "Torres", "nehuen@yopmail.com", "+56 9 65899544", "Calle la cabaña", UserType.Admin);

            return;
        }

        private async Task CheckRolesAsync()
        {
            await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
            await _usersUnitOfWork.CheckRoleAsync(UserType.User.ToString());
        }
        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string
                                                phone, string address, UserType userType)
        {
            var user = await _usersUnitOfWork.GetUserAsync(email);
            if (user == null)
            {
                var city = await _context.Cities.FirstOrDefaultAsync(x => x.Name == "Concepción");
                city ??= await _context.Cities.FirstOrDefaultAsync();

                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = city,
                    UserType = userType,
                };
                await _usersUnitOfWork.AddUserAsync(user, "123456");
                await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());

                var token = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
                await _usersUnitOfWork.ConfirmEmailAsync(user, token);
            }
            return user;
        }
        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Chile",
                    States =
                    [  
                        new State()
                        {
                            Name = "Región del Biobío",
                                Cities = 
                                [
                                    new() { Name = "Concepción" },
                                    new() { Name = "Tomé" },
                                    new() { Name = "Talcahuano" },
                                ]
                        },    
                     ]
                  });
             }
            await _context.SaveChangesAsync();
        }

        private async Task CheckCategoryAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Computadores y Tablets" });
                _context.Categories.Add(new Category { Name = "Accesorios y Periféricos" });
                _context.Categories.Add(new Category { Name = "Almacenamiento y Discos Duros" });
                _context.Categories.Add(new Category { Name = "Audífonos" });
                _context.Categories.Add(new Category { Name = "Computación" });
                await _context.SaveChangesAsync();
            }
        }
    }
}