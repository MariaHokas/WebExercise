using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSQLAzure.Models;

namespace WebSQLAzure.Controllers
{
    public class HenkilotController : Controller
    {
        // GET: Henkilot
        public ActionResult Index()
        {
            return View();

        }
        public JsonResult GetList()
        {

            //Tämä malli antaa enemmän mahdollisuuksia
            ProjektittietokantaEntities entities = new ProjektittietokantaEntities();
            //List<Customer> model = entities.Customers.ToList();
            var model = (from h in entities.Henkilot
                         select new
                         {
                             Henkiloid = h.Henkiloid,
                             Nimi = h.Nimi,
                             Sukunimi = h.Sukunimi,
                             Katuosoite = h.Katuosoite,
                             Esimies = h.Esimies
                         }).ToList();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSingleHenkilo(string id)
        {

            //Tämä malli antaa enemmän mahdollisuuksia
            ProjektittietokantaEntities entities = new ProjektittietokantaEntities();
            //List<Customer> model = entities.Customers.ToList();
            var model = (from h in entities.Henkilot
                         where h.Henkiloid.ToString() == id
                         select new
                         {
                             Henkiloid = h.Henkiloid,
                             Nimi = h.Nimi,
                             Sukunimi = h.Sukunimi,
                             Katuosoite = h.Katuosoite,
                             Esimies = h.Esimies
                         }).FirstOrDefault();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(Henkilot henk)
        {
            ProjektittietokantaEntities entities = new ProjektittietokantaEntities();
            int id = henk.Henkiloid;

            bool OK = false;

            if (henk.Henkiloid == 0)

            {

                // kyseessä on uuden asiakkaan lisääminen, kopioidaan kentät
                Henkilot dbItem = new Henkilot()
                {
                    //Henkiloid = henk.Henkiloid,
                    Nimi = henk.Nimi,
                    Sukunimi = henk.Sukunimi,
                    Katuosoite = henk.Katuosoite,
                    Esimies = henk.Esimies

                };

                // tallennus tietokantaan
                entities.Henkilot.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            else
            {
                // muokkaus, haetaan id:n perusteella riviä tietokannasta

                Henkilot dbItem = (from h in entities.Henkilot
                                   where h.Henkiloid == id
                                   select h).FirstOrDefault();

                if (dbItem != null)
                {
                    dbItem.Nimi = henk.Nimi;
                    dbItem.Sukunimi = henk.Sukunimi;
                    dbItem.Katuosoite = henk.Katuosoite;
                    dbItem.Esimies = henk.Esimies;

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
            Henkilot dbItem = (from h in entities.Henkilot
                               where h.Henkiloid.ToString() == id
                               select h).FirstOrDefault();

            if (dbItem != null)
            {
                // tietokannasta poisto
                entities.Henkilot.Remove(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }
    }
}