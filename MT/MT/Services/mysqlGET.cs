using Acr.UserDialogs;
using Java.Lang.Reflect;
using Java.Security;
using MT.Models;
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

        #region QuerySingleString
        public async Task<string> querySingleStringFromDatabase(string datatablename, string targetcolumn, string whereclause, string parameter)
        {
            UserDialogs.Instance.ShowLoading("Loading...", maskType: MaskType.Black);
            string result = null;
            refreshQueryString();

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
            string result = null;
            refreshQueryString();

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

        #region login - get user info log

        public userloginProfileModel mysqlloadLoggedUserInfo(int id)
        {
            userloginProfileModel returnresult = new userloginProfileModel();
            refreshQueryString();


            try
            {
                MySqlConnection.Open();

                MySqlCommand = MySqlConnection.CreateCommand();

                MySqlCommand.CommandText = @"SELECT * FROM `user_app_view` WHERE id = @param;";
                MySqlCommand.Parameters.AddWithValue("@param", id);

                // execute the command and read the results
                var reader = MySqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    returnresult.Id = id;
                    returnresult.Fullname = reader.GetString("fullname");
                    returnresult.Branchid = reader.GetInt32("branchid");
                    returnresult.Branchname = reader.GetString("branchname");
                }

                Application.Current.Properties["loggedin"] = returnresult;
                Preferences.Set("islogged", true);
                Preferences.Set("userlogged", id);
                Application.Current.SavePropertiesAsync();

                MySqlConnection.Close();

            }
            catch (Exception ex)
            {
                MySqlConnection.Close();
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(ex.Message, "Error", "Okay");
                return null;
            }

            return returnresult;
        }

        public userloginProfileModel mysqlgetloggedUserInfo()
        {
            userloginProfileModel model = (userloginProfileModel)Application.Current.Properties["loggedin"];
            return model;
        }
        #endregion
    }
}
