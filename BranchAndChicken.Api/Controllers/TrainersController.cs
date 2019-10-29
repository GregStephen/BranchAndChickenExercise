using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BranchAndChicken.Api.Commands;
using BranchAndChicken.Api.DataAccess;
using BranchAndChicken.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BranchAndChicken.Api.Controllers
{
    [Route("api/trainers")]
    [ApiController]
    public class TrainersController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Trainer>> GetAllTrainers()
        {
            var repo = new TrainerRepository();
            return repo.GetAll();
        }

 /*       [HttpGet("{name}")]
        public ActionResult<Trainer> GetByName(string name)
        {
            var repo = new TrainerRepository();
            return repo.Get(name);
        }*/

        [HttpDelete("{name}")]
        public IActionResult DeleteTrainer(string name)
        {
            var repo = new TrainerRepository();
            repo.Remove(name);

            return Ok();
        }
/*        [HttpPut("{id}")]
        public IActionResult UpdateTrainer(UpdateTrainerCommand updatedTrainerCommand, int id)
        {
            var repo = new TrainerRepository();

            var updatedTrainer = new Trainer()
            {
                Name = updatedTrainerCommand.Name,
                YearsOfExperience = updatedTrainerCommand.YearsOfExperience,
                Speciality = updatedTrainerCommand.Speciality
            };

           var trainerThatGotUpdated = repo.Update(updatedTrainer, id);
            return Ok(trainerThatGotUpdated);
        }*/

        [HttpPost]
        public IActionResult CreateTrainer(AddTrainerCommand newTrainerCommand)
        {
            var newTrainer = new Trainer()
            {
                Name = newTrainerCommand.Name,
                YearsOfExperience = newTrainerCommand.YearsOfExperience,
                Speciality = newTrainerCommand.Speciality
            };
            var repo = new TrainerRepository();
            var trainerThatGotCreated = repo.Add(newTrainer);
            return Created($"api/trainers/{trainerThatGotCreated.Name}", trainerThatGotCreated);

        }
    }
}