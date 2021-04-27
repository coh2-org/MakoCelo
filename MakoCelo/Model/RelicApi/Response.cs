using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MakoCelo.Model.RelicApi
{
    public class Response
    {
        public Result Result { get; set; }
        public LeaderBoard[] LeaderBoardStats { get; set; }
        public StatGroup[] StatGroups { get; set; }
    }

    public class Result
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

    [DebuggerDisplay("LeaderBoardId = {" + nameof(LeaderBoardId) + "}, Wins = {" + nameof(Wins) + ("}, Losses = { " + nameof(Losses) + "}"))]
    public class LeaderBoard
    {
        public int Disputes { get; set; }
        public int Drops { get; set; }
        public string LastMatchDate { get; set; }
        [JsonProperty("leaderboard_id")]
        public string LeaderBoardId { get; set; }
        public int Losses { get; set; }
        public int Rank { get; set; }
        public string RankLevel { get; set; }
        public int RankTotal { get; set; }
        public int RegionRank { get; set; }
        public int RegionRankTotal { get; set; }
        [JsonProperty("statgroup_id")]
        public string StatGroupId { get; set; }
        public int Streak { get; set; }
        public int Wins { get; set; }
    }

    public class StatGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public Member[] Members { get; set; }

    }

    public class Member
    {
        public string Alias { get; set; }
        public string Country { get; set; }
        [JsonProperty("leaderboardregion_id")]
        public int LeaderBoardRegionId { get; set; }
        //public int Level { get; set; }
        //public string Name { get; set; }
        [JsonProperty("personal_statgroup_id")]
        public string PersonalStatGroupId { get; set; }
        [JsonProperty("profile_id")]
        public string ProfileId { get; set; }
        //public int Xp { get; set; }
    }
}
