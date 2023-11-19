using ConsoleApp.Model;
using ConsoleApp.Model.Enum;
using ConsoleApp.OutputTypes;

namespace ConsoleApp;

public class QueryHelper : IQueryHelper
{
    /// <summary>
    /// Get Deliveries that has payed
    /// </summary>
    public IEnumerable<Delivery> Paid(IEnumerable<Delivery> deliveries) =>
        deliveries.Where(x => x.PaymentId != null);  //TODO: Завдання 1

    /// <summary>
    /// Get Deliveries that now processing by system (not Canceled or Done)
    /// </summary>
    public IEnumerable<Delivery> NotFinished(IEnumerable<Delivery> deliveries) =>
        deliveries.Where(x => x.Status != DeliveryStatus.Cancelled && x.Status != DeliveryStatus.Done); //TODO: Завдання 2

    /// <summary>
    /// Get DeliveriesShortInfo from deliveries of specified client
    /// </summary>
    public IEnumerable<DeliveryShortInfo> DeliveryInfosByClient(IEnumerable<Delivery> deliveries, string clientId) //TODO: Завдання 3
        => deliveries.Where(x => x.ClientId == clientId).Select(x => 
        new DeliveryShortInfo()
        {
            Id = x.Id,
            StartCity = x.Direction.Origin.City,
            EndCity = x.Direction.Destination.City,
            ClientId = x.ClientId,
            Type = x.Type,
            LoadingPeriod = x.LoadingPeriod,
            ArrivalPeriod = x.ArrivalPeriod,
            Status = x.Status,
            CargoType = x.CargoType
        });

    /// <summary>
    /// Get first ten Deliveries that starts at specified city and have specified type
    /// </summary>
    public IEnumerable<Delivery> DeliveriesByCityAndType(IEnumerable<Delivery> deliveries, string cityName, DeliveryType type) =>
        deliveries.Where(x => x.Type == type && x.Direction.Origin.City == cityName).Take(10);      //TODO: Завдання 4

    /// <summary>
    /// Order deliveries by status, then by start of loading period
    /// </summary>
    public IEnumerable<Delivery> OrderByStatusThenByStartLoading(IEnumerable<Delivery> deliveries) =>
        deliveries.OrderBy(x => x.Status).ThenBy(x => x.LoadingPeriod.Start);   //TODO: Завдання 5

    /// <summary>
    /// Count unique cargo types
    /// </summary>
    public int CountUniqCargoTypes(IEnumerable<Delivery> deliveries) =>
        deliveries.DistinctBy(x => x.CargoType).Count();    //TODO: Завдання 6

    /// <summary>
    /// Group deliveries by status and count deliveries in each group
    /// </summary>
    public Dictionary<DeliveryStatus, int> CountsByDeliveryStatus(IEnumerable<Delivery> deliveries) =>
        deliveries.GroupBy(x => x.Status).ToDictionary(x => x.Key, x => x.Count());     //TODO: Завдання 7

    /// <summary>
    /// Group deliveries by start-end city pairs and calculate average gap between end of loading period and start of arrival period (calculate in minutes)
    /// </summary>
    public IEnumerable<AverageGapsInfo> AverageTravelTimePerDirection(IEnumerable<Delivery> deliveries)
        => deliveries.GroupBy(x => x.Direction).Select(x => 
        new AverageGapsInfo
        {
            StartCity = x.Key.Origin.City,
            EndCity = x.Key.Destination.City,
            AverageGap = x.Average(x => (x.ArrivalPeriod.Start - x.LoadingPeriod.End).Value.Minutes),
        });//TODO: Завдання 8

    /// <summary>
    /// Paging helper
    /// </summary>
    public IEnumerable<TElement> Paging<TElement, TOrderingKey>(IEnumerable<TElement> elements,
        Func<TElement, TOrderingKey> ordering,
        Func<TElement, bool>? filter = null,
        int countOnPage = 100,
        int pageNumber = 1) => elements.OrderBy(ordering).Where(filter ?? (x => true)).Skip((pageNumber-1)*countOnPage).Take(countOnPage); //TODO: Завдання 9 
}