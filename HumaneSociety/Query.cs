using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {

        public static void AddAnimal(Animal animal)
        {
            HumanSocietyDataContext MyTable = new HumanSocietyDataContext();
            MyTable.Animals.InsertOnSubmit(animal);
            MyTable.SubmitChanges();
        }
        public static void RemoveAnimal(Animal animal)
        {
            HumanSocietyDataContext MyTable = new HumanSocietyDataContext();
            MyTable.Animals.DeleteOnSubmit(animal);
            MyTable.SubmitChanges();
        }

        public static int BreedSearch(string inputBreed, HumanSocietyDataContext MyTable)    //have get breed call breed search within it once your donw wrtiing it 
        {
            var existingBreed = (from row in MyTable.Breeds where row.breed1 == inputBreed select row.ID).FirstOrDefault();
            return existingBreed;
        }

        public static int GetBreed(string inputBreed)
        {
            HumanSocietyDataContext MyTable = new HumanSocietyDataContext();
            int testBreed = BreedSearch(inputBreed, MyTable)



            if BreedSearch() returns null, call AddBreed() and then return that breed ID;
            
        }


    }
}
