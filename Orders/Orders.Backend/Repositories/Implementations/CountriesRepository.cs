using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Response;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                                 .Include(c => c.States)
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

            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<Country>>()
            {
                WassSuccees = true,
                Result = await queryable
                                .OrderBy(x => x.Name)
                                .Paginate(pagination)
                                .ToListAsync()
            };
        }

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.Countries.AsQueryable();

            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);

            return new ActionResponse<int>
            {
                WassSuccees = true,
                Result = totalPages
            };
        }

        public override async Task<ActionResponse<IEnumerable<Country>>> GetPaginationAsync(PaginationDTO pagination)
        {
            var queryable = _context.Countries
                            .Include(c => c.States)
                            .AsQueryable();

            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                var filter = pagination.Filter.ToLower();
                queryable = queryable.Where(x => x.Name.ToLower().Contains(filter));
            }

            return new ActionResponse<IEnumerable<Country>>
            {
                WassSuccees = true,
                Result = await queryable.Skip(pagination.Page).Take(pagination.RecordsNumber).ToListAsync()
            };

        }

        public override async Task<ActionResponse<int>> GetTotalRecordAsync(PaginationDTO pagination)
        {
            var queryable = _context.Countries.AsQueryable();

            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                var filter = pagination.Filter.ToLower();
                queryable = queryable.Where(x => x.Name.ToLower().Contains(filter));
            }

            int count = await queryable.CountAsync();
            return new ActionResponse<int>
            {
                WassSuccees = true,
                Result = count
            };
        }

        public async Task<IEnumerable<Country>> GetComboAsync()
        {
            return await _context.Countries
                                 .OrderBy(x => x.Name)
                                 .ToListAsync();


        }
    }
}
