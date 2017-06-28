using System.Collections;
using System.Collections.Generic;

namespace EraCS.Variable
{
    public delegate void VariableChangedHandler<T>(IVariable<T> sender, int index, T oldValue, ref T newValue);

    public interface IVariable : IEnumerable, ISerializable
    {
        bool IsSaveData { get; }
        string Name { get; }
        int Size { get; }

        void Reset();
    }

    public interface IVariable<T> : IVariable, IEnumerable<T>
    {
        event VariableChangedHandler<T> VariableChanged;
        T this[int index] { get; set; }
        void Fill(T value, int lower, int higher);
    }

    public interface INamedVariable<T> : IVariable<T>
    {
        T this[string name] { get; set; }
    }
}
