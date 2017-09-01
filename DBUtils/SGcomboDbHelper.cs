using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using NpgsqlTypes;

using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;
using SGcomboLib.Npgsql.Config;

////////////////////////////////////////////////////////////////////////////
//	Copyright 2017 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////


namespace SGcomboLib.Npgsql.DBUtils
{
    public class SGcomboDbHelper
    {
        public static  NpgsqlConnection OpenConnection()
        {
            NpgsqlConnection MyNpgsqlConnection = new NpgsqlConnection();
            string StrConnect = SGcomboConfigProvider.GetConnectionString();

            MyNpgsqlConnection.ConnectionString = StrConnect;
            MyNpgsqlConnection.Open();
            return MyNpgsqlConnection;
        }

        

        private static  NpgsqlCommand CreateSPCommand(string SP_Name, NpgsqlConnection MyNpgsqlConnection)
        {
            NpgsqlCommand MyNpgsqlCommand = MyNpgsqlConnection.CreateCommand();
            MyNpgsqlCommand.CommandType = CommandType.StoredProcedure;

            MyNpgsqlCommand.CommandText = SP_Name;
            return MyNpgsqlCommand;
        }



    }


}
