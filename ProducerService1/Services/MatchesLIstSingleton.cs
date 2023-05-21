namespace ProducerService1.Services
{
    public class MatchesListSingleton: IMatchesListSingleton
    {
        private static readonly Lazy<MatchesListSingleton> _instance = new Lazy<MatchesListSingleton>(() => new MatchesListSingleton());

        public static MatchesListSingleton Instance
        {
            get { return _instance.Value; }
        }

        private List<int> _MatchesList; 

        private MatchesListSingleton()
        {
            _MatchesList = new List<int>();
        }

        public void AddMatch(int match)
        {
            _MatchesList.Add(match);
        }

        public List<int> GetMatches()
        {
            return _MatchesList;
        }

        public bool Contains(int match)
        {
            return _MatchesList.Contains(match);
        }
    }
}
