using TrainJourney;

namespace TrainJourneyTests;

public class JourneyCalculatorShould
{
    // ReSharper disable once InconsistentNaming
    // Some of these fields are effectively constants for tests
    private readonly Timetable EmptyTimetable = new(Array.Empty<Train>());
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
        var result = new JourneyCalculator(EmptyTimetable).GetNextTrainTime(AStartLocation, ADestination);
        Assert.That(result, Is.EqualTo(TrainJourneyConstants.NoAvailableTrains));
    }

    [Test]
    public void Throw_exception_when_no_start_location_provided()
    {
        Assert.Throws<ArgumentException>(() => new JourneyCalculator(EmptyTimetable).GetNextTrainTime(Nowhere, ADestination));
    }

    [Test]
    public void Throw_exception_when_no_destination_provided()
    {
        Assert.Throws<ArgumentException>(() => new JourneyCalculator(EmptyTimetable).GetNextTrainTime(AStartLocation, Nowhere));
    }

    [Test]
    public void Return_departure_time_from_start_location_when_there_is_a_matching_train()
    {
        var timetable = new Timetable(new[]
        {
            new Train(new[]
            {
                new Stop(AStartLocation, new TimeOnly(10, 0)),
                new Stop(ADestination, new TimeOnly(11, 0))
            })
        });
        var journeyCalculator = new JourneyCalculator(timetable);
        var result = journeyCalculator.GetNextTrainTime(AStartLocation, ADestination);
        Assert.That(result, Is.EqualTo("10:00am"));
    }
}