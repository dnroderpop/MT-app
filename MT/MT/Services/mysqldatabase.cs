using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Android.OS;
using MySqlConnector;
using System.Threading.Tasks;
using MT.Models;
using Acr.UserDialogs;

namespace MT.Services
{
    class mysqldatabase
    {
        MySqlCommand MySqlCommand;
        MySqlConnection MySqlConnection;
        MySqlConnectionStringBuilder builder;

        public mysqldatabase()
        {
            MySqlCommand = new MySqlCommand();
            builder = new MySqlConnectionStringBuilder();
        }

        public async Task<bool> tryConnectionAsync(string server, string userid, string password, string database, uint port)
        {
            
            builder = new MySqlConnectionStringBuilder
            {
                Server = server,
                UserID = userid,
                Database = database,
                Password = password,
                ConnectionTimeout = 30,
            };

            MySqlConnection = new MySqlConnection(builder.ConnectionString);
            try
            {
                UserDialogs.Instance.ShowLoading("Connecting to database...");
                _ =MySqlConnection.OpenAsync();
                MySqlConnection.Close();
            }
            catch (Exception ex) {
                UserDialogs.Instance.HideLoading();
                _ = App.Current.MainPage.DisplayAlert(ex.Source, ex.Message, "Okay");
                return await Task.FromResult(false);
            }
            UserDialogs.Instance.HideLoading();
            return await Task.FromResult(true);

        }
    }
}
