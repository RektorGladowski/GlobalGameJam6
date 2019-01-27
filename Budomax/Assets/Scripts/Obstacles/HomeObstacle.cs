public class HomeObstacle : Obstacle
{
    public override void Drop()
    {
        EndGamePopupData endGamePopupData = new EndGamePopupData() { hasPlayerWon = false, totalScore = (int)HouseManager.instance.GetHeight() };
        PopupManager.instance.ShowEndGamePopup(endGamePopupData);
    }

    private void Update()
    {
        UIManager.instance.UpdateAverageEnergy(100 * health / 10f);
    }
}
