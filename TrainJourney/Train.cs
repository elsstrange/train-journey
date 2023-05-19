namespace TrainJourney;

public class Train
{
    public IEnumerable<Stop> Stops { get; }
    public Train(IEnumerable<Stop> stops)
    {
        Stops = stops;
    }
}