namespace TrainJourney;

public interface ITrain
{
    IEnumerable<Stop> Stops { get; }
}