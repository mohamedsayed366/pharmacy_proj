using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharma.Models;
using Pharma.DAL;

namespace Pharma.Controllers
{
    public class CartController : Controller
    {
        private PharmaContext Db = new PharmaContext();

        public ActionResult AddToCart(int id)
        {
            var product = Db.Products.SingleOrDefault(c => c.Id == id);
            if (product == null)
                return HttpNotFound();


            var cartInDb = Db.Carts.SingleOrDefault(c => c.Product_id == id);
            if (cartInDb != null)
            {
                return RedirectToAction("ListProducts", "Product");
            }

            else
            {
                Cart cart = new Cart();
                cart.Product_id = id;
                cart.Added_To = DateTime.Now;
                Db.Carts.Add(cart);
                Db.SaveChanges();
            }

            return RedirectToAction("HomePage", "Customer");
        }




        public ActionResult ListCart()
        {

            var cart = Db.Carts.ToList();

            return PartialView(cart);
        }





        public ActionResult Delete(int id)
        {
            var cartInDb = Db.Carts.Single(p => p.Product_id == id);

            Db.Carts.Remove(cartInDb);

            Db.SaveChanges();


            return RedirectToAction("HomePage", "Customer");

        }
    }
}