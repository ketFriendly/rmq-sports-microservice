using ConsumerService2.DTOs;
using dotenv.net.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using dotenv.net.Utilities;


namespace ConsumerService2.Services
{
    public class MessageConsumerService : IMessageConsumer
    {
        private readonly IMatchesCountListSingleton _matchesCountListSingleton;
        public MessageConsumerService(IMatchesCountListSingleton matchesCountListSingleton)
        {
            _matchesCountListSingleton = matchesCountListSingleton;
        }
        public void ConsumeMessage(byte[] body)
        {
            var message = Encoding.UTF8.GetString(body);
            MatchInfoDTO matchInfo = JsonConvert.DeserializeObject<MatchInfoDTO>(message);
            Console.WriteLine($"Message received: {matchInfo}");

            string fileName = $"{matchInfo.match_id}.txt";
            string contentToWrite;

            if (_matchesCountListSingleton.Contains(matchInfo.match_id))
            {
                _matchesCountListSingleton.IncrementCounterAndRow(matchInfo.match_id);
                Tuple<int,int> counterAndRow = _matchesCountListSingleton.GetRowNumberAndMatchCount(matchInfo.match_id);
                var snapshotCounter = int.Parse(EnvReader.GetStringValue("SNAPSHOT_COUNT"));

                contentToWrite = $"{counterAndRow.Item2}. Change: new value for market {matchInfo.market_id} is {matchInfo.value}";
                if (counterAndRow.Item1 == snapshotCounter)
                {
                    _matchesCountListSingleton.IncrementRow(matchInfo.match_id);
                    contentToWrite += $"\n{counterAndRow.Item2 + 1}. Snapshot: {matchInfo.name} \n\t\t markets: \n\t\t\t id:{matchInfo.market_id} value: {matchInfo.value}";
                }
                    _CreateAFile(contentToWrite, fileName);
            }
            else
            {
                 contentToWrite = $"1. Match created: {matchInfo.sport} event {matchInfo.name} starts at {matchInfo.start_time}";
                _matchesCountListSingleton.AddMatch(matchInfo.match_id);
                _CreateAFile(contentToWrite, fileName);
            }
        }

        private void _CreateAFile(string contentToWrite, string fileName)
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string baseDirectory = Path.GetDirectoryName(exePath);
            string projectRoot = Directory.GetParent(baseDirectory).Parent.Parent.FullName;
            string folderPath = Path.Combine(projectRoot, "Logs");

            string filePath = Path.Combine(folderPath, fileName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(contentToWrite);
            }

        }
    }
}
