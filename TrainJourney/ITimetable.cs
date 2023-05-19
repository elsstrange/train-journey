namespace TrainJourney;

public interface ITimetable
{
    IEnumerable<ITrain> TrainsBetween(string startLocation, string destination);
}