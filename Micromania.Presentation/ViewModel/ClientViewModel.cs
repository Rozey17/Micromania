﻿using Micromania.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Catel;
using Catel.MVVM;
using Catel.Collections;

namespace Micromania.Presentation.ViewModel
{
    public class ClientViewModel : ViewModelBase
    {
        public ClientViewModel()
        {
            _repository = new ClientRepository();

            BuyGameCommand = new Command(() => BuyGame(SelectedGame));
            AddMoneyCommand = new Command(() => AddMoney());
            BuyGameWithGVCommand = new Command(() => UseGiftVoucher(SelectedGame));

            Games = new ObservableCollection<Game>(new[] { Game.CBC, Game.Uncharted, Game.Uncharted2, Game.Uncharted4, Game.GOW, Game.MGS });

            MoneyAmounts = new FastObservableCollection<MoneyModel>(new[] { new MoneyModel("$ 10", Money.Ten), new MoneyModel("$ 25", Money.TwentyFive),
                new MoneyModel("$ 50", Money.Fifty), new MoneyModel("$ 100", Money.Hundred)});

            _client = Client.Ferrad;
        }

        private void BuyGame(Game game)
        {
            string error = _client.CanBuyGame(game);

            if (error != string.Empty)
            {
                NotifyClient(error);
                return;
            }

            _client.BuyGame(game);
            _repository.Save(_client);
            NotifyClient("Vous avez acheté un jeu.");
        }

        private void AddMoney()
        {
            _client.AddMoney(SelectedMoneyAmount.Value);
            _repository.Save(_client);
            NotifyClient($"Vous avez ajouté {SelectedMoneyAmount.Name.ToString()}");
        }

        private void UseGiftVoucher(Game game)
        {
            string error = _client.CanUseGiftVoucher(game);

            if (error != string.Empty)
            {
                NotifyClient(error);
                return;
            }

            _client.UseGiftVoucher(game);
            _repository.Save(_client);
            NotifyClient("Vous avez utilisé un bon d'achat.");
        }

        private void NotifyClient(string message)
        {
            Message = message;
            OnPropertyChanged(nameof(MoneyInWallet));
            OnPropertyChanged(nameof(Points));
            OnPropertyChanged(nameof(QualifyingPurchases));
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(GiftVoucher));
        }

        private readonly Client _client;

        private readonly ClientRepository _repository;

        private string _message = "";
        public string Message
        {
            get { return _message; }
            private set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public decimal MoneyInWallet => _client.MoneyInWallet;
        public string Points => _client.Points.ToString();
        public int QualifyingPurchases => _client.QualifyingPurchases;
        public string Status => _client.Status.ToString();
        public decimal GiftVoucher => _client.GiftVoucher;

        public Command BuyGameCommand { get; private set; }
        public Command AddMoneyCommand { get; private set; }
        public Command BuyGameWithGVCommand { get; private set; }
        public ObservableCollection<Game> Games { get; }
        public FastObservableCollection<MoneyModel> MoneyAmounts { get; }
        public MoneyModel SelectedMoneyAmount { get; set; }
        public Game SelectedGame { get; set; }

        public class MoneyModel
        {
            public MoneyModel(string name, Money value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; }
            public Money Value { get; }
        }
    }
}
