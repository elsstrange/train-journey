namespace TrainJourney;

public class Stop
{
    public string Location { get; }
    public TimeOnly DepartureTime { get; }

    public Stop(string location, TimeOnly departureTime)
    {
        Location = location;
        DepartureTime = departureTime;
    }
}