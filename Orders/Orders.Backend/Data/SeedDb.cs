﻿using Microsoft.EntityFrameworkCore;
using Orders.Backend.Helpers;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Enums;

namespace Orders.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUsersUnitOfWork _usersUnitOfWork;
        private readonly IFileStorage _fileStorage;

        public SeedDb(DataContext context, IUsersUnitOfWork usersUnitOfWork, IFileStorage fileStorage)
        {
            _context = context;
            _usersUnitOfWork = usersUnitOfWork;
            _fileStorage = fileStorage;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoryAsync();
            await CheckRolesAsync();
            //await CheckProductsAsync();


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

        //private async Task CheckProductsAsync()
        //{
            //if (!_context.Products.Any())
            //{
            //    await AddProductAsync("Adidas Barracuda", 270000M, 12F, new List<string>() { "Calzado", "Deportes" }, new
            //    List<string>() { "adidas_barracuda.png" });
            //    await AddProductAsync("Adidas Superstar", 250000M, 12F, new List<string>() { "Calzado", "Deportes" }, new
            //    List<string>() { "Adidas_superstar.png" });
            //    await AddProductAsync("Aguacate", 5000M, 500F, new List<string>() { "Comida" }, new List<string>() { "Aguacate1.png", "Aguacate2.png", "Aguacate3.png" });
            //    await AddProductAsync("AirPods", 1300000M, 12F, new List<string>() { "Tecnología", "Apple" }, new
            //    List<string>() { "airpos.png", "airpos2.png" });
            //    await AddProductAsync("Akai APC40 MKII", 2650000M, 12F, new List<string>() { "Tecnología" }, new
            //    List<string>() { "Akai1.png", "Akai2.png", "Akai3.png" });
            //    await AddProductAsync("Apple Watch Ultra", 4500000M, 24F, new List<string>() { "Apple", "Tecnología" }, new
            //    List<string>() { "AppleWatchUltra1.png", "AppleWatchUltra2.png" });
            //    await AddProductAsync("Audifonos Bose", 870000M, 12F, new List<string>() { "Tecnología" }, new List<string>() { "audifonos_bose.png" });
            //    await AddProductAsync("Bicicleta Ribble", 12000000M, 6F, new List<string>() { "Deportes" }, new List<string>() { "bicicleta_ribble.png" });
            //    await AddProductAsync("Camisa Cuadros", 56000M, 24F, new List<string>() { "Ropa" }, new List<string>() { "camisa_cuadros.png" });
            //    await AddProductAsync("Casco Bicicleta", 820000M, 12F, new List<string>() { "Deportes" }, new List<string>() { "casco_bicicleta.png", "casco.png" });

            //    await AddProductAsync("Gafas deportivas", 160000M, 24F, new List<string>() { "Deportes" }, new List<string>() { "Gafas1.png", "Gafas2.png", "Gafas3.png" });
            //    await AddProductAsync("Hamburguesa triple carne", 25500M, 240F, new List<string>() { "Comida" }, new
            //    List<string>() { "Hamburguesa1.png", "Hamburguesa2.png", "Hamburguesa3.png" });
            //    await AddProductAsync("iPad", 2300000M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "ipad.png" });
            //    await AddProductAsync("iPhone 13", 5200000M, 6F, new List<string>() { "Tecnología", "Apple" }, new
            //    List<string>() { "iphone13.png", "iphone13b.png", "iphone13c.png", "iphone13d.png" });
            //    await AddProductAsync("Johnnie Walker Blue Label 750ml", 1266700M, 18F, new List<string>() { "Licores" }, new
            //    List<string>() { "JohnnieWalker3.png", "JohnnieWalker2.png", "JohnnieWalker1.png" });
            //    await AddProductAsync("KOOY Disfraz inflable de gallo para montar", 150000M, 28F, new List<string>() { "Juguetes" }, new List<string>() { "KOOY1.png", "KOOY2.png", "KOOY3.png" });
            //    await AddProductAsync("Mac Book Pro", 12100000M, 6F, new List<string>() { "Tecnología", "Apple" }, new
            //    List<string>() { "mac_book_pro.png" });
            //    await AddProductAsync("Mancuernas", 370000M, 12F, new List<string>() { "Deportes" }, new List<string>() { "mancuernas.png" });
            //    await AddProductAsync("Mascarilla Cara", 26000M, 100F, new List<string>() { "Belleza" }, new List<string>() { "mascarilla_cara.png" });
            //    await AddProductAsync("New Balance 530", 180000M, 12F, new List<string>() { "Calzado", "Deportes" }, new
            //    List<string>() { "newbalance530.png" });
            //    await AddProductAsync("New Balance 565", 179000M, 12F, new List<string>() { "Calzado", "Deportes" }, new
            //    List<string>() { "newbalance565.png" });
            //    await AddProductAsync("Nike Air", 233000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "nike_air.png" });
            //    await AddProductAsync("Nike Zoom", 249900M, 12F, new List<string>() { "Calzado", "Deportes" }, new
            //    List<string>() { "nike_zoom.png" });
            //    await AddProductAsync("Buso Adidas Mujer", 134000M, 12F, new List<string>() { "Ropa", "Deportes" }, new
            //    List<string>() { "buso_adidas.png" });
            //    await AddProductAsync("Suplemento Boots Original", 15600M, 12F, new List<string>() { "Nutrición" }, new
            //    List<string>() { "Boost_Original.png" });
            //    await AddProductAsync("Whey Protein", 252000M, 12F, new List<string>() { "Nutrición" }, new List<string>() { "whey_protein.png" });
            //    await AddProductAsync("Arnes Mascota", 25000M, 12F, new List<string>() { "Mascotas" }, new List<string>() { "arnes_mascota.png" });
            //    await AddProductAsync("Cama Mascota", 99000M, 12F, new List<string>() { "Mascotas" }, new List<string>() { "cama_mascota.png" });
            //    await AddProductAsync("Teclado Gamer", 67000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new
            //    List<string>() { "teclado_gamer.png" });
            //    await AddProductAsync("Ring de Lujo 17", 1600000M, 33F, new List<string>() { "Autos" }, new List<string>() { "Ring1.png", "Ring2.png" });
            //    await AddProductAsync("Silla Gamer", 980000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new
            //    List<string>() { "silla_gamer.png" });
            //    await AddProductAsync("Mouse Gamer", 132000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new
            //    List<string>() { "mouse_gamer.png" });
            //    await _context.SaveChangesAsync();
            //}
        //}


        private async Task AddProductAsync(string name, decimal price, float stock, List<string> categories, List<string> images)
        {
            Product prodcut = new()
            {
                Description = name,
                Name = name,
                Price = price,
                Stock = stock,
                ProductCategories = new List<ProductCategory>(),
                ProductImages = new List<ProductImage>()
            };
            foreach (var categoryName in categories)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
                if (category != null)
                {
                    prodcut.ProductCategories.Add(new ProductCategory { Category = category });
                }
            }
            foreach (string? image in images)
            {
                var filePath = $"{Environment.CurrentDirectory}\\Images\\products\\{image}";
                var fileBytes = File.ReadAllBytes(filePath);
                var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "products");
                prodcut.ProductImages.Add(new ProductImage { Image = imagePath });
            }
            _context.Products.Add(prodcut);
        }


    }
}