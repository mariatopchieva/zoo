using System;
using System.Collections.Generic;
using System.Text;
using Zoo.Data.Models;

namespace Zoo.Service.DTO
{
    public class AnimalDTO
    {
        public AnimalDTO(Animal animal)
        {
            this.Id = animal.Id;
            this.Name = animal.Name;
            this.HealthPoints = animal.HealthPoints;
            this.IsDead = animal.IsDead;
            this.SpeciesType = animal.SpeciesType;
            this.SpeciesId = animal.SpeciesId;
            this.CreatedOn = animal.CreatedOn;
            this.ModifiedOn = animal.ModifiedOn;
            this.DiedOn = animal.DiedOn;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public Species SpeciesType { get; set; }

        public int SpeciesId { get; set; }

        public int HealthPoints { get; set; }

        public bool IsDead { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime ModifiedOn { get; set; }

        public DateTime DiedOn { get; set; }
    }
}
