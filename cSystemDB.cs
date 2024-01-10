using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SQLite;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Windows.UI.Xaml.Shapes;


namespace sCommon.Database
{
    public static class cSystemDB
    {
        private static string DBPath = "./Settings.db";

        private static string currentDirectory = Directory.GetCurrentDirectory();
        private static string connectionString = $"Data Source={DBPath};Version=3;";
        private static string[] TableNames = { "Config", "Notify" };
        public static void InitializeDatabase()
        {
            if (File.Exists(DBPath))
                return;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                foreach (string TName in TableNames)
                {
                    string createTableQuery = "";

                    switch (TName)
                    {
                        case "Config":
                            createTableQuery = $"CREATE TABLE {TName} (Key TEXT PRIMARY KEY, Value TEXT)";
                            break;

                        case "Notify":
                            createTableQuery = $"CREATE TABLE {TName} (SheetName TEXT PRIMARY KEY, CheckDate TEXT, DisplayData TEXT)";
                            break;
                    }

                    using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void SaveDataPath(string path)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    // 이미 키가 존재하는 경우 업데이트, 없는 경우 삽입
                    command.CommandText = $"INSERT OR REPLACE INTO {TableNames[0]} (Key, Value) VALUES (@Key, @Value)";
                    command.Parameters.AddWithValue("@Key", "DataPath");
                    command.Parameters.AddWithValue("@Value", path);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void SaveAlert(string SheetName, string Date, string DispInfo)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    // 이미 키가 존재하는 경우 업데이트, 없는 경우 삽입
                    command.CommandText = $"INSERT OR REPLACE INTO {TableNames[1]} (SheetName, CheckDate, DisplayData) VALUES (@SheetName, @CheckDate, @DisplayData)";
                    command.Parameters.AddWithValue("@SheetName", SheetName);
                    command.Parameters.AddWithValue("@CheckDate", Date);
                    command.Parameters.AddWithValue("@DisplayData", DispInfo);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void RemoveAlert(string SheetName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    // 이미 키가 존재하는 경우 업데이트, 없는 경우 삽입
                    command.CommandText = $"DELETE FROM {TableNames[1]} WHERE SheetName = @SheetName";
                    command.Parameters.AddWithValue("@SheetName", SheetName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static string ReadDataPath()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"SELECT Value FROM {TableNames[0]} WHERE Key = @Key";
                    command.Parameters.AddWithValue("@Key", "DataPath");

                    var result = command.ExecuteScalar();
                    return result as string;
                }
            }
        }

        public static Tuple<string, string, string> ReadAlert(string sheetName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"SELECT CheckDate, DisplayData FROM {TableNames[1]} WHERE SheetName = @SheetName";
                    command.Parameters.AddWithValue("@SheetName", sheetName);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string checkDate = reader["CheckDate"].ToString();
                            string displayData = reader["DisplayData"].ToString();
                            return new Tuple<string, string, string>(sheetName, checkDate, displayData);
                        }
                        else
                        {
                            return null; // Sheet not found
                        }
                    }
                }
            }
        }


    }
}