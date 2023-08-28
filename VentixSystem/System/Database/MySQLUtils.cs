using System;
using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using VentixSystem.System.Entity;
using VentixSystem.System.Model.Rank;

namespace VentixSystem.System.Database
{
    public class MySQLUtils
    {
        internal MySQLUtils()
        {
            MySqlConnection connection = CreateConnection();
            try
            {
                connection.Open();
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
            }
        }

        public static MySqlConnection CreateConnection()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection($"SERVER={VentixSystem.Instance.Configuration.Instance.DatabaseHost};DATABASE={VentixSystem.Instance.Configuration.Instance.DatabaseName};UID={VentixSystem.Instance.Configuration.Instance.DatabaseUser};PASSWORD={VentixSystem.Instance.Configuration.Instance.DatabasePassword};PORT={VentixSystem.Instance.Configuration.Instance.DatabasePort};");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return connection;
        }

        public static void CreateDefaultTables()
        {
            using (MySqlConnection connection = CreateConnection())
            {
                try
                {
                    MySqlCommand command = connection.CreateCommand();
                    connection.Open();
                        command.CommandText =
                            $@"CREATE TABLE IF NOT EXISTS ventix_player
                            (
                                `steam64` BIGINT(20) PRIMARY KEY,
                                `rank` VARCHAR(64),
                                `balance` INT,
                                `first_join` DATETIME
                            ) COLLATE = 'utf8_general_ci' ENGINE = InnoDB;";
                        command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        public static void LoadOrCreatePlayer(VentixPlayer ventixPlayer)
        {
            using (MySqlConnection connection = CreateConnection())
            {
                try
                {
                    MySqlCommand command = connection.CreateCommand();
                    connection.Open();

                    command.CommandText = $"SELECT * FROM ventix_player WHERE steam64 = '{ventixPlayer.GetUnturnedPlayer().SteamProfile.SteamID64}'";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ventixPlayer.Balance = reader.GetInt32("balance");
                            switch (reader.GetString("rank"))
                            {
                                case "DEFAULT":
                                    ventixPlayer.Rank = Rank.DEFAULT;
                                    break;
                                case "VIP":
                                    ventixPlayer.Rank = Rank.VIP;
                                    break;
                                case "EPIC":
                                    ventixPlayer.Rank = Rank.EPIC;
                                    break;
                                case "MEGA":
                                    ventixPlayer.Rank = Rank.MEGA;
                                    break;
                                case "ULTRA":
                                    ventixPlayer.Rank = Rank.ULTRA;
                                    break;
                                case "FRIEND":
                                    ventixPlayer.Rank = Rank.FRIEND;
                                    break;
                                case "STAFF":
                                    ventixPlayer.Rank = Rank.STAFF;
                                    break;
                                case "OWNER":
                                    ventixPlayer.Rank = Rank.OWNER;
                                    break;
                            }
                        }
                        else
                        {
                            ventixPlayer.Balance = 500;
                            ventixPlayer.Rank = Rank.DEFAULT;
                            UpdatePlayer(ventixPlayer);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        public static void UpdatePlayer(VentixPlayer ventixPlayer)
        {
            using (MySqlConnection connection = CreateConnection())
            {
                try
                {
                    MySqlCommand command = connection.CreateCommand();
                    connection.Open();
                    
                    command.CommandText = $"SELECT * FROM ventix_player WHERE steam64 = '{ventixPlayer.GetUnturnedPlayer().SteamProfile.SteamID64}'";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            command.CommandText = $@"INSERT INTO ventix_player (steam64, rank, balance, first_join) VALUES (@steam64, @rank, @balance, NOW())";
                            command.Parameters.AddWithValue("@steam64", ventixPlayer.GetUnturnedPlayer().SteamProfile.SteamID64);
                            command.Parameters.AddWithValue("@rank", ventixPlayer.Rank.ToString());
                            command.Parameters.AddWithValue("@balance", ventixPlayer.Balance);  
                            command.ExecuteNonQuery();       
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }
        
    }
}