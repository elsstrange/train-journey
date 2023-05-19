namespace TrainJourney;

public interface IJourneyCalculator
{
    string GetNextTrainTime(string startLocation, string destination);
}