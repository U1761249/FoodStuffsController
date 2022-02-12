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

        private string hostname { get; }
        private string port { get; }
        private string database { get; }
        private string username { get; }
        private string password { get; }

        public DBManager()
        {
            List<string> data = SQLConnectionSettings.getConnection();

            this.hostname = data[0];
            this.port =     data[1];
            this.database = data[2];
            this.username = data[3];
            this.password = data[4];
        }

       

        public DataTable queryDatabase(string query)
        {
            try
            {
                string connectionString = $"server = {hostname}; port = {port}; database = {database}; user = {username}; password = {password};";

                var conn = new MySqlConnection(connectionString);            
                
                conn.Open();
                PopupBoxes.ShowError("SQL Error", "Connection Opened");
                conn.Close();
                PopupBoxes.ShowError("SQL Error", "Connection Closed");
            }
            catch (Exception err) { PopupBoxes.ShowError("SQL Error", "There was an error connecting to the databsae."); }
            return null;
}
        }
    }

