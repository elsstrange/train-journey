namespace TrainJourney;

public class Timetable
{
    public IEnumerable<Train> Trains { get; }
    public Timetable(IEnumerable<Train> trains)
    {
        Trains = trains;
    }
}