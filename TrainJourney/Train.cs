namespace TrainJourney;

public class Train : ITrain
{
    public IEnumerable<Stop> Stops { get; }
    bool ITrain.Departed(string stop)
    {
        throw new NotImplementedException();
    }
    
    public Train(IEnumerable<Stop> stops)
    {
        Stops = stops;
    }
}