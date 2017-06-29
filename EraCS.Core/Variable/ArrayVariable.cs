using System.Collections.Generic;
using Newtonsoft.Json;

namespace EraCS.Variable
{
    public class ArrayVariable<T> : Variable<T>
    {
        [JsonProperty(PropertyName = "data")]
        protected T[] data;

        public ArrayVariable(string name, int size) : base(name, size)
        {
            data = new T[size];
        }

        public override T this[int index]
        {
            get => data[index];
            set => OnVariableChanged(index, ref data[index], ref value);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            foreach (T t in data) yield return t;
        }

        public override void Reset()
        {
            data.Initialize();
        }
    }

    public class NamedVariable<T> : ArrayVariable<T>, INamedVariable<T>
    {
        public NamedVariable(string name, int size, IReadOnlyDictionary<string, int> nameDic) : base(name, size)
        {
            NameDic = nameDic;
        }

        public T this[string name]
        {
            get => this[NameDic[name]];
            set => this[NameDic[name]] = value;
        }

        [JsonProperty]
        public IReadOnlyDictionary<string, int> NameDic { get; private set; }
    }
}
