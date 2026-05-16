public static class PropsTurn
{
    public static int Turn { get; private set; } = 1;
    
    public static void NextTurn()
    {
        Turn++;
    }

    public static void Reset()
    {
        Turn = 1;
    }
}
