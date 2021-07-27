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

public class Garage : IEnumerable
{
   // System.Array already implements IEnumerator!
   private int[] carArray = new int[4];
   public Garage()
   {
      carArray[0] = 0;
      carArray[1] = 10;
      carArray[2] = 20;
      carArray[3] = 30;
   }
   public IEnumerator GetEnumerator()
   {
      // Return the array object's IEnumerator.
      return carArray.GetEnumerator();
   }
}

class TimeLineParser
{

    DateTime currentDate;
    TimelineContainer timelineContainer;
    IEnumerator tlEnumerator;
    string itemBuffer;
    public Boolean hasNext{get;set;}

    public void CheckDateChange(){

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
                    var visitDate = obj.GetDate();
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