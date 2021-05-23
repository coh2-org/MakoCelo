using MakoCelo.Model;
using MakoCelo.Model.RelicApi;

namespace MakoCelo.Scanner
{
    public interface IRelicApiResponseMapper
    {
        void MapResponseToMatch(Match matchFound, Response response);
    }
}