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

        public GameMode GameMode => (GameMode) (Players.Count / 2);

        public IEnumerable<Player> AxisPlayers => Players.Where(x => x.CurrentFaction is Faction.Okw or Faction.Ost);
        public IEnumerable<Player> AlliesPlayers => Players.Where(x => x.CurrentFaction is Faction.Sov or Faction.Ukf or Faction.Usf);
    }


    public class Player
    {
        public string Name { get; set; }
        public string RelicId { get; set; }
        public Faction CurrentFaction { get; set; }
        public Country Country { get; set; }
        public PersonalStats CurrentPersonalStats { get; set; }
        public List<PersonalStats> PersonalStats { get; set; } = new();
        public List<Team> Teams { get; set; } = new();
        public Team CurrentTeam { get; set; }
        public TeamStats CurrentTeamStats {get; set; }
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
        public int Rank { get; set; }

        public string FormattedRank => Rank <= 0 ? "---" : $"{Rank}";
        public int Wins { get; set; }
        public int Losses { get; set; }

        public string WinLossPercentRatio =>
            ((double)Wins /
             (Losses + Wins)).ToString("P");

        public int RankLevel { get; set; }
        public string FormattedRankLevel => RankLevel <= 0 ? "---" : $"L-{RankLevel}";
        public int TotalPlayers { get; set; }

        public string FormattedElo => Rank <= 0 ? "---" : ((double) Rank / TotalPlayers).ToString("P");
    }

    public class TeamStats
    {
        public Side Side { get; set; }
        public int Rank { get; set; }
        public string FormattedRank => Rank <= 0 ? "---" : $"{Rank}";
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int RankLevel { get; set; }
        public string FormattedRankLevel => RankLevel <= 0 ? "---" : $"L-{RankLevel}";
        public int  TotalTeams { get; set; }
        public string WinLossPercentRatio =>
            ((double)Wins /
             (Losses + Wins)).ToString("P");

        public string FormattedElo => Rank <= 0 ? "---" : ((double) Rank / TotalTeams).ToString("P");
    }

    public class Team
    {
        public string Id { get; set; }
        public List<TeamMember> Players { get; set; }
        public List<TeamStats> TeamStats { get; set; } = new(2);
    }

    public class TeamMember
    {
        public string Name { get; set; }
        public string RelicId { get; set; }
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
