using FCA.Data.Chain;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FCA.Data
{
    public class DataContainer
    {
        string filePath;
        string tablename;
        string connectionString;

        public DataContainer(string filename)
        {
            this.filePath = @Application.dataPath + "/lattices/sql/" + filename;
            Request req = new Request(filePath);
            Handler createHandler = new CreationHandler();
            Handler insertHandler = new InsertionHandler();
            createHandler.setSuccesor(insertHandler);

            createHandler.handle(req);

            tablename = req.getTableName();
            connectionString = req.getConnectionString();
        }

        private string createQuery(List<string> conditions)
        {
            string baseString = $"SELECT COUNT(*) AS total FROM {tablename}";
            if ((conditions == null) || (conditions.Count() == 0))
            {
                return baseString;
            }

            string clauseStart = $" WHERE {conditions[0]} ";
            string clauseContinuation;
            if (conditions.Count() > 1)
            {
                clauseContinuation = "AND " + string.Join(" AND ", conditions.Skip(1).ToArray());
            }
            else
            {
                clauseContinuation = "";
            }

            return baseString + clauseStart + clauseContinuation;
        }

        private string createLifetrackQuery(string subjectCondition, string timeCondition, string searchedCondition)
        {
            return $"SELECT COUNT(*) as total FROM {tablename} WHERE ({subjectCondition}) AND ({timeCondition}) AND ({searchedCondition})";

        }

        public int getLifetrackCondition(string subjectCondition, string timeCondition, string searchedCondition)
        {
            string sqlQuery = createLifetrackQuery(subjectCondition, timeCondition, searchedCondition);
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(sqlQuery, connection);
                int result = int.Parse(command.ExecuteScalar().ToString());

                command.Dispose();
                connection.Close();

                return result;
            }
        }

        public int getTotal(List<string> conditions)
        {
            string sqlString = createQuery(conditions);
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(sqlString, connection);
                int result = int.Parse(command.ExecuteScalar().ToString());
                command.Dispose();
                connection.Close();
                return result;
            }
        }
    }
}


