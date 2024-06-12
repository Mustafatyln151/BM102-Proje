using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_bmprojeui1.MVVM.Model
{
    class GroupModel
    {
        public string GroupName { get; set; }
        public string ImageSource { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }
    }
}
