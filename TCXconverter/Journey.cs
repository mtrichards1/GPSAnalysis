using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCXconverter
{
    class Journey
    {
        private List<Position> positions;
        private int seconds; //journey time in seconds
        private double distance; //distance in meters
        private double maxSpeed; // in kmph
        public string name;

        public Journey(string name, int time, double distance, double maxspeed, List<Position> positions)
        {
            this.name = name;
            this.distance = distance;
            this.maxSpeed = maxspeed;
            this.positions = positions;
            this.seconds = time;
        }

        public int Seconds
        {
            get
            {
                return seconds;
            }
        }

        public double Maxspeed
        {
            get
            {
                return maxSpeed;
            }
        }

        public double Distance
        {
            get
            {
                return distance;
            }
        }

        public List<Position> Positions
        {
            get
            {
                return positions;
            }
        }

        // name can be left public as this can be changed - it does not need to relate to the list of positions.


        public void addPosition(Position p)
        {
            // will add position - will not automatically recalculate the major statistics within the object. 

        }

        public double calcMaxspeed() //updates the maxspeed using the positions data
        {
            return 1.0;
        }
        public double calcDistance() //updates the Distance using the positions data
        {
            return 1.0;
        }
        public double calcSeconds() //total time (not moving time)
        {
            // seconds = p.d - positions(positions.Count - 1).d;
            return 1.0;
        }

    }
}
