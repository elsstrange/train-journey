namespace TrainJourney;

public class Train : ITrain
{
    public IEnumerable<Stop> Stops { get; }
    public Train(IEnumerable<Stop> stops)
    {
        Stops = stops;
    }
}