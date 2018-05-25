using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    class User
    {
        protected string name;
        protected string userName;

        public virtual void LogIn()
        {

        }
        protected bool CheckIfNewUser()
        {
            List<string> options = new List<string>() { "Are you a new User?", "yes", "no" };
            UserInterface.DisplayUserOptions(options);
            string input = UserInterface.GetUserInput();
            if(input.ToLower() == "yes" || input.ToLower() == "y")
            {
                return true;
            }
            else if(input.ToLower() == "no" || input.ToLower() == "n")
            {
                return false;
            }
            else
            {
                Console.Clear();
                UserInterface.DisplayUserOptions("Input not recognized please try again");
                return CheckIfNewUser();
            }
        }
        protected virtual void LogInPreExistingUser()
        {

        }
        protected virtual void RunUserMenus()
        {

        }
        protected IQueryable<Animal> SearchForAnimal()
        {
            HumaneSocietyDataContext context = new HumaneSocietyDataContext();
            var animals = from data in context.Animals select data;

            var searchParameters = GetAnimalCriteria();
            if (searchParameters.ContainsKey(1))
            {
                animals = (from data in animals where data.Breed1.Catagory1.catagory1 == searchParameters[1] select data);
            }
            if (searchParameters.ContainsKey(2))
            {
                animals = (from data in animals where data.Breed1.breed1 == searchParameters[2] select data);
            }
            if (searchParameters.ContainsKey(3))
            {
                animals = (from data in animals where data.name == searchParameters[3] select data);
            }
            if (searchParameters.ContainsKey(4))
            {
                animals = (from data in animals where data.age == int.Parse(searchParameters[4]) select data);
            }
            if (searchParameters.ContainsKey(5))
            {
                animals = (from data in animals where data.demeanor == searchParameters[5] select data);
            }
            if (searchParameters.ContainsKey(6))
            {
                bool parameter = GetBoolParamater(searchParameters[6]);
                animals = (from data in animals where data.kidFriendly == parameter select data);
            }
            if (searchParameters.ContainsKey(7))
            {
                bool parameter = GetBoolParamater(searchParameters[7]);
                animals = (from data in animals where data.petFriendly == parameter select data);
            }
            if (searchParameters.ContainsKey(8))
            {
                animals = (from data in animals where data.weight == int.Parse(searchParameters[8]) select data);
            }
            if (searchParameters.ContainsKey(9))
            {
                animals = (from data in animals where data.ID == int.Parse(searchParameters[9]) select data);
            }
            return animals;
        }
        protected bool GetBoolParamater(string input)
        {
            if (input.ToLower() == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected Dictionary<int, string> GetAnimalCriteria()
        {
            Dictionary<int, string> searchParameters = new Dictionary<int, string>();
            bool isSearching = true;
            while (isSearching)
            {
                Console.Clear();
                List<string> options = new List<string>() { "Select Search Criteia: (Enter number and choose finished when finished)", "1. Category", "2. Breed", "3. Name", "4. Age", "5. Demeanor", "6. Kid friendly", "7. Pet friendly", "8. Weight","9. ID", "10. Finished" };
                UserInterface.DisplayUserOptions(options);
                string input = UserInterface.GetUserInput();
                if (input.ToLower() == "10" || input.ToLower() == "finished")
                {
                    isSearching = false;
                    continue;
                }
                else
                {
                    searchParameters = UserInterface.EnterSearchCriteria(searchParameters, input);
                }
            }
            return searchParameters;
        }
    }
}
