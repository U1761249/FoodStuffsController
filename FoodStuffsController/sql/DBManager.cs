using FoodStuffsController.gui.MessageBoxes;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FoodStuffsController.sql
{
    class DBManager
    {
        private MySqlConnection conn;
        private MySqlCommand cmd;

        private string hostname { get; }
        private string port { get; }
        private string database { get; }
        private string username { get; }
        private string password { get; }

        public DBManager()
        {
            List<string> data = SQLConnectionSettings.getConnection();

            this.hostname = data[0];
            this.port = data[1];
            this.database = data[2];
            this.username = data[3];
            this.password = data[4];

            string connectionString = $"server = {hostname}; port = {port}; database = {database}; user = {username}; password = {password};";

            conn = new MySqlConnection(connectionString);
        }


        /// <summary>
        /// Run a SELECT query on the database.
        /// Return the results in a DataTable.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable queryDatabase(string query)
        {
            DataTable result = new DataTable();
            try
            {
                conn.Open();

                cmd = new MySqlCommand(query, conn);

                // create data adapter
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(result);

                da.Dispose();

                conn.Close();
            }
            catch (Exception err) { PopupBoxes.ShowError("SQL Error", "There was an error connecting to the databsae."); }
            return result;
        }

        /// <summary>
        /// Update a value within the database.
        /// Return a bool value indicating update success.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool updateDatabase(string query)
        {
            bool updateSuccess = false;
            try
            {
                conn.Open();

                cmd = new MySqlCommand(query, conn);

                int result = cmd.ExecuteNonQuery();
                updateSuccess = result > 0;

                conn.Close();
            }
            catch (Exception err) { PopupBoxes.ShowError("SQL Error", "There was an error connecting to the databsae."); }
            return updateSuccess;
        }

        /// <summary>
        /// Query the database to see if any data is returned.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool databaseContains(string query) 
        {
            bool updateSuccess = false;
            try
            {
                conn.Open();

                cmd = new MySqlCommand(query, conn);

                int result = (int)cmd.ExecuteScalar();

                updateSuccess = result > 0;

                conn.Close();
            }
            catch (Exception err) { PopupBoxes.ShowError("SQL Error", "There was an error connecting to the databsae."); }
            return updateSuccess;
        }
    }
}

