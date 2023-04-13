using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using MT.Models;
using Acr.UserDialogs;
using Xamarin.Essentials;
using Xamarin.Forms;
using MySqlConnector;
using System.Threading;

namespace MT.Services
{
    class mysqldatabase
    {
        public static MySqlCommand MySqlCommand;
        public static MySqlConnection MySqlConnection;
        public static MySqlConnectionStringBuilder builder;

        public mysqldatabase()
        {
            MySqlCommand = new MySqlCommand();
            MySqlConnection = new MySqlConnection();
        }

        public async Task tryConnectionAsync(string server, string userid, string password, string database, uint port)
        {
            var result = false;
            var errormessage = "";
            await Task.Run(() =>
            {

                //build connection
                builder = new MySqlConnectionStringBuilder
                {
                    Server = server,
                    UserID = userid,
                    Database = database,
                    Password = password,
                    ConnectionTimeout = 30,
                };

                MySqlConnection.ConnectionString = builder.ConnectionString;

                try
                {
                    //Try Simple Connection
                    MySqlConnection.Open();
                    result = true;

                    // create a DB command and set the SQL statement with parameters
                    MySqlCommand = MySqlConnection.CreateCommand();

                    MySqlCommand.CommandText = @"SELECT * FROM trans_sts WHERE id = @OrderId;";
                    MySqlCommand.Parameters.AddWithValue("@OrderId", 65536);

                    // execute the command and read the results
                    var reader = MySqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        var id = reader.GetInt32("id");
                    }
                    MySqlConnection.Close();
                }
                catch (Exception ex)
                {
                    errormessage = ex.Message;
                    MySqlConnection.Close();
                    result = false;
                }


            }).ConfigureAwait(true);

            if (result) { 
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
            else
                await Application.Current.MainPage.DisplayAlert("Error", errormessage, "Okay").ConfigureAwait(true);
        }

    }
}
