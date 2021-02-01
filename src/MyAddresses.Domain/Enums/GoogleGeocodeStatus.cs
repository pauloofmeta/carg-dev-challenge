namespace MyAddresses.Domain.Enums
{
    public enum GoogleGeocodeStatus
    {
        OK,
        ZERO_RESULTS,
        OVER_DAILY_LIMIT,
        OVER_QUERY_LIMIT,
        REQUEST_DENIED,
        INVALID_REQUEST,
        UNKNOWN_ERROR
    }
}