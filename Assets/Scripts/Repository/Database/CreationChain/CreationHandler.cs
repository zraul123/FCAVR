using Mono.Data.Sqlite;
using System.IO;
using UnityEngine;


namespace FCA.Data.Chain
{
    public class CreationHandler : Handler
    {

        public override void handle(Request req)
        {
            //string dbPath = "URI=file:" + Application.persistentDataPath + "/temp.db";
            assureDbLocation();
            req.setConnectionString("Data Source=temp.db;Version=3;");
            using (SqliteConnection dbConnection = new SqliteConnection(req.getConnectionString()))
            {
                dbConnection.Open();
                createTable(dbConnection, req.getCreationString());
                putTableName(req);
                dbConnection.Close();
            }


            successor.handle(req);
        }

        private void assureDbLocation()
        {
            if (!File.Exists("temp.db"))
                SqliteConnection.CreateFile("temp.db");
        }

        private void createTable(SqliteConnection dbConnection, string creationString)
        {
            string existsCreationString = creationString.Replace("CREATE TABLE", "CREATE TABLE IF NOT EXISTS");

            SqliteCommand sqliteCommand = new SqliteCommand(existsCreationString, dbConnection);
            sqliteCommand.ExecuteNonQuery();
            sqliteCommand.Dispose();
        }

        private void putTableName(Request req)
        {
            string[] args = req.getCreationString().Split(' ');

            Debug.Log(args[2]);
            req.setTableName(args[2]);
        }
    }
}
