using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

namespace uidatabind
{
    public class DataBindContext : MonoBehaviour
    {
        private DataContext _dataContext;

        public object this[string key]
        {
            get => _dataContext[key];
            set
            {
                if (_dataContext == null)
                {
                    _dataContext = new DataContext();
                    _dataContext.contextChanged += ChangeAllBindData;
                }
            }
        }

        public bool ContainsKey(string key) => _dataContext.ContainsKey(key);

        public void ChangeAllBindData(string key)
        {
            var children = GetComponentsInChildren<IBindable>().ToList();

            if (children == null)
                return;

            children.ForEach(child =>
            {
                if (string.IsNullOrEmpty(child.key) || child.key == key)
                    child.Bind(_dataContext);
            });
        }

        public void BindAll()
        {
            var children = GetComponentsInChildren<IBindable>().ToList();
            
            if (children == null)
                return;

            if (_dataContext == null)
                _dataContext = new DataContext();

            children.ForEach(child => child.Bind(_dataContext));
        }
    }
}
