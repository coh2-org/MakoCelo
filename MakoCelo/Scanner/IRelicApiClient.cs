using MakoCelo.Model.RelicApi;

namespace MakoCelo.Scanner
{
    public interface IRelicApiClient
    {
        Response GetPlayerStats(string[] playersIds);
    }
}