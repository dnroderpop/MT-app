using Acr.UserDialogs;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;
using Xamarin.Forms;
using MT.Models;
using CommunityToolkit.Mvvm.Input;
using Java.Lang.Reflect;

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

            Server = Application.Current.Properties["server"].ToString();
            Port = Application.Current.Properties["port"].ToString();
            Username = Application.Current.Properties["userid"].ToString();
            Password = Application.Current.Properties["password"].ToString();
            Database = Application.Current.Properties["database"].ToString();

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
            int id = 0, branchid = 0;

            username = userProfile.username;
            password = userProfile.password;
            fullname = userProfile.fullname;

            UserDialogs.Instance.ShowLoading("Loading...", maskType: MaskType.Black);
            string result = null;

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

                    // execute the command and read the results
                    MySqlCommand.ExecuteNonQuery();

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
    }
}
