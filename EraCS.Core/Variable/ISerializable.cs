using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EraCS.Variable
{
    public interface ISerializable
    {
        void Serialize(BinaryWriter writer);
        void DeSerialize(BinaryReader reader);
    }
}
