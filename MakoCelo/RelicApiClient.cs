﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MakoCelo.Model;
using MakoCelo.Model.RelicApi;
using Newtonsoft.Json;
using Tracer.NLog;

namespace MakoCelo
{
    public class RelicApiClient
    {
        private readonly HttpClient _httpClient = new();
        private readonly JsonSerializer _jsonSerializer = new();
        

        public Response GetPlayerStats(string[] playersIds)
        {
            using var content = Task.Run(async () =>
            {
                var result = _httpClient.GetAsync(
                    "https://coh2-api.reliclink.com/community/leaderboard/GetPersonalStat?title=coh2&profile_ids=[" +
                    string.Join(",", playersIds) + "]").Result;
                    result.EnsureSuccessStatusCode();
                    return await result.Content.ReadAsStreamAsync();

            }).Result; // TODO: make whole app Async :)


            using StreamReader sr = new StreamReader(content);
            using JsonReader reader = new JsonTextReader(sr);

            var deserializeObject = _jsonSerializer.Deserialize<Response>(reader);
            

            if (deserializeObject != null && deserializeObject.Result.Message != "SUCCESS")
            {
                Log.Error(
                    $"Get RID - Error in Relic API, status code: {deserializeObject.Result.Code}, Message: {deserializeObject.Result.Message} ");
                throw new HttpRequestException("Relic API returned data with result different than SUCCESS");

            }

            return deserializeObject;
        }
    }
}