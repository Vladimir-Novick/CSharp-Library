using SGcomboLib.Npgsql.Config;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

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
    public class SGcomboTable<T>
    {


    	 public List<T> LoadFromTable(String tableName) {
		 
	 		 String sql = $"select * from public.{tableName} ";
	  		return LoadFromSelect(sql);
			
		 }


        public List<T> LoadFromSelect(String sql)
        {

            bool debug = SGcomboConfigProvider.ConfigSettings.Debug;



            if (debug)
            {
                Console.WriteLine($"Start: {sql}");
            }

                List<T> table = new List<T>();

           


            using (NpgsqlConnection connection = SGcomboDbHelper.OpenConnection())
            {

                DataSet dset = new DataSet("dataTable");
                using (NpgsqlDataAdapter NpAdapter = new NpgsqlDataAdapter())
                {
                    NpAdapter.SelectCommand = new NpgsqlCommand(sql, connection);
                    NpAdapter.Fill(dset, "dataTable");

                    if (debug)
                    {
                        Console.WriteLine($"Read all data from table: {sql}");
                    }


                    DataTable dtsource = dset.Tables["dataTable"];

                    List<String> columnNames = new List<string>();

                    foreach (DataColumn column in dtsource.Columns)
                    {
                        String columnName = column.ColumnName;
                        columnNames.Add(columnName);

                    }

                    int RowCount = 0;

                    foreach (DataRow row in dtsource.Rows)
                    {
                        Object dst = (Object)Activator.CreateInstance(typeof(T));
                        foreach (String column in columnNames)
                        {
                            try
                            {
                                var safeValue = row[column];

                                var p = dst.GetType().GetProperty(column);
                                if (p != null)
                                {
                                    p.SetValue(dst, safeValue);
                                } else
                                {
#if (DEBUG)
                                    Console.WriteLine($"field doesn't exist. SQL: {sql} , Field: {column}");
#endif

                                }
                            }
                            catch (Exception ex)
                            {
                                if (RowCount == 0)
                                {
                                    if (debug)
                                    {
                                        Console.WriteLine($"Error: {ex.Message} sql: {sql} , Field: {column}" );
                                    }
                                }
                            }

                        }
                        T insert = (T)dst;
                        table.Add(insert);
                        RowCount++;


                        if (debug && (RowCount > 0))
                        {
                            Console.WriteLine("Exit by debug option");
                            break;
                        }
                    }
                }

            }

            return table;
        }
    }
}
