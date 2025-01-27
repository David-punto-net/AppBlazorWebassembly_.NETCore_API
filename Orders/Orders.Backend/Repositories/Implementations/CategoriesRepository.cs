using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Response;
using System.Linq;

namespace Orders.Backend.Repositories.Implementations
{
    public class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
    {
        private readonly DataContext _context;

        public CategoriesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<IEnumerable<Category>>> GetPaginationAsync(PaginationDTO pagination)
        {
            var queryable = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                var filter = pagination.Filter.ToLower();
                queryable = queryable.Where(x => x.Name.ToLower().Contains(filter));
            }

            return new ActionResponse<IEnumerable<Category>>
            {
                WassSuccees = true,
                Result = await queryable.Skip(pagination.Page).Take(pagination.RecordsNumber).ToListAsync()
            };

        }

        public override async Task<ActionResponse<int>> GetTotalRecordAsync(PaginationDTO pagination)
        {
            var queryable = _context.Categories.AsQueryable();

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

        public async Task<IEnumerable<Category>> GetComboAsync()
        {
            return await _context.Categories
                                 .OrderBy(x => x.Name)
                                 .ToListAsync();

        }
    }
}
