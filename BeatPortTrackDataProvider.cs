﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web.Script.Serialization;

namespace TracktorTagger
{
    public class BeatPortTrackDataProvider : ITrackDataProvider
    {

        public BeatPortTrackDataProvider()
        {
            PerPage = 150;
        }

        int _perPage;
        public int PerPage
        {
            get
            { return _perPage; }
            set
            {
                if (value > 0 & value <= 150)
                {
                    _perPage = value;
                }
            }
        }





        public IEnumerable<Track> GetTracks(string searchQuery)
        {
            var json_data = string.Empty;

            if (string.IsNullOrEmpty(searchQuery)) throw new ArgumentNullException("searchQuery");


            List<Track> returnTracks = new List<Track>();
            

            try
            {

                using (var webclient = new WebClient())
                {


                    System.UriBuilder uriBuilder = new UriBuilder("http:", "api.beatport.com");
                    uriBuilder.Path = "catalog/3/search";

                    string query = "query=" + searchQuery + "&page=" + 1 + "&facets[]=fieldType:track&perPage="+PerPage.ToString();

                    uriBuilder.Query = query;




                    json_data = webclient.DownloadString(uriBuilder.Uri.AbsoluteUri);


                    var jss = new JavaScriptSerializer();
                    var dict = jss.Deserialize<Dictionary<string,dynamic>>(json_data);

                    int count = dict["metadata"]["count"];


                    foreach(Dictionary<string,Object> trackData in dict["results"])
                    {

                        Track track = new Track();

                        //Artist(s)
                        System.Collections.ArrayList artists = (System.Collections.ArrayList)trackData["artists"];

                        List<string> artistNames = new List<string>();

                        foreach (Dictionary<string, object> artist in artists)
                        {

                            string artistType = (string)artist["type"];

                            if (artistType != "artist") continue;

                            artistNames.Add((string)artist["name"]);
                        }

                        string artistsStr = string.Join(", ", artistNames);
                        if (!String.IsNullOrEmpty(artistsStr)) track.Artist = artistsStr;


                        //Title
                        string title = (string)trackData["name"];
                        if (!string.IsNullOrEmpty(title)) track.Title = title;



                        //Remixer(s)
                        System.Collections.ArrayList remixers = (System.Collections.ArrayList)trackData["artists"];

                        List<string> remixerNames = new List<string>();

                        foreach (Dictionary<string, object> remixer in remixers)
                        {
                            string remixerType = (string)remixer["type"];

                            if (remixerType != "remixer") continue;

                            remixerNames.Add((string)remixer["name"]);
                        }

                        string remixerStr = string.Join(", ", remixerNames);
                        if (!String.IsNullOrEmpty(remixerStr)) track.Remixer = remixerStr;




                        //Mix
                        string mix = (string)trackData["mixName"];
                        if (!string.IsNullOrEmpty(mix)) track.Mix = mix;


                        //release date
                        string releaseDateStr = (string)trackData["releaseDate"];
                        if (!string.IsNullOrEmpty(releaseDateStr)) track.ReleaseDate = DateTime.Parse(releaseDateStr);


                        //Label
                        Dictionary<string, object> labelDict = (Dictionary<string, object>)trackData["label"];
                        string labelStr = (string)labelDict["name"];
                        if (!string.IsNullOrEmpty(labelStr)) track.Label = labelStr;

                        //Album Title
                        Dictionary<string, object> releaseDict = (Dictionary<string, object>)trackData["release"];
                        string releaseStr = (string)releaseDict["name"];
                        if (!string.IsNullOrEmpty(releaseStr)) track.AlbumTitle = releaseStr;


                        //Genre(s)
                        System.Collections.ArrayList genres = (System.Collections.ArrayList)trackData["genres"];

                        List<string> genreNames = new List<string>();

                        foreach (Dictionary<string, object> genre in genres)
                        {
                            genreNames.Add((string)genre["name"]);
                        }

                        string genreStr = string.Join(", ", genreNames);
                        if (!String.IsNullOrEmpty(genreStr)) track.Genre = genreStr;


                        
                        
                        
                                                
                        //track.CatalogNumber;

                        //track.Key;
                        Dictionary<string, object> keyData = (Dictionary<string, object>)trackData["key"];

                        Dictionary<string, object> standardKeyData = (Dictionary<string,object>)keyData["standard"];

                        string letterString = (string)standardKeyData["letter"];
                        char letter = letterString[0];

                        bool sharp = (bool)standardKeyData["sharp"];
                        bool flat = (bool)standardKeyData["flat"];

                        Accidental a;

                        if (sharp) a = Accidental.Sharp;
                        else if (flat) a = Accidental.Flat;
                        else a = Accidental.Natural;

                        string chordStr = (string)standardKeyData["chord"];

                        Chord c = Chord.Major;
                        if (chordStr == "major") c = Chord.Major;
                        else if (chordStr == "minor") c = Chord.Minor;

                        track.Key = new Key(letter, a, c);

                        
                        
              
               


                        //URL
                        System.UriBuilder trackUriBuilder = new UriBuilder("http:", "www.beatport.com");

                        string slugString = (string)trackData["slug"];
                        int trackId = (int)trackData["id"];

                        trackUriBuilder.Path = "track/"+ slugString + "/" + trackId.ToString();

                        track.URL = trackUriBuilder.Uri.AbsoluteUri;
                        

                        //Not Support by BeatPort
                        //track.Lyrics;
                        //track.Producer;

                        

                        returnTracks.Add(track);
                    
                    }

     
                }//close web client



            }
            catch
            {
                throw;
            }


            return returnTracks;



        }
    }
}