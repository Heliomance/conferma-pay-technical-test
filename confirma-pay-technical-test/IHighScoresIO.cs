using System.Collections.Generic;

namespace confirma_pay_technical_test
{
    interface IHighScoresIO
    {
        List<HighScoresModel> Read();
        void Write(List<HighScoresModel> scores);
    }
}
