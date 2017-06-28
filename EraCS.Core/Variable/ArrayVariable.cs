using System.Collections.Generic;
using EraCS.Variable.Serializer;

namespace EraCS.Variable
{
    public class ArrayVariable<T> : Variable<T>
    {
        protected T[] data;

        public ArrayVariable(string name, bool isSaveData, int size, ISerializer<T> serializer) : base(name, isSaveData, size, serializer)
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

        protected override void Initialize(T[] initData)
        {
            data = initData;
        }
    }

    public class NamedVariable<T> : ArrayVariable<T>, INamedVariable<T>
    {
        public NamedVariable(string name, bool isSaveData, int size, ISerializer<T> serializer, IReadOnlyDictionary<string, int> nameDic) : base(name, isSaveData, size, serializer)
        {
            NameDic = nameDic;
        }

        public T this[string name]
        {
            get => this[NameDic[name]];
            set => this[NameDic[name]] = value;
        }

        public IReadOnlyDictionary<string, int> NameDic { get; }
    }
}
