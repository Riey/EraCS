using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EraCS.Variable.Serializer
{
    public sealed class StringSerializer : ISerializer<string>
    {
        private StringSerializer() { }

        public static readonly StringSerializer Instance = new StringSerializer();

        public string DeSerialize(BinaryReader reader) => reader.ReadString();

        public void Serialize(BinaryWriter writer, string value) => writer.Write(value);
    }
}
