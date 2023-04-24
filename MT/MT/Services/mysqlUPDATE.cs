using Acr.UserDialogs;
using MySqlConnector;
using System;
using Xamarin.Essentials;

namespace MT.Services
{
    internal class mysqlUPDATE
    {
        private static MySqlCommand MySqlCommand;
        private static MySqlConnection MySqlConnection;
        private static MySqlConnectionStringBuilder builder;
        private string Server, Port, Username, Password, Database;

        public mysqlUPDATE()
        {
            MySqlCommand = new MySqlCommand();
            MySqlConnection = new MySqlConnection();
            refreshQueryString();
        }

        void refreshQueryString()
        {

            Server = Preferences.Get("server", "122.54.146.208");
            Port = Preferences.Get("port", "3306");
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

        public void updateqtyProductOrder(bool istemp, int idnumber, double qty)
        {
            refreshQueryString();

            try
            {
                MySqlConnection.Open();
                MySqlCommand = MySqlConnection.CreateCommand();
                var commandtext = @"UPDATE `temp_pahabol` SET `qty`=@qty WHERE id = @value;";
                if (!istemp)
                    commandtext = @"UPDATE `trans_pahabol_on` SET `qty`=@qty WHERE id = @value;";
                MySqlCommand.CommandText = commandtext;
                MySqlCommand.Parameters.AddWithValue("@value", idnumber);
                MySqlCommand.Parameters.AddWithValue("@qty", qty);
                MySqlCommand.ExecuteNonQuery();

                MySqlConnection.Close();

            }
            catch (Exception ex)
            {
                MySqlConnection.Close();
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Toast(ex.Message);
            }
        }
    }
}

