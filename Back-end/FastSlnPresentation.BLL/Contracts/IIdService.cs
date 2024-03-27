namespace FastSlnPresentation.BLL.Contracts
{
    public interface IIdService
    {
        string GetNextId();
        public string GetNextId(string name, string format = "{0}-{1}");
        void Reset();
        void Reset(string name);
    }
}
