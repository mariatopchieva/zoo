using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoo.Data.Context;
using Zoo.Data.Models;
using Zoo.Service.Contracts;
using Zoo.Service.DTO;

namespace Zoo.Service
{
    public class ZooService : IZooService
    {
        private readonly ZooDbContext context;

        public ZooService(ZooDbContext _context)
        {
            this.context = _context;
        }

        /// <summary>
        /// Creates an animal upon user's input and records it in the database.
        /// </summary>
        /// <param name="createAnimalDTO">User's input needed for recording of a new animal in the database.</param>
        /// <returns>AnimalDTO of the created animal or throws an exception 
        /// if the specified species was not found in the database and animal was not created.</returns>
        public async Task<AnimalDTO> CreateAnimalAsync(CreateAnimalDTO createAnimalDTO)
        {
            var species = await this.context.Species
                                .FirstOrDefaultAsync(x => x.Name == createAnimalDTO.Species);

            if(species == null)
            {
                throw new ArgumentNullException("There is no such species in the zoo.");
            }

            var animal = new Animal()
            {
                Name = createAnimalDTO.Name,
                SpeciesType = species,
                SpeciesId = species.Id,
                IsDead = false,
                IsDeleted = false,
                HealthPoints = 100,
                CreatedOn = DateTime.Now,
            };

            await this.context.Animals.AddAsync(animal);
            await this.context.SaveChangesAsync();

            var animalFromDb = await this.context.Animals.FirstOrDefaultAsync(x => x.Name == animal.Name);
            return new AnimalDTO(animalFromDb);
        }

        /// <summary>
        /// Creates species upon user's input and records it in the database.
        /// </summary>
        /// <param name="speciesName">User's input needed for recording of new species in the database.</param>
        /// <returns>SpeciesDTO of the created species or throws an exception 
        /// if the species has already been created.</returns>
        public async Task<SpeciesDTO> CreateSpeciesAsync(CreateSpeciesDTO createSpeciesDTO)
        {
            var species = await this.context.Species
                                .FirstOrDefaultAsync(x => x.Name == createSpeciesDTO.SpeciesName);
            
            if(species == null)
            {
                var newSpecies = new Species()
                {
                    Name = createSpeciesDTO.SpeciesName,
                    CreatedOn = DateTime.Now
                };

                await this.context.Species.AddAsync(newSpecies);
                await this.context.SaveChangesAsync();

                var speciesFromDb = await this.context.Species
                                          .FirstOrDefaultAsync(x => x.Name == newSpecies.Name);

                return new SpeciesDTO(speciesFromDb);
            }

            return new SpeciesDTO(species);
        }

        /// <summary>
        /// Sets the isDeleted property of the animal passed as parameter to true.
        /// </summary>
        /// <param name="id">The Id of the animal to delete</param>
        /// <returns>True if successful, otherwise false</returns>
        public async Task<bool> DeleteAnimalAsync(int id)
        {
            var animal = await this.context.Animals
                               .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);

            if(animal == null)
            {
                return false;
            }

            animal.IsDeleted = true;
            animal.ModifiedOn = DateTime.Now;

            await this.context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// The method reduces the health... animals with highest health points and increases their health points 
        /// </summary>
        /// <returns>Collection of all alive animals with their current Health points data.</returns>
        public async Task<IEnumerable<AnimalDTO>> AnimalsGetHungry()
        {
            var animals = await this.context.Animals.Where(x => x.IsDead == false && x.IsDeleted == false)
                                .ToListAsync();

            if (animals == null)
            {
                throw new ArgumentNullException("No alive animals in the zoo.");
            }

            Random random = new Random();

            for (int i = 0; i < animals.Count; i++)
            {
                var animal = await this.context.Animals.FirstOrDefaultAsync(x => x.Id == animals[i].Id);

                int randomNumber = random.Next(10, 26);

                if (animal.HealthPoints - randomNumber >= 0)
                {
                    animal.HealthPoints -= randomNumber;
                    bool newDeathStatus = CheckIsDead(animals[i], randomNumber).Result;

                    if (animal.IsDead == false && newDeathStatus == true)
                    {
                        animal.DiedOn = DateTime.Now;
                        animal.IsDead = newDeathStatus;
                    }

                    animal.ModifiedOn = DateTime.Now;

                    await this.context.SaveChangesAsync();
                }
            }

            return GetAliveAnimalsAsync().Result;
        }

