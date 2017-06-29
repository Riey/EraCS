using EraCS.Variable;
using System;
using Newtonsoft.Json;

namespace EraCS.Core.Test.Program
{
    [JsonObject]
    public class TestVariableData
    {
        [JsonProperty]
        public IVariable<DateTime> Time { get; private set; } = new ArrayVariable<DateTime>(nameof(Time), 1);
    }
}
