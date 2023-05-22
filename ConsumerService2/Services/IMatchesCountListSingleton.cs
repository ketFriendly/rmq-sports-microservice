namespace ConsumerService2.Services
{
    public interface IMatchesCountListSingleton
    {
        void AddMatch(int match);
        public bool Contains(int match);
        public void IncrementCounterAndRow(int match);
        public void IncrementRow(int match);
        public void IncrementCounter(int match);
        public Tuple<int, int> GetRowNumberAndMatchCount(int match);

    }
}
