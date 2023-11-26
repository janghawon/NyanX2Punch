public delegate void ScoreChange(int hostScore, int clientScore);
public delegate void EndGame(bool isWin);

public class SignalHub
{
    public static ScoreChange OnScoreChanged;
    public static EndGame OnEndGame;
}
