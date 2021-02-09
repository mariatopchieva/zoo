using System;
using System.Collections.Generic;
using System.Text;
using Zoo.Data.Models;

namespace Zoo.Service.DTO
{
    public class SpeciesDTO
    {
        public SpeciesDTO(Species species)
        {
            this.Id = species.Id;
            this.Name = species.Name;
            this.CreatedOn = species.CreatedOn;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
