using ClassLibrary2;
using CodeMetier.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMetier.Treatments
{
    public class OrderTreatment : GenericImplement<OrderSet>
    {
        private OrderEntities db = new OrderEntities();

        public bool Validation(ref OrderSet order, ref List<Product> Products)
        {
            List<Product> _products = new List<Product>();
            

            //List<_orderLineSet> __orderLines = new List<_orderLineSet>();
            
            bool _valid = false;


            //foreach (var _itemO in order._orderLineSet)
            //{
            //    _products.Add(_itemO.Product);
            //}

            foreach (var _item in order.OrderLineSet)
            {
                if ((db.Product.Where(u => u.Barcode == _item.Product.Barcode && u.PctCode == _item.Product.PctCode && u.Name == _item.Product.Name)).Count()!=0 && _item.Quantity!=0)
                {
                    _valid = true;

                }
                else if ((db.Product.Where(u => u.Barcode == _item.Product.Barcode || (u.PctCode == _item.Product.PctCode && u.Name == _item.Product.Name))).Count()!= 0 && _item.Quantity != 0)
                {
                    _valid = true;
                    Product _p = db.Product.Where(u => u.Barcode == _item.Product.Barcode || (u.PctCode == _item.Product.PctCode && u.Name == _item.Product.Name)).FirstOrDefault();
                    _item.Product.Name = _p.Name;
                    _item.Product.PctCode = _p.PctCode;
                    _item.Product.Barcode = _p.Barcode;

                }
                else if ((db.Product.Where(u => u.PctCode == _item.Product.PctCode || (u.Barcode == _item.Product.Barcode && u.Name == _item.Product.Name))).Count()!= 0 && _item.Quantity != 0)
                {
                    _valid = true;
                    Product _p = db.Product.Where(u => u.Barcode == _item.Product.Barcode || (u.PctCode == _item.Product.PctCode && u.Name == _item.Product.Name)).FirstOrDefault();
                    _item.Product.Name = _p.Name;
                    _item.Product.PctCode = _p.PctCode;
                    _item.Product.Barcode = _p.Barcode;

                }
                else if ((db.Product.Where(u => u.Name == _item.Product.Name || (u.Barcode == _item.Product.Barcode && u.PctCode == _item.Product.PctCode))).Count() != 0 && _item.Quantity != 0)
                {
                    _valid = true;
                    Product _p = db.Product.Where(u => u.Barcode == _item.Product.Barcode || (u.PctCode == _item.Product.PctCode && u.Name == _item.Product.Name)).FirstOrDefault();
                    _item.Product.Name = _p.Name;
                    _item.Product.PctCode = _p.PctCode;
                    _item.Product.Barcode = _p.Barcode;

                }
                else
                {
                    _item.Status = "No match found";
                    Products.Add(_item.Product);
                }
            }

        
            return _valid;
        }
        public bool OrderStatus(List<Product> products,ref string message)
        {
            if (products.Count() == 0)
            {
                message = "Certains produits n'ont pas pu être identifié";
                return false;
            }
            else
            {
                message = "Commande traité avec succès";
                return true;
            }
           
        }
        public bool Invoice(OrderSet order)
        {
            try
            {
                InvoiceSet _invoice = new InvoiceSet();
                GenericImplement<InvoiceSet> _GenericImplement = new GenericImplement<InvoiceSet>();
                _invoice.OrderSet = db.OrderSet.Find(order.IdOrder);
                _invoice.InvoiceNumber = db.InvoiceSet.Count() + 1;
                _invoice.InvoiceDate = DateTime.Now;
                _invoice.HTAmount = order.HTAmount;
                _invoice.TTCAmount = order.TTCAmount;
                _invoice.TotalOfLines = (short)_invoice.InvoiceLineSet.Count();
                _invoice.Order_Id = order.IdOrder;
                _GenericImplement.Insert(_invoice);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
        }

       
    }
}
