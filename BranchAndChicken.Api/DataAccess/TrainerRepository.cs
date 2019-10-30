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
                    trainers.Add(GetTrainerFromDataReader(dataReader));
                }
                return trainers;
            } 
        }

        public Trainer Get(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = 
                    $@"SELECT *
                       FROM Trainer
                       WHERE Trainer.Name = @trainerName";

                cmd.Parameters.AddWithValue("trainerName", name);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return GetTrainerFromDataReader(reader);
                }
            }
            return null;
        }

        public bool Remove(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = 
                    @"DELETE
                      FROM Trainer
                      WHERE [name] = @name";
                cmd.Parameters.AddWithValue("name", name);
                return cmd.ExecuteNonQuery() == 1;
            }
        }

       public Trainer Update(Trainer updatedTrainer, int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    UPDATE [Trainer]
                    SET [Name] = @name
                          ,[YearsOfExperience] = @yearsOfExperience
                          ,[Specialty] = @specialty
                    OUTPUT inserted.*
                    WHERE id = @id";
                cmd.Parameters.AddWithValue("name", updatedTrainer.Name);
                cmd.Parameters.AddWithValue("yearsOfExperience", updatedTrainer.YearsOfExperience);
                cmd.Parameters.AddWithValue("specialty", updatedTrainer.Speciality);
                cmd.Parameters.AddWithValue("id", id);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return GetTrainerFromDataReader(reader);
                }
                return null;
            }
        }
        public Trainer Add(Trainer newTrainer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO [Trainer]
                        ([Name]
                        ,[YearsOfExperience]
                        ,[Specialty])
	                OUTPUT insterted.*
                    VALUES
                        (@name
                        ,@yearsOfExperience
                        ,@specialty)";
                cmd.Parameters.AddWithValue("name", newTrainer.Name);
                cmd.Parameters.AddWithValue("yearsOfExperience", newTrainer.YearsOfExperience);
                cmd.Parameters.AddWithValue("specialty", newTrainer.Speciality);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return GetTrainerFromDataReader(reader);
                }
                return null;
            }
        }

        Trainer GetTrainerFromDataReader(SqlDataReader reader)
        {
            var id = (int)reader["Id"];
            var name = (string)reader["Name"];
            var yearsOfExperience = (int)reader["YearsOfExperience"];
            Enum.TryParse<Speciality>(reader["Specialty"].ToString(), out var speciality);
            var trainer = new Trainer
            {
                Speciality = speciality,
                Id = id,
                Name = name,
                YearsOfExperience = yearsOfExperience
            };
            return trainer;
        }
    }
}
