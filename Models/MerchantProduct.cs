using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Models
{
    public class MerchantProduct : INotifyPropertyChanged
    {
        private int _stock;
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Stock { 
            get {
                return _stock; 
            } 
            set {
                _stock = value;
                OnPropertyRaised("Stock");
            } 
        }

        public int Col { get; set; }

        public int Row { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public string ProductImage { get; set; }

        public string ProductId { get; set; }


        public List<string> ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public virtual ProductType ProductType {get;set;}

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public double GetFinalPrice()
        {
            return Price - (Price * Discount);
        }

        public void RemoveStock(int removeQuantity)
        {
           this.Stock -= removeQuantity;
        }

        public void AddStock(int addQuantity)
        {
            this.Stock += addQuantity;
        }

        public string GetDisplayPrice()
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            return Price.ToString("#,###", cul.NumberFormat);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
   