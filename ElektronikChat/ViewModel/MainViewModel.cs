using ElektronikChat.Core;
using ElektronikChat.Core.Net;
using ElektronikChat.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
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

        //dostepne widoki
        public HomeViewModel HomeVM { get; set; }
        public LoginViewModel LoginVM { get; set; }
        public RegisterViewModel RegisterVM { get; set; }
        public TextChatVIewModel TextChatVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel() //zmiana widoku
        {
            HomeVM = new HomeViewModel();
            LoginVM = new LoginViewModel();
            RegisterVM = new RegisterViewModel();
            TextChatVM = new TextChatVIewModel();

            CurrentView = RegisterVM;//domyslny widok po uruchomieniu aplikacji

            HomeViewCommand = new RelayCommand(o => 
            {
                CurrentView = HomeVM;
            });
            LoginViewCommand = new RelayCommand(o =>
            {
                CurrentView = LoginVM;
            });
            RegisterViewCommand = new RelayCommand(o =>
            {
                CurrentView = RegisterVM;
            });
            TextChatViewCommand = new RelayCommand(o =>
            {
                CurrentView = TextChatVM;
            });
        }
    }
}
