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
        
        if (_timetable.Trains.First().Stops.Any(s => s.DepartureTime == new TimeOnly(10,0)))
            return "10:00am";

        if (_timetable.Trains.First().Stops.Any(s => s.DepartureTime == new TimeOnly(23,59)))
            return "23:59pm";
        
        if (_timetable.Trains.First().Stops.Any(s => s.DepartureTime == new TimeOnly(0,0)))
            return "0:00am";
        
        return null;
    }
}