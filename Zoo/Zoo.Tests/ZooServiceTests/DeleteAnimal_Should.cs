using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zoo.Data.Context;
using Zoo.Data.Models;
using Zoo.Service;

namespace Zoo.Tests.ZooServiceTests
{
    [TestClass]
    public class DeleteAnimal_Should
    {
        [TestMethod]
        public async Task ReturnTrue_WhenParamsAreValid()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnTrue_WhenParamsAreValid));

            Species species = new Species
            {
                Id = 1,
                Name = "Leo"
            };

            Animal firstAnimal = new Animal
            {
                Id = 25,
                Name = "Kiki",
                SpeciesId = 1,
                SpeciesType = species,
                HealthPoints = 100,
                IsDead = false,
                IsDeleted = false,
                CreatedOn = DateTime.Now
            };

            Animal secondAnimal = new Animal
            {
                Id = 26,
                Name = "Leo",
                SpeciesId = 1,
                SpeciesType = species,
                HealthPoints = 100,
                IsDead = false,
                IsDeleted = false,
                CreatedOn = DateTime.Now
            };

            using (var arrangeContext = new ZooDbContext(options))
            {
                arrangeContext.Species.Add(species);
                arrangeContext.Animals.Add(firstAnimal);
                arrangeContext.Animals.Add(secondAnimal);
                arrangeContext.SaveChanges();
            }

            using(var assertContext = new ZooDbContext(options))
            {
                //Act
                var sut = new ZooService(assertContext);

                var result = await sut.DeleteAnimalAsync(25);

                //Assert
                Assert.IsTrue(result);
            }
        }
    }
}
