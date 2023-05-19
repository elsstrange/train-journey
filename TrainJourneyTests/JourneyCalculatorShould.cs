using TrainJourney;

namespace TrainJourneyTests;

public class JourneyCalculatorShould
{
    private const string AStartLocation = "StartLocation";
    private const string ADestination = "Destination";
    private const string Nowhere = "";

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Return_no_available_trains_when_there_is_no_timetable()
    {
        var result = new JourneyCalculator().GetNextTrainTime(AStartLocation, ADestination);
        Assert.That(result, Is.EqualTo(TrainJourneyConstants.NoAvailableTrains));
    }

    [Test]
    public void Throw_exception_when_no_start_location_provided()
    {
        Assert.Throws<ArgumentException>(() => new JourneyCalculator().GetNextTrainTime(Nowhere, ADestination));
    }

    [Test]
    public void Throw_exception_when_no_destination_provided()
    {
        Assert.Throws<ArgumentException>(() => new JourneyCalculator().GetNextTrainTime(AStartLocation, Nowhere));
    }
}