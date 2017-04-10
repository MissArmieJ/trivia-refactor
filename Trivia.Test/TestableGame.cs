using UglyTrivia;

namespace Trivia.Test
{
    public class TestableGame : Game
    {
        private readonly ITriviaConsole _console;

        public TestableGame(ITriviaConsole console)
        {
            this._console = console;
        }

        protected override void Display(string message)
        {
            _console.WriteLine(message);
        }
    }
}