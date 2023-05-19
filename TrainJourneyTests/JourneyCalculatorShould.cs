using FakeItEasy;
using TrainJourney;

namespace TrainJourneyTests;

public class JourneyCalculatorShould
{
    private ITimetable _timetable;
    private JourneyCalculator _journeyCalculator;
    
    private Train[] NoTrains;
    private const string AStartLocation = "StartLocation";
    private const string ADestination = "Destination";
    private const string Nowhere = "";

    [SetUp]
    public void Setup()
    {
        _timetable = A.Fake<ITimetable>();
        _journeyCalculator = new JourneyCalculator(_timetable);
    }

    [Test]
    public void Return_no_available_trains_when_there_is_no_timetable()
    {
        NoTrains = Array.Empty<Train>();
        A.CallTo(() => _timetable.TrainsBetween(AStartLocation, ADestination)).Returns(NoTrains);
        var result = _journeyCalculator.GetNextTrainTime(AStartLocation, ADestination);
        Assert.That(result, Is.EqualTo(TrainJourneyConstants.NoAvailableTrains));
    }

    [Test]
    public void Throw_exception_when_no_start_location_provided()
    {
        Assert.Throws<ArgumentException>(() => _journeyCalculator.GetNextTrainTime(Nowhere, ADestination));
    }

    [Test]
    public void Throw_exception_when_no_destination_provided()
    {
        Assert.Throws<ArgumentException>(() => _journeyCalculator.GetNextTrainTime(AStartLocation, Nowhere));
    }

    [TestCase(10, 0, "10:00am")]
    [TestCase(0, 0, "00:00am")]
    [TestCase(23, 59, "23:59pm")]
    public void Return_departure_time_from_start_location_when_there_is_a_matching_train(int departureHour,
        int departureMinute, string formattedDeparture)
    {
        var singleValidTrain = new[]
        {
            new Train(new[]
            {
                new Stop(AStartLocation, new TimeOnly(departureHour, departureMinute)),
                new Stop(ADestination, new TimeOnly(0, 0))
            })
        };

        A.CallTo(() => _timetable.TrainsBetween(AStartLocation, ADestination)).Returns(singleValidTrain);

        var result = _journeyCalculator.GetNextTrainTime(AStartLocation, ADestination);
        Assert.That(result, Is.EqualTo(formattedDeparture));
    }

    [Test]
    public void Return_the_earliest_train_when_there_are_multiple_matching_trains()
    {
        var multipleValidTrains = new[]
        {
            new Train(new[]
            {
                new Stop(AStartLocation, new TimeOnly(10, 0)),
                new Stop(ADestination, new TimeOnly(11, 0))
            }),
            new Train(new[]
            {
                new Stop(AStartLocation, new TimeOnly(10, 1)),
                new Stop(ADestination, new TimeOnly(11, 0))
            })
        };
        
        A.CallTo(() => _timetable.TrainsBetween(AStartLocation, ADestination)).Returns(multipleValidTrains);

        var result = _journeyCalculator.GetNextTrainTime(AStartLocation, ADestination);
        Assert.That(result, Is.EqualTo("10:00am"));
    }
}