using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EraCS.Variable.Serializer
{
    public sealed class Serializer<T> : ISerializer<T>
    {
        private readonly Action<BinaryWriter, T> _serializer;
        private readonly Func<BinaryReader, T> _deserializer;

        public Serializer(Action<BinaryWriter, T> serializer, Func<BinaryReader, T> deserializer)
        {
            _serializer = serializer;
            _deserializer = deserializer;
        }

        public T DeSerialize(BinaryReader reader) => _deserializer(reader);
        public void Serialize(BinaryWriter writer, T value) => _serializer(writer, value);
    }
}
