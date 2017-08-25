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
using Xamarin.Forms;

namespace Tikects.ViewModel
{
    public class CheckTicketViewModel : User , INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        private User user;
        private ApiService apiService;
        private DialogService dialogService;
        private NavigationService navigationService;
        private bool isEnabled;
        private bool isRunning;
        private string message;
        private Color color;
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

        public string Message
        {
            set
            {
                if (message != value)
                {
                    message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
                }
            }
            get
            {
                return message;
            }
        }

        public Color Color
        {
            set
            {
                if (color != value)
                {
                    color = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color"));
                }
            }
            get
            {
                return color;
            }
        }
        #endregion

        #region Constructor
        public CheckTicketViewModel(User user) {
            this.user = user;
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;

            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            IsEnabled = true;
            IsRunning = false;
            Message = "Wait for read ticket";
            Color = Color.Gray;
        }
        #endregion

        #region Method

        #endregion
        public async void CheckTicket()
        {
            IsEnabled = false;
            IsRunning = true;
            if (string.IsNullOrEmpty(TicketCode))
            {
                IsEnabled = true;
                IsRunning = false;
                await dialogService.ShowMessage("Error", "Debe ingresar un ticket "+ UserId);
                return;
            } else if (TicketCode.Length != 4) {
                IsEnabled = true;
                IsRunning = false;
                await dialogService.ShowMessage("Error", "El ticket debe ser de 4 dígitos");
                return;
            }

            var response = await apiService.Get(
                "http://checkticketsback.azurewebsites.net",
                "/api",
                "/Tickets/",
                TicketCode);

            if (!response.IsSuccess)
            {
                if (response.Message.Equals("NotFound"))
                {
                    SaveTicket();
                    return;
                }
                IsEnabled = true;
                IsRunning = false;
                await dialogService.ShowMessage("Error", response.Message);
            }
            User u = new User();
            u = (User)response.Result;
            Message = u.TicketCode+" TICKET YA LEIDO";
            Color = Color.Red;
            IsEnabled = true;
            IsRunning = false;
            return;
        }

        public async void SaveTicket()
        {
            User u = new User();
            u.TicketCode = TicketCode;
            u.UserId = UserId;
            var response = await apiService.Post(
                "http://checkticketsback.azurewebsites.net",
                "/api",
                "/Tickets",
                u);

            if (!response.IsSuccess)
            {
                IsEnabled = true;
                IsRunning = false;
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }
            u = (User)response.Result;
            Message = u.TicketCode+ ", ACCESO AUTORIZADO";
            Color = Color.Green;
            IsEnabled = true;
            IsRunning = false;
            return;
        }

        #region Command
        //esste comando lo definio la region en el COntactsPage.xaml
        public ICommand CheckTicketCommand
        {
            get
            { return new RelayCommand(CheckTicket); }
        }
        #endregion
    }
}
