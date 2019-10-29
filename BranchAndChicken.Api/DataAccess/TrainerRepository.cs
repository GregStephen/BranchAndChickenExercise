using BranchAndChicken.Api.Commands;
using BranchAndChicken.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BranchAndChicken.Api.DataAccess
{
    public class TrainerRepository
    {
    /*    static List<Trainer> _trainers = new List<Trainer>
          {
              new Trainer
              {
                  Id = 0,
                  Name = "Greg",
                  Speciality = Speciality.TaeCluckDo,
                  YearsOfExperience = 3
              },
              new Trainer
              {
                  Id = 1,
                  Name = "Martin",
                  Speciality = Speciality.Chousting,
                  YearsOfExperience = 1
              },
              new Trainer
              {
                  Id = 2,
                  Name = "Breeen",
                  Speciality = Speciality.Chudo,
                  YearsOfExperience = 3
              }
          };*/

        string _connectionString = "Server=localhost;Database=BranchAndChicken;Trusted_Connection=True;";

        public List<Trainer> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"SELECT * 
                                FROM Trainer";

                var dataReader = cmd.ExecuteReader();

                var trainers = new List<Trainer>();
                while (dataReader.Read())
                {
                    var id = (int)dataReader["Id"];
                    var name = (string)dataReader["Name"];
                    var yearsOfExperience = (int)dataReader["YearsOfExperience"];
                    Enum.TryParse<Speciality>((string)dataReader["Specialty"], out var speciality);

                    var trainer = new Trainer
                    {
                        Speciality = speciality,
                        Id = id,
                        Name = name,
                        YearsOfExperience = yearsOfExperience
                    };
                    trainers.Add(trainer);
                }

                dataReader.Dispose();
                cmd.Dispose();
                connection.Close();
            }

            return trainers;
        }

        public Trainer Get(string name)
        {
            var trainer = _trainers.FirstOrDefault(t => t.Name == name);
            return trainer;
        }

        public void Remove(string name)
        {
            _trainers.Remove(Get(name));
        }

        public Trainer Update(Trainer updatedTrainer, int id)
        {
            var trainerToUpdate = _trainers.First(trainer => trainer.Id == id);
            trainerToUpdate.Name = updatedTrainer.Name;
            trainerToUpdate.Speciality = updatedTrainer.Speciality;
            trainerToUpdate.YearsOfExperience = updatedTrainer.YearsOfExperience;
            return trainerToUpdate;
        }
        public Trainer Add(Trainer newTrainer)
        {
            _trainers.Add(newTrainer);
            return newTrainer;
        }
    }
}
