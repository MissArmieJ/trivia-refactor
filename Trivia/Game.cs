using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Player
    {
        public string Name { get; private set; }
    }

    public class Category
    {
        private readonly LinkedList<string> _questions = new LinkedList<string>();
        private string _name;

        public Category(string name)
        {
            _name = name;
        }

        public void GenerateQuestions()
        {
            for (var i = 0; i < 50; i++)
            {
                _questions.AddLast(_name + " Question " + i);
            }
        }
    }


    public class Game
    {
        private int _currentPlayer;
        private readonly bool[] _playerInPenaltyBox = new bool[6];
        private bool _isGettingOutOfPenaltyBox;
        private readonly int[] _playerCategories = new int[6];
        private readonly List<string> _players = new List<string>();
        private readonly int[] _playerCoins = new int[6];

        private readonly LinkedList<string> _popQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _rockQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _scienceQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _sportsQuestions = new LinkedList<string>();

        private readonly Category _popCategory = new Category("Pop");
        private readonly Category _rockCategory = new Category("Rock");
        private readonly Category _scienceCategory = new Category("Science");
        private readonly Category _sportsCategory = new Category("Sports");

        public Game()
        {
            CreateQuestions();
        }

        public bool IsPlayable() => NumberOfPlayers >= 2;

        public int NumberOfPlayers => _players.Count;

        public bool AddPlayer(string name)
        {
            _players.Add(name);
            _playerCategories[NumberOfPlayers] = 0;
            _playerCoins[NumberOfPlayers] = 0;
            _playerInPenaltyBox[NumberOfPlayers] = false;

            Display(name + " was added");
            Display("They are player number " + _players.Count);
            return true;
        }

        public void Roll(int roll)
        {
            Display(_players[_currentPlayer] + " is the current player");
            Display("They have rolled a " + roll);

            if (_playerInPenaltyBox[_currentPlayer])
            {
                if (roll % 2 == 0)
                {
                    Display(_players[_currentPlayer] + " is not getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = false;
                }
                else
                {
                    Display(_players[_currentPlayer] + " is getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = true;
                    AskNextQuestion(roll);
                }
            }
            else
            {
                AskNextQuestion(roll);
            }
        }

        private void AskNextQuestion(int roll)
        {
            UpdatePlayerPlace(roll);

            Display(_players[_currentPlayer] + "'s new location is " + _playerCategories[_currentPlayer]);
            Display("The category is " + CurrentCategory());

            AskQuestion();
        }

        private void CreateQuestions()
        {
            _popCategory.GenerateQuestions();
            _rockCategory.GenerateQuestions();
            _sportsCategory.GenerateQuestions();
            _scienceCategory.GenerateQuestions();

            for (var i = 0; i < 50; i++)
            {
                _popQuestions.AddLast("Pop Question " + i);
                _scienceQuestions.AddLast("Science Question " + i);
                _sportsQuestions.AddLast("Sports Question " + i);
                _rockQuestions.AddLast("Rock Question " + i);
            }
        }

        private void UpdatePlayerPlace(int roll)
        {
            _playerCategories[_currentPlayer] = _playerCategories[_currentPlayer] + roll;

            if (_playerCategories[_currentPlayer] > 11)
            {
                _playerCategories[_currentPlayer] = _playerCategories[_currentPlayer] - 12;
            }
        }

        private void AskQuestion()
        {
            if (CurrentCategory() == "Pop")
            {
                Display(_popQuestions.First());
                _popQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Science")
            {
                Display(_scienceQuestions.First());
                _scienceQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Sports")
            {
                Display(_sportsQuestions.First());
                _sportsQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Rock")
            {
                Display(_rockQuestions.First());
                _rockQuestions.RemoveFirst();
            }
        }

        private string CurrentCategory()
        {
            return GetCategoryFor(_playerCategories[_currentPlayer]);
        }

        private string GetCategoryFor(int i)
        {
            Dictionary<int, string> categories = new Dictionary<int, string>
            {
                {0, "Pop"},
                {1, "Science"},
                {2, "Sports"},
                {4, "Pop"},
                {5, "Science"},
                {6, "Sports"},
                {8, "Pop"},
                {9, "Science"},
                {10, "Sports"},
            };

            return categories.ContainsKey(i) ? categories[i] : "Rock";
        }

        public bool WasCorrectlyAnswered()
        {
            if (_playerInPenaltyBox[_currentPlayer])
            {
                if (_isGettingOutOfPenaltyBox)
                {
                    Display("Answer was correct!!!!");
                    _playerCoins[_currentPlayer]++;
                    Display(_players[_currentPlayer]
                            + " now has "
                            + _playerCoins[_currentPlayer]
                            + " Gold Coins.");

                    var winner = DidPlayerWin();
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count) _currentPlayer = 0;

                    return winner;
                }

                _currentPlayer++;
                if (_currentPlayer == _players.Count) _currentPlayer = 0;
                return true;
            }
            {
                Display("Answer was correct!!!!");
                _playerCoins[_currentPlayer]++;
                Display(_players[_currentPlayer]
                        + " now has "
                        + _playerCoins[_currentPlayer]
                        + " Gold Coins.");

                var winner = DidPlayerWin();
                _currentPlayer++;
                if (_currentPlayer == _players.Count) _currentPlayer = 0;

                return winner;
            }
        }

        public bool WrongAnswer()
        {
            Display("Question was incorrectly answered");
            Display(_players[_currentPlayer] + " was sent to the penalty box");
            _playerInPenaltyBox[_currentPlayer] = true;

            _currentPlayer++;
            if (_currentPlayer == _players.Count) _currentPlayer = 0;
            return true;
        }

        private bool DidPlayerWin()
        {
            return _playerCoins[_currentPlayer] != 6;
        }

        protected virtual void Display(string message)
        {
            Console.WriteLine(message);
        }
    }
}