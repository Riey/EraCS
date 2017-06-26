using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EraCS.Variable.Serializer
{
    public interface ISerializer<T>
    {
        void Serialize(BinaryWriter writer, T value);
        T DeSerialize(BinaryReader reader);
    }
}
