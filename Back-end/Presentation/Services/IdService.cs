using Presentation.Contracts;

namespace Presentation.Services
{
    public class IdService : IIdSerivice
    {
        private int nextId;

        public IdService()
        {
            nextId = 0;
        }

        public string GetNextId()
        {
            return nextId++.ToString();
        }

        public void Reset()
        {
            nextId = 0;
        }
    }
}
