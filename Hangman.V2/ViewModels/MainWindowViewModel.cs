using Hangman.V2.ViewModels.Commands;
using System.Collections.ObjectModel;
using System.Linq;

namespace Hangman.V2.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
		private const int POSSIBLE_GUESSES_FOR_ONE_GAME = 10;
		private const string YOU_WIN_TEXT = "You Win!";
        private const string YOU_LOSE_TEXT = "You Lose!";

        private ObservableCollection<char> guessedWord;
        private string rightWord;
        private string endResultText;
        private byte triesLeft;
        private char? guessedCharacter;
        private bool gameIsStarted;
        private bool gameIsOver;

        public MainWindowViewModel() 
		{
            this.GameIsStarted = false;
            this.GameIsOver = false;
            this.TriesLeft = POSSIBLE_GUESSES_FOR_ONE_GAME;
            this.GuessedWord = new ObservableCollection<char>();
		}

        public RelayCommand NewGameCommand => new(execution => NewGame());
        public RelayCommand StartGameCommand => new(execution => StartGame(), canExecute => CanStartGame());
        public RelayCommand AddCharacterCommand => new(execution => AddCharacter(), canExecute => CanAddCharacter());

        public ObservableCollection<char> GuessedWord
        {
            get => this.guessedWord;
            set
            {
                this.guessedWord = value;
                this.OnPropertyChanged();
            }
        }

        public string RightWord
		{
			get => this.rightWord;
			set 
			{
                this.rightWord = value;
                this.OnPropertyChanged();
			}
		}

		public string EndResultText
		{
			get => this.endResultText;
            set 
			{
                this.endResultText = value;
                this.OnPropertyChanged();
			}
		}

		public byte TriesLeft
        {
            get => this.triesLeft;
            set
            {
                this.triesLeft = value;
                this.OnPropertyChanged();
            }
        }

        public char ?GuessedCharacter
		{
			get => this.guessedCharacter;
            set
            {
                this.guessedCharacter = value;
                this.OnPropertyChanged();
			}
		}

        public bool GameIsStarted
        {
            get => this.gameIsStarted;
            set
            {
                this.gameIsStarted = value;
                this.OnPropertyChanged();
            }
        }

        public bool GameIsOver
        {
            get => this.gameIsOver;
            set
            {
                this.gameIsOver = value;
                this.OnPropertyChanged();
            }
        }

        private void AddCharacter()
        {
			bool guessed = false;
			var place = 0;

            foreach(char c in this.RightWord)
			{
				if(this.GuessedCharacter == c)
				{
                    this.GuessedWord[place] = c;
					guessed = true;
				}

				place++;
			}

			if (!guessed)
			{
                this.TriesLeft--;
			}

            this.GuessedCharacter = null;

			if(this.TriesLeft == 0)
			{
                this.EndGame();
                this.EndResultText = YOU_LOSE_TEXT;
			}
			else if(new string(this.GuessedWord.ToArray()) == this.RightWord)
			{
                this.EndGame();
                this.EndResultText = YOU_WIN_TEXT;
			}
        }

		private void EndGame()
		{
            this.GuessedWord.Clear();
            this.GameIsStarted = false;
            this.GameIsOver = true;
        }

        private bool CanAddCharacter()
        {
            return this.GuessedCharacter != null;
        }
		
		private void NewGame()
		{
            this.GameIsOver = false;
            this.RightWord = string.Empty;
            this.EndResultText = string.Empty;
        }

        private void StartGame()
        {
            this.GuessedWord.Clear();
            this.GameIsStarted = true;
            this.TriesLeft = POSSIBLE_GUESSES_FOR_ONE_GAME;

            for (int i = 0; i < this.RightWord.Length; i++)
            {
                this.GuessedWord.Add('_');
            }
        }

        private bool CanStartGame()
        {
			if (this.RightWord != null)
			{
                return this.RightWord.Length >= 2;
			}

			return false;
        }
    }
}