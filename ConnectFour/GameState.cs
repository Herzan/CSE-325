namespace ConnectFour;

/// <summary>
/// Holds all state for a game of Connect Four: the board, whose turn it is,
/// win detection, and (as the assignment's added feature) a running move
/// history plus consecutive-win tracking for each player.
/// </summary>
public class GameState
{
    public const int Rows = 6;
    public const int Columns = 7;

    // grid[0, col] is the BOTTOM row of the board; grid[Rows-1, col] is the top.
    private byte[,] grid = new byte[Rows, Columns];
    private byte[] columnHeights = new byte[Columns];
    private int movesPlayed;
    private WinState? cachedResult;

    public enum WinState
    {
        None,
        Player1_Wins,
        Player2_Wins,
        Tie
    }

    /// <summary>Whose turn it currently is: 1 or 2.</summary>
    public byte PlayerTurn { get; private set; } = 1;

    /// <summary>Index (0-41) of the next piece to be placed. Used by the UI
    /// to know which of the 42 rendered game-piece elements to animate.</summary>
    public int CurrentTurn => movesPlayed;

    // ---- Added feature: move history & consecutive-win tracking ----

    /// <summary>Human-readable log of every move made in the current game.</summary>
    public List<string> MoveHistory { get; } = new();

    public byte? LastWinner { get; private set; }
    public int Player1ConsecutiveWins { get; private set; }
    public int Player2ConsecutiveWins { get; private set; }
    public int TotalGamesPlayed { get; private set; }

    // ------------------------------------------------------------------

    public bool IsColumnFull(int column) => columnHeights[column] >= Rows;

    /// <summary>
    /// Resets the board for a new game. Career stats (consecutive wins,
    /// total games played) are intentionally preserved across games.
    /// </summary>
    public void ResetBoard()
    {
        grid = new byte[Rows, Columns];
        columnHeights = new byte[Columns];
        movesPlayed = 0;
        PlayerTurn = 1;
        cachedResult = null;
        MoveHistory.Clear();
    }

    /// <summary>
    /// Plays a piece for the current player in the given column (0-6).
    /// Returns the "drop" row (1-6) used to select the CSS drop animation,
    /// matching the distance the piece visually falls.
    /// </summary>
    public byte PlayPiece(byte column)
    {
        if (column >= Columns)
        {
            throw new ArgumentException($"Column must be between 0 and {Columns - 1}.");
        }

        if (IsColumnFull(column))
        {
            throw new ArgumentException($"Column {column + 1} is full. Choose another column.");
        }

        byte row = columnHeights[column]; // 0 = bottom row
        grid[row, column] = PlayerTurn;
        columnHeights[column]++;

        byte landingRow = (byte)(Rows - row); // 1..6, matches the drop1..drop6 CSS classes

        MoveHistory.Add($"Move {movesPlayed + 1}: Player {PlayerTurn} dropped a piece into column {column + 1}");
        movesPlayed++;

        PlayerTurn = PlayerTurn == 1 ? (byte)2 : (byte)1;

        return landingRow;
    }

    /// <summary>
    /// Scans the board for four-in-a-row (horizontal, vertical, or either
    /// diagonal). The first time a win or tie is detected it is recorded
    /// into the consecutive-win stats; further calls return the cached
    /// result so stats are never double-counted.
    /// </summary>
    public WinState CheckForWin()
    {
        if (cachedResult.HasValue)
        {
            return cachedResult.Value;
        }

        (int dr, int dc)[] directions = { (0, 1), (1, 0), (1, 1), (1, -1) };

        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                byte value = grid[row, col];
                if (value == 0)
                {
                    continue;
                }

                foreach (var (dr, dc) in directions)
                {
                    bool connectsFour = true;
                    for (int step = 1; step < 4; step++)
                    {
                        int r = row + (dr * step);
                        int c = col + (dc * step);
                        if (r < 0 || r >= Rows || c < 0 || c >= Columns || grid[r, c] != value)
                        {
                            connectsFour = false;
                            break;
                        }
                    }

                    if (connectsFour)
                    {
                        var result = value == 1 ? WinState.Player1_Wins : WinState.Player2_Wins;
                        RecordResult(value);
                        cachedResult = result;
                        return result;
                    }
                }
            }
        }

        if (movesPlayed >= Rows * Columns)
        {
            RecordResult(null);
            cachedResult = WinState.Tie;
            return WinState.Tie;
        }

        return WinState.None;
    }

    private void RecordResult(byte? winner)
    {
        TotalGamesPlayed++;

        if (winner is null)
        {
            // A tie breaks both players' streaks.
            Player1ConsecutiveWins = 0;
            Player2ConsecutiveWins = 0;
            LastWinner = null;
            return;
        }

        if (LastWinner == winner)
        {
            if (winner == 1) Player1ConsecutiveWins++;
            else Player2ConsecutiveWins++;
        }
        else
        {
            if (winner == 1)
            {
                Player1ConsecutiveWins = 1;
                Player2ConsecutiveWins = 0;
            }
            else
            {
                Player2ConsecutiveWins = 1;
                Player1ConsecutiveWins = 0;
            }
        }

        LastWinner = winner;
    }
}
