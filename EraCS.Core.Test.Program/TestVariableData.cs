using System;
using Newtonsoft.Json;

namespace EraCS.Core.Test.Program
{
    [JsonObject]
    public class TestVariableData
    {
        [JsonProperty]
        public DateTime Time { get; set; }
    }
}
