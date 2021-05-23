using MakoCelo.Model;
using MakoCelo.Model.RelicApi;
using MakoCelo.My.Resources;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tracer.NLog;

namespace MakoCelo.Scanner
{
    public class RelicApiResponseMapper : IRelicApiResponseMapper
    {
        private readonly Dictionary<string, ValueTuple<Faction, GameMode>> _LeaderBoardDictionary = new()
        {
            { "4", (Faction.Ost, GameMode.V1) },
            { "8", (Faction.Ost, GameMode.V2) },
            { "12", (Faction.Ost, GameMode.V3) },
            { "16", (Faction.Ost, GameMode.V4) },

            { "5", (Faction.Sov, GameMode.V1) },
            { "9", (Faction.Sov, GameMode.V2) },
            { "13", (Faction.Sov, GameMode.V3) },
            { "17", (Faction.Sov, GameMode.V4) },

            { "6", (Faction.Okw, GameMode.V1) },
            { "10", (Faction.Okw, GameMode.V2) },
            { "14", (Faction.Okw, GameMode.V3) },
            { "18", (Faction.Okw, GameMode.V4) },

            { "7", (Faction.Usf, GameMode.V1) },
            { "11", (Faction.Usf, GameMode.V2) },
            { "15", (Faction.Usf, GameMode.V3) },
            { "19", (Faction.Usf, GameMode.V4) },

            { "51", (Faction.Ukf, GameMode.V1) },
            { "52", (Faction.Ukf, GameMode.V2) },
            { "53", (Faction.Ukf, GameMode.V3) },
            { "54", (Faction.Ukf, GameMode.V4) }
        };
        private readonly Dictionary<string, string> CountryCodeToNameDictionary = new();

        public RelicApiResponseMapper()
        {

            try
            {
                using var myReader =
                    new TextFieldParser(
                        new StringReader(Resources.country_defs))
                    {
                        TextFieldType = FieldType.Delimited
                    };
                myReader.SetDelimiters(",");
                while (!myReader.EndOfData)
                {
                    var CurrentRow = myReader.ReadFields();
                    CountryCodeToNameDictionary.Add(Strings.LCase(CurrentRow[1]), CurrentRow[0]);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public void MapResponseToMatch(Match matchFound, Response response)
        {

            foreach (var currentPlayer in matchFound.Players.Where(x => !x.IsAIPlayer))
            {
                var currentPlayerData = response.StatGroups
                    .First(x => x.Type == 1 && x.Members.Any(x => x.ProfileId == currentPlayer.RelicId)).Members
                    .First();
                currentPlayer.StatGroupId = currentPlayerData.PersonalStatGroupId;
                currentPlayer.Country = new Country
                {
                    Code = currentPlayerData.Country,
                    Name = CountryCodeToNameDictionary[currentPlayerData.Country]
                };

                foreach (var leaderBoardPlayerData in response.LeaderBoardStats.Where(x =>
                    x.StatGroupId == currentPlayerData.PersonalStatGroupId && _LeaderBoardDictionary.ContainsKey(x.LeaderBoardId)))
                {
                    var (faction, gameMode) = _LeaderBoardDictionary[leaderBoardPlayerData.LeaderBoardId];

                    var personalStats = new PersonalStats
                    {
                        Faction = faction,
                        GameMode = gameMode,
                        Losses = leaderBoardPlayerData.Losses,
                        Wins = leaderBoardPlayerData.Wins,
                        Rank = Convert.ToInt32(leaderBoardPlayerData.Rank),
                        RankLevel = leaderBoardPlayerData.RankLevel,
                        TotalPlayers = leaderBoardPlayerData.RankTotal
                    };

                    currentPlayer.PersonalStats.Add(personalStats);
                    if (currentPlayer.CurrentFaction == personalStats.Faction && matchFound.GameMode == personalStats.GameMode)
                        currentPlayer.CurrentPersonalStats = personalStats;
                }


                currentPlayer.Teams = response.StatGroups.Where(statGroup =>
                        statGroup.Type != 1 &&
                        statGroup.Members.Any(member => member.ProfileId == currentPlayer.RelicId))
                    .Select((statGroup, i) =>
                    {
                        return new Team
                        {
                            Id = statGroup.Id,
                            Players = statGroup.Members.Select(y => new TeamMember
                            {
                                Name = y.Alias,
                                RelicId = y.ProfileId
                            }).ToList(),
                            TeamStats = response.LeaderBoardStats
                                .Where(leaderBoard => leaderBoard.StatGroupId == statGroup.Id).Take(2)
                                .Select(leaderBoard => new TeamStats
                                {
                                    Side = leaderBoard.LeaderBoardId is "20" or "22" or "24"
                                        ? Side.Axis
                                        : Side.Allies,
                                    Rank = leaderBoard.Rank,
                                    RankLevel = leaderBoard.RankLevel,
                                    Wins = leaderBoard.Wins,
                                    Losses = leaderBoard.Losses,
                                    TotalTeams = leaderBoard.RankTotal
                                }).ToList()
                        };
                    }).ToList();

            }

            var allTeams = matchFound.AlliesPlayers.SelectMany(x => x.Teams)
                .Where(x => x.Players.Count <= (int)matchFound.GameMode);

            var teamGrouping = allTeams.GroupBy(x => x.Id).Where(x => x.Count() == x.First().Players.Count)
                .OrderByDescending(y => y.Count()).FirstOrDefault();

            if (teamGrouping != null)
            {
                var team = teamGrouping.First();
                foreach (var matchFoundAlliesPlayer in matchFound.AlliesPlayers)
                {
                    if (team.Players.Any(x => x.RelicId == matchFoundAlliesPlayer.RelicId))
                    {

                        matchFoundAlliesPlayer.CurrentTeam = team;
                        matchFoundAlliesPlayer.CurrentTeamStats =
                            team.TeamStats.First(x => x.Side == Side.Allies);
                    }

                }

            }

            var axsTeams = matchFound.AxisPlayers.SelectMany(x => x.Teams)
                .Where(x => x.Players.Count <= (int)matchFound.GameMode);

            teamGrouping = axsTeams.GroupBy(x => x.Id).Where(x => x.Count() == x.First().Players.Count)
                .OrderByDescending(y => y.Count()).FirstOrDefault();

            if (teamGrouping != null)
            {
                var team = teamGrouping.First();
                foreach (var matchFoundAlliesPlayer in matchFound.AxisPlayers)
                {
                    if (team.Players.Any(x => x.RelicId == matchFoundAlliesPlayer.RelicId))
                    {
                        matchFoundAlliesPlayer.CurrentTeam = team;
                        matchFoundAlliesPlayer.CurrentTeamStats =
                            team.TeamStats.First(x => x.Side == Side.Axis);
                    }

                }

            }

        }
    }
}
