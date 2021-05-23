using MakoCelo.Model;

namespace MakoCelo.Scanner
{
    public interface ILogFileParser
    {
        Match ParseGameLog(string filePath);
    }
}