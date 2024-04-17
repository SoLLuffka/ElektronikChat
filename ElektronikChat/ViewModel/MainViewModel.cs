using ElektronikChat.Core;
using ElektronikChat.Core.Net;
using ElektronikChat.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ElektronikChat.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        //relay komendy do zmiany widoku z menu glownego
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand LoginViewCommand { get; set; }
        public RelayCommand RegisterViewCommand { get; set; }
        public RelayCommand TextChatViewCommand { get; set; }
        //public RelayCommand OptionsViewCommand { get; set; }
        //public RelayCommand FlashCardsViewCommand { get; set; }
        //public RelayCommand CreateFlashCardViewCommand { get; set; }
        //public RelayCommand SchoolViewCommand { get; set; }
        public RelayCommand DisconnectViewCommand { get; set; }

        //dostepne widoki
        public HomeViewModel HomeVM { get; set; }
        public LoginViewModel LoginVM { get; set; }
        public RegisterViewModel RegisterVM { get; set; }
        public TextChatVIewModel TextChatVM { get; set; }
        //public OptionsViewModel OptionsVM { get; set; }
        //public FlashCardsViewModel FlashCardsVM { get; set; }
        //public CreateFlashCardsViewModel CreateFlashCardVM { get; set; }
        //public SchoolViewModel SchoolVM { get; set; }
        public DisconnectViewModel DisconnectVM { get; set; }

        //zabezpieczenie navBara przed zalogowaniem

        public EventHandler CurrentViewChanged;

        private object _currentView;

        // zmienne czatu

        Server _server;


        public object CurrentView
        {
            get { return _currentView; }
            set { 
                _currentView = value;
                OnPropertyChanged();
                OnCurrentViewChanged();
            }
        }

        protected virtual void OnCurrentViewChanged() //jakies gowno ktore sprawia ze to dziala
        {
            CurrentViewChanged?.Invoke(this, EventArgs.Empty);
        }

        public MainViewModel() //zmiana widoku
        {

            HomeVM = new HomeViewModel();
            LoginVM = new LoginViewModel();
            RegisterVM = new RegisterViewModel();
            TextChatVM = new TextChatVIewModel();
            //OptionsVM = new OptionsViewModel();
            //FlashCardsVM = new FlashCardsViewModel();
            //CreateFlashCardVM = new CreateFlashCardsViewModel();
            //SchoolVM = new SchoolViewModel();
            DisconnectVM = new DisconnectViewModel();

            CurrentView = LoginVM;//domyslny widok po uruchomieniu aplikacji

            HomeViewCommand = new RelayCommand(o => 
            {
                CurrentView = HomeVM;
            });
            LoginViewCommand = new RelayCommand(o =>
            {
                CurrentView = LoginVM;
                SessionManager.LeaveSession(_server.uid, LoginVM.Login);
                TextChatVIewModel.SetUsername(String.Empty);
            });
            RegisterViewCommand = new RelayCommand(o =>
            {
                CurrentView = RegisterVM;
            });
            TextChatViewCommand = new RelayCommand(o =>
            {
                CurrentView = TextChatVM;
            });
            //OptionsViewCommand = new RelayCommand(o =>
            //{
            //    CurrentView = OptionsVM;
            //});
            //FlashCardsViewCommand = new RelayCommand(o =>
            //{
            //    CurrentView = FlashCardsVM;
            //});
            //CreateFlashCardViewCommand = new RelayCommand(o =>
            //{
            //    CurrentView = CreateFlashCardVM;
            //});
            //SchoolViewCommand = new RelayCommand(o =>
            //{
            //    CurrentView = SchoolVM;
            //});
            DisconnectViewCommand = new RelayCommand(o =>
            {
                CurrentView = DisconnectVM;
            });

            // metody i zmienne czatu

            _server = Server.Instance;
            //_server.ConnectToServer2();

        }

    }
}
