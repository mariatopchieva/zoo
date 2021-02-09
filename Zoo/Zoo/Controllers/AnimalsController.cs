using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zoo.Service.Contracts;
using Zoo.Service.DTO;

namespace Zoo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly IZooService zooService;

        public AnimalsController(IZooService _zooService)
        {
            this.zooService = _zooService;
        }

        //GET /animals
        [HttpGet]
        public IActionResult GetAllAnimals()
        {
            var animals = this.zooService.GetAllAnimalsAsync().Result;

            if(animals.ToList().Count == 0)
            {
                return NotFound();
            }

            return Ok(animals);
        }

        //GET animals/alive
        [HttpGet]
        [Route("alive")]
        public IActionResult GetAliveAnimals()
        {
            var animals = this.zooService.GetAliveAnimalsAsync().Result;

            if (animals.ToList().Count == 0)
            {
                return NotFound();
            }

            return Ok(animals);
        }

        //GET animals/feed
        [HttpGet]
        [Route("feed")]
        public IActionResult GetAnimalsAfterFeed()
        {
            var animals = this.zooService.FeedMostHealthyAnimals().Result;

            if (animals.ToList().Count == 0)
            {
                return NotFound();
            }

            return Ok(animals);
        }

        //GET animals/get-hungry
        [HttpGet]
        [Route("get-hungry")]
        public IActionResult GetAnimalsHungry()
        {
            var animals = this.zooService.AnimalsGetHungry().Result;

            if (animals.ToList().Count == 0)
            {
                return NotFound();
            }

            return Ok(animals);
        }

        //POST /animals
        [HttpPost]
        public IActionResult CreateAnimal([FromBody] CreateAnimalDTO animalDTO)
        {
            if(animalDTO == null)
            {
                return BadRequest();
            }

            var animalNewDTO = new CreateAnimalDTO
            {
                Name = animalDTO.Name,
                Species = animalDTO.Species
            };

            var animal = this.zooService.CreateAnimalAsync(animalNewDTO).Result;

            return Created("post", animal);
        }

        //DELETE animals/id
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteAnimal(int id)
        {
            var result = this.zooService.DeleteAnimalAsync(id).Result;

            if(result == true)
            {
                return Ok();
            }

            return NotFound();
        }

        //POST /species
        [HttpPost("/species")]
        public IActionResult CreateSpecies([FromBody] CreateSpeciesDTO createSpeciesDTO)
        {
            if (createSpeciesDTO == null)
            {
                return BadRequest();
            }

            var species = this.zooService.CreateSpeciesAsync(createSpeciesDTO).Result;

            return Created("post", species);
        }

        //GET /species
        [HttpGet("/species")]
        public IActionResult GetAllSpecies()
        {
            var species = this.zooService.GetAllSpeciesAsync().Result;

            if (species.ToList().Count == 0)
            {
                return NotFound();
            }

            return Ok(species);
        }
    }
}
