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
        public void add_a_player_()
        {
            _game.add(PlayerJohn);

            _console.Received().WriteLine(PlayerJohn + " was added");
            _console.Received().WriteLine("They are player number 1");
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

        [Test]
        public void on_a_roll_display_current_player_name()
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(3);

            _console.Received().WriteLine(PlayerJohn + " is the current player");
        }

        [Test]
        public void on_a_roll_display_roll_number()
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(3);

            _console.Received().WriteLine("They have rolled a 3");
        }

        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(11, 11)]
        public void on_a_roll_displayed_current_player_location_is_the_roll_when_roll_is_less_then_twelve(int roll, int expectedLocation)
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(roll);

            _console.Received().WriteLine(PlayerJohn + "'s new location is " + expectedLocation);
        }

        [TestCase(1, 1)]
        [TestCase(3, 3)]
        [TestCase(11, 11)]
        public void on_second_roll_after_wrong_answer_when_roll_is_odd_current_player_location_is_the_roll_when_roll_is_less_then_twelve(int roll, int expectedLocation)
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(roll);
            _game.wrongAnswer();
            _game.roll(roll);
            _game.wrongAnswer();
            _game.roll(roll);

            _console.Received().WriteLine(PlayerJohn + "'s new location is " + expectedLocation);
        }

        [TestCase(12, 0)]
        [TestCase(13, 1)]
        [TestCase(14, 2)]
        [TestCase(24, 12)]
        public void on_a_roll_above_eleven_displayed_current_player_location_is_the_roll_minus_12(int roll, int expectedLocation)
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(roll);

            _console.Received().WriteLine(PlayerJohn + "'s new location is " + expectedLocation);
        }

        [TestCase(13, 1)]
        [TestCase(25, 13)]
        public void on_second_roll_after_wrong_answer_when_roll_is_odd_and_above_eleven_current_player_location_is_theroll_minus_12(int roll, int expectedLocation)
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(roll);
            _game.wrongAnswer();
            _game.roll(roll);
            _game.wrongAnswer();
            _game.roll(roll);

            _console.Received().WriteLine(PlayerJohn + "'s new location is " + expectedLocation);
        }

        [TestCase(0, "Pop")]
        [TestCase(1, "Science")]
        [TestCase(2, "Sports")]
        [TestCase(3, "Rock")]
        [TestCase(4, "Pop")]
        [TestCase(5, "Science")]
        [TestCase(6, "Sports")]
        [TestCase(7, "Rock")]
        [TestCase(8, "Pop")]
        [TestCase(9, "Science")]
        [TestCase(10, "Sports")]
        [TestCase(11, "Rock")]
        [TestCase(12, "Pop")]
        public void on_a_roll_display_category(int roll, string expectedCategory)
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(roll);

            _console.Received().WriteLine("The category is " + expectedCategory);
        }

        [TestCase(0, "Pop")]
        [TestCase(1, "Science")]
        [TestCase(2, "Sports")]
        [TestCase(3, "Rock")]
        [TestCase(4, "Pop")]
        [TestCase(5, "Science")]
        [TestCase(6, "Sports")]
        [TestCase(7, "Rock")]
        [TestCase(8, "Pop")]
        [TestCase(9, "Science")]
        [TestCase(10, "Sports")]
        [TestCase(11, "Rock")]
        [TestCase(12, "Pop")]
        public void on_first_roll_display_question_for_category(int roll, string expectedCategoryOnFirstRoll)
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(roll);

            _console.Received().WriteLine(expectedCategoryOnFirstRoll + " Question 0");
        }


        [TestCase(0, "Pop", "Pop")]
        [TestCase(1, "Science", "Sports")]
        [TestCase(2, "Sports", "Pop")]
        [TestCase(3, "Rock", "Sports")]
        [TestCase(4, "Pop", "Pop")]
        [TestCase(5, "Science", "Sports")]
        [TestCase(6, "Sports", "Pop")]
        [TestCase(7, "Rock", "Sports")]
        [TestCase(8, "Pop", "Pop")]
        [TestCase(9, "Science", "Sports")]
        [TestCase(10, "Sports", "Pop")]
        [TestCase(11, "Rock", "Sports")]
        [TestCase(12, "Pop", "Pop")]
        public void on_second_roll_display_question_for_new_category(int roll, string expectedCategoryOnFirstRoll, string expectedCategoryOnSecondRoll)
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(roll);
            _game.roll(roll);

            _console.Received().WriteLine(expectedCategoryOnFirstRoll + " Question 0");
            _console.Received().WriteLine(expectedCategoryOnSecondRoll + " Question 0");
        }

        [Test]
        public void on_wrong_answer_display_wrong_answer_and_penalty_box_messages()
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(1);
            _game.wrongAnswer();

            _console.Received().WriteLine("Question was incorrectly answered");
            _console.Received().WriteLine(PlayerJohn +" was sent to the penalty box");
        }

        [Test]
        public void on_wrong_answer_change_current_player()
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(1);
            _game.wrongAnswer();
            _game.roll(1);

            Received.InOrder(
             () =>
             {
                 _console.WriteLine(PlayerJohn + " is the current player");
                 _console.WriteLine(PlayerOliver + " is the current player");
                });
        }

        [Test]
        public void on_second_roll_after_wrong_answer_when_roll_is_even_player_stays_in_penalty_box()
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(1);
            _game.wrongAnswer();
            _game.roll(1);
            _game.wrongAnswer();
            _game.roll(2);

            _console.Received().WriteLine(PlayerJohn + " is not getting out of the penalty box");
        }

        [Test]
        public void on_second_roll_after_wrong_answer_when_roll_is_odd_player_leaves_penalty_box()
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(1);
            _game.wrongAnswer();
            _game.roll(1);
            _game.wrongAnswer();
            _game.roll(1);

            _console.Received().WriteLine(PlayerJohn + " is getting out of the penalty box");
        }

        [Test]
        public void on_correct_answer_display_correct_answer_messages()
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(1);
            _game.wasCorrectlyAnswered();

            _console.Received().WriteLine("Answer was correct!!!!");
        }

        [Test]
        public void on_correct_answer_display_gold_coins()
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(1);
            _game.wasCorrectlyAnswered();

            _console.Received().WriteLine(PlayerJohn+ " now has 1 Gold Coins.");
        }

        [Test]
        public void on_second_correct_answer_display_increased_gold_coins()
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(1);
            _game.wasCorrectlyAnswered();
            _game.roll(1);
            _game.wrongAnswer();
            _game.roll(1);
            _game.wasCorrectlyAnswered();

            _console.Received().WriteLine(PlayerJohn + " now has 2 Gold Coins.");
        }

        [Test]
        public void on_a_correct_answer_after_previous_wrong_when_roll_is_odd_display_answer_correct_message()
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(1);
            _game.wrongAnswer();
            _game.roll(1);
            _game.wrongAnswer();
            _game.roll(1);
            _game.wasCorrectlyAnswered();

            _console.Received().WriteLine("Answer was correct!!!!");
        }

        [Test]
        public void on_a_correct_answer_after_previous_wrong_when_roll_is_even_does_not_display_answer_correct_message()
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(1);
            _game.wrongAnswer();
            _game.roll(1);
            _game.wrongAnswer();
            _game.roll(2);
            _game.wasCorrectlyAnswered();

            _console.DidNotReceive().WriteLine("Answer was correct!!!!");
        }

        [Test]
        public void answer_switches_players()
        {
            _game.add(PlayerJohn);
            _game.add(PlayerOliver);

            _game.roll(1);
            _console.Received().WriteLine(PlayerJohn + " is the current player");

            _game.wasCorrectlyAnswered();
            _game.roll(1);
            _console.Received().WriteLine(PlayerOliver + " is the current player");

            _game.wrongAnswer();
            _game.roll(2);
            _console.Received().WriteLine(PlayerJohn + " is the current player");

            _game.wasCorrectlyAnswered();
            _game.roll(2);
            _console.Received().WriteLine(PlayerOliver + " is the current player");

            _game.wasCorrectlyAnswered();
            _game.roll(2);
            _console.Received().WriteLine(PlayerJohn + " is the current player");
        }
    }
}