using FastSlnPresentation.BLL.Contracts;

namespace FastSlnPresentation.BLL.Services
{
    public class IdService : IIdService
    {
        private const string DefaultName = "default";
        private int _nextId;
        private Dictionary<string, int> _keyValuePairs;

        public IdService()
        {
            _keyValuePairs = new Dictionary<string, int> { { DefaultName, 0 } };
        }

        public string GetNextId()
        {
            _nextId++;
            return _nextId.ToString();
        }

        public string GetNextId(string name, string format = "{0}-{1}")
        {
            if (!_keyValuePairs.ContainsKey(name))
            {
                _keyValuePairs.Add(name, 0);
            }
            _keyValuePairs[name]++;
            return string.Format(format, name, _keyValuePairs[name]);
        }

        public void Reset()
        {
            _nextId = 0;
            _keyValuePairs.Clear();
            _keyValuePairs.Add(DefaultName, 0);
        }

        public void Reset(string name)
        {
            if (_keyValuePairs.ContainsKey(name))
            {
                _keyValuePairs[name] = 0;
            }
            else
            {
                _keyValuePairs.Add(name, 0);
            }
        }
    }
}
