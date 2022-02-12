using Mono.Data.Sqlite;
using System.Linq;
using UnityEngine;

namespace FCA.Data.Chain
{
    class InsertionHandler : Handler
    {
        public override void handle(Request req)
        {
            /*
            using (SqliteConnection dbConnection = new SqliteConnection(req.getConnectionString()))
            {
                dbConnection.Open();
                if (!checkCount(dbConnection, req))
                {
                    removeEverything(dbConnection, req);
                    insertEverything(dbConnection, req);
                }
                dbConnection.Close();
            }
            */
        }

        private void insertEverything(SqliteConnection dbConnection, Request req)
        {
            foreach (string insertionString in req.getInsertionStrings())
            {
                SqliteCommand insertCommand = new SqliteCommand(insertionString, dbConnection);
                insertCommand.ExecuteNonQuery();
                insertCommand.Dispose();
            }
        }

        private void removeEverything(SqliteConnection connection, Request req)
        {
            SqliteCommand deleteCommand = new SqliteCommand($"DELETE FROM {req.getTableName()}", connection);
            deleteCommand.ExecuteNonQuery();
            deleteCommand.Dispose();
        }

        private bool checkCount(SqliteConnection connection, Request req)
        {
            SqliteCommand totalInDatabaseCommand = new SqliteCommand($"SELECT COUNT(*) AS total FROM {req.getTableName()}", connection);

            int totalInDatabase = int.Parse(totalInDatabaseCommand.ExecuteScalar().ToString());
            int totalInRequest = req.getInsertionStrings().Count() + 1;

            totalInDatabaseCommand.Dispose();

            Debug.Log($"DB : {totalInDatabase} ; Req: {totalInRequest}");

            return (totalInDatabase == totalInRequest);
        }

    }
}
