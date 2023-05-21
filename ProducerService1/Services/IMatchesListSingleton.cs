namespace ProducerService1.Services
{
    public interface IMatchesListSingleton
    {
        void AddMatch(int match);
        List<int> GetMatches();
        public bool Contains(int match);
    }
}
