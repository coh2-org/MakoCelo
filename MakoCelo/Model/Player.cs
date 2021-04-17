using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakoCelo.Model
{
    public class Player
    {
        public string Name { get; set; }
        public string Rank { get; set; }
        public string RelicId { get; set; }
        public string SteamId { get; set; } // Probably unused after 64 bit patch
        public string Faction { get; set; }
        public int Team { get; set; }
        public Country Country { get; set; }
        public Stats Stats { get; set; }
        public List<Team> Teams { get; set; }

    }

    public class Country
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }

    public class Stats
    {
        public string Faction { get; set; }
        public GameMode GameMode { get; set; }
        public string Rank { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int WinLossRatio { get; set; }
    }

    public class TeamStats
    {
        public string Rank { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }

    public class Team
    {
        public List<string> Player { get; set; }
        public TeamStats AxisStats { get; set; }
        public TeamStats AlliesStats { get; set; }
    }


    public enum GameMode
    {
        V1,
        V2,
        V3,
        V4

    }
}
