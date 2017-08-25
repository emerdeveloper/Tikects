using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tikects.Models;
using Tikects.Services;
using UIKit;

namespace Tikects.ViewModel
{
    public class LoginViewModel : User, INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties

        #endregion

        #region Attributes
        private ApiService apiService;
        private DialogService dialogService;
        private NavigationService navigationService;
        private bool isEnabled;
        private bool isRunning;
        #endregion

        #region Properties
        public bool IsEnabled
        {
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabled"));
                }
            }
            get
            {
                return isEnabled;
            }
        }

        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get
            {
                return isRunning;
            }
        }

        #endregion

        #region Constructor
        public LoginViewModel() {
            //instance = this;
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            IsEnabled = true;
            IsRunning = false;
            //NavigationController.NavigationBar.TintColor = UIColor;
        }
        #endregion

       /*#region Singleton
        static LoginViewModel instance;

        public static LoginViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new LoginViewModel();
            }

            return instance;
        }
        #endregion*/

        #region Methods
        public async void Login()
        {
            IsEnabled = false;
            IsRunning = true;
            if (string.IsNullOrEmpty(Email))
            {
                IsEnabled = true;
                IsRunning = false;
                await dialogService.ShowMessage("Error", "El email es obligatorio");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                IsEnabled = true;
                IsRunning = false;
                await dialogService.ShowMessage("Error", "La contraseña es obligatoria");
                return;
            }
            User u = new User();
            u.Email = Email;
            u.Password = Password;
            u.UserId = null;
            var response = await apiService.Post(
                "http://checkticketsback.azurewebsites.net",
                "/api",
                "/Users/Login",
                u);

            if (!response.IsSuccess)
            {
                IsEnabled = true;
                IsRunning = false;
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }
            //await dialogService.ShowConfirm("Aceptar", ""+response.Result);
            //User u = new User();
            u = (User)response.Result;
           /* new User {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
            };*/
            var maninViewModel = MainViewModel.GetInstance();
            maninViewModel.CheckTicket = new CheckTicketViewModel(u);
            await navigationService.Navigate("CheckTicketPage");
            IsRunning = false;
            IsEnabled = true;
        }
        #endregion

        #region Command
        //esste comando lo definio la region en el COntactsPage.xaml
        public ICommand LoginCommand
        {
            get
            { return new RelayCommand(Login); }
        }
        #endregion
    }
}
