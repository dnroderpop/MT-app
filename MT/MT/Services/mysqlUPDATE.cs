using Acr.UserDialogs;
using MySqlConnector;
using System;
using Xamarin.Essentials;

namespace MT.Services
{
    internal class mysqlUPDATE
    {
        private static MySqlCommand MySqlCommand;
        private static MySqlConnection MySqlConnection;
        private static MySqlConnectionStringBuilder builder;
        private string Server, Port, Username, Password, Database;

        public mysqlUPDATE()
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

        public void updateqtyProductOrder(bool istemp, int idnumber, double qty)
        {
            refreshQueryString();

            try
            {
                MySqlConnection.Open();
                MySqlCommand = MySqlConnection.CreateCommand();
                var commandtext = @"UPDATE `temp_pahabol` SET `qty`=@qty WHERE id = @value;";
                if (!istemp)
                    commandtext = @"UPDATE `trans_pahabol_on` SET `qty`=@qty WHERE id = @value;";
                MySqlCommand.CommandText = commandtext;
                MySqlCommand.Parameters.AddWithValue("@value", idnumber);
                MySqlCommand.Parameters.AddWithValue("@qty", qty);
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

        public void updateorderapproval(int idnumber)
        {
            refreshQueryString();
            //var ordernumber = 0;
            try
            {
                ////get for settings_trans current number
                //MySqlConnection.Open();
                //MySqlCommand = MySqlConnection.CreateCommand();
                //var commandtext = @"Select pahabolo_num from settings_trans";
                //MySqlCommand.CommandText = commandtext;
                //var reader = MySqlCommand.ExecuteReader();
                //while (reader.Read())
                //{
                //    ordernumber = reader.GetInt32(0);
                //}
                //MySqlConnection.Close();

                //MySqlConnection.Open();
                //MySqlCommand = MySqlConnection.CreateCommand();
                //commandtext = @"UPDATE `temp_pahabol` SET `order_number`=@ordernumber WHERE id = @idnumber;";
                //MySqlCommand.CommandText = commandtext;
                //MySqlCommand.Parameters.AddWithValue("@idnumber", idnumber);
                //MySqlCommand.Parameters.AddWithValue("@ordernumber", ordernumber);
                //MySqlCommand.ExecuteNonQuery();

                //MySqlConnection.Close();

                MySqlConnection.Open();
                MySqlCommand = MySqlConnection.CreateCommand();
                var commandtext = @"INSERT INTO trans_pahabol_on 
                        (`trans_pahabol_on`.`branch`, `trans_pahabol_on`.`prod`, `trans_pahabol_on`.`qty`, `trans_pahabol_on`.`date`, `trans_pahabol_on`.`order_number`,`trans_pahabol_on`.`able`)
                        SELECT `temp_pahabol`.`branch`, `temp_pahabol`.`prod`, `temp_pahabol`.`qty`, `temp_pahabol`.`date`, `temp_pahabol`.`order_number`, `temp_pahabol`.`able`
                        from temp_pahabol WHERE id = @idnumber";
                MySqlCommand.CommandText = commandtext;
                MySqlCommand.Parameters.AddWithValue("@idnumber", idnumber);
                MySqlCommand.Parameters.AddWithValue("@able", 1);
                MySqlCommand.ExecuteNonQuery();
                MySqlConnection.Close();


                MySqlConnection.Open();
                MySqlCommand = MySqlConnection.CreateCommand();
                commandtext = @"UPDATE `temp_pahabol` SET `able`=@able WHERE id = @idnumber;";
                MySqlCommand.CommandText = commandtext;
                MySqlCommand.Parameters.AddWithValue("@idnumber", idnumber);
                MySqlCommand.Parameters.AddWithValue("@able", 0);
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

