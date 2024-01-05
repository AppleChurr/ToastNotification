using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SQLite;
using System.IO;
using System.ComponentModel;
using System.Threading;


namespace sCommon.Database
{
    public static class cSystemDB
    {

        private static object disposeLock = new object();
        private static bool disposeFlag = false;
        public static bool isDispose
        {
            get { bool flag; lock (disposeLock) { flag = disposeFlag; } return flag; }
            set { lock (disposeLock) { disposeFlag = value; } }
        }

        private static string DBPath = "./Settings.db";

        #region Static Enum

        public static KeyValuePair<string, string> eDataPATH { get; } = new KeyValuePair<string, string>("DataPath", "Path");
        public static KeyValuePair<string, string> eAlertSheet { get; } = new KeyValuePair<string, string>("SheetIndex", "Index");
        public static KeyValuePair<string, string> eSTATE_BOARD_BAUD_RATE { get; } = new KeyValuePair<string, string>("StateBoard", "BaudRate");
        public static KeyValuePair<string, string> eSTATE_BOARD_IP { get; } = new KeyValuePair<string, string>("StateBoard", "BoardIP");
        public static KeyValuePair<string, string> eSTATE_BOARD_PORT { get; } = new KeyValuePair<string, string>("StateBoard", "BoardPort");
        public static KeyValuePair<string, string> eSTATE_BOARD_AUTO_SET { get; } = new KeyValuePair<string, string>("StateBoard", "AutoSend");

        #endregion

        public static readonly string TRUE = "TRUE";
        public static readonly string FALSE = "FALSE";




        private static BackgroundWorker _worker = null;
        private static void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // 현재 작업 디렉토리 경로 가져오기
            string currentDirectory = Directory.GetCurrentDirectory();

            while (!isDispose)
            {
                //Console.WriteLine("cSystemDB_worker_DoWork");

                if (messageQueue.Count <= 0)
                {
                    Thread.Sleep(100);
                    continue;
                }

                KeyValuePair<KeyValuePair<string, string>, string>[] copyQueue = new KeyValuePair<KeyValuePair<string, string>, string>[messageQueue.Count];

                lock (_lockQueue)
                {
                    copyQueue = (KeyValuePair<KeyValuePair<string, string>, string>[])(messageQueue.ToArray().Clone());
                    messageQueue.Clear();
                }
                
                InitializeDatabase();

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    foreach (KeyValuePair<KeyValuePair<string, string>, string> message in copyQueue)
                    {
                        using (SQLiteCommand command = new SQLiteCommand(connection))
                        {
                            command.CommandText = $"INSERT OR REPLACE INTO {message.Key.Key} (Key, Value) VALUES (@Key, @Value)";
                            command.Parameters.AddWithValue("@Key", message.Key.Value);
                            command.Parameters.AddWithValue("@Value", message.Value);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private static List<KeyValuePair<KeyValuePair<string, string>, string>> messageQueue = new List<KeyValuePair<KeyValuePair<string, string>, string>>();
        private static object _lockQueue = new object();


        private static string connectionString = $"Data Source={DBPath};Version=3;";
        private static string[] TableNames = { "MainProgram", "StateBoard", "VideoWall", "Nova", "MediaPC" };

        private static object _lockInit = new object();
        private static void InitializeDatabase()
        {
            lock (_lockInit)
            {
                if (File.Exists(DBPath))
                    return;

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    foreach (string TName in TableNames)
                    {
                        string createTableQuery = $"CREATE TABLE {TName} (Key TEXT PRIMARY KEY, Value TEXT)";

                        using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static void SaveSetting(KeyValuePair<string, string> TableKey, string value)
        {
            if (_worker == null)
            {
                _worker = new BackgroundWorker() { WorkerSupportsCancellation = true };
                _worker.DoWork += _worker_DoWork;
                _worker.RunWorkerAsync();
            }

            lock (_lockQueue)
            {
                messageQueue.Add(new KeyValuePair<KeyValuePair<string, string>, string>(TableKey, value));
            }
        }

        public static string GetSetting(KeyValuePair<string, string> TableKey)
        {
            InitializeDatabase();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    try
                    {
                        command.CommandText = $"SELECT Value FROM {TableKey.Key} WHERE Key = @Key";
                        command.Parameters.AddWithValue("@Key", TableKey.Value);

                        var result = command.ExecuteScalar();
                        return result?.ToString();
                    }
                    catch { return "faild"; }
                }
            }

        }


        public static void Dispose()
        {
            isDispose = true;

            Thread.Sleep(1000);
            _worker.CancelAsync();
            _worker.Dispose();
        }
    }
}
