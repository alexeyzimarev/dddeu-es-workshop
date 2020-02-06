namespace EventSourcing.Domain
{
    public struct Theater
    {
        public Theater(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}