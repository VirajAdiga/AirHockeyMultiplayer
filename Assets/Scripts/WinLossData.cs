using System;

[Serializable]
public class WinLossData
{
    public int wins;
    public int losses;

    public WinLossData(int wins,int losses)
    {
        this.wins = wins;
        this.losses = losses;
    }
}
