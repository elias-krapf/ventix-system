using System;
using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using VentixSystem.System.Entity;
using VentixSystem.System.Model.Rank;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;

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
                connection.Close();
                
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
                        command.CommandText = "CREATE TABLE IF NOT EXISTS `ventix_vault` (`id` int(11) NOT NULL AUTO_INCREMENT,`durability` int(3) NOT NULL,`stacksize` int(11) NULL,`x` int(11) NULL,`y` int(11) NULL,`rotation` int(11) NULL,`itemid` int(4) NOT NULL,`metadata` varchar(255) NOT NULL,`csteamid` varchar(32) NOT NULL,PRIMARY KEY (`id`)) ";

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
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();

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
                            reader.Close();
                            
                            ventixPlayer.Balance = 500;
                            ventixPlayer.Rank = Rank.DEFAULT;
                            UpdatePlayer(ventixPlayer);
                        }
                        
                        connection.Close();
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
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    
                    command.CommandText = $"SELECT * FROM ventix_player WHERE steam64 = '{ventixPlayer.GetUnturnedPlayer().SteamProfile.SteamID64}'";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            
                            command.CommandText = $@"INSERT INTO ventix_player (steam64, rank, balance, first_join) VALUES (@steam64, @rank, @balance, NOW())";
                            command.Parameters.AddWithValue("@steam64", ventixPlayer.GetUnturnedPlayer().SteamProfile.SteamID64);
                            command.Parameters.AddWithValue("@rank", ventixPlayer.Rank.ToString());
                            command.Parameters.AddWithValue("@balance", ventixPlayer.Balance);  
                            command.ExecuteNonQuery();       
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }
        
         internal static List<ItemJar> RetrieveItems(CSteamID csteamid)
        {
            List<ItemJar> list = new List<ItemJar>();
            try
            {
                int num = 0;
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat(new object[]
                {
                    "select count(*) from `ventix_vault` where `csteamid` = ",
                    csteamid,
                    ";"
                });
                mySqlConnection.Open();
                object obj = mySqlCommand.ExecuteScalar();
                if (obj != null)
                {
                    num = int.Parse(obj.ToString());
                }
                mySqlConnection.Close();
                if (num != 0)
                {
                    mySqlCommand.CommandText = string.Concat(new object[]
                    {
                        "select * from `ventix_vault` where `csteamid` = '",
                        csteamid,
                        "' order by id;"
                    });
                    mySqlConnection.Open();
                    MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                    while (mySqlDataReader.Read())
                    {
                        list.Add(new ItemJar((byte)mySqlDataReader.GetInt32("x"), (byte)mySqlDataReader.GetInt32("y"), (byte)mySqlDataReader.GetInt32("rotation"), new Item(mySqlDataReader.GetUInt16("itemid"), true)
                        {
                            durability = (byte)mySqlDataReader.GetInt32("durability"),
                            metadata = Convert.FromBase64String(mySqlDataReader.GetString("metadata")),
                            amount = (byte)mySqlDataReader.GetInt32("stacksize")
                        }));
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            } 
            return list;
        }

        internal static void UpdateItem(CSteamID cSteamID, ItemJar item)
        {
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                string text = (item.item.metadata == null) ? "" : Convert.ToBase64String(item.item.metadata);
                mySqlCommand.CommandText = string.Concat(new object[]
                {
                    "update `ventix_vault` set `metadata`='"+text+"' where `csteamid`='"+cSteamID+"' and `itemid` = "+ item.item.id+" limit 1;"
                });
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        internal static bool AddItem(CSteamID csteamid, ItemJar item)
        {
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                string text = (item.item.metadata == null) ? "" : Convert.ToBase64String(item.item.metadata);
                mySqlCommand.CommandText = string.Concat(new object[]
                {
                    "insert into `ventix_vault` (`csteamid`,`durability`,`x`,`y`,`rotation`,`metadata`,`itemid`,`stacksize`) values(",
                    csteamid,
                    ",",
                    item.item.durability,
                    ",",
                    item.x,
                    ",",
                    item.y,
                    ",",
                    item.rot,
                    ",'",
                    text,
                    "',",
                    item.item.id,
                    ",",
                    item.item.amount,
                    ");"
                });
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }
        internal static bool DeleteItem(CSteamID csteamid, ItemJar item)
        {
            try
            {
                MySqlConnection mySqlConnection = CreateConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                string text = (item.item.metadata == null) ? "" : Convert.ToBase64String(item.item.metadata);
                mySqlCommand.CommandText = string.Concat(new object[]
                {
                    "delete from `ventix_vault` where csteamid = ",
                    csteamid,
                    " and durability = ",
                    item.item.durability,
                    " and metadata = '",
                    text,
                    "' and itemid = ",
                    item.item.id,
                    " and stacksize = ",
                    item.item.amount,
                    " limit 1;"
                });
                mySqlConnection.Open();
                mySqlCommand.ExecuteScalar();
                mySqlConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }
        
    }
}