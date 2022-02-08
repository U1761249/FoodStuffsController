using System;
using System.Collections.Generic;
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

        DBConn()
        {
            string jsonFile = "DBConfig.json";

        }
    }
}
