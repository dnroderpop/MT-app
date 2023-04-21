using Acr.UserDialogs;
using MT.Models;
using MySqlConnector;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MT.Services
{
    internal class mysqlINSERT
    {
        private static MySqlCommand MySqlCommand;
        private static MySqlConnection MySqlConnection;
        private static MySqlConnectionStringBuilder builder;
        private string Server, Port, Username, Password, Database;

        public mysqlINSERT()
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

        public async Task Register(userProfileModel userProfile)
        {
            string username, password, fullname;
            int id = 0, branchid;

            username = userProfile.username;
            password = userProfile.password;
            fullname = userProfile.fullname;
            branchid = userProfile.branch;

            refreshQueryString();

            UserDialogs.Instance.ShowLoading("Loading...", maskType: MaskType.Black);

            await Task.Run(() =>
            {
                try
                {
                    //Try Simple Connection
                    MySqlConnection.Open();

                    MySqlCommand = MySqlConnection.CreateCommand();

                    //Input first the User in User account table so i can get the ID from next query
                    MySqlCommand.CommandText = @"INSERT INTO `user_accounts`(`user`, `password`, `rights`, `nickname`, `pending`, `able`) VALUES (@user, @pass, @rights, @nickname, @pending, @able);";
                    MySqlCommand.Parameters.AddWithValue("@user", username);
                    MySqlCommand.Parameters.AddWithValue("@pass", password);
                    MySqlCommand.Parameters.AddWithValue("@rights", 0);
                    MySqlCommand.Parameters.AddWithValue("@nickname", fullname);
                    MySqlCommand.Parameters.AddWithValue("@pending", 0);
                    MySqlCommand.Parameters.AddWithValue("@able", 1);
                    MySqlCommand.ExecuteNonQuery();

                    //reset connection
                    MySqlConnection.Close();
                    MySqlConnection.Open();

                    // execute the command and read the results

                    //next is search for the username and password of the new registered account
                    //then add branch
                    MySqlCommand.CommandText = @"SELECT `id` from `user_accounts` where user = @param1 and password = @param2;";
                    MySqlCommand.Parameters.AddWithValue("@param1", username);
                    MySqlCommand.Parameters.AddWithValue("@param2", password);

                    var reader = MySqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        id = reader.GetInt32("id");
                    }

                    //reset connection
                    MySqlConnection.Close();
                    MySqlConnection.Open();

                    MySqlCommand.CommandText = @"INSERT INTO `user_branch_app`(`ID`, `Branch`) VALUE (@param3, @param4);";
                    MySqlCommand.Parameters.AddWithValue("@param3", id);
                    MySqlCommand.Parameters.AddWithValue("@param4", branchid);

                    UserDialogs.Instance.Toast("Successfully Registered");
                    MySqlCommand.ExecuteNonQuery();


                    MySqlConnection.Close();

                }
                catch (Exception ex)
                {
                    MySqlConnection.Close();
                    UserDialogs.Instance.Alert(ex.Message, "Error", "Okay");

                }

            });

            UserDialogs.Instance.HideLoading();



        }
    }
}
