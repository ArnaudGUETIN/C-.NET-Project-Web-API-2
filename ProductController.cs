using ClassLibrary2;
using CodeMetier.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebAppCegedimNext.Controllers
{
    public class ProductController : ApiController 
    {
        //POST:api/Product
        [ResponseType(typeof(OrderSet))]
        public IHttpActionResult Post(Product P)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            GenericImplement<Product> _products = new GenericImplement<Product>();
            _products.Insert(P);
            return CreatedAtRoute("DefaultApi", new { id = P.IdProduct }, _products);


        }
        //GET:api/Product
        public IEnumerable<Product> GetAll()
        {
            GenericImplement<Product> _GenericMethod = new GenericImplement<Product>();
            IEnumerable<Product> products = _GenericMethod.GetAll();
            return products;
        }

        [ResponseType(typeof(OrderSet))]
        public IHttpActionResult GetProductById(int Id)
        {
            GenericImplement<Product> _GenericMethod = new GenericImplement<Product>();
            Product _product= _GenericMethod.GetSingleById(Id);
            return Ok(_product);
        }

    }
}
