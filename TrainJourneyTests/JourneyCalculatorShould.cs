using FakeItEasy;
using TrainJourney;

namespace TrainJourneyTests;

public class JourneyCalculatorShould
{
    private const string AStartLocation = "StartLocation";
    private const string ADestination = "Destination";
    private const string Nowhere = "";
    private ITimetable _timetable;
    private JourneyCalculator _journeyCalculator;

    private readonly Train[] NoTrains = Array.Empty<Train>();

    private readonly TimeOnly _tenAM = new(10, 0);
    private readonly TimeOnly _elevenAM = new(11, 0);
    private readonly TimeOnly _oneMinutePastTenAM = new(10, 1);

    [SetUp]
    public void Setup()
    {
        _timetable = A.Fake<ITimetable>();
        _journeyCalculator = new JourneyCalculator(_timetable);
    }

    [Test]
    public void Return_no_available_trains_when_there_is_no_timetable()
    {
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
        var stops = new[]
        {
            new Stop(AStartLocation, new TimeOnly(departureHour, departureMinute)),
            new Stop(ADestination, _elevenAM)
        };
        
        var singleValidTrain = A.Fake<ITrain>();
        A.CallTo(() => singleValidTrain.Stops).Returns(stops);
        A.CallTo(() => _timetable.TrainsBetween(AStartLocation, ADestination)).Returns(new[] { singleValidTrain });

        var result = _journeyCalculator.GetNextTrainTime(AStartLocation, ADestination);

        Assert.That(result, Is.EqualTo(formattedDeparture));
    }

    [Test]
    public void Return_the_earliest_train_when_there_are_multiple_matching_trains()
    {
        var earliestDeparture = new Stop(AStartLocation, _tenAM);
        var laterDeparture = new Stop(AStartLocation, _oneMinutePastTenAM);
        var arrivalAtDestination = new Stop(ADestination, _elevenAM);

        var earliestTrain = A.Fake<ITrain>();
        A.CallTo(() => earliestTrain.Stops).Returns(new[]
        {
            earliestDeparture,
            arrivalAtDestination
        });

        var laterTrain = A.Fake<ITrain>();
        A.CallTo(() => laterTrain.Stops).Returns(new[]
        {
            laterDeparture,
            arrivalAtDestination
        });

        var multipleValidTrains = new[] { earliestTrain, laterTrain };
        A.CallTo(() => _timetable.TrainsBetween(AStartLocation, ADestination)).Returns(multipleValidTrains);

        var result = _journeyCalculator.GetNextTrainTime(AStartLocation, ADestination);

        Assert.That(result, Is.EqualTo("10:00am"));
    }
    
    [Test]
    public void Return_no_available_trains_when_all_trains_have_departed()
    {
        var stops = new[]
        {
            new Stop(AStartLocation, new TimeOnly(0,0)),
            new Stop(ADestination, new TimeOnly(0, 1) )
        };
        
        var departedTrain = A.Fake<ITrain>();
        A.CallTo(() => departedTrain.Departed(AStartLocation)).Returns(true);
        A.CallTo(() => departedTrain.Stops).Returns(stops);
        var departedTrains = new []{ departedTrain };
        
        A.CallTo(() => _timetable.TrainsBetween(AStartLocation, ADestination)).Returns(departedTrains);

        var result = _journeyCalculator.GetNextTrainTime(AStartLocation, ADestination);

        Assert.That(result, Is.EqualTo(TrainJourneyConstants.NoAvailableTrains));
    }

    [Test]
    public void Return_earliest_undeparted_train_when_some_trains_have_departed()
    {
        var earliestDeparture = new Stop(AStartLocation, _tenAM);
        var laterDeparture = new Stop(AStartLocation, _oneMinutePastTenAM);
        var arrivalAtDestination = new Stop(ADestination, _elevenAM);

        var departedTrain = A.Fake<ITrain>();
        A.CallTo(() => departedTrain.Stops).Returns(new[]
        {
            earliestDeparture,
            arrivalAtDestination
        });
        A.CallTo(() => departedTrain.Departed(AStartLocation)).Returns(true);

        var futureTrain = A.Fake<ITrain>();
        A.CallTo(() => futureTrain.Stops).Returns(new[]
        {
            laterDeparture,
            arrivalAtDestination
        });
        A.CallTo(() => futureTrain.Departed(AStartLocation)).Returns(false);

        var multipleValidTrains = new[] { departedTrain, futureTrain };
        A.CallTo(() => _timetable.TrainsBetween(AStartLocation, ADestination)).Returns(multipleValidTrains);

        var result = _journeyCalculator.GetNextTrainTime(AStartLocation, ADestination);

        Assert.That(result, Is.EqualTo("10:01am"));
    }
}