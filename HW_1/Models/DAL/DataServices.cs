using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace HW_1.Models.DAL
{
    public class DataServices
    {
        static List<Episode> episodeList;
        static List<User> userList;

        public SqlDataAdapter da;
        public DataTable dt;

        public DataServices()
        {

        }
        // Connect- This method creates a connection to the database according to the.. 
        //..connectionString name in the web.config 
        public SqlConnection connect(String conString)
        {
            // read the connection string from the configuration file
            string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        // BuildInsertCommand- Build the Insert command String
        private SqlCommand BuildInsertCommand(Object Obj, SqlConnection con)
        {
            if (Obj is User)
            {
                User temp = (User)Obj;
                // use a string builder to create the dynamic string
                string sql_insert = "Values(@username,@userlastname,@email,@password,@cellphone,@gender,@address,@genre,@yearofbirth,@isAdmin,@points)";
                string prefix = "INSERT INTO Users_2021 " + "(username,userlastname,email,password,cellphone,gender,address,genre,yearofbirth,isAdmin,points)";
                string CommandText = prefix + sql_insert;
                SqlCommand cmd = new SqlCommand(CommandText, con);
                cmd.Parameters.AddWithValue("@username", temp.Name);
                cmd.Parameters.AddWithValue("@userlastname", temp.LastName);
                cmd.Parameters.AddWithValue("@email", temp.Email);
                cmd.Parameters.AddWithValue("@password", temp.Password);
                cmd.Parameters.AddWithValue("@cellphone", temp.Cellphone);
                cmd.Parameters.AddWithValue("@gender", temp.Gender);
                cmd.Parameters.AddWithValue("@address", temp.Address);
                cmd.Parameters.AddWithValue("@genre", temp.Genre);
                cmd.Parameters.AddWithValue("@yearofbirth", temp.YearBirth);
                cmd.Parameters.AddWithValue("@isAdmin", 0);
                cmd.Parameters.AddWithValue("@points", 0);

                return cmd;
            }
            if (Obj is Episode)
            {
                Episode temp = (Episode)Obj;
                // use a string builder to create the dynamic string
                string sql_insert = "Values(@series_id,@episode_id,@episode_name,@air_date,@episode_overeview,@poster_path,@season_number)";
                string prefix = "INSERT INTO Episodes_2021 " + "(series_id,episode_id,episode_name,air_date,episode_overeview,poster_path,season_number)";
                string CommandText = prefix + sql_insert;
                SqlCommand cmd = new SqlCommand(CommandText, con);
                cmd.Parameters.AddWithValue("@series_id", temp.SeriesId);
                cmd.Parameters.AddWithValue("@episode_id", temp.Id);
                cmd.Parameters.AddWithValue("@episode_name", temp.EpisodeName);
                cmd.Parameters.AddWithValue("@air_date", temp.BroadcastDate);
                cmd.Parameters.AddWithValue("@episode_overeview", temp.Description);
                cmd.Parameters.AddWithValue("@poster_path", temp.Img);
                cmd.Parameters.AddWithValue("@season_number", temp.SeasonNumber);

                return cmd;
            }

            else if (Obj is Series)
            {

                Series temp = (Series)Obj;
                // use a string builder to create the dynamic string
                string sql_insert = "Values(@id,@name,@first_air_date,@origin_country,@original_language,@overview,@popularity,@poster_path)";
                string prefix = "INSERT INTO Series_2021 " + "(id, name, first_air_date,origin_country,original_language,overview,popularity,poster_path)";
                string CommandText = prefix + sql_insert;
                SqlCommand cmd = new SqlCommand(CommandText, con);
                cmd.Parameters.AddWithValue("@id", temp.Id);
                cmd.Parameters.AddWithValue("@name", temp.Name);
                cmd.Parameters.AddWithValue("@first_air_date", temp.First_air_date);
                cmd.Parameters.AddWithValue("@origin_country", temp.Origin_country);
                cmd.Parameters.AddWithValue("@original_language", temp.Original_language);
                cmd.Parameters.AddWithValue("@overview", temp.Overview);
                cmd.Parameters.AddWithValue("@popularity", temp.Popularity);
                cmd.Parameters.AddWithValue("@poster_path", temp.Poster_path.ToString());

                return cmd;
            }
            else
            {
                return new SqlCommand();
            }

        }

        public int addPoints(int id)
        {
            SqlConnection con = new SqlConnection();

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string sql_insert = "UPDATE Users_2021 SET points += 5 WHERE userid = " + id;
            string CommandText =  sql_insert;
            SqlCommand cmd = new SqlCommand(CommandText, con);


            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    return 0;
                }
                else throw;
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }


        }



        //create SQL command 
        private SqlCommand CreateCommand(SqlCommand cmd, SqlConnection con)
        {


            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

            return cmd;
        }
        
        //Get the Whole Usrer_2021 table
        public List<User> GetUsers()
        {
            SqlConnection con = null;
            List<User> userList = new List<User>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Users_2021";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    User u = new User();

                    u.Name = (string)dr["username"];
                    u.LastName = (string)dr["userlastname"];
                    u.Email = (string)dr["email"];
                    u.Password = (string)dr["password"];
                    u.Cellphone = (string)dr["cellphone"];
                    u.Gender = (string)dr["gender"];
                    u.Genre = (string)dr["genre"];
                    u.Address = (string)dr["address"];
                    u.Id = (int)dr["userid"];
                    u.YearBirth = (int)dr["yearofbirth"];
                    u.IsAdmin = (bool)dr["isAdmin"];

                    userList.Add(u);
                }

                return userList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        //Get the Recommendations
        public List<Series> getRecommendations(int yearOfBirth, string favGenre)
        {
            SqlConnection con = null;
            List<Series> SeriesList = new List<Series>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT TOP 3 COUNT(*) AS occurrences, e.series_id FROM Favorites_2021 AS f INNER JOIN"
                     + " Episodes_2021 as e ON f.episode_id1 = e.episode_id INNER JOIN"
                     +  " Users_2021 as u ON u.userid = f.userid1 WHERE u.genre ='"+ favGenre +"'"
                     +   " AND"
                     +   " u.yearofbirth > " + (yearOfBirth-5).ToString()+ " AND u.yearofbirth < "+ (yearOfBirth+5).ToString()
                     + " GROUP BY e.series_id"
                     +   " ORDER BY occurrences DESC";
               SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Series s = new Series();
                    s.Id = (int)dr["series_id"];
                    SeriesList.Add(s);
                }

                return SeriesList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        //Valid user login
        public User validLoginFromDB(string mail, string password)
        {
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Users_2021 WHERE email =" + "'" + mail + "'" + " and password = " + "'" + password + "' ";
                SqlCommand cmd = new SqlCommand(selectSTR, con);


                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                User u = new User();

                while (dr.Read())
                {   // Read till the end of the data into a row
                    u.Id = Convert.ToInt32(dr["userid"]);
                    u.Name = (string)dr["username"];
                    u.LastName = (string)dr["userlastname"];
                    u.Email = (string)dr["email"];
                    u.Password = (string)dr["password"];
                    u.Cellphone = (string)dr["cellphone"];
                    u.Gender = (string)dr["gender"];
                    u.Address = (string)dr["address"];
                    u.IsAdmin = (bool)dr["isAdmin"];
                    u.Genre = (string)dr["genre"];
                    u.Points = (int)dr["points"];
                    u.YearBirth = (int)dr["yearofbirth"];
                }
                return u;

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }
        
        //Check in insreting an episode if its already in the Favoritae_2021 - PER USER!
        public int checkDuplicate(Episode episode, int id)
        {
            SqlConnection con = null;

            try
            {
                int count = 0;
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                string sql_insert2 = "select * from Favorites_2021 where userid1 = @userid1 and episode_id1 = @episode_id1";
                SqlCommand cmd = new SqlCommand(sql_insert2, con);
                cmd.Parameters.AddWithValue("@userid1", id);
                cmd.Parameters.AddWithValue("@episode_id1", episode.Id);


                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end


                while (dr.Read())
                {   // Read till the end of the data into a row
                    count++;
                }
                if (count == 0)
                {

                    return addToFav(episode, id);
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        //Add to Favorites_2021 a loved episode
        public int addToFav(Episode episode, int id)
        {

            SqlConnection con = new SqlConnection();

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            string sql_insert = "Values(@userid1,@episode_id1)";
            string prefix = "INSERT INTO Favorites_2021 " + "(userid1,episode_id1)";
            string CommandText = prefix + sql_insert;
            SqlCommand cmd = new SqlCommand(CommandText, con);
            cmd.Parameters.AddWithValue("@userid1", id);
            cmd.Parameters.AddWithValue("@episode_id1", episode.Id);


            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    return 0;
                }
                else throw;
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
       
        //Insert SQL command
        public int InsertToSQL<T>(T obj)
        {
            SqlCommand sendCmd = new SqlCommand();
            SqlConnection con = new SqlConnection();

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            sendCmd = BuildInsertCommand(obj, con);      // helper method to build the insert string

            try
            {
                int numEffected = sendCmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    return 0;
                }
                else throw;
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        
        //Add User to the databse
        public int InsertUserDS(User user)
        {
            Console.WriteLine("InsertUser - dataservise.cs step 3"); //^

            if (userList == null)
                userList = new List<User>();

            userList.Add(user);

            return 1;
        }

        //Insert an episode to the database
        public int Insert(Episode episode)
        {
            if (episodeList == null)
                episodeList = new List<Episode>();

            episodeList.Add(episode);

            return 1;
        }

        //Get episodes from Episodes_2021 table. Return with likes per episode
        public List<Episode> GetEpisode()
        {
            SqlConnection con = null;
            List<Episode> episodesList = new List<Episode>();
            IDictionary<int, int> episodeLikes = GetEpisodeLikesReal();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Episodes_2021";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Episode ep = new Episode();
                    ep.SeriesId = (int)dr["series_id"];
                    ep.Id = (int)dr["episode_id"];
                    ep.Name = (string)dr["episode_name"];
                    ep.Img = (string)dr["poster_path"];
                    ep.Description = (string)dr["episode_overeview"];
                    ep.BroadcastDate = dr["air_date"].ToString();
                    ep.SeasonNumber = Convert.ToInt32(dr["season_number"]);
                    //insert here likes per episode
                    ep.Likes = episodeLikes[(int)dr["episode_id"]];

                    episodesList.Add(ep);
                }

                return episodesList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        //Dictionary of liked series. Sum all the episodes in the series
        public IDictionary<int, int> GetEpisodeLikes()//series
            {
                
                SqlConnection con = null;
                List<Series> seriesesList = new List<Series>();
                try
                {
                    con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file
                                                          //IDictionary<string, int> episodeLikes = new Dictionary<string, int>();

                    Dictionary<int, int> episodeLikes = new Dictionary<int, int>();
                    String selectSTR = "SELECT * FROM Episodes_2021";
                    SqlCommand cmd = new SqlCommand(selectSTR, con);

                    // get a reader
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                while (dr.Read())
                    {   // Read till the end of the data into a row
                    if (episodeLikes.ContainsKey((int)dr["series_id"]) == true)
                    {
                        episodeLikes[(int)dr["series_id"]] += 1;
                    }
                    else
                    {
                        episodeLikes.Add((int)dr["series_id"], 1);

                    }
                }

                    return episodeLikes;
                }
                catch (Exception ex)
                {
                    // write to log
                    throw (ex);
                }
                finally
                {
                    if (con != null)
                    {
                        con.Close();
                    }

                }

            }
        public IDictionary<int, int> GetEpisodeLikesReal()
        {

            SqlConnection con = null;
            List<Series> seriesesList = new List<Series>();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file
                                                     //IDictionary<string, int> episodeLikes = new Dictionary<string, int>();

                Dictionary<int, int> episodeLikes = new Dictionary<int, int>();
                String selectSTR = "SELECT * FROM Favorites_2021";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                while (dr.Read())
                {   // Read till the end of the data into a row
                    if (episodeLikes.ContainsKey((int)dr["episode_id1"]) == true)
                    {
                        episodeLikes[(int)dr["episode_id1"]] += 1;
                    }
                    else
                    {
                        episodeLikes.Add((int)dr["episode_id1"], 1);

                    }
                }

                return episodeLikes;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        //Get series from Series_2021. Return also with how much users liked episodes IN per series
        public List<Series> GetSeries()
            {
                SqlConnection con = null;
                List<Series> seriesesList = new List<Series>();

                try
                {
                    con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file
                                                         //IDictionary<string, int> episodeLikes = new Dictionary<string, int>();

                    IDictionary<int, int> episodeLikes = GetEpisodeLikes();
                    String selectSTR = "SELECT * FROM Series_2021";
                    SqlCommand cmd = new SqlCommand(selectSTR, con);

                    // get a reader
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                    while (dr.Read())
                    {   // Read till the end of the data into a row
                        Series s = new Series();
                        s.Id = (int)dr["id"];
                        s.Name = (string)dr["name"];
                        s.First_air_date = dr["first_air_date"].ToString();
                        s.Origin_country = (string)dr["origin_country"];
                        s.Original_language = (string)dr["original_language"];
                        s.Overview = (string)dr["overview"];
                        s.Popularity = (float)Convert.ToDouble(dr["popularity"]);
                        s.Poster_path = (string)dr["poster_path"];
                        s.Likes = episodeLikes[(int)dr["id"]]; //SHOW HERE NUM OF LIKES FROM DIC
                        seriesesList.Add(s);
                    }

                    return seriesesList;
                }
                catch (Exception ex)
                {
                    // write to log
                    throw (ex);
                }
                finally
                {
                    if (con != null)
                    {
                        con.Close();
                    }

                }
            }
        
        //Get an episode by its name
        public List<Episode> GetEpisodeByTvName(string tvName, string user_id)
            {
                SqlConnection con = null;

                try
                {
                    con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                    String selectSTR = "SELECT *"
                                        + " FROM Favorites_2021 as f"
                                        + " INNER JOIN Episodes_2021 as e ON e.episode_id = f.episode_id1"
                                        + " INNER JOIN Series_2021 as s ON e.series_id = s.id"
                                        + " WHERE f.userid1 = " + "'" + user_id + "'" + " and s.name =" + tvName;
                    SqlCommand cmd = new SqlCommand(selectSTR, con);


                    // get a reader
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                    List<Episode> listOfepisodes = new List<Episode>();
                    while (dr.Read())
                    {   // Read till the end of the data into a row
                        Episode e = new Episode();
                        e.Id = Convert.ToInt32(dr["episode_id"]);
                        e.SeriesId = Convert.ToInt32(dr["series_id"]);
                        e.SeasonNumber = Convert.ToInt32(dr["season_number"]);
                        e.EpisodeName = (string)dr["episode_name"];
                        e.Img = (string)dr["poster_path"];
                        e.Description = (string)dr["episode_overeview"];
                        e.BroadcastDate = Convert.ToDateTime(dr["air_date"]).ToString();

                        listOfepisodes.Add(e);

                    }
                    return listOfepisodes;

                }
                catch (Exception ex)
                {
                    // write to log
                    throw (ex);
                }
                finally
                {
                    if (con != null)

                    {
                        con.Close();
                    }

                }
            }
        
        //Get episodes by its user id
        public List<Episode> GetUserEpisodesById(string user_id)
        {
                SqlConnection con = null;

                try
                {
                    con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                    String selectSTR = "SELECT *"
                                        + " FROM Favorites_2021 as f"
                                        + " INNER JOIN Episodes_2021 as e ON e.episode_id = f.episode_id1"
                                        + " INNER JOIN Series_2021 as s ON e.series_id = s.id"
                                        + " WHERE f.userid1 = " + "'" + user_id + "'";
                    SqlCommand cmd = new SqlCommand(selectSTR, con);

                    // get a reader
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                    List<Episode> listOfepisodes = new List<Episode>();
                    while (dr.Read())
                    {   // Read till the end of the data into a row
                        Episode e = new Episode();
                        e.Id = Convert.ToInt32(dr["episode_id"]);
                        e.SeriesId = Convert.ToInt32(dr["series_id"]);
                        e.SeasonNumber = Convert.ToInt32(dr["season_number"]);
                        e.EpisodeName = (string)dr["name"];
                        e.Img = (string)dr["poster_path"];
                        e.Description = (string)dr["episode_overeview"];
                        e.BroadcastDate = Convert.ToDateTime(dr["air_date"]).ToString();

                        listOfepisodes.Add(e);
                    }
                    return listOfepisodes;
                }
                catch (Exception ex)
                {
                    // write to log
                    throw (ex);
                }
                finally
                {
                    if (con != null)
                    {
                        con.Close();
                    }
                }
        }
    }
}