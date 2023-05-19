namespace TrainJourney;

public class JourneyCalculator : IJourneyCalculator
{
    public string GetNextTrainTime(string startLocation, string destination)
    {
        if (string.IsNullOrEmpty(startLocation) || string.IsNullOrEmpty(destination))
            throw new ArgumentException("Start location must be specified");
        
        return TrainJourneyConstants.NoAvailableTrains;
    }
}