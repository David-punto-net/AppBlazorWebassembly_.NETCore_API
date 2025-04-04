﻿using Orders.Backend.Repositories.Implementations;
using Orders.Backend.Repositories.Interfaces;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Response;

namespace Orders.Backend.UnitsOfWork.Implementations
{
    public class CitiesUnitsOfWork : GenericUnitsOfWork<City>, ICitiesUnitsOfWork
    {
        private readonly ICitiesRepository _citiesRepository;

        public CitiesUnitsOfWork(IGenericRepository<City> repository, ICitiesRepository citiesRepository) : base(repository)
        {
            _citiesRepository = citiesRepository;
        }

        public override async Task<ActionResponse<IEnumerable<City>>> GetAsync(PaginationDTO pagination) => await _citiesRepository.GetAsync(pagination);

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _citiesRepository.GetTotalPagesAsync(pagination);

        public override async Task<ActionResponse<IEnumerable<City>>> GetPaginationAsync(PaginationDTO pagination) => await _citiesRepository.GetPaginationAsync(pagination);

        public override async Task<ActionResponse<int>> GetTotalRecordAsync(PaginationDTO pagination) => await _citiesRepository.GetTotalRecordAsync(pagination);

        public async Task<IEnumerable<City>> GetComboAsync(int stateId) => await _citiesRepository.GetComboAsync(stateId);
    }
}