﻿using Acr.UserDialogs;
using MySqlConnector;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MT.Services
{
    internal class mysqDELETE
    {
        private static MySqlCommand MySqlCommand;
        private static MySqlConnection MySqlConnection;
        private static MySqlConnectionStringBuilder builder;
        private string Server, Port, Username, Password, Database;

        public mysqDELETE()
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

        public void deleteProductOrder(bool istemp,int idnumber)
        {
            refreshQueryString();

            try
            {
                MySqlConnection.Open();
                MySqlCommand = MySqlConnection.CreateCommand();
                var commandtext = @"DELETE FROM `temp_pahabol` WHERE id = @value ";
                if (!istemp)
                    commandtext = @"DELETE FROM `trans_pahabol_on` WHERE id = @value ";
                MySqlCommand.CommandText = commandtext;
                MySqlCommand.Parameters.AddWithValue("@value", idnumber);
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

        public void deleteSavedOrders(int branchid)
        {
            refreshQueryString();

            try
            {
                MySqlConnection.Open();
                MySqlCommand = MySqlConnection.CreateCommand();
                var commandtext = @"DELETE FROM `temp_save_order` WHERE branchid = @value ";
                MySqlCommand.CommandText = commandtext;
                MySqlCommand.Parameters.AddWithValue("@value", branchid);
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

        public void deleteAllProductOrder(DateTime date, int Branchid)
        {
            refreshQueryString();

            // just to shorten the code
            string datepath = date.ToString("yyyy-MM-dd");
            try
            {
                MySqlConnection.Open();
                MySqlCommand = MySqlConnection.CreateCommand();
                var commandtext = @"DELETE FROM `temp_pahabol` WHERE branch = @branchid and date = @date and able = 1";
                MySqlCommand.CommandText = commandtext;
                MySqlCommand.Parameters.AddWithValue("@branchid", Branchid);
                MySqlCommand.Parameters.AddWithValue("@date", date);
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
