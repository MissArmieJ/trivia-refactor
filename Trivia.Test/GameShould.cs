using NSubstitute;
using NUnit.Framework;

namespace Trivia.Test
{
    [TestFixture]
    public class GameShould
    {
        const string PlayerJohn = "John";
        const string PlayerOliver = "Oliver";

        TestableGame _game;
        private ITriviaConsole _console;

        [SetUp]
        public void Setup()
        {
            _console = Substitute.For<ITriviaConsole>();
            _game = new TestableGame(_console);
        }

        [Test]
        public void add_a_player()
        {
            var playerAdded = _game.add(PlayerJohn);

            _console.Received().WriteLine(PlayerJohn + " was added");
            _console.Received().WriteLine("They are player number 1");

            Assert.That(playerAdded, Is.True);
        }

        [Test]
        public void add_2_players()
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            Received.InOrder(
                () =>
                {
                    _console.WriteLine(PlayerJohn + " was added");
                    _console.WriteLine("They are player number 1");
                    _console.WriteLine(PlayerOliver + " was added");
                    _console.WriteLine("They are player number 2");
                });
        }

        [Test]
        public void not_be_playable_if_it_has_only_1_player()
        {
            _game.add(PlayerJohn);

            Assert.That(_game.isPlayable, Is.False);
        }

        [Test]
        public void be_playable_if_it_has_2_players()
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            Assert.That(_game.isPlayable, Is.True);
        }
    }
}