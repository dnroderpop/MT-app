using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using MySqlConnector;
using MT.Models;
using Xamarin.Essentials;
using Acr.UserDialogs;
using MT.Views;

namespace MT.Services
{
    class mysqldatabase
    {
        public static MySqlCommand MySqlCommand;
        public static MySqlConnection MySqlConnection;
        public static MySqlConnectionStringBuilder builder;
        mysqlGET mysqlget;
        public mysqldatabase()
        {
            MySqlCommand = new MySqlCommand();
            MySqlConnection = new MySqlConnection();
            mysqlget = new mysqlGET();
        }

        public async Task<bool> tryConnectionAsync()
        {
            var result = false;
            var errormessage = "";


            bool isloggedin = Preferences.Get("islogged", false);
            int userloggedid = Preferences.Get("userlogged", 0);

            string server, userid, database, password, port;
            server = Preferences.Get("server", "122.54.146.208");
            userid = Preferences.Get("database", "mangtinapay");
            database = Preferences.Get("userid", "rodericks");
            password = Preferences.Get("password", "mtchoco");
            port = Preferences.Get("port", "3306");

            if (isloggedin)
            {
                userloginProfileModel userloginProfile = mysqlget.mysqlloadLoggedUserInfo(userloggedid);
                if (userloginProfile != null) { 
                    Application.Current.Properties["loggedin"] = userloginProfile;
                    result = true;
                }
                else
                    result = false;
                
            }
            else
                try
                {

                    //build connection
                    builder = new MySqlConnectionStringBuilder
                    {
                        Server = server,
                        UserID = userid,
                        Database = database,
                        Password = password,
                        Port = uint.Parse(port),
                        ConnectionTimeout = 30,
                    };

                    MySqlConnection.ConnectionString = builder.ConnectionString;
                    //Try Simple Connection
                    MySqlConnection.Open();

                    // create a DB command and set the SQL statement with parameters
                    MySqlCommand = MySqlConnection.CreateCommand();

                    MySqlCommand.CommandText = @"SELECT * FROM trans_sts WHERE 1 Limit 1;";

                    // execute the command and read the results
                    var reader = MySqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        var id = reader.GetInt32("id");
                    }
                    result = true;
                    MySqlConnection.Close();

                }
                catch (Exception ex)
                {
                    MySqlConnection.Close();
                    errormessage = ex.Message;
                    await Application.Current.MainPage.DisplayAlert("Error", errormessage, "Okay").ConfigureAwait(true);
                    result = false;
                }


            return result;
        }

        public async Task loadbranchandproducts()
        {
            await Task.Run(() =>
            {
                string Server, Username, Password, Database, Port;
                Server = Preferences.Get("server", "122.54.146.208");
                Username = Preferences.Get("userid", "rodericks");
                Password = Preferences.Get("password", "mtchoco");
                Database = Preferences.Get("database", "mangtinapay");
                Port = Preferences.Get("port", "3306");
                loadedProfileModel loadedProfile = new loadedProfileModel();

                //build connection
                builder = new MySqlConnectionStringBuilder
                {
                    Server = Server,
                    UserID = Username,
                    Database = Database,
                    Password = Password,
                    ConnectionTimeout = 30,
                };

                MySqlConnection.ConnectionString = builder.ConnectionString;

                try
                {
                    //Try Simple Connection
                    MySqlConnection.Open();

                    // create a DB command and set the SQL statement with parameters
                    MySqlCommand = MySqlConnection.CreateCommand();

                    //get Branch
                    MySqlCommand.CommandText = @"SELECT * FROM `source_branch` where able = 1;";

                    // execute the command and read the results
                    var reader = MySqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        var loadedid = reader.GetInt32("id");
                        var loadedname = reader.GetString("name");
                        loadedProfile.BranchProfiles.Add(new branchProfileModel(loadedid, loadedname));
                    }
                    MySqlConnection.Close();

                    MySqlConnection.Open();
                    //get product
                    MySqlCommand = MySqlConnection.CreateCommand();

                    MySqlCommand.CommandText = @"SELECT * FROM `source_product` where able = 1;";

                    // execute the command and read the results

                    var reader2 = MySqlCommand.ExecuteReader();
                    while (reader2.Read())
                    {
                        var loadedid = reader2.GetInt32("id");
                        var loadedname = reader2.GetString("name");
                        var loadedcategory = reader2.GetString("category");
                        var loadedsrpc = reader2.GetDouble("srpc");
                        var loadedsrpb = reader2.GetDouble("srpc");
                        var loadedavew = reader2.GetDouble("avew");
                        var loadedyield = reader2.GetDouble("yielding");
                        loadedProfile.productProfileModels.Add(new productProfileModel(loadedid, loadedname, loadedcategory, loadedsrpc, loadedsrpb, loadedavew, loadedyield));
                    }

                    Application.Current.Properties["loadedprofile"] = loadedProfile;

                    MySqlConnection.Close();
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.Toast(ex.Message);
                    MySqlConnection.Close();
                }

                UserDialogs.Instance.HideLoading();
                return Task.CompletedTask;
            });
        }

        public loadedProfileModel getBranchandproducts()
        {
            loadedProfileModel loadedProfile = (loadedProfileModel)Application.Current.Properties["loadedprofile"];
            return loadedProfile;
        }
    }
}
