using EraCS.Variable.Serializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace EraCS.Variable
{
    public abstract class Variable<T> : IVariable<T>
    {
        protected readonly ISerializer<T> serializer;

        public abstract T this[int index] { get; set; }

        public bool IsSaveData { get; }
        public string Name { get; }
        public int Size { get; }

        public event VariableChangedHandler<T> VariableChanged;

        protected Variable(string name, bool isSaveData, int size, ISerializer<T> serializer)
        {
            Name = name;
            IsSaveData = isSaveData;
            Size = size;
            this.serializer = serializer;
        }

        protected void OnVariableChanged(int index, ref T oldValue, ref T newValue)
        {
            VariableChanged?.Invoke(this, index, oldValue, ref newValue);
            oldValue = newValue;
        }

        public void Fill(T value, int lower, int higher)
        {
            if (higher < lower) throw new ArgumentOutOfRangeException(nameof(higher));

            for (int i = lower; i <= higher; i++) this[i] = value;
        }

        protected abstract void Initialize(T[] initData);

        public abstract IEnumerator<T> GetEnumerator();
        public abstract void Reset();

        public virtual void Serialize(BinaryWriter writer)
        {
            if (!IsSaveData) throw new InvalidOperationException($"{this} is not savable");


            writer.Write(Size);

            foreach (var d in this)
                serializer.Serialize(writer, d);
        }

        public virtual void DeSerialize(BinaryReader reader)
        {
            if (!IsSaveData) throw new InvalidOperationException($"{this} is not savable");

            var size = reader.ReadInt32();
            var data = new T[size];

            for (int i = 0; i < data.Length; i++) data[i] = serializer.DeSerialize(reader);
            Initialize(data);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            return $"Variable [{Name}]";
        }
    }
}
