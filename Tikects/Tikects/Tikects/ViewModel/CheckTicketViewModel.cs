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
    public class CheckTicketViewModel : User
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
            } else if (TicketCode.Length < 4) {
                IsEnabled = true;
                IsRunning = false;
                await dialogService.ShowMessage("Error", "El ticket debe ser de 4 dígitos");
                return;
            }


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
