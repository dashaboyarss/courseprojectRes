using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace курсач3
{
    public class Point
    {
        public string name;
        public double sum;
        public int weeks;

        public Point(string name, double sum, int weeks)
        {
            this.name = name;
            this.sum = sum;
            this.weeks = weeks;
        }

        public Point (Plan plan)
        {
            this.name = plan.name;
            this.sum = plan.currentAmount + plan.currentInvestAmount;
            this.weeks = plan.time - plan.RemainingTime;
        }
    }
}
