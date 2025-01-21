using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Response;

namespace Orders.Backend.Repositories.Implementations
{
    public class StatesRepository : GenericRepository<State>, IStatesRepository
    {
        private readonly DataContext _context;

        public StatesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<State>> GetAsync(int id)
        {
            var country = await _context.States
                                .Include(c => c.Cities)
                                .FirstOrDefaultAsync(c => c.Id == id);
            if (country is null)
            {
                return new ActionResponse<State>
                {
                    WassSuccees = false,
                    Message = "Estado no existe."
                };
            }

            return new ActionResponse<State>
            {
                WassSuccees = true,
                Result = country
            };
        }

        public override async Task<ActionResponse<IEnumerable<State>>> GetAsync()
        {
            var countries = await _context.States
                                  .OrderBy(c => c.Name)
                                  .Include(c => c.Cities)
                                  .ToListAsync();
            return new ActionResponse<IEnumerable<State>>()
            {
                WassSuccees = true,
                Result = countries
            };
        }

        public override async Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.States
            .Include(x => x.Cities)
            .Where(x => x.Country!.Id == pagination.Id)
            .AsQueryable();
            return new ActionResponse<IEnumerable<State>>
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
            var queryable = _context.States
            .Where(x => x.Country!.Id == pagination.Id)
            .AsQueryable();

            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);

            return new ActionResponse<int>
            {
                WassSuccees = true,
                Result = totalPages
            };
        }
    }
}