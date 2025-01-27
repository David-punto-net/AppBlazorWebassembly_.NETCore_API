using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Response;

namespace Orders.Backend.UnitsOfWork.Interfaces
{
    public interface ICategoriesUnitsOfWork
    {
        Task<ActionResponse<IEnumerable<Category>>> GetPaginationAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalRecordAsync(PaginationDTO pagination);

        Task<IEnumerable<Category>> GetComboAsync();
    }
}
