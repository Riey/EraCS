using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EraCS.Variable.Serializer
{
    public sealed class IntegerSerializer : ISerializer<int>
    {
        private IntegerSerializer() { }

        public static readonly IntegerSerializer Instance = new IntegerSerializer();


        public int DeSerialize(BinaryReader reader) => reader.ReadInt32();

        public void Serialize(BinaryWriter writer, int value) => writer.Write(value);
    }
}
