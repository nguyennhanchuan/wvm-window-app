using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Models
{
    public class ProductType : INotifyPropertyChanged
    {
        private bool _isSelected;
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsSelected
        {
            get => _isSelected; set
            {
                _isSelected = value;
                OnPropertyRaised("IsSelected");
            }
        }

        public string ProductTypeImage { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
