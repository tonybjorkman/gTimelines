using System;
using System.Collections.Generic;
using System.Text.Json;


namespace test1
{
    class NamedPlace{

        public String name{get;set;}
        public int latE7{get;set;}
        public int longE7{get;set;}

        public NamedPlace(){}

        public NamedPlace(string name, int lat, int lng){
            this.name=name;
            this.latE7=lat;
            this.longE7=lng;
        }

        // distance between two places
        public int distanceE7(NamedPlace other){
            var d_lat = this.latE7-other.latE7;
            var d_lng = this.longE7-other.longE7;
            
            int result = (int)Math.Round(Math.Sqrt(Math.Pow(d_lat,2) + Math.Pow(d_lng,2)));
            return result;
        }

    }

    class GeoFencingService
    {
    
        List<NamedPlace> places;

        string filename;

        public GeoFencingService(string filename){
            this.filename=filename;
            places = new List<NamedPlace>();
        }

        public void Initialize(){
            places.Add(new NamedPlace("Hemma",598581404,176425056));
            places.Add(new NamedPlace("Gym",222582304,112422356));
        }

        public void Serialize(){
            var jsonstr = JsonSerializer.Serialize(places);
            System.IO.File.WriteAllText(filename,jsonstr);
            System.Console.WriteLine("Serialized JSON:"+jsonstr);
        }

        public List<NamedPlace> Deserialize(){
            string locations = System.IO.File.ReadAllText(filename);
            System.Console.WriteLine("savedPlaces contains:"+locations);
            places = JsonSerializer.Deserialize<List<NamedPlace>>(locations);  
            return places;
        }

        public NamedPlace GetClosestPlace(){
            throw new NotImplementedException();
        }

        public static void Test(){
            var gfs = new GeoFencingService("savedPlaces.json");
            gfs.Initialize();
            gfs.Serialize();
            var items = gfs.Deserialize();
            System.Console.WriteLine("end");
        }
    }
}