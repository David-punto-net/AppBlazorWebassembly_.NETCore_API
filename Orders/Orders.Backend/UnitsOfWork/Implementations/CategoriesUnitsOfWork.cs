using Orders.Backend.Repositories.Interfaces;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Response;

namespace Orders.Backend.UnitsOfWork.Implementations
{
    public class CategoriesUnitsOfWork : GenericUnitsOfWork<Category>, ICategoriesUnitsOfWork
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoriesUnitsOfWork(IGenericRepository<Category> repository, ICategoriesRepository categoriesRepository) : base(repository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public override async Task<ActionResponse<IEnumerable<Category>>> GetPaginationAsync(PaginationDTO pagination) => await _categoriesRepository.GetPaginationAsync(pagination);

        public override async Task<ActionResponse<int>> GetTotalRecordAsync(PaginationDTO pagination) => await _categoriesRepository.GetTotalRecordAsync(pagination);
    }
}