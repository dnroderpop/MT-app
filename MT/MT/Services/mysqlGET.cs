using Acr.UserDialogs;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MT.Services
{
    internal class mysqlGET
    {
        private static MySqlCommand MySqlCommand;
        private static MySqlConnection MySqlConnection;
        private static MySqlConnectionStringBuilder builder;
        private string Server, Port, Username, Password, Database;

        public mysqlGET()
        {
            MySqlCommand = new MySqlCommand();
            MySqlConnection = new MySqlConnection();

            Server =   Preferences.Get("server", "122.54.146.208");
            Port =     Preferences.Get("port", "3306");
            Username = Preferences.Get("userid", "rodericks");
            Password = Preferences.Get("password", "mtchoco");
            Database = Preferences.Get("database", "mangtinapay");

            builder = new MySqlConnectionStringBuilder
            {
                Server = Server,
                Port = uint.Parse(Port),
                UserID = Username,
                Database = Database,
                Password = Password,
                ConnectionTimeout = 30
            };

            MySqlConnection.ConnectionString = builder.ConnectionString;
        }

        #region QuerySingleString
        public async Task<string> querySingleStringFromDatabase(string datatablename, string targetcolumn, string whereclause, string parameter)
        {
            UserDialogs.Instance.ShowLoading("Loading...", maskType: MaskType.Black);
            string result = null;

            await Task.Run(() =>
            {
                try
                {
                    //Try Simple Connection
                    MySqlConnection.Open();

                    MySqlCommand = MySqlConnection.CreateCommand();

                    MySqlCommand.CommandText = @"SELECT " + targetcolumn + " FROM " + datatablename + " WHERE " + whereclause + " = @OrderId;";
                    MySqlCommand.Parameters.AddWithValue("@OrderId", parameter);

                    // execute the command and read the results
                    var reader = MySqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        result = reader.GetValue(targetcolumn).ToString();
                    }
                    MySqlConnection.Close();

                }
                catch (Exception ex)
                {
                    MySqlConnection.Close();
                    UserDialogs.Instance.Alert(ex.Message, "Error", "Okay");

                }

            });

            UserDialogs.Instance.HideLoading();
            return result;
        }

        public async Task<string> querySingleStringFromDatabase(string datatablename, string targetcolumn, string whereclause, string whereclause2, string parameter, string parameter2)
        {
            UserDialogs.Instance.ShowLoading("Loading...", maskType: MaskType.Black);
            string result = null;

            await Task.Run(() =>
            {
                try
                {
                    //Try Simple Connection
                    MySqlConnection.Open();

                    MySqlCommand = MySqlConnection.CreateCommand();

                    MySqlCommand.CommandText = @"SELECT " + targetcolumn + " FROM " + datatablename + " WHERE " + whereclause + " = @param1 and " + whereclause2 + " = @param2;";
                    MySqlCommand.Parameters.AddWithValue("@param1", parameter);
                    MySqlCommand.Parameters.AddWithValue("@param2", parameter2);

                    // execute the command and read the results
                    var reader = MySqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        result = reader.GetValue(targetcolumn).ToString();
                    }
                    MySqlConnection.Close();

                }
                catch (Exception ex)
                {
                    MySqlConnection.Close();
                    UserDialogs.Instance.Alert(ex.Message, "Error", "Okay");

                }

            });

            UserDialogs.Instance.HideLoading();
            return result;
        }
        #endregion


    }
}
