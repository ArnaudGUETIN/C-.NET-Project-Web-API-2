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
using Newtonsoft.Json.Linq;
using System.Web.Http.Results;
using System.Text;

namespace WebAppCegedimNext.Controllers
{
    public class OrderJsonController : ApiController
    {
        private OrderEntities db = new OrderEntities();
        //private PharmaEntities db1= new PharmaEntities();
        OrderSet order = new OrderSet();
        
        // GET: api/OrderJson
        public List<OrderSet> GetOrder()
        {

            GenericImplement<OrderSet> _GenericMethod = new GenericImplement<OrderSet>();
            return _GenericMethod.GetAll().ToList();

        }

        // GET: api/OrderJson/5
        [ResponseType(typeof(OrderSet))]
        public IHttpActionResult GetOrder(int id)
        {
            GenericImplement<OrderSet> _GenericMethod = new GenericImplement<OrderSet>();
            OrderSet _orderSet = _GenericMethod.Find(id);
            if (_orderSet == null)
            {
                return NotFound();
            }

            return Ok(_orderSet);
        }

        


        // GET: api/OrderJson?_IdC=value
        [ResponseType(typeof(OrderSet))]
        public IHttpActionResult GetCreateOrder(string IdC)
        {
            int id = int.Parse(IdC);
            Customer custumer = db.Customer.Find(id);
            GenericImplement<OrderSet> _GenericMethod = new GenericImplement<OrderSet>();
            if (custumer == null)
            {
                return NotFound();
            }
            OrderSet order = new OrderSet();

          
            _GenericMethod.Insert(order);
            return Ok(order);
        }



        // GET: api/OrderJson?_IdO=value
        [ResponseType(typeof(OrderLineSet))]
        public IHttpActionResult GetOrderLinesOfOrder(string _IdO)
        {
            int _ido = int.Parse(_IdO);
            OrderSet order = db.OrderSet.Find(_ido);

            GenericImplement<OrderLineSet> _GenericMethod = new GenericImplement<OrderLineSet>();
            if (order == null)
            {

                return NotFound();
            }

            List<OrderLineSet> _orderLine = new List<OrderLineSet>();

            _orderLine = db.OrderLineSet.Where(u => u.Order_Id == _ido).ToList();
            return Ok(_orderLine);
        }



        // GET: api/OrderJson?_IdOL=value1
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProductOfOrder(string _IdOL)
        {
            int _idol = int.Parse(_IdOL);
            
            OrderLineSet _orderL = db.OrderLineSet.Find(_idol);
            List<Product> _products = new List<Product>();

            GenericImplement<OrderLineSet> _GenericMethod = new GenericImplement<OrderLineSet>();
            if (_orderL == null)
            {

                return NotFound();
            }

            _products = db.Product.Where(u => u.OrderLine_Id == _idol).ToList();
            return Ok(_products);
        }



        // PUT: api/OrderJson/
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder([FromBody] JObject Response)
        {
            OrderSet orderSet = new OrderSet();
            orderSet = Response.ToObject<OrderSet>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           

            db.OrderSet.Add(orderSet);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                
                    throw;
               
            }

            return StatusCode(HttpStatusCode.NoContent);
        }








        // POST: api/OrderJson
        public IHttpActionResult PostOrder([FromBody] JObject Response)
        {
            OrderTreatment _treatment = new OrderTreatment();
            OrderSet orderSet         = new OrderSet();
            orderSet                  = Response.ToObject<OrderSet>();
            List<Product> Products = new List<Product>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_treatment.Validation(ref orderSet,ref Products))
            {
                orderSet.Note = "Commande enregistrée";
                //_treatment.Insert(orderSet);
                //Products.Add(db.Product.FirstOrDefault());
            }
            else 
            {
                orderSet.Note = "Certains élements de la commande ne sont pas identifiables";


            }



            if (Products.Count()==0 || Products==null)
            {
                _treatment.Invoice(orderSet);
                
                return Ok( Products);
            }
            else
            {
                orderSet.Note = "Ces produits n'ont pas été trouvés";
                 return CreatedAtRoute("DefaultApi", new { id = orderSet.IdOrder }, order);
            }
            
        }


        // POST: api/OrderJson?x=value
        //[ResponseType(typeof(OrderSet))]
        public IHttpActionResult PostSupplierResponse([FromBody] JObject Response,string x)
        {
            OrderTreatment _treatment = new OrderTreatment();
            OrderSet       _orderSet  = new OrderSet();
            List<Product>  _products  = new List<Product>();
            string         _Message   = "";

            _orderSet = Response["Order"].ToObject<OrderSet>();
            _products = Response["Products"].ToObject<List<Product>>();
            _Message  = Response["Message"].ToObject<string>();


            if(_treatment.OrderStatus(_products,ref _Message))
            {
                _treatment.Invoice(_orderSet);
            }


            return CreatedAtRoute("DefaultApi", new { id = _orderSet.IdOrder }, Response);
        }





        // DELETE: api/OrderJson/5
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

            return Ok(orderSet);
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