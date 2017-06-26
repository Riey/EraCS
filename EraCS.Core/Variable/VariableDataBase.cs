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
        {
            get
            {
                return
                    from property in this.GetType().GetRuntimeProperties()
                    where typeof(IVariable).GetTypeInfo().IsAssignableFrom(property.PropertyType.GetTypeInfo())
                    select (IVariable)property.GetValue(this) into var
                    where var.IsSaveData
                    select var;
            }
        }

        public IVariable<int> Result { get; } = new ArrayVariable<int>(nameof(Result), false, 1000, Serializer.IntegerSerializer.Instance);
        public IVariable<string> ResultS { get; } = new ArrayVariable<string>(nameof(ResultS), false, 1000, Serializer.StringSerializer.Instance);
        
        public void Save(Stream output, bool leaveOpen)
        {
            using (var writer = new BinaryWriter(output, Encoding.UTF8, leaveOpen))
            {
                Serialize(writer);
            }
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
            foreach (var var in SavableVariables)
            {
                writer.Write(var.Name);
                var.Serialize(writer);
            }
        }

        public void DeSerialize(BinaryReader reader)
        {
            var varDic = SavableVariables.ToDictionary(var => var.Name);

            while(reader.PeekChar() != -1)
            {
                if (varDic.TryGetValue(reader.ReadString(), out var var))
                    var.DeSerialize(reader);
            }
        }
    }
}
