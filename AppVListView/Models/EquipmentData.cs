using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppVListView.Models
{
    public class EquipmentData : INotifyPropertyChanged
    {
        private string _equipmentId;
        private string _equipmentName;
        private string _status;
        private double _temperature;
        private double _pressure;
        private double _vibration;
        private DateTime _timestamp;
        private string _location;

        public string EquipmentId
        {
            get => _equipmentId;
            set { _equipmentId = value; OnPropertyChanged(nameof(EquipmentId)); }
        }

        public string EquipmentName
        {
            get => _equipmentName;
            set { _equipmentName = value; OnPropertyChanged(nameof(EquipmentName)); }
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        public double Temperature
        {
            get => _temperature;
            set { _temperature = value; OnPropertyChanged(nameof(Temperature)); }
        }

        public double Pressure
        {
            get => _pressure;
            set { _pressure = value; OnPropertyChanged(nameof(Pressure)); }
        }

        public double Vibration
        {
            get => _vibration;
            set { _vibration = value; OnPropertyChanged(nameof(Vibration)); }
        }

        public DateTime Timestamp
        {
            get => _timestamp;
            set { _timestamp = value; OnPropertyChanged(nameof(Timestamp)); }
        }

        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(nameof(Location)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
