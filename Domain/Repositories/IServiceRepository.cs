﻿using Domain.Entities;

namespace Domain.Repositories;
public interface IServiceRepository
{
    Task<IEnumerable<Service>> GetAllServicesAsync();
    Task<Service?> GetServiceByIdAsync(int id);
    Task<IEnumerable<Service>> GetServicesByNameAsync(string name);
    Task<IEnumerable<Service>> GetServicesByPriceAsync(decimal price, decimal priceMax);
    Task<Service> AddServiceAsync(Service service);
    Task UpdateServiceAsync(int id, Service service);
    Task DeleteServiceAsync(int id);

}