using PROG260_Week6;
using System.Data;
using System.Data.SqlClient;

namespace PROG260_Week7
{
    /// <summary>
    /// 
    /// I used the inclass examples availiable from the powerpoint as base to start accessing the database
    /// I improved upon them by dynamically reading the header of the txt file to generate the SQL column heads
    /// 
    /// I used ChatGPT to generate the comments to this code
    /// 
    /// 
    /// </summary>
    internal class Program
    {
        // Stores the database connection string
        public static string ConnectionString { get; set; }

        static void Main(string[] args)
        {
            // Initialize and configure a SQL Server connection string
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder["Server"] = @"(localdb)\MSSQLLocalDB";
            sqlBuilder["Trusted_Connection"] = true;
            sqlBuilder["Integrated Security"] = "SSPI";
            sqlBuilder["Initial Catalog"] = "PROG260FA23";
            ConnectionString = sqlBuilder.ToString();

            // Initialize input file
            FileInfo FI_Characters = new FileInfo("Chars.csv");
            InputFile IF_Characters = new InputFile(FI_Characters.FullName, FI_Characters.Name, FI_Characters.Extension);

            // Clear data from database tables
            DELETEfrom("Chars_Sword", "");
            DELETEfrom("Chars_Magic", "");
            DELETEfrom("Chars", "");

            Console.WriteLine("Read in file, create InputFile object");

            // Write data from the input file to the database
            WriteToDataBase(IF_Characters);
            Console.WriteLine("Write InputFile to Database");

            // Execute SQL queries to populate tables
            SQLQuery("INSERT INTO Chars_Sword (S_Char, Type, Map_Location, Original_Character) " +
                "SELECT Character, Type, Map_Location, Original_Character " +
                "FROM Chars " +
                "WHERE Sword_Fighter = 1;");

            SQLQuery("INSERT INTO Chars_Magic (M_Char, Type, Map_Location, Original_Character) " +
                "SELECT Character, Type, Map_Location, Original_Character " +
                "FROM Chars " +
                "WHERE Magic_User = 1;");

            // Execute SQL queries to select and print data
            SELECTAndPrint(
                "Chars.Character, Chars_Sword.Type, Chars_Sword.Map_Location, Chars_Sword.Original_Character, Chars.Sword_Fighter, Chars.Magic_User",
                "FROM Chars INNER JOIN Chars_Sword ON Chars.Character = Chars_Sword.S_Char",
                "Chars_Sword_INJOIN.txt"
            );

            SELECTAndPrint(
                "Chars.Character, Chars_Magic.Type, Chars_Magic.Map_Location, Chars_Magic.Original_Character, Chars.Sword_Fighter, Chars.Magic_User",
                "FROM Chars INNER JOIN Chars_Magic ON Chars.Character = Chars_Magic.M_Char",
                "Chars_Magic_INJOIN.txt"
            );

            SELECTAndPrint("*", "FROM Chars WHERE Map_Location is NULL", "Lost.txt");

            SELECTAndPrint("*", "FROM Chars_Sword WHERE Type <> 'Human'", "SwordNonHuman.txt");
        }

        // Method to write data from a file to the database
        public static void WriteToDataBase(IFile file)
        {
            using (StreamReader reader = new StreamReader(file.Path))
            {
                // Read the first line to get column names
                var table_cols = reader.ReadLine().Split(",");

                string inlineSQL_cols = ColumnSQl(table_cols);

                // Read and process data lines
                var line = reader.ReadLine();
                while (line != null)
                {
                    var fields = line.Split(",");

                    string inlineSQL_values = ValueSQL(fields);

                    // Generate the SQL INSERT statement and execute it
                    string inlineSQL = $@"INSERT [dbo].[{file.Name}] ({inlineSQL_cols}) VALUES ({inlineSQL_values})";

                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(inlineSQL, conn))
                        {
                            var query = cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }

                    line = reader.ReadLine();
                }
                reader.Close();
                reader.Dispose();
            }
        }

        // Method to generate SQL column names
        public static string ColumnSQl(string[] cols)
        {
            string inlineSQL_cols = "";
            foreach (var col in cols)
            {
                if (col == cols.Last())
                {
                    inlineSQL_cols += $"[{col}]";
                }
                else
                {
                    inlineSQL_cols += $"[{col}], ";
                }
            }
            return inlineSQL_cols;
        }

        // Method to generate SQL values
        public static string ValueSQL(string[] fields)
        {
            string inlineSQL_values = "";
            int index = 0;
            foreach (var field in fields)
            {
                if (index == fields.Count() - 1)
                {
                    if (field.Contains("NULL"))
                    {
                        inlineSQL_values += $"{field}";
                    }
                    else
                    {
                        inlineSQL_values += $"'{field}'";
                    }
                }
                else
                {
                    if (field.Contains("NULL"))
                    {
                        inlineSQL_values += $"{field}, ";
                    }
                    else
                    {
                        inlineSQL_values += $"'{field}', ";
                    }
                }
                index++;
            }
            return inlineSQL_values;
        }

        // Method to filter and delete data from the database
        public static void DELETEfrom(string database, string filterSQl)
        {
            string inlineSQL = $@"DELETE FROM {database} {filterSQl}";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(inlineSQL, conn))
                {
                    var query = cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        // Method to execute a SQL query
        public static void SQLQuery(string query)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    var sql = comm.ExecuteScalar();
                }
            }
        }

        // Method to execute a SQL SELECT query and print the results
        public static void SELECTAndPrint(string cols, string filterSQL, string outfile)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT {cols} {filterSQL}";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        using (StreamWriter writer = new StreamWriter(outfile))
                        {
                            if (reader.HasRows)
                            {
                                int fieldCount = reader.FieldCount;

                                // Write column names to the file
                                for (int i = 0; i < fieldCount; i++)
                                {
                                    writer.Write(reader.GetName(i));
                                    if (i < fieldCount - 1)
                                    {
                                        writer.Write("\t");
                                    }
                                }
                                writer.WriteLine();

                                while (reader.Read())
                                {
                                    for (int i = 0; i < fieldCount; i++)
                                    {
                                        writer.Write(reader[i]);
                                        if (i < fieldCount - 1)
                                        {
                                            writer.Write("\t");
                                        }
                                    }
                                    writer.WriteLine();
                                }
                            }
                            else
                            {
                                writer.WriteLine("No rows returned by the query.");
                            }
                        }
                    }
                }

                connection.Close();
            }
        }

    }

}