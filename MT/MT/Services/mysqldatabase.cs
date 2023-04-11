using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Android.OS;
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
        MySqlCommand MySqlCommand;
        MySqlConnection MySqlConnection;
        MySqlConnectionStringBuilder builder;

        public mysqldatabase()
        {
        }

        public async Task tryConnectionAsync(string server, string userid, string password, string database, uint port)
        {
            var result = false;
            MySqlCommand = new MySqlCommand();
            MySqlConnection = new MySqlConnection();

            //build connection
            builder = new MySqlConnectionStringBuilder
            {
                Server = server,
                UserID = userid,
                Database = database,
                Password = password,
                ConnectionTimeout = 30,

            };

            result = await sampleconnection();

            if (result)
                App.Current.MainPage = new NavigationPage(new LoginPage());
        }

        async Task<bool> sampleconnection()
        {
            var result = false;
            MySqlConnection.ConnectionString = builder.ConnectionString;

            try
            {
                //Try Simple Connection
                MySqlConnection.Open();
                result = true;

                // create a DB command and set the SQL statement with parameters
                var command = MySqlConnection.CreateCommand();

                command.CommandText = @"SELECT * FROM trans_sts WHERE id = @OrderId;";
                command.Parameters.AddWithValue("@OrderId", 65536);

                // execute the command and read the results
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                { var id = reader.GetInt32("id");
                    await App.Current.MainPage.DisplayAlert("ID", id.ToString(), "close");
                }
                MySqlConnection.Close();
            }
            catch (Exception ex)
            {
                MySqlConnection.Close();
                await App.Current.MainPage.DisplayAlert(ex.Source, ex.Message, "Okay");
                result = false;
            }

            return result;
        }
    }
}
