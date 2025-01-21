using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Response;

namespace Orders.Backend.Repositories.Implementations
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly DataContext _context;

        public CountriesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<Country>> GetAsync(int id)
        {
            var country = await _context.Countries
                                .Include(c => c.States!)
                                .ThenInclude(c => c.Cities)
                                .FirstOrDefaultAsync(c => c.Id == id);
            if (country is null)
            {
                return new ActionResponse<Country>
                {
                    WassSuccees = false,
                    Message = "País no existe."
                };
            }

            return new ActionResponse<Country>
            {
                WassSuccees = true,
                Result = country
            };
        }

        public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync()
        {
           var countries = await _context.Countries
                                 //.Include(c => c.States)
                                 .OrderBy(x => x.Name)
                                 .ToListAsync();
            return new ActionResponse<IEnumerable<Country>>()
            {
                WassSuccees = true,
                Result = countries
            };
        }

        public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination)
        {
            var queryable =  _context.Countries
                                     .Include(c => c.States)
                                    .AsQueryable();
            return new ActionResponse<IEnumerable<Country>>()
            {
                WassSuccees = true,
                Result = await queryable
                                .OrderBy(x => x.Name)
                                .Paginate(pagination)
                                .ToListAsync()
            };
        }

    }
}
