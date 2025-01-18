using Orders.Shared.Entities;

namespace Orders.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoryAsync();

            return;
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