using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace uidatabind
{
    public class DataContext
    {
        public event Action<string> contextChanged;
        private IDictionary<string, object> _activeBinds = new Dictionary<string, object>();

        public bool ContainsKey(string key) => _activeBinds.ContainsKey(key);

        public object this[string key]
        {
            get => _activeBinds[key];
            set
            {
                if (value == null)
                    return;
                _activeBinds[key] = value;
                if (value is INotifyCollectionChanged)
                    (value as INotifyCollectionChanged).OnCollectionChanged += contextChanged;
                contextChanged(key);
            }
        }
    }
}

