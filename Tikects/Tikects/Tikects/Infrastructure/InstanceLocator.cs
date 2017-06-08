using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tikects.ViewModel;

namespace Tikects.Infrastructure
{
    public class InstanceLocator
    {
        #region Properties
        public MainViewModel Main { get; set; }
        #endregion

        #region Constructor
        public InstanceLocator()
        {
            Main = new MainViewModel();
        }
        #endregion
    }
}
