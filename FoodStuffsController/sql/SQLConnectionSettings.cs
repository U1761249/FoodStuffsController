using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStuffsController.sql
{
    /// <summary>
    /// This static class is in place of a propper implementation, but will surfice for the scope of this project.
    /// </summary>
    static class SQLConnectionSettings
    {


        private const string hostname = "selene.hud.ac.uk";
        private const string port = "3306";
        private const string database = "u1761249";
        private const string username = "u1761249";
        private const string password = "AB07oct21ab";


    public static List<string> GetConnection() 
        {

            return new List<string>() { hostname, port, database, username, password };

        }

    }
}
