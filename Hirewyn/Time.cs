using System;
using System.Collections.Generic;
using System.Text;

namespace Hirewyn
{
    public partial class Time
    {
        private int value;
        private TimeEnum timeType;
        public Time()
        {
            
        }
        public int Value { get => value; set => this.value = value; }
        public TimeEnum TimeType { get => timeType; set => timeType = value; }
        public string GetTime()
        {
            string time = String.Empty;
            if (timeType == TimeEnum.Instantaneous)
            {
                time = "Instantaneous";
            }
            else if (timeType == TimeEnum.Special)
            {
                time = "Special";
            }
            else if (timeType == TimeEnum.UntilDispelled)
            {
                time = "Until Dispelled";
            }
            else
            {
                if (timeType != TimeEnum.BonusAction)
                {
                    time = value + " " + timeType.ToString().ToLower();
                }
                else
                {
                    time = value + " bonus action";
                }
            }
            return time;
        }
    }
}
