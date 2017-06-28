using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EraCS
{
    public enum InputType
    {
        ANYKEY,
        INT,
        STR,
    }

    public sealed class InputRequest
    {
        public InputType InputType { get; }
        public bool HasTimeout { get; }
        public bool IsOneInput { get; }
        public string DefaultValue { get; }
        public long EndTime { get; }

        public InputRequest(InputType inputType, bool isOneInput = false)
        {
            InputType = inputType;
            IsOneInput = isOneInput;
        }

        public InputRequest(InputType inputType, long endTime, string defaultValue = null, bool isOneInput = false) : this(inputType, isOneInput)
        {
            HasTimeout = true;
            EndTime = endTime;
            DefaultValue = defaultValue ?? string.Empty;
        }
    }
}
