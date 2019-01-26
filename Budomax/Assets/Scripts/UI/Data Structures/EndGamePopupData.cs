
public struct EndGamePopupData
{
    public int totalScore;
    public bool hasPlayerWon;

    public EndGamePopupData(int totalScore, bool hasPlayerWon)
    {
        this.totalScore = totalScore;
        this.hasPlayerWon = hasPlayerWon;
    }
}
