using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceTwins.DeviceSimulator.UI.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private string _serialNumber;

        public string SerialNumber
        {
            get { return _serialNumber; }
            set
            {
                _serialNumber = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel()
        {
            SerialNumber = Guid.NewGuid().ToString();
        }
    }

}
