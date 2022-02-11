using FoodStuffsController.gui.MessageBoxes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace FoodStuffsController.sql
{
    class DBConn
    {

        private string hostname { get; }
        private string database { get; }
        private string username { get; }
        private string password { get; }

        public DBConn()
        {
            string jsonFile = "DBConfig.json";
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, jsonFile);
            JObject data = JObject.Parse(File.ReadAllText(jsonPath));

            this.hostname = data["hostname"].ToString();
            this.database = data["database"].ToString();
            this.username = data["username"].ToString();
            this.password = data["password"].ToString();


        }

        private string makeConnectionString()
        {
            //jdbc:mysql://selene.hud.ac.uk:3306/u1761249

            return $"Data Source = {hostname}; Initial Catalog = {database}; User Id = {username}; Password = {password}";
        }

        public DataTable queryDatabase(string query)
        {
            using (SqlConnection connection = new SqlConnection(makeConnectionString()))
            {
                try
                {
                    // Connection is closed automatically outside the using statement.
                    connection.Open();
                }
                catch (Exception err) { PopupBoxes.ShowError("SQL Error", "There was an error connecting to the databsae."); }
            }
            return null;
        }
    }
}
