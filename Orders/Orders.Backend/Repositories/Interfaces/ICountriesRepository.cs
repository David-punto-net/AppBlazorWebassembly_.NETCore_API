﻿using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Response;

namespace Orders.Backend.Repositories.Interfaces
{
    public interface ICountriesRepository
    {
        Task<ActionResponse<Country>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<Country>>> GetAsync();

        Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

        Task<ActionResponse<IEnumerable<Country>>> GetPaginationAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalRecordAsync(PaginationDTO pagination);

        Task<IEnumerable<Country>> GetComboAsync();
    }
}