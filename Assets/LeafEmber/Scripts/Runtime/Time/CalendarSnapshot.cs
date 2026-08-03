using System;

namespace LeafEmber.Time
{

[Serializable]
public sealed class CalendarSnapshot
{
    public int year = 1;
    public int month = 1;
    public int day = 1;
    public DayBlock block = DayBlock.Morning;

    public CalendarSnapshot Copy()
    {
        return new CalendarSnapshot
        {
            year = year,
            month = month,
            day = day,
            block = block,
        };
    }
}
}
