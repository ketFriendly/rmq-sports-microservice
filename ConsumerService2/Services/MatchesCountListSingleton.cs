using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsumerService2.Services
{
    public class MatchesCountListSingleton : IMatchesCountListSingleton
    {
        private static readonly Lazy<MatchesCountListSingleton> _instance = new(() => new MatchesCountListSingleton());

        public static MatchesCountListSingleton Instance
        {
            get { return _instance.Value; }
        }

        private Dictionary<int, Tuple<int, int>> _MatchesCountList;

        private MatchesCountListSingleton()
        {
            _MatchesCountList = new Dictionary<int, Tuple<int, int>>();
        }

        public void AddMatch(int match)
        {
            _MatchesCountList.Add(match, new Tuple<int, int>(1, 1));
        }
        public bool Contains(int match)
        {
            return _MatchesCountList.ContainsKey(match);
        }
        public Tuple<int,int> GetRowNumberAndMatchCount(int match)
        {
            return _MatchesCountList[match];
        }
        public void IncrementCounter(int match)
        {
            if (_MatchesCountList.ContainsKey(match))
            {
                var current = _MatchesCountList[match];
                _MatchesCountList[match] = new Tuple<int, int>(current.Item1 + 1, current.Item2);
            }
        }
        public void IncrementCounterAndRow(int match)
        {
            if (_MatchesCountList.ContainsKey(match))
            {
                var current = _MatchesCountList[match];
                _MatchesCountList[match] = new Tuple<int, int>(current.Item1 + 1, current.Item2 + 1);
            }
        }
        public void IncrementRow(int match)
        {
            if (_MatchesCountList.ContainsKey(match))
            {
                var current = _MatchesCountList[match];
                _MatchesCountList[match] = new Tuple<int, int>(current.Item1, current.Item2 + 1);
            }
        }
    }
}
