using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
//using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ClassLibrary2;
using CodeMetier.Implementation;
using CodeMetier.Treatments;
using System.Net.Http;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace WebAppCegedimNext.Controllers
{
    public class OrderXmlController : ApiController
    {
        private OrderEntities db = new OrderEntities();
        public OrderXmlController()
        {
            var xml = GlobalConfiguration.Configuration.Formatters.XmlFormatter;
    xml.UseXmlSerializer = true;
        }

        // GET: api/OrderXml
        public IHttpActionResult GetOrder()
        {
            OrderSet _Order = new OrderSet();
            
            
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(_Order.GetType());
            var _StringWriter = new StringWriter();
            XmlWriter _Writer = XmlWriter.Create(_StringWriter);
            x.Serialize(_Writer, _Order);
            string _Xml = _StringWriter.ToString();
            
            return Ok(_Xml);

        }

        // GET: api/OrderXml/5
        [ResponseType(typeof(OrderSet))]
        public IHttpActionResult GetOrder(int id)
        {
            OrderSet _Order = db.OrderSet.Find(id);
            if (_Order == null)
            {
                return NotFound();
            }
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(_Order.GetType());
            var _StringWriter = new StringWriter();
            XmlWriter _Writer = XmlWriter.Create(_StringWriter);
            x.Serialize(_Writer, _Order);
            string _Xml = _StringWriter.ToString();
            return Ok(_Xml);


        }


        // GET: api/Order?_IdC=value
        [ResponseType(typeof(OrderSet))]
        public IHttpActionResult GetCreateOrder(string _IdC)
        {
            int _id = int.Parse(_IdC);
            Customer custumer = db.Customer.Find(_id);
            GenericImplement<OrderSet> _GenericMethod = new GenericImplement<OrderSet>();
            if (custumer == null)
            {
                return NotFound();
            }
            OrderSet _Order = new OrderSet();
            _GenericMethod.Insert(_Order);

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(_Order.GetType());
            var _StringWriter = new StringWriter();
            XmlWriter _Writer = XmlWriter.Create(_StringWriter);
            x.Serialize(_Writer, _Order);
            string _Xml = _StringWriter.ToString();
            return Ok(_Xml);

        }



        // GET: api/OrderXml?_IdO=value
        [ResponseType(typeof(OrderLineSet))]
        public IHttpActionResult GetCreate_orderLine(string _IdO)
        {
            int _ido = int.Parse(_IdO);
            OrderSet order = db.OrderSet.Find(_ido);

            GenericImplement<OrderLineSet> _GenericMethod = new GenericImplement<OrderLineSet>();
            if (order == null)
            {

                return NotFound();
            }

            OrderLineSet _OrderLine = new OrderLineSet();
          
            _GenericMethod.Insert(_OrderLine);

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(_OrderLine.GetType());
            var _StringWriter = new StringWriter();
            XmlWriter _Writer = XmlWriter.Create(_StringWriter);
            x.Serialize(_Writer, _OrderLine);
            string _Xml = _StringWriter.ToString();
            return Ok(_Xml);

        }



        // GET: api/OrderXml?_IdOL=value1&_IdP=value2
        [ResponseType(typeof(OrderLineSet))]
        public IHttpActionResult GetAddProduct(string _IdOL, string _IdP)
        {
            int _idol = int.Parse(_IdOL);
            int _idp = int.Parse(_IdP);
            OrderLineSet orderL = db.OrderLineSet.Find(_idol);
            Product product = db.Product.Find(_idp);

            GenericImplement<OrderLineSet> _GenericMethod = new GenericImplement<OrderLineSet>();
            if (orderL == null)
            {

                return NotFound();
            }

            if (orderL.Product == product)
            {
                orderL.Quantity = orderL.Quantity + 1;
                _GenericMethod.Update(orderL);
                _GenericMethod.SaveAll();
            }
            else
            {
                OrderLineSet _orderLine = (OrderLineSet)GetCreate_orderLine("" + orderL.Order_Id);
                _orderLine.Quantity = 1;
                _orderLine.Product = product;
                _GenericMethod.SaveAll();
                System.Xml.Serialization.XmlSerializer x1 = new System.Xml.Serialization.XmlSerializer(_orderLine.GetType());
                var _StringWriter1 = new StringWriter();
                XmlWriter _Writer1 = XmlWriter.Create(_StringWriter1);
                x1.Serialize(_Writer1, _orderLine);
                string _Xml1 = _StringWriter1.ToString();
                return Ok(_Xml1);

            }
            System.Xml.Serialization.XmlSerializer x2 = new System.Xml.Serialization.XmlSerializer(orderL.GetType());
            var _StringWriter2 = new StringWriter();
            XmlWriter _Writer2 = XmlWriter.Create(_StringWriter2);
            x2.Serialize(_Writer2, orderL);
            string _Xml2 = _StringWriter2.ToString();
            return Ok(_Xml2);
        }



        // PUT: api/OrderXml/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, OrderSet orderSet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderSet.IdOrder)
            {
                return BadRequest();
            }

            db.Entry(orderSet).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderSetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }








        // POST: api/OrderXml
        [ResponseType(typeof(OrderSet))]
        public IHttpActionResult PostOrder([FromBody] OrderSet orderSet)
        {
           // OrderSet orderSet = new OrderSet();
            OrderTreatment _treatment = new OrderTreatment();
            List<Product> Products = new List<Product>();
            //orderSet =request;
            foreach (var _item in db.OrderLineSet)
            {
                orderSet.OrderLineSet.Add(_item);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_treatment.Validation(ref orderSet, ref Products))
            {
                _treatment.Insert(orderSet);
            }
            else
            {

            }
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(orderSet.GetType());
            var _StringWriter = new StringWriter();
            XmlWriter _Writer = XmlWriter.Create(_StringWriter);
            x.Serialize(_Writer, orderSet);
            string _Xml = _StringWriter.ToString();
            



            return CreatedAtRoute("DefaultApi", new { id = orderSet.IdOrder }, _Xml);
            
        }





        // DELETE: api/OrderXml/5
        [ResponseType(typeof(OrderSet))]
        public IHttpActionResult DeleteOrder(int id)
        {
            OrderSet orderSet = db.OrderSet.Find(id);
            if (orderSet == null)
            {
                return NotFound();
            }

            db.OrderSet.Remove(orderSet);
            db.SaveChanges();

            
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<OrderSet>(orderSet,
              new System.Net.Http.Formatting.XmlMediaTypeFormatter
              {
                  UseXmlSerializer = true
              })
            });

        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        private bool OrderSetExists(int id)
        {
            return db.OrderSet.Count(e => e.IdOrder == id) > 0;
        }




    }
}