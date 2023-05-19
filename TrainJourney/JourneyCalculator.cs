namespace TrainJourney;

public class JourneyCalculator : IJourneyCalculator
{
    public string GetNextTrainTime(string startLocation, string destination)
    {
        return TrainJourneyConstants.NoAvailableTrains;
    }
}