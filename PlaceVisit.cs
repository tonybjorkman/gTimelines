using System.Collections.Generic;
using NPOI.SS.Util;
using System;

namespace google{
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

    public interface ITimelineObject
    {
        public DateTime GetDate();

        public Boolean isSameDay(DateTime date);
    }
    public class SourceInfo
    {
        public int deviceTag { get; set; }
    }

    public class StartLocation
    {
        public int latitudeE7 { get; set; }
        public int longitudeE7 { get; set; }
        public SourceInfo sourceInfo { get; set; }
    }

    public class EndLocation
    {
        public int latitudeE7 { get; set; }
        public int longitudeE7 { get; set; }
        public SourceInfo sourceInfo { get; set; }
    }

    public class Duration
    {
        public string startTimestampMs { get; set; }
        public string endTimestampMs { get; set; }
        public DateTime GetDate(){
            return DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(startTimestampMs)).ToLocalTime().DateTime;
        }
        private string startTimeStampString(){
            return DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(startTimestampMs)).ToLocalTime().ToString();
        }
        private string durationString(){
            long endms=long.Parse(endTimestampMs);
            long startms=long.Parse(startTimestampMs);
            System.TimeSpan t = System.TimeSpan.FromMilliseconds(endms-startms);
            string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", 
                        t.Hours, 
                        t.Minutes, 
                        t.Seconds);
                        return answer;
        }

        public override string ToString()
        {
            return this.startTimeStampString() + $" ({this.durationString()})";
        }
    }

    public class Activity
    {
        public string activityType { get; set; }
        public double probability { get; set; }
    }

    public class Waypoint
    {
        public int latE7 { get; set; }
        public int lngE7 { get; set; }
    }

    public class WaypointPath
    {
        public List<Waypoint> waypoints { get; set; }
    }

    public class Point
    {
        public int latE7 { get; set; }
        public int lngE7 { get; set; }
        public string timestampMs { get; set; }
        public int accuracyMeters { get; set; }
    }

    public class SimplifiedRawPath
    {
        public List<Point> points { get; set; }

    }
    public class Location
    {
        public int latitudeE7 { get; set; }
        public int longitudeE7 { get; set; }
        public int accuracyMetres { get; set; }
        public string placeId { get; set; }
        public string address { get; set; }
        public string name { get; set; }
        public SourceInfo sourceInfo { get; set; }
        public double locationConfidence { get; set; }
    
        public override string ToString(){
            return $"{name},{address} coord:lat-{((double)latitudeE7)/10000000},long-{(double)longitudeE7/10000000}";
        }
    }

    public class ParkingEvent
    {
        public string timestampMs { get; set; }
        public Location location { get; set; }
    }

    public class ActivitySegment : ITimelineObject
    {
        public Boolean isSameDay(DateTime date){
            return (date.Date == duration.GetDate().Date);
        }

        public DateTime GetDate(){
            return duration.GetDate();
        }
        public StartLocation startLocation { get; set; }
        public EndLocation endLocation { get; set; }
        public Duration duration { get; set; }
        public int distance { get; set; }
        public string activityType { get; set; }
        public string confidence { get; set; }
        public List<Activity> activities { get; set; }
        public WaypointPath waypointPath { get; set; }
        public SimplifiedRawPath simplifiedRawPath { get; set; }
        public ParkingEvent parkingEvent { get; set; }

        public override string ToString()
        {
            return activityType;
        }
    }

    public class OtherCandidateLocation
    {
        public int latitudeE7 { get; set; }
        public int longitudeE7 { get; set; }
        public string placeId { get; set; }
        public double locationConfidence { get; set; }
    }

    public class PlaceVisit : ITimelineObject
    {
        public Boolean isSameDay(DateTime date){
            return (date.Date == duration.GetDate().Date);
        }

        public DateTime GetDate(){
            return duration.GetDate();
        }

        public Location location { get; set; }
        public Duration duration { get; set; }
        public string placeConfidence { get; set; }
        public int centerLatE7 { get; set; }
        public int centerLngE7 { get; set; }
        public int visitConfidence { get; set; }
        public List<OtherCandidateLocation> otherCandidateLocations { get; set; }
        public string editConfirmationStatus { get; set; }
    
        public override string ToString(){
            return $"Visited {location} \n {duration}";
        }
    }

///Contains either an Activity or a Place
    public class TimelineObject
    {
        public ActivitySegment activitySegment { get; set; }
        public PlaceVisit placeVisit { get; set; }

        public ITimelineObject Get(){
            if (activitySegment is not null){
                return activitySegment;
            }
            else if (placeVisit is not null){
                return placeVisit;
            }
            else {
                throw new System.Exception("no object in timelineObject");
            }
        }

        public override string ToString()
        {
            if (activitySegment is not null){
                return activitySegment.ToString();
            }
            else if (placeVisit is not null){
                return placeVisit.ToString();
            }
            else {
                return "Empty";
            }
        }
    }

    public class TimelineContainer
    {
        public List<TimelineObject> timelineObjects { get; set; }
    }
}