namespace TrainJourney;

public interface ITimetable
{
    IEnumerable<Train> TrainsBetween(string startLocation, string destination);
}