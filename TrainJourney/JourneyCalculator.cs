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

        var futureTrains = _timetable.TrainsBetween(startLocation, destination)
            .Where(t => !t.Departed(startLocation));

        if (!futureTrains.Any())
            return TrainJourneyConstants.NoAvailableTrains;

        return futureTrains.Select(t => t.Stops.Single(s => s.Location == startLocation))
            .OrderBy(s => s.DepartureTime).First()
            .DepartureTime.ToString("HH:mmtt").ToLower();
    }
}