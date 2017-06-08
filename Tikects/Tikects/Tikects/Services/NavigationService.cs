using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tikects.Views;

namespace Tikects.Services
{
    public class NavigationService
    {
        public async Task Navigate(string pageName)//pagina a la cual se va a navegar
        {
            switch (pageName)
            {
                case "CheckTicketPage":
                    await App.Current.MainPage.Navigation.PushAsync(new CheckTicketPage());//agregamos la nueva pagina
                    break;
                default:
                    break;
            }
        }

        public async Task Back()//retrocedemos a la pagina anterior
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
