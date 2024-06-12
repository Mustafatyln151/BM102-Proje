using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_bmprojeui1.MVVM.Model
{
    internal class MessageModel
    {
        public string Username { get; set; }/*GUID EKLE*/
        /*public string UsernameColor { get; set; }*/
        public string ImageSource { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public bool IsNativeOrigin { get; set; }
        public bool? FirstMessage { get; set; }
    }
}
