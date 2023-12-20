using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Models
{
    public class Cart
    {
        private Dictionary<Guid, CartItem> CartDic { get; set; }

        public List<CartItem> CartItems {
            get { 
                var a = CartDic.Values.ToList();
                return a;
            }
            set => CartItems = value; 
        }

        public double Total
        {
            get => GetTotal();
        }

        private CartType Type;


        public Cart()
        {
            CartDic = new Dictionary<Guid, CartItem>();
        }

        public double GetTotal()
        {
            return CartDic.Values.Sum(cartItem => cartItem.GetFinalPrice());
        }

        public void AddProduct(MerchantProduct product)
        {
            if (CartDic.ContainsKey(product.Id))
            {
                CartDic[product.Id].Add();
            } else
            {
                CartDic.Add(product.Id, CartItem.FromProduct(product));
            }
        }

        public void RemoveProduct(MerchantProduct product)
        {
            if (CartDic.ContainsKey(product.Id))
            {
                CartDic[product.Id].Remove();
                if (CartDic[product.Id].Quantity == 0)
                {
                    CartDic.Remove(product.Id);
                }
            }

        }
    }

    public enum CartType
    {
        Cash,
        OnlinePayment
    }

    public class CartItem
    {
        public static CartItem FromProduct(MerchantProduct product)
        {
            var cartItem = new CartItem()
            {
                Product = product,
                Quantity = 1,
                Total = product.Price
            };
            return cartItem;
        }


        public int Quantity { get; set; }
        public double Total { get; set; }
        public MerchantProduct Product { get; set; }

        public void Add()
        {
            Quantity++;
            Total = Quantity * Product.Price;
        }   

        public void Remove()
        {
            Quantity--;
            Total = Quantity * Product.Price;

        }

        internal double GetFinalPrice()
        {
            return Product.GetFinalPrice() * Quantity;
        }
    }
}
