using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zoo.Data.Models;
using Zoo.Service.DTO;

namespace Zoo.Service.Contracts
{
    public interface IZooService
    {
        Task<AnimalDTO> CreateAnimalAsync(CreateAnimalDTO createAnimalDTO);

        Task<bool> CheckIsDead(Animal animal, int randomNumber);

        Task<SpeciesDTO> CreateSpeciesAsync(CreateSpeciesDTO createSpeciesDTO);

        Task<IEnumerable<AnimalDTO>> GetAllAnimalsAsync();

        Task<IEnumerable<SpeciesDTO>> GetAllSpeciesAsync();

        Task<IEnumerable<AnimalDTO>> GetAliveAnimalsAsync();

        Task<bool> DeleteAnimalAsync(int id);

        Task<IEnumerable<AnimalDTO>> FeedMostHealthyAnimals();

        Task<IEnumerable<AnimalDTO>> AnimalsGetHungry();
    }
}
