using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyMedicineSupplyMicroservice.Models
{
    public class PharmacyMedicineSupplyTemp
    {
        public string PharmacyName { get; set; }
        public string MedicineName { get; set; }
        public int SupplyCount { get; set; }

        List<PharmacyMedicineSupplyTemp> pahamacies = new List<PharmacyMedicineSupplyTemp>();
        public PharmacyMedicineSupplyTemp()
        {

        }

        public PharmacyMedicineSupplyTemp(string pharmacyName, string medicineName, int supplyCount)
        {
            PharmacyName = pharmacyName;
            MedicineName = medicineName;
            SupplyCount = supplyCount;
        }

        public List<PharmacyMedicineSupplyTemp> getCartItems()
        {
            pahamacies.Add(new PharmacyMedicineSupplyTemp("abc", "paracetomal", 50));
            pahamacies.Add(new PharmacyMedicineSupplyTemp("xyz", "citirizine", 25));
            pahamacies.Add(new PharmacyMedicineSupplyTemp("pqr", "saridon", 75));
            pahamacies.Add(new PharmacyMedicineSupplyTemp("efg", "babygesic", 50));
            pahamacies.Add(new PharmacyMedicineSupplyTemp("klm", "delcon", 100));
            return pahamacies;
        }

    }
}
