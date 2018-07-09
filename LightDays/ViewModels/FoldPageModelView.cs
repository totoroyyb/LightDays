using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Days.ViewModels
{
    public class FoldPageModelView : ViewModelBase
    {
        private bool _isMottoShown;
        public bool isMottoShown
        {
            get { return _isMottoShown; }
            set
            {
                SetProperty(ref _isMottoShown, value);
            }
        }
    }
}
