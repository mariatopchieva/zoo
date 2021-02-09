using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zoo.Data.Abstract;

namespace Zoo.Data.Models
{
    public class Animal : Entity
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Species Id")]
        public int SpeciesId { get; set; }

        [DisplayName("Species")]
        public Species SpeciesType { get; set; }

        [DisplayName("Health points")]
        public int HealthPoints { get; set; } = 100;

        [DisplayName("Death status")]
        public bool IsDead { get; set; } = false;

        [DisplayName("Is deleted")]
        public bool IsDeleted { get; set; } = false;

        [DisplayName("Modified on")]
        public DateTime ModifiedOn { get; set; }

        [DisplayName("Died on")]
        public DateTime DiedOn { get; set; }
    }
}
