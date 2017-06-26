using System;
using System.Collections.Generic;
using EraCS.Variable.Serializer;

namespace EraCS.Variable
{
    public class ArrayVariable<T> : Variable<T>
    {
        protected T[] _data;

        public ArrayVariable(string name, bool isSaveData, int size, ISerializer<T> serializer) : base(name, isSaveData, size, serializer)
        {
            _data = new T[size];
        }

        public override T this[int index]
        {
            get => _data[index];
            set => OnVariableChanged(index, ref _data[index], ref value);
        }

        public override IEnumerator<T> GetEnumerator() { for (int i = 0; i < _data.Length; i++) yield return _data[i]; }

        public override void Reset()
        {
            _data.Initialize();
        }

        protected override void Initialize(T[] data)
        {
            _data = data;
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
