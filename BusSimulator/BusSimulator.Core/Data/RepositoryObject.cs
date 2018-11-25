namespace BusSimulator.Core.Data
{
    public abstract class RepositoryObject
    {
        public int Id { get; set; }

        public abstract void CopyTo(RepositoryObject other);
    }
}
