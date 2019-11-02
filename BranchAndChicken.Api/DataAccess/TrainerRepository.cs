using BranchAndChicken.Api.Commands;
using BranchAndChicken.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace BranchAndChicken.Api.DataAccess
{
    public class TrainerRepository
    {
        string _connectionString = "Server=localhost;Database=BranchAndChicken;Trusted_Connection=True;";

        public List<Trainer> GetAll()
        {  
            using (var db = new SqlConnection(_connectionString))
            {
                var trainers = db.Query<Trainer>
                    (
                    @"SELECT * 
                      FROM Trainer"
                    );
                return trainers.ToList();
            } 
        }

        public Trainer Get(string name)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = @"SELECT *
                            FROM Trainer
                            WHERE Trainer.Name = @trainerName";
                var parameters = new { trainerName = name };
                var trainer = db.QueryFirst<Trainer>(sql, parameters);
                return trainer;
            }
        }

        public bool Remove(string name)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = @"DELETE
                            FROM Trainer
                            WHERE [name] = @name";
                return db.Execute(sql, new { name }) == 1;
            }
        }

       public Trainer Update(Trainer updatedTrainer, int id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = @"
                    UPDATE [Trainer]
                    SET [Name] = @name
                          ,[YearsOfExperience] = @yearsOfExperience
                          ,[Specialty] = @specialty
                    OUTPUT inserted.*
                    WHERE id = @id";

                updatedTrainer.Id = id;
                var trainer = db.QueryFirst<Trainer>(sql, updatedTrainer);
                return trainer;
            }
        }

        public Trainer Add(Trainer newTrainer)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = @"
                    INSERT INTO [Trainer]
                        ([Name]
                        ,[YearsOfExperience]
                        ,[Specialty])
	                OUTPUT insterted.*
                    VALUES
                        (@name
                        ,@yearsOfExperience
                        ,@specialty)";
                return db.QueryFirst<Trainer>(sql, newTrainer);
            }
        }
    }
}
