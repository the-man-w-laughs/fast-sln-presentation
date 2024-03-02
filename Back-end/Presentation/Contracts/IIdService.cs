namespace Presentation.Contracts
{
    public interface IIdSerivice
    {
        public string GetNextId();

        public void Reset();
    }
}
