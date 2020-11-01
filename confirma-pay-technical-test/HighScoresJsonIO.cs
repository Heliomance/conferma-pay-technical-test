using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace confirma_pay_technical_test
{
    class HighScoresJsonIO : IHighScoresIO
    {
        private const string filePath = "HighScores.json";

        public List<HighScoresModel> Read()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    return JsonSerializer.Deserialize<List<HighScoresModel>>(File.ReadAllText(filePath));
                }
                catch (JsonException)
                {
                    Console.WriteLine(
                        "WARNING: The High Scores Table file has been corrupted or is invalid. All high scores have been lost.");
                    return null;
                }
            }

            return null;
        }

        public void Write(List<HighScoresModel> scores)
        {
            var highScoresString = JsonSerializer.Serialize(scores);
            File.WriteAllText(filePath, highScoresString);
        }
    }
}