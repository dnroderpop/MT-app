using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using Java.Lang.Reflect;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<string> querySingleStringFromDatabase(string datatablename, string targetcolumn, string whereclause,string whereclause2, string parameter,string parameter2)
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

                    MySqlCommand.CommandText = @"SELECT " + targetcolumn + " FROM " + datatablename + " WHERE " + whereclause + " = @param1 and "+ whereclause2 +" = @param2;";
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
                    UserDialogs.Instance.Alert(ex.Message,"Error", "Okay");

                }

            });

            UserDialogs.Instance.HideLoading();
            return result;
        }
        #endregion


    }
}
