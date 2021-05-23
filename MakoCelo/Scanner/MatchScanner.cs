﻿using System;
using System.Linq;
using MakoCelo.Model;
using Tracer.NLog;

namespace MakoCelo.Scanner
{
    public class MatchScanner
    {
        private readonly ILogFileParser _logFileParser;
        private readonly IRelicApiClient _relicApiClient;
        private readonly IRelicApiResponseMapper _relicApiResponseMapper;
        
        public event EventHandler NewMatchFound;
        

        public MatchScanner(ILogFileParser logFileParser, IRelicApiClient relicApiClient, IRelicApiResponseMapper relicApiResponseMapper)
        {
            _logFileParser = logFileParser;
            _relicApiClient = relicApiClient;
            _relicApiResponseMapper = relicApiResponseMapper;
        }

        protected virtual void OnNewMatchFound(EventArgs e)
        {
            EventHandler handler = NewMatchFound;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public ScanningResult ScanForMatch(Match previousMatch, string warningsLogFilePath)
        {
            var result = new ScanningResult();
            try
            {
                result.Match = _logFileParser.ParseGameLog(warningsLogFilePath);

                if (result.IsMatchFound() && (previousMatch == null || result.Match.Id != previousMatch.Id))
                {
                    result.IsNewMatch = true;
                    OnNewMatchFound(EventArgs.Empty);

                    var response = _relicApiClient.GetPlayerStats(result.Match.Players.Where(x => !x.IsAIPlayer).Select(x => x.RelicId).ToArray());

                    _relicApiResponseMapper.MapResponseToMatch(result.Match, response);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                result.Success = false;
            }
            return result;
        }


    }
}