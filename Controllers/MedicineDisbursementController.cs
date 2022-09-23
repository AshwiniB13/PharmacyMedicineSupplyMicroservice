using CSPharmacyMedicineSupplyManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PharmacyMedicineSupplyMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CSPharmacyMedicineSupplyMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineDisbursementController : ControllerBase
    {
        private readonly PharmacyMedicineSupplyManagementContext db;

        public MedicineDisbursementController(PharmacyMedicineSupplyManagementContext _db)
        {
            db = _db;
        }



        [HttpPost]
        [Route("PharmaSupply")]
        public async Task<List<PharmacyMedicineSupply>> PharmacySupply(List<MedicineDemand> demandList)
        {
            List<PharmacyMedicineSupply> returnPharmacyList = new List<PharmacyMedicineSupply>();

            string Baseurl = "https://localhost:44318/";
            List<MedicineStock> medicineStockList = new List<MedicineStock>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/MedicineStock");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ProductResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    medicineStockList = JsonConvert.DeserializeObject<List<MedicineStock>>(ProductResponse);

                    List<PharmacyMedicineSupply> pharmacyList = new List<PharmacyMedicineSupply>();
                    pharmacyList = db.PharmacyMedicineSupplies.ToList();

                    foreach (var item in medicineStockList)
                    {
                        foreach (var medicineItem in demandList)
                        {
                            if (medicineItem.Medicine == item.MedicineName && medicineItem.DemandCount < item.NumberOfTabletsInStock)
                            {
                                var returnList=pharmacyList.Where(s => s.MedicineName == item.MedicineName).ToList();
                                returnPharmacyList.AddRange(returnList);
                            }
                        }
                    }

                    return returnPharmacyList;
                }
                else
                {
                    return null;
                }
            }
        }

        [HttpGet] //Fetch Data
        public async Task<ActionResult> GetPharmacyMedicineSupplies()
        {
            return Ok(await db.PharmacyMedicineSupplies.ToListAsync());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetMedicineByName(int id)
        {
            PharmacyMedicineSupply p = await db.PharmacyMedicineSupplies.FindAsync(id);
            if (p != null)
            {
                return Ok(p);

            }
            else
                return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> AddMedicine(PharmacyMedicineSupply p)
        {
            if (ModelState.IsValid)
            {
                db.PharmacyMedicineSupplies.Add(p);
                await db.SaveChangesAsync();
                return Ok(p);
            }
            else
                return BadRequest();
        }


        [HttpPut]
        public async Task<ActionResult> EditMedicine(int id, PharmacyMedicineSupply P)
        {
            PharmacyMedicineSupply prod = db.PharmacyMedicineSupplies.Find(id);
            if (prod != null)
            {
                //db.PharmacyMedicineSupplies.Update(P);

                prod.SupplyCount = P.SupplyCount;
            }

            db.Entry(prod).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok(P);
        }

        [HttpDelete]
        public async Task<ActionResult> EditMedicine(int id)
        {
            PharmacyMedicineSupply prod = db.PharmacyMedicineSupplies.Find(id);
            db.PharmacyMedicineSupplies.Remove(prod);
            await db.SaveChangesAsync();
            return Ok(prod);
        }

    }
}
