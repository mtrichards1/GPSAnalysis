using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace TCXconverter
{
    
    class Program
    {  
        static void Main(string[] args)
        {
            string path=null;

            XmlDocument doc = new XmlDocument();
            
                  bool error = true;
                do{
            try{
      
                Console.WriteLine("Please enter a file location for the TCX file:");
                path = Console.ReadLine();

                // Need to use workaround to Load in the tcx file to memory and edit it before parsing.  Could not figure out how to deal with the
                //schema line containing <TrainingCenterDatabase xsi:schemaLocation....etc.
                
                var oldLines = File.ReadAllLines(path);
                var newLines = oldLines.Where(line => !line.Contains("TrainingCenterDatabase"));
                // File.WriteAllLines(path, newLines); // needed if we want to update the original text file


                doc.LoadXml(String.Join(" ", newLines)); // converts string array to normal string before loading into the xmldocument

            Console.WriteLine("pathloaded");
                error = false;
            }
            catch(FileNotFoundException e){

                Console.WriteLine("File not found");

            }
            }while(error);

            //http://www.doublecloud.org/2013/08/parsing-xml-in-c-a-quick-working-sample/ code from here


            //doc.Load(@"C:\Users\Martin\Downloads\Morning_Ride.tcx"); used for testing

            try
            {

                XmlNodeList nodes = doc.DocumentElement.SelectNodes("/Activities/Activity/Lap");

                int time = Int32.Parse(nodes.Item(0).SelectSingleNode("TotalTimeSeconds").InnerText);
                Console.WriteLine("Journey time = " + time+" seconds");


                double maxspeed = double.Parse(nodes.Item(0).SelectSingleNode("MaximumSpeed").InnerText);
                Console.WriteLine("Max Speed = " + maxspeed/1000 + " kmph");


                double dist = double.Parse(nodes.Item(0).SelectSingleNode("DistanceMeters").InnerText);
                Console.WriteLine("Total Distance = " + dist + " meters");

                Console.WriteLine();
            
                nodes = doc.DocumentElement.SelectNodes("/Activities/Activity/Lap/Track/Trackpoint");
                
                Console.WriteLine("number of points:" + nodes.Count);
                List<Position> ps = new List<Position>();
                // extracting stats data from tcx file

                string dt; // used to hold the string in the format 2016-06-27T06:55:26Z once extracted from xml doc
                Position pos;

                foreach (XmlNode node in nodes)
                {
                    pos = new Position(); // creates new position object to be added.           
                    
                    pos.alt = double.Parse(node.SelectSingleNode("AltitudeMeters").InnerText);
                    
                    // parsing the string format of 2016-06-27T06:55:26Z
                    dt = node.SelectSingleNode("Time").InnerText;
                    int year = int.Parse(dt.Substring(0,4));
                    int month = int.Parse(dt.Substring(5,2));
                    int day = int.Parse(dt.Substring(8,2));
                    int hr = int.Parse(dt.Substring(11,2));
                    int min = int.Parse(dt.Substring(14,2));
                    int s = int.Parse(dt.Substring(17,2));

                    pos.d = new DateTime(year, month, day, hr, min, s);

                    XmlNodeList xx = node.SelectNodes("Position/LatitudeDegrees");
                    pos.lat = double.Parse(xx.Item(0).InnerText);

                    xx = node.SelectNodes("Position/LongitudeDegrees");
                    pos.lon = double.Parse(xx.Item(0).InnerText);

                    ps.Add(pos);  
                    
                }

                Console.WriteLine("Total books: " + ps.Count);

                Journey j = new Journey(Path.GetFileName(path),time,dist,maxspeed,ps);

                ProduceCSV(j,Path.GetFileNameWithoutExtension(path)+".csv"); //produces CSV file with the given name.
            

            }
            catch (XmlException e)
            {
                Console.WriteLine("XML file in wrong format for journey data");
                Console.ReadLine();
                System.Environment.Exit(0);

            }

            Console.ReadLine();
           

            
            }

        public static void ProduceCSV(Journey j, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(j.name);
                writer.WriteLine("Stats from Strava");
                writer.WriteLine("Time moving,"+j.Seconds);
                writer.WriteLine("distance moved,"+j.Distance);
                writer.WriteLine("max speed,"+j.Maxspeed);

                writer.WriteLine(); //newline for formatting

                // writing position data with header line

                writer.WriteLine("Date,Time,Latitude,Longitude,Altitude");
                foreach(Position p in j.Positions){
                writer.WriteLine(p.d.ToShortDateString()+","+p.d.ToLongTimeString()+","+p.lat+","+p.lon+","+p.alt);

                }

                Console.WriteLine("file sucessfully written");

            }



        }

    }
}
