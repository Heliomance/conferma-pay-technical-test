using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace confirma_pay_technical_test
{
    class GameLoop
    {
        private HighScores highScores;
        private readonly int targetScore;
        private readonly int pointsPerGuess;
        public GameLoop()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            targetScore = config.GetSection("pointsToWin").Get<int>();
            pointsPerGuess = config.GetSection("pointsPerGuess").Get<int>();
            highScores = new HighScores();
            MainLoop();
        }

        public void MainLoop()
        {
            while (true)
            {
                Console.WriteLine("Welcome to the Higher Lower game!");
                Console.WriteLine("(P)lay    (H)igh Scores    (Q)uit");
                char response = Utils.GetInput(new[] {'p', 'h', 'q'});
                Console.Clear();
                switch (response)
                {
                    case 'p':
                        bool alive = true;
                        int score = 0;
                        Game game = new Game();
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        while (alive && score < targetScore)
                        {
                            alive = game.Play();
                            if (alive)
                            {
                                score+=pointsPerGuess;
                            }
                        }
                        stopwatch.Stop();
                        Console.WriteLine($"Your score: {score} points. Total time: {string.Format("{0:0.##}",stopwatch.Elapsed.TotalSeconds)} seconds.");

                        if (score == targetScore)
                        {
                            Console.WriteLine("You win! Well done!!!");
                        }
                        highScores.AddHighScore(score, stopwatch.Elapsed.TotalSeconds);
                        Console.WriteLine();

                        break;
                    case 'h':
                        highScores.ShowHighScores();
                        break;
                    case 'q':
                        return;
                }
            }
        }
    }
}