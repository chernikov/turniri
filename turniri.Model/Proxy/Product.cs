using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{
    public partial class Product
    {
        public enum TypeEnum
        {
            Code = 0x01,
            GoldMoney = 0x02,
            RealGood = 0x03
        }

        public enum AgeRestrictType
        {
            ZeroPlus = 0x01,
            SixPlus = 0x02,
            TwelvePlus = 0x03,
            SixteenPlus = 0x04,
            Adult = 0x05
        }

        public string ImagePath189
        {
            get
            {
                return Image + "?width=189&height=268&mode=crop";
            }
        }

        public bool IsCodeTyped
        {
            get
            {
                return Type == (int)TypeEnum.Code || Type == (int)TypeEnum.RealGood;
            }
        }

        public int UnselledCodes
        {
            get
            {
                return ProductCodes.Count(p => !p.IsSelled);
            }
        }

        public int AvailableCodes
        {
            get
            {
                return ProductCodes.Count(p => !p.IsSelled && !p.IsReserved);
            }
        }

        public int TotalCodes
        {
            get
            {
                return ProductCodes.Count();
            }
        }

        public IEnumerable<Catalog> SubCatalogs
        {
            get
            {
                return ProductCatalogs.Select(p => p.Catalog);
            }
        }

        public string SubCatalogsList
        {
            get
            {
                return string.Join(", ", SubCatalogs.Select(p => p.Name));
            }
        }

        public IEnumerable<ProductPrice> SubProductPrices
        {
            get
            {
                return ProductPrices.Where(p => !p.IsDeleted).AsEnumerable();
            }
        }

        public IEnumerable<ProductImage> SubProductImages
        {
            get
            {
                return ProductImages.AsEnumerable();
            }
        }

        public IEnumerable<ProductVideo> SubProductVideos
        {
            get
            {
                return ProductVideos.AsEnumerable();
            }
        }


        public bool PreOrder
        {
            get
            {
                var item = ProductPrices.OrderBy(p => p.Price).FirstOrDefault();
                if (item != null)
                {
                    return item.Preorder;
                }
                return true;
            }
        }

        public double? OldPrice
        {
            get
            {
                var item = ProductPrices.OrderBy(p => p.Price).FirstOrDefault();
                if (item != null)
                {
                    return item.OldPrice;
                }
                return null;
            }
        }

        public double? Price
        {
            get
            {
                var item = ProductPrices.OrderBy(p => p.Price).FirstOrDefault();
                if (item != null)
                {
                    return item.Price;
                }
                return null;
            }
        }

        public IEnumerable<ProductCode> SubProductCodes
        {
            get
            {
                return ProductCodes.AsEnumerable();
            }
        }

        public IEnumerable<ProductVariation> SubProductVariations
        {
            get
            {
                return ProductVariations.AsEnumerable();
            }
        }

        public int RestOfCodes(int? ProductVariationID)
        {
            return ProductCodes.Count(p => p.ProductVariationID == ProductVariationID && !p.CartProductID.HasValue);
        }

        public string PriceStr
        {
            get
            {
                return Price.HasValue ? (Price.Value.ToString("0,0.00") + " руб.") : "не указана";
            }
        }

        public IEnumerable<SimilarProduct> BackSimilarProducts
        {
            get
            {
                return SimilarProducts1;
            }
        }

        public ProductPrice ProductPrice
        {
            get
            {

                return ProductPrices.FirstOrDefault() ?? new ProductPrice() { Product = this };
            }

        }
    }

}