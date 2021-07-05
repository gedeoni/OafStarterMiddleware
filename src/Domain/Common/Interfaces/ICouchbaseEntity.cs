namespace Domain.Common.Interfaces
{
    public interface ICouchbaseEntity
    {
        public string Id { get; set; }
        public string Entity { get; set; }
    }
}