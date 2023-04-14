using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using MySqlConnector;
using MT.Models;
using Xamarin.Essentials;
using Acr.UserDialogs;

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

            if (result)
                Application.Current.MainPage = new NavigationPage(new LoginPage());

            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", errormessage, "Okay").ConfigureAwait(true);
                UserDialogs.Instance.HideLoading();
            }
        }

        public async Task loadbranchandproducts()
        {
            await Task.Run(() => {
                string Server, Username, Password, Database;
                Server = Preferences.Get("server", "122.54.146.208");
                Username = Preferences.Get("userid", "rodericks");
                Password = Preferences.Get("password", "mtchoco");
                Database = Preferences.Get("database", "mangtinapay");
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
