using System.IO;

namespace EraCS.Variable
{
    public interface ISerializable
    {
        void Serialize(BinaryWriter writer);
        void DeSerialize(BinaryReader reader);
    }
}
