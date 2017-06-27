using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

namespace EraCS.Variable
{
    public abstract class VariableDataBase : ISerializable
    {
        protected virtual IEnumerable<IVariable> SavableVariables
            =>
                from property in this.GetType().GetRuntimeProperties()
                where typeof(IVariable).GetTypeInfo().IsAssignableFrom(property.PropertyType.GetTypeInfo())
                select (IVariable) property.GetValue(this)
                into var
                where var.IsSaveData
                select var;

        protected virtual void Save(BinaryWriter writer)
        {
        }

        public void Save(Stream output, bool leaveOpen)
        {
            using (var writer = new BinaryWriter(output, Encoding.UTF8, leaveOpen))
            {
                Serialize(writer);
                Save(writer);
            }
        }

        protected virtual void Load(BinaryReader reader)
        {
        }

        public void Load(Stream input, bool leaveOpen)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, leaveOpen))
            {
                DeSerialize(reader);
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            var savableVariables = SavableVariables.ToArray();
            writer.Write(savableVariables.Length);

            foreach (var var in savableVariables)
            {
                writer.Write(var.Name);
                var.Serialize(writer);
            }
        }

        public void DeSerialize(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            var varDic = SavableVariables.ToDictionary(var => var.Name);

            for (int i = 0; i < count; i++)
            {
                if (varDic.TryGetValue(reader.ReadString(), out var var))
                    var.DeSerialize(reader);
            }
        }
    }
}
