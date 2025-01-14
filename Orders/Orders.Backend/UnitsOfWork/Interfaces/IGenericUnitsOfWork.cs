using Orders.Shared.Response;

namespace Orders.Backend.UnitsOfWork.Interfaces
{
    public interface IGenericUnitsOfWork<T> where T : class
    {
        Task<ActionResponse<T>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<T>>> GetAsync();

        Task<ActionResponse<T>> AddAsync(T model);

        Task<ActionResponse<T>> DeleteAsync(int id);

        Task<ActionResponse<T>> UpdateAsync(T model);
    }
}
