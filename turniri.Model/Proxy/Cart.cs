using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Cart
    {
        public enum OrderTypes : int
        {
            Created = 0x01,     //  Создан но не заполнен
            Prepared = 0x02,    //  Пользователь подтвердил
            Canceled = 0x03,    //  Отменена
            Accepted = 0x04,    //  Принят к исполнению (проверено менеджером)
            Delivered = 0x05    //  Доставлено (после распечатки)
        }

        public enum PaymentTypes : int
        {
            GoldMoney = 0x01, 
            Yandex = 0x02,
            Qiwi = 0x03
        }

        public string PaymentTypeStr
        {
            get
            {
                switch (PaymentType)
                {
                    case 0 :
                        return "не выбрано";
                    case (int)PaymentTypes.GoldMoney :
                        return "Золотыми Ти";
                    case (int)PaymentTypes.Yandex:
                        return "Яндекс.Деньгами";
                    case (int)PaymentTypes.Qiwi:
                        return "QIWI.Кошелек";
                }
                return string.Empty;
            }
        }

        public User Customer
        {
            get
            {
                return User1;
            }
            set
            {
                User1 = value;
            }
        }

        public User Manager
        {
            get
            {
                return User;
            }
            set
            {
                User = value;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return !CartProducts.Any();
            }
        }

        public int Count 
        {
            get
            {
                return CartProducts.Sum(p => p.Quantity);
            }
        }

        public double TotalSum
        {
            get
            {
                return CartProducts.Sum(p => p.Price * p.Quantity);
            }
        }

        public IEnumerable<CartProduct> SubCartProducts
        {
            get
            {
                return CartProducts.ToList();
            }
        }

        public bool HasPreorder
        {
            get
            {
                return CartProducts.Any(p => p.ProductPrice.Preorder);
            }
        }

        public bool NeedToProcess
        {
            get
            {
                return CartProducts.Any(p => p.Quantity > p.ProductCodes.Count() && (p.Product.Type == (int)Product.TypeEnum.Code || p.Product.Type == (int)Product.TypeEnum.RealGood));
            }
        }

        public string Description
        {
            get
            {
                var sb = new StringBuilder();

                sb.AppendLine(string.Format("#{0}", ID));
                sb.AppendLine("====================");
                foreach (var item in CartProducts)
                {
                    sb.AppendLine(string.Format("{0} x {1} {2} : {3}", item.Product.Name, item.Quantity, item.Price, item.Sum));
                }
                sb.AppendLine("====================");
                sb.AppendLine(string.Format("Итого:{0}", TotalSum));

                return sb.ToString();
            }
        }
	}
}