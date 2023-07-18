using System;

namespace uidatabind
{
    public interface INotifyCollectionChanged
    {
        event Action<string> OnCollectionChanged;
    }
}