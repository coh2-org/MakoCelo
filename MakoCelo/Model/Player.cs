using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakoCelo.Model
{
    public class Match
    {
        //network session GUID
        public string Id { get; set; }

        public List<Player> Players { get; set; } = new(8);

        public bool IsMatchFound()
        {
            return !string.IsNullOrEmpty(Id);
        }

        public GameMode MatchMode => (GameMode) (Players.Count / 2);
    }


    public class Player
    {
        public string Name { get; set; }
        public string Rank { get; set; }
        public string RelicId { get; set; }
        // Probably unused after 64 bit patch
        public string SteamId { get; set; } 
        public Faction Faction { get; set; }
        public int Team { get; set; }
        public Country Country { get; set; }
        public List<PersonalStats> PersonalStats { get; set; } = new();
        public List<Team> Teams { get; set; } = new();
        public string StatGroupId { get; set; }

    }

    public class Country
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }

    public class PersonalStats
    {
        public Faction Faction { get; set; }
        public GameMode GameMode { get; set; }
        public string Rank { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string WinLossPercentRatio { get; set; }
        public string RankLevel { get; set; }
        public int TotalPlayers { get; set; }
    }

    public class TeamStats
    {
        public Side Side { get; set; }
        public string Rank { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string RankLevel { get; set; }
        public string WinLossPercentRatio { get; set; }
    }

    public class Team
    {
        public List<string> Players { get; set; }
        public List<TeamStats> TeamStats { get; set; } = new(2);
    }

    public enum Faction
    {
        Ost = 1,
        Sov = 2,
        Okw = 3,
        Usf = 4,
        Ukf = 5
    }

    public enum GameMode
    {
        V1 = 1,
        V2 = 2,
        V3 = 3,
        V4 = 4

    }

    public enum Side
    {
        Axis = 1,
        Allies = 2
    }
}
