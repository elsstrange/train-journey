namespace TrainJourney;

public class JourneyCalculator : IJourneyCalculator
{
    private readonly ITimetable _timetable;

    public JourneyCalculator(ITimetable timetable)
    {
        _timetable = timetable;
    }

    public string GetNextTrainTime(string startLocation, string destination)
    {
        if (string.IsNullOrEmpty(startLocation) || string.IsNullOrEmpty(destination))
            throw new ArgumentException("Start location and destination must be specified");

        var trains = _timetable.TrainsBetween(startLocation, destination);
        
        if (!trains.Any())
            return TrainJourneyConstants.NoAvailableTrains;

        return trains.OrderBy(t => t.Stops.Single(s => s.Location == startLocation).DepartureTime).First()
            .Stops.Single(s => s.Location == startLocation)
            .DepartureTime.ToString("HH:mmtt").ToLower();
    }
}