namespace TrainJourney;

public class JourneyCalculator : IJourneyCalculator
{
    private readonly Timetable _timetable;

    public JourneyCalculator(Timetable timetable)
    {
        _timetable = timetable;
    }

    public string GetNextTrainTime(string startLocation, string destination)
    {
        if (string.IsNullOrEmpty(startLocation) || string.IsNullOrEmpty(destination))
            throw new ArgumentException("Start location must be specified");

        if (!_timetable.Trains.Any())
            return TrainJourneyConstants.NoAvailableTrains;

        return _timetable.Trains.Single()
            .Stops.Single(s => s.Location == startLocation)
            .DepartureTime.ToString("HH:mmtt").ToLower();
    }
}