        /// <summary>
        /// The method feeds the 90% animals with highest health points and increases their health points 
        /// by a random number in the range between 0 and 20.
        /// </summary>
        /// <returns>Collection of animals, which have been fed, with their current data.</returns>
        public async Task<IEnumerable<AnimalDTO>> FeedMostHealthyAnimals()
        {
            int aliveAnimals = await this.context.Animals
                               .Where(x => x.IsDead == false && x.IsDeleted == false)
                               .CountAsync();

            int animalsToFeed = (int)Math.Floor(aliveAnimals * 0.9);

            var animals = await this.context.Animals.Where(x => x.IsDead == false && x.IsDeleted == false)
                                .OrderByDescending(x => x.HealthPoints)
                                .Take(animalsToFeed).ToListAsync();

            if(animals == null)
            {
                throw new ArgumentNullException("No alive animals in the zoo.");
            }

            Random random = new Random();
            List<AnimalDTO> changedAnimals = new List<AnimalDTO>();

            for (int i = 0; i < animals.Count; i++)
            {
                var animal = await this.context.Animals.FirstOrDefaultAsync(x => x.Id == animals[i].Id);
                
                int randomNumber = random.Next(0, 21);

                if(animal.HealthPoints + randomNumber <= 100)
                {
                    animal.HealthPoints += randomNumber;
                    animal.ModifiedOn = DateTime.Now;
                    
                    await this.context.SaveChangesAsync();

                    var changedAnimal = await this.context.Animals.FirstOrDefaultAsync(x => x.Id == animals[i].Id);
                    changedAnimals.Add(new AnimalDTO(changedAnimal));
                }
            }

            return changedAnimals;
        }

        /// <summary>
        /// Checks whether an animal, passed as a parameter, has a isDead 
        /// </summary>
        /// <param name="animal"></param>
        /// <returns></returns>
        public async Task<bool> CheckIsDead(Animal animal, int randomNumber)
        {
            var species = await this.context.Species
                                        .FirstOrDefaultAsync(s => s.Id == animal.SpeciesId);

            string speciesName = species.Name.ToLower();

            if (speciesName == "monkey")
            {
                if (animal.HealthPoints - randomNumber < 30)
                {
                    return true;
                }
            }
            else if (speciesName == "giraffe")
            {
                if (animal.HealthPoints - randomNumber < 50)
                {
                    return true;
                }
            }
            else
            {
                if (animal.HealthPoints - randomNumber < 10)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The method collects all alive animals in the database.
        /// </summary>
        /// <returns>List of all animals, whose isDead property equals false</returns>
        public async Task<IEnumerable<AnimalDTO>> GetAliveAnimalsAsync()
        {
            var aliveAnimals = await this.context.Animals
                                    .Where(x => x.IsDead == false && x.IsDeleted == false)
                                    .Select(animal => new AnimalDTO(animal)).ToListAsync();

            return aliveAnimals;
        }

        /// <summary>
        /// The method collects all animals in the database.
        /// </summary>
        /// <returns>List of all animals, whose isDeleted property equals false</returns>
        public async Task<IEnumerable<AnimalDTO>> GetAllAnimalsAsync()
        {
            var allAnimals = await this.context.Animals
                        .Where(x => x.IsDeleted == false)
                        .Select(animal => new AnimalDTO(animal)).ToListAsync();

            return allAnimals;
        }

        /// <summary>
        /// The method collects all species in the database.
        /// </summary>
        /// <returns>List of all species.</returns>
        public async Task<IEnumerable<SpeciesDTO>> GetAllSpeciesAsync()
        {
            var allSpecies = await this.context.Species
                        .Select(species => new SpeciesDTO(species)).ToListAsync();

            return allSpecies;
        }
    }
}
