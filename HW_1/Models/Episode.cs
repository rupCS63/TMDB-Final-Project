using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HW_1.Models.DAL;

namespace HW_1.Models
{
    public class Episode
    {

        //---------data fields-------
        int id;
        int seriesId;
        string name;
        int seasonNumber;
        string episodeName;
        string img;
        string description;
        string broadcastDate;
        int likes;

        //---------constructors-------
        public Episode()
        {

        }
        public Episode(int id,int seriesId, string name, int seasonNumber, string episodeName, string img, string description, string broadcastDate, int likes)
        {
            this.id = id;
            this.SeriesId = seriesId;
            this.name = name;
            this.seasonNumber = seasonNumber;
            this.episodeName = episodeName;
            this.img = img;
            this.description = description;
            this.broadcastDate = broadcastDate;
            this.Likes = likes;
        }
        //properties
        public string Name { get => name; set => name = value; }
        public int SeasonNumber { get => seasonNumber; set => seasonNumber = value; }
        public string EpisodeName { get => episodeName; set => episodeName = value; }
        public string Img { get => img; set => img = value; }
        public string Description { get => description; set => description = value; }
        public string BroadcastDate { get => broadcastDate; set => broadcastDate = value; }
        public int Id { get => id; set => id = value; }
        public int SeriesId { get => seriesId; set => seriesId = value; }
        public int Likes { get => likes; set => likes = value; }

        public int InsertToSQL(int id)
        {
            DataServices ds = new DataServices();
            ds.InsertToSQL(this);//insert episode to sql
            return ds.checkDuplicate(this, id);
        }
        public int Insert()
        {
            DataServices ds = new DataServices();
            ds.Insert(this);
            return 1;
        }

        public List<Episode> Get()
        {
            DataServices ds = new DataServices();
            List<Episode> sList = ds.GetEpisode();
            return sList;
        }

        public List<Episode> GetEpisodeByTvName(string tvName,string user_id)
        {
            DataServices ds = new DataServices();
            List<Episode> sList = ds.GetEpisodeByTvName(tvName, user_id);
            return sList;
        }

        public List<Episode> GetUserEpisodesById(string user_id)
        {
            DataServices ds = new DataServices();
            List<Episode> eList = ds.GetUserEpisodesById(user_id);
            return eList;
        }

       



    }



}