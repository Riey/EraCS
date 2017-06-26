using EraCS.Variable;
using EraCS.Variable.Serializer;
using System;

namespace EraCS.Core.Test.Program
{
    public class TestVariableData : VariableDataBase
    {
        public IVariable<DateTime> Time { get; } =
            new ArrayVariable<DateTime>(
                name: nameof(Time),
                isSaveData: true,
                size: 1,
                serializer: new Serializer<DateTime>(
                    (writer, value) => writer.Write(value.ToBinary()),
                    (reader) => DateTime.FromBinary(reader.ReadInt64()))
                );
    }
}
