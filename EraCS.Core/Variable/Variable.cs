using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EraCS.Variable
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Variable<T> : IVariable<T>
    {
        public abstract T this[int index] { get; set; }
        
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "size")]
        public int Size { get; private set; }

        public event VariableChangedHandler<T> VariableChanged;

        protected Variable(string name, int size)
        {
            Name = name;
            Size = size;
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

        public abstract IEnumerator<T> GetEnumerator();
        public abstract void Reset();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            return $"Variable [{Name}]";
        }
    }
}
