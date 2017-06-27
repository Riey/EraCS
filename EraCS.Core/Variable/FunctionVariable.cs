using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EraCS.Variable.Serializer;

namespace EraCS.Variable
{
    public sealed class FunctionVariable<T> : Variable<T>
    {
        private readonly Func<int, T> _valueFactory;

        public FunctionVariable(string name, int size, Func<int, T> valueFactory) : base(name, false, size, null)
        {
            _valueFactory = valueFactory;
        }

        public override T this[int index]
        {
            get => _valueFactory(index);
            set => throw new NotSupportedException();
        }

        public override IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Size; i++)
                yield return _valueFactory(i);
        }

        public override void Reset()
        {
        }

        protected override void Initialize(T[] data)
        {
        }
    }
}
