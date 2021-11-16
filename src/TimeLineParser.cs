using google;
using System;
using System.Collections.Generic;
using System.Collections;
/*
Takes a TimelineObject and parses it in such a way that gets secondary
information such as if an activity is missing or data seems dirty in some
other way. This parser can be extended as more and more irregularities
is found in the raw data-dump.

By doing this it will be possible to get a human readable log from the
rawdata, which can be manually inspected for errors/missing data.

Ultimately this class will be responsible for parsing the raw data into "clean" information which
can be used by some other presentation class for providing the final output. 
*/
//Buffers against direct dependencies to the google api classes.
class ClassifiedEvent
{
    public ClassifiedEvent(DateTime start,DateTime end,string name, string descr,string category){
        this.start=start;
        this.end=end;
        this.name=name;
        this.description=descr;
        this.category=category;
    }

    public ClassifiedEvent(PlaceVisit p){
        start = p.GetStartDate();
        end = p.GetEndDate();
        Classify(p);
    }

    public void Classify(PlaceVisit p){
        name=p.GetName();
        description=p.GetDescription();
        toString=p.ToString();

        if(name.Contains("Actic"))
            category="Gym/Sim";
        else if(name.Contains("ICA"))
            category="Handla mat";
        else if(name.Contains("Bergagatan")){
            category="Socialt";
            name="Besökt Ari";    
        }
        if(p.GetAddress().Contains("Dragarbrunnsgatan 50") || p.GetAddress().Contains("Bredgränd ")){
            category="";
            name="Hemma";    
        }
            
        
        description=description+"\nCat:"+category;
    }
    string toString;
    public DateTime start{get;set;}
    public DateTime end{get;set;}
    public string name{get;set;}
    public string description{get;set;}
    public string category{get;set;}

    public override string ToString()
    {
        return toString;
    }
}

class TimeLineParser
{
    DateTime currentDate;
    TimelineContainer timelineContainer;
    IEnumerator tlEnumerator;
    string itemBuffer;
    public Boolean hasNext{get;set;}

    //rat: here I can choose between getting detailed info from the timelineobject or
    //or pass the CalendarEvent to timelineobject. If I make timelineobject know of
    //the calendarEvent then I add another dependency.  
    public ClassifiedEvent GetNextClassifiedEvent(){
        bool hasItem;
        bool isPlace=false;
        ITimelineObject obj=null;
        do{
            hasItem = tlEnumerator.MoveNext();
            if(hasItem){
                obj = ((TimelineObject) tlEnumerator.Current).Get();
                isPlace = obj.isPlace();
            } 
        } while (hasItem && !isPlace);

        if (obj is not null){
            var classified = new ClassifiedEvent((PlaceVisit)obj);
            return classified;
        } else {
            return null;
        }
    }
    

    //gets next item which will be a string
    public string GetNext(){

        if (itemBuffer is not null){
            var tmp = itemBuffer;
            itemBuffer=null;
            return tmp;
        }
        
        if(tlEnumerator.MoveNext()){
            ITimelineObject obj = ((TimelineObject) tlEnumerator.Current).Get();
                if(obj.isSameDay(currentDate)){
                    return obj.ToString();
                } else {
                    var visitDate = obj.GetStartDate();
                    itemBuffer = obj.ToString();
                    currentDate = visitDate;
                    return $"**New Date** {visitDate.ToString("dddd, dd MMMM yyyy")}";
                }
        } 
        else 
        {
            hasNext=false;
            return "End of file";
        }
    }


    public TimeLineParser(TimelineContainer tojb){
        this.timelineContainer=tojb;
        this.tlEnumerator = timelineContainer.timelineObjects.GetEnumerator();
        this.hasNext=true;
    }

    private void ParseActivity(){

    }

    private void ParsePlace(){

    }

}