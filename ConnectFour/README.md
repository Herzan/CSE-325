# Connect Four — Blazor Web App

A "Connect Four" game built with Blazor Server (.NET 8), created by following the
Microsoft Learn "Build a Connect Four game with Blazor" module, plus one added
feature required by the assignment:

**Added feature — Game Activity panel**
- **Move history**: every drop is logged ("Move 3: Player 2 dropped a piece into
  column 5") in a scrollable list, so players can review the whole game.
- **Consecutive win tracking**: the app remembers, across games (until the app is
  restarted), how many games in a row each player has won, and resets a player's
  streak the moment the other player wins or the game ties. Total games played is
  also shown.
- As a small bonus, drop buttons for full columns are visually disabled and
  ignored, and clicks are ignored once a game has ended (until Reset is pressed).

## Project structure
```
ConnectFour/
├── ConnectFour.csproj
├── Program.cs                     # app startup / DI / render mode
├── GameState.cs                   # board, turns, win detection, streaks (singleton service)
├── appsettings.json
├── Components/
│   ├── App.razor                  # root HTML document
│   ├── Routes.razor                # router
│   ├── _Imports.razor
│   ├── Board.razor                # the game board + controls + activity panel
│   ├── Board.razor.css            # scoped styles + drop animations
│   ├── Layout/
│   │   ├── MainLayout.razor       # sidebar + top "About" bar shell
│   │   ├── MainLayout.razor.css
│   │   ├── NavMenu.razor          # Home / Counter / Fetch data sidebar links
│   │   └── NavMenu.razor.css
│   └── Pages/
│       ├── Home.razor             # "/" page, hosts <Board>
│       ├── Counter.razor          # standard template demo page
│       └── Weather.razor          # standard template "Fetch data" demo page
└── wwwroot/app.css
```

## Prerequisites
- **.NET 8 SDK** — free download: https://dotnet.microsoft.com/download/dotnet/8.0
  (Verify with `dotnet --version`, should print `8.x.x`.)
- Optional: Visual Studio 2022 (17.8+) if you'd rather use an IDE instead of the CLI.

## How to run it (command line)
Unzip the project, then from a terminal:

```bash
cd ConnectFour
dotnet restore
dotnet run
```

You'll see output like:
```
Now listening on: https://localhost:7220
Now listening on: http://localhost:5220
```

Open that URL in your browser (Chrome/Edge/Firefox) and play the game.

To auto-rebuild on file changes while you're developing, use:
```bash
dotnet watch run
```

## How to run it (Visual Studio 2022)
1. Unzip the project.
2. Double-click `ConnectFour.csproj` to open it in Visual Studio (or File > Open > Project/Solution).
3. Press **F5** (or the green Run arrow) to build and launch it in your browser.

## How to test it
- Click the 🔽 buttons above the board to drop pieces; turns alternate automatically
  between Player 1 (red) and Player 2 (yellow).
- Try filling a column completely — its drop button becomes disabled and greyed out.
- Get four in a row (any direction) to see the win banner and the **Reset the game**
  button appear.
- Check the **Game activity** panel below the board:
  - The **move history** list should show every move you made, in order.
  - Play another full game and win again with the same player — their **win streak**
    counter should increase. Win with the other player and the streak should reset.
  - **Total games played** increments each time a game ends (win or tie).
- Click **Reset the game** to start a new game — the board and move history clear,
  but the win-streak stats persist (they're stored in a singleton service shared by
  the whole app, so they reset only when you stop and restart the app).

## Notes
- The app uses the standard Blazor Web App shell: a dark sidebar (Home / Counter /
  Fetch data) and a top "About" bar, matching the layout you get from
  `dotnet new blazor`. Counter and Weather are the framework's stock demo pages,
  included so the sidebar links go somewhere — the graded work is the Connect Four
  board and the Game Activity feature on the Home page.
- The board uses the classic yellow board / red & blue pieces from the tutorial.
  Colors are just parameters on `<Board>` in `Home.razor` — easy to change.
- `Components/App.razor` links `ConnectFour.styles.css` in the `<head>`. This is
  Blazor's auto-generated bundle of every component's scoped `*.razor.css` file
  (here, `Board.razor.css`, `MainLayout.razor.css`, `NavMenu.razor.css`). It's
  produced automatically at build time — don't delete that `<link>` tag, or the
  board frame, piece colors, drop animations, and sidebar styling will disappear
  even though the page still "works".
- This was verified by building and running the project with the .NET 8 SDK,
  and by unit-testing `GameState`'s win detection (horizontal, vertical, diagonal),
  full-column error handling, and win-streak logic directly.
- Uses Bootstrap (via CDN) purely for base typography/spacing; the board itself is
  styled with the CSS from the Microsoft Learn module.
