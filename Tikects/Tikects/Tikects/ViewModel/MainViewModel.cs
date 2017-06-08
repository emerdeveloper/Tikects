using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tikects.ViewModel
{
    public class MainViewModel
    {
        
        #region Properties
        public LoginViewModel Users { get; set; }

        public CheckTicketViewModel CheckTicket { get; set; }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            instance = this;

            Users = new LoginViewModel();
        }
        #endregion

        #region Singleton
        static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }
        #endregion
    }
}
