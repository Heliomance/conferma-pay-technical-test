using ConsoleTables;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace confirma_pay_technical_test
{
    class HighScores
    {
        public List<HighScoresModel> HighScoreTable { get; private set; }

        private const string _highScoresBanner = @"
___  / / /__(_)______ ___  /_      __  ___/_______________________________
__  /_/ /__  /__  __ `/_  __ \     _____ \_  ___/  __ \_  ___/  _ \_  ___/
_  __  / _  / _  /_/ /_  / / /     ____/ // /__ / /_/ /  /   /  __/(__  ) 
/_/ /_/  /_/  _\__, / /_/ /_/      /____/ \___/ \____//_/    \___//____/  
              /____/                                                      ";

        private int _highScoresLength;
        private IHighScoresIO highScoresIO;


        public HighScores()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            highScoresIO = new HighScoresJsonIO();
            _highScoresLength = config.GetSection("highScoresLength").Get<int>();
            PopulateHighScores();
        }

        private void PopulateHighScores()
        {
            HighScoreTable = highScoresIO.Read() ?? new List<HighScoresModel>();
        }

        public void AddHighScore(int score, double time)
        {
            if (HighScoreTable.Count < _highScoresLength ||
                score > HighScoreTable.Min(entry => entry.Score) ||
                score == HighScoreTable.Min(entry => entry.Score) &&
                time < HighScoreTable.Where(entry => entry.Score == score).Max(entry => entry.Time))
            {
                Console.WriteLine("High Score! Please enter your name for the high score table:");
                string name = Console.ReadLine();
                HighScoreTable.Add(new HighScoresModel()
                {
                    Name = name,
                    Date = DateTime.Now,
                    Score = score,
                    Time = time
                });
                HighScoreTable = HighScoreTable
                    .OrderByDescending(entry => entry.Score)
                    .ThenBy(entry => entry.Time)
                    .Take(_highScoresLength)
                    .ToList();

                highScoresIO.Write(HighScoreTable);
            }
        }

        public void ShowHighScores()
        {
            Console.WriteLine(_highScoresBanner);
            Console.WriteLine();

            var table = new ConsoleTable("Place", "Name", "Score", "Time", "Date");
            for (int i = 0; i < _highScoresLength; i++)
            {
                if (i < HighScoreTable.Count)
                {
                    table.AddRow(
                        i + 1,
                        HighScoreTable[i].Name,
                        HighScoreTable[i].Score,
                        string.Format("{0:0.##}", HighScoreTable[i].Time),
                        HighScoreTable[i].Date.ToString("G"));
                }
                else
                {
                    table.AddRow(i + 1, "", "", "", "");
                }
            }

            table.Write();
            Console.WriteLine();
        }
    }
}