using System;
using System.Collections.Generic;
using System.Text;

namespace Hirewyn
{
    public partial class Distance
    {
        int value;
        DistanceEnum distanceType;

        public Distance()
        {

        }

        public int Value { get => value; set => this.value = value; }
        public DistanceEnum DistanceType { get => distanceType; set => distanceType = value; }
        public string GetDistance()
        {
            string distance = String.Empty;
            if (distanceType == DistanceEnum.Special)
            {
                distance = "Special";
            }
            else if (distanceType == DistanceEnum.Self)
            {
                distance = "Self";
            }
            else if (distanceType == DistanceEnum.Touch)
            {
                distance = "Touch";
            }
            else if (distanceType == DistanceEnum.Sight)
            {
                distance = "Sight";
            }
            else if (distanceType == DistanceEnum.Unlimited)
            {
                distance = "Unlimited";
            }
            else
            {
                distance = value + " " + distanceType.ToString().ToLower();
            }
            return distance;
        }
    }
}
