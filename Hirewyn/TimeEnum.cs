namespace Hirewyn
{
    public partial class Time
    {
        public enum TimeEnum
        {
            Special = -1,
            Instantaneous = 0,
            Reaction = 1,
            BonusAction = 2,
            Action = 3,
            Round = 4,
            Minute = 5,
            Hour = 6,
            Day = 7,
            Year = 8,
            UntilDispelled = 9
        };
    }
}
