﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracktorTagger
{

    public class PlaceHolderTrackDataSearch : ITrackDataSearch
    {
        private string _searchQuery;
        private int _total;
        private int _perpage;

        private int _loadedPages;

        public PlaceHolderTrackDataSearch(PlaceHolderTrackDataSource source, string searchQuery, int totalResults, int resultsPerPage)
        {
            this._searchQuery = searchQuery;

            this.DataSource = source;
            SearchQuery = searchQuery;

            this._total = totalResults;
            this._perpage = resultsPerPage;
            _loadedPages = 0;

            this._tracks = new List<TrackData>();
        }

        public string SearchQuery
        {
            get;
            private set;
        }

  

        private List<TrackData> _tracks;
        public IList<TrackData> Results
        {
            get { return _tracks.AsReadOnly(); }
        }





        public IList<TrackData> LoadMoreResults()
        {

            List<TrackData> returnList = new List<TrackData>();





            for(int i = 1; i <= _perpage; i++)
            {

                int trackIdInt = _perpage * _loadedPages + i;

                string trackId = trackIdInt.ToString();

                if(trackIdInt > _total) break;

                TrackData data = new TrackData("PlaceHolderTrackDataSource",
       "id" + trackId,
       "artist " + trackId,
       "title " + trackId,
       "mix " + trackId,
       "remixer " + trackId,
       "release " + trackId,
       "producer " + trackId,
       "label " + trackId,
       "cat no " + trackId,
       "lyrics " + trackId,
       "genre " + trackId,
        KeyEnum.A,
        DateTime.Now,
        new Uri(@"http://www.google.com"));

                returnList.Add(data);
                _tracks.Add(data);

            }

            if(returnList.Count > 0) _loadedPages++;

            

            return returnList;

        }

        public ITrackDataSource DataSource
        {
            get;
            private set;
        }


        public int TotalResults
        {
            get { return _total; }
        }
    }

    public class PlaceHolderTrackDataSource : ITrackDataSource
    {

        public PlaceHolderTrackDataSource()
        {
        
        }


        public TrackData GetTrack(string trackId)
        {
            TrackData data = new TrackData("PlaceHolderTrackDataSource",
                "id" + trackId,
                "artist " + trackId,
                "title " + trackId,
                "mix " + trackId,
                "remixer " + trackId,
                "release " + trackId,
                "producer " + trackId,
                "label " + trackId,
                "cat no " + trackId,
                "lyrics " + trackId,
                "genre " + trackId,
                 KeyEnum.A,
                 DateTime.Now,
                 new Uri(@"http://www.google.com"));

            return data;
        }

        public string Name
        {
            get { return "Placeholder Datasource"; }
        }

        public ITrackDataSearch GetTrackDataSearch(string searchQuery)
        {
            var s = new PlaceHolderTrackDataSearch(this, searchQuery, 100, 10);
            s.LoadMoreResults();

            return s;
        }


        public bool ProvidesTitle
        {
            get { return true; }
        }

        public bool ProvidesMix
        {
            get { return true; }
        }

        public bool ProvidesArtist
        {
            get { return true; }
        }

        public bool ProvidesRemixer
        {
            get { return true; }
        }

        public bool ProvidesProducer
        {
            get { return true; }
        }

        public bool ProvidesRelease
        {
            get { return true; }
        }

        public bool ProvidesReleased
        {
            get { return true; }
        }

        public bool ProvidesLabel
        {
            get { return true; }
        }

        public bool ProvidesCatalogNo
        {
            get { return true; }
        }

        public bool ProvidesGenre
        {
            get { return true; }
        }

        public bool ProvidesKey
        {
            get { return true; }
        }
    }
}
