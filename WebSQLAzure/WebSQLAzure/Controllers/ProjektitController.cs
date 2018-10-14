using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSQLAzure.Controllers
{
    public class ProjektitController : Controller
    {
        // GET: Projektit
        public JsonResult GetList()
        {

            //Tämä malli antaa enemmän mahdollisuuksia
            ProjektittietokantaEntities entities = new ProjektittietokantaEntities();
            //List<Customer> model = entities.Customers.ToList();
            var model = (from p in entities.Projektit
                         select new
                         {
                             Projektiid = p.Projektiid,
                             Nimi = p.Nimi

                         }).ToList();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSingleProjektit(int id)
        {

            //Tämä malli antaa enemmän mahdollisuuksia
            ProjektittietokantaEntities entities = new ProjektittietokantaEntities();
            //List<Customer> model = entities.Customers.ToList();
            var model = (from p in entities.Projektit
                         where p.Projektiid == id
                         select new
                         {
                             Projektiid = p.Projektiid,
                             Nimi = p.Nimi
                         }).FirstOrDefault();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(Projektit pro)
        {
            ProjektittietokantaEntities entities = new ProjektittietokantaEntities();
            int id = pro.Projektiid;

            bool OK = false;

            if (pro.Projektiid == 0)

            {

                // kyseessä on uuden asiakkaan lisääminen, kopioidaan kentät
                Projektit dbItem = new Projektit()
                {
                    //Projektiid = p.Projektiid,
                    Nimi = pro.Nimi

                };

                // tallennus tietokantaan
                entities.Projektit.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            else
            {
                // muokkaus, haetaan id:n perusteella riviä tietokannasta

                Projektit dbItem = (from p in entities.Projektit
                                    where p.Projektiid == id
                                    select p).FirstOrDefault();

                if (dbItem != null)
                {
                    dbItem.Nimi = pro.Nimi;


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
            Projektit dbItem = (from p in entities.Projektit
                                where p.Projektiid.ToString() == id
                                select p).FirstOrDefault();

            if (dbItem != null)
            {
                // tietokannasta poisto
                entities.Projektit.Remove(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }
    }

    internal class ProjektittietokantaEntities
    {
        public ProjektittietokantaEntities()
        {
        }
    }
}