using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSQLAzure.Models;

namespace WebSQLAzure.Controllers
{
    public class TunnitController : Controller
    {
        // GET: Tunnit
        public ActionResult Index()
        {
            return View();

        }
        public JsonResult GetList()
        {

            //Tämä malli antaa enemmän mahdollisuuksia
            ProjektittietokantaEntities entities = new ProjektittietokantaEntities();
            //List<Customer> model = entities.Customers.ToList();
            var model = (from t in entities.Tunnit
                         select new
                         {
                             Tunnitid = t.Tunnitid,
                             Projektiid = t.Projektiid,
                             Henkiloid = t.Henkiloid,
                             Pvm = t.Pvm,
                             Tunnit1 = t.Tunnit1
                         }).ToList();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSingleTunnit(string id)
        {

            //Tämä malli antaa enemmän mahdollisuuksia
            ProjektittietokantaEntities entities = new ProjektittietokantaEntities();
            //List<Customer> model = entities.Customers.ToList();
            var model = (from t in entities.Tunnit
                         where t.Tunnitid.ToString() == id
                         select new
                         {
                             Tunnitid = t.Tunnitid,
                             Projektiid = t.Projektiid,
                             Henkiloid = t.Henkiloid,
                             Pvm = t.Pvm,
                             Tunnit1 = t.Tunnit1
                         }).FirstOrDefault();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(Tunnit tunn)
        {
            ProjektittietokantaEntities entities = new ProjektittietokantaEntities();

            int id = tunn.Tunnitid;

            bool OK = false;

            if (tunn.Tunnitid == 0)

            {

                // kyseessä on uuden asiakkaan lisääminen, kopioidaan kentät
                Tunnit dbItem = new Tunnit()
                {
                    //Tunnitid = tunn.Tunnitid,
                    Projektiid = tunn.Projektiid,
                    Henkiloid = tunn.Henkiloid,
                    Pvm = tunn.Pvm,
                    Tunnit1 = tunn.Tunnit1

                };

                // tallennus tietokantaan
                entities.Tunnit.Add(dbItem);
                entities.SaveChanges();
                OK = true;

            }
            else
            {
                // muokkaus, haetaan id:n perusteella riviä tietokannasta

                Tunnit dbItem = (from t in entities.Tunnit
                                 where t.Tunnitid == id
                                 select t).FirstOrDefault();

                if (dbItem != null)
                {
                    dbItem.Projektiid = tunn.Projektiid;
                    dbItem.Henkiloid = tunn.Henkiloid;
                    dbItem.Pvm = tunn.Pvm;
                    dbItem.Tunnit1 = tunn.Tunnit1;

                    // tallennus tietokantaan
                    entities.SaveChanges();
                    OK = true;
                }
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Delete(string id)
        {
            ProjektittietokantaEntities entities = new ProjektittietokantaEntities();


            bool OK = false;
            Tunnit dbItem = (from t in entities.Tunnit
                             where t.Tunnitid.ToString() == id
                             select t).FirstOrDefault();

            if (dbItem != null)
            {
                // tietokannasta poisto
                entities.Tunnit.Remove(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }
    }
}