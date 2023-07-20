using System.Linq;
using UnityEngine;

namespace uidatabind
{
    public class DataBindContext : MonoBehaviour
    {
        private DataContext _dataContext;

        private object this[string key]
        {
            get => _dataContext[key];
            set
            {
                if (_dataContext == null)
                {
                    _dataContext = new DataContext();
                    _dataContext.contextChanged += ChangeAllBindData;
                }
                _dataContext[key] = value;
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
                if (string.IsNullOrEmpty(child.Key) || child.Key == key)
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

        public void SetValue(string key, object value)
        {
            this[key] = value;
        }
    }
}
