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
            HumaneSocietyDataContext MyTable = new HumaneSocietyDataContext();
            MyTable.Animals.InsertOnSubmit(animal);
            MyTable.SubmitChanges();
        }

        public static void AddNewClient(string nameF, string nameL, string username, string password, string email, string streetAdress, int zipCode, int state)
        {
            var client = new Client();
            client.firstName = nameF;
            client.lastName = nameL;
            client.userName = username;
            client.pass = password;
            client.email = email;

            var userAddress = new UserAddress();
            userAddress.addessLine1 = streetAdress;
            userAddress.USState = GetStateByID(state);
            userAddress.zipcode = zipCode;

            client.UserAddress1 = userAddress;

            using(var db = new HumaneSocietyDataContext())
            {
                db.Clients.InsertOnSubmit(client);
            } 
        }

        public static void AddNewShot(string name)
        {
            using (var db = new HumaneSocietyDataContext())
            {
                var shot = new Shot();
                shot.name = name;

                db.Shots.InsertOnSubmit(shot);

                db.SubmitChanges();
            }
        }

        public static void AddUsernameAndPassword(Employee employee)
        {
            using (var db = new HumaneSocietyDataContext())
            {
                var employeeResult = db.Employees.Where(e => e.ID == employee.ID).FirstOrDefault();
                employeeResult.userName = employee.userName;
                employeeResult.pass = employee.pass;
                db.SubmitChanges();
            }
        }

        public static void Adopt(Animal animal, Client client)
        {
            var clientAnimalJunction = new ClientAnimalJunction();
            clientAnimalJunction.Animal1 = animal;
            clientAnimalJunction.Client1 = client;
            clientAnimalJunction.approvalStatus = "pending";

            using (HumaneSocietyDataContext db = new HumaneSocietyDataContext())
            {
                db.ClientAnimalJunctions.InsertOnSubmit(clientAnimalJunction);
                db.SubmitChanges();
            }
        }

        public static void RemoveAnimal(Animal animal)
        {
            HumaneSocietyDataContext MyTable = new HumaneSocietyDataContext();
            MyTable.Animals.DeleteOnSubmit(animal);
            MyTable.SubmitChanges();
        }

        public static int? BreedSearch(string inputBreed, HumaneSocietyDataContext MyTable)    //have get breed call breed search within it once your donw wrtiing it 
        {
            int? existingBreed = (from row in MyTable.Breeds where row.breed1 == inputBreed select row.ID).FirstOrDefault();
            return existingBreed;
        }

        public static int GetBreed(string inputBreed)
        {
            HumaneSocietyDataContext MyTable = new HumaneSocietyDataContext();
            int existingBreed;
            int? testBreed = BreedSearch(inputBreed, MyTable);
            if (testBreed == null)
            {
                AddBreed(inputBreed);
                existingBreed = (int)testBreed;
                return existingBreed;
            }
            else {
                existingBreed = (int)testBreed;
                return existingBreed;
            }
        }

        public static void AddBreed(string inputBreed)
        {
            HumaneSocietyDataContext MyTable = new HumaneSocietyDataContext();
            var newBreed = new Breed();
            newBreed.breed1 = inputBreed;
            MyTable.Breeds.InsertOnSubmit(newBreed);
            MyTable.SubmitChanges();
        }

        public static bool CheckEmployeeUserNameExist(string username)
        {
            using (var db = new HumaneSocietyDataContext())
            {
                return db.Employees.Where(e => e.userName == username).FirstOrDefault() != null;
            }
        }

        private static Employee CreateEmployee(Employee employee)
        {
            try
            {
                using(HumaneSocietyDataContext db = new HumaneSocietyDataContext())
                {
                    try
                    {
                        db.Employees.InsertOnSubmit(employee);
                        db.SubmitChanges();
                        return employee;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }

            } catch (Exception)
            {
                return null;
            }
        }

        public static Employee EmployeeLogin(string username, string password)
        {
            using (var db = new HumaneSocietyDataContext())
            {
                return db.Employees.Where(e => e.userName == username && e.pass == password).First();
            }
        }

        public static void EnterUpdate(Animal animal, Dictionary<int, string> updates)
        {
            bool breedUpdated = false;

            using (var db = new HumaneSocietyDataContext())
            {
                foreach (KeyValuePair<int, string> update in updates)
                {
                    switch (update.Key)
                    {
                        case 1:
                        case 2:
                            if (!breedUpdated)
                            {
                                int? categoryInt;
                                int? breedInt;

                                if (updates.ContainsKey(1))
                                {
                                    categoryInt = db.Catagories.Where(c => c.catagory1 == updates[1]).Select(c => c.ID).FirstOrDefault();
                                }
                                else
                                {
                                    categoryInt = animal.Breed1.catagory;
                                }

                                if (categoryInt == null)
                                {
                                    var category = new Catagory();
                                    category.catagory1 = updates[1];
                                    db.Catagories.InsertOnSubmit(category);
                                    db.SubmitChanges();
                                    categoryInt = db.Catagories.Where(c => c.catagory1 == updates[1]).Select(c => c.ID).FirstOrDefault();
                                }

                                if (updates.ContainsKey(2))
                                {
                                    breedInt = db.Breeds.Where(b => b.catagory == categoryInt && b.breed1 == updates[2]).Select(b => b.ID).FirstOrDefault();
                                }
                                else
                                {
                                    breedInt = db.Breeds.Where(b => b.breed1 == animal.Breed1.breed1 && b.catagory == categoryInt).Select(b => b.ID).FirstOrDefault();
                                }

                                if (breedInt == null)
                                {
                                    var breed = new Breed();
                                    breed.catagory = categoryInt;
                                    breed.breed1 = updates[2];
                                    db.Breeds.InsertOnSubmit(breed);
                                    db.SubmitChanges();
                                    breedInt = db.Breeds.Where(b => b.catagory == categoryInt && b.breed1 == updates[2]).Select(b => b.ID).FirstOrDefault();
                                }

                                animal.breed = breedInt;
                                breedUpdated = true;
                            }
                            break;

                        case 3:
                            animal.name = update.Value;
                            break;

                        case 4:
                            animal.age = Int32.Parse(update.Value);
                            break;

                        case 5:
                            animal.demeanor = update.Value;
                            break;

                        case 6:
                            animal.kidFriendly = bool.Parse(update.Value);
                            break;

                        case 7:
                            animal.petFriendly = bool.Parse(update.Value);
                            break;

                        case 8:
                            animal.weight = Int32.Parse(update.Value);
                            break;

                        default:
                            break;
                    }

                }

                var animalResult = db.Animals.Where(a => a.ID == animal.ID).FirstOrDefault();
                animalResult.Breed1 = animal.Breed1;
                animalResult.name = animal.name;
                animalResult.age = animal.age;
                animalResult.demeanor = animal.demeanor;
                animalResult.kidFriendly = animal.kidFriendly;
                animalResult.petFriendly = animal.petFriendly;
                animalResult.weight = animal.weight;

                db.SubmitChanges();
            }
        }

        public static Client GetClient(string userName, string password)
        {
            try
            {
                using(HumaneSocietyDataContext db = new HumaneSocietyDataContext())
                {
                    return db.Clients.Where(c => c.userName == userName && c.pass == password).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Animal GetAnimalByID(int iD)
        {
            try
            {
                using(HumaneSocietyDataContext db = new HumaneSocietyDataContext())
                {
                    var animalResult = db.Animals.Where(a => a.ID == iD).FirstOrDefault();
                    return animalResult;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static int GetDiet()
        {
            string food = UserInterface.GetStringData("food type", "the animal's");
            int amount = UserInterface.GetIntegerData($"{food} for this animal", "the serving amount of");

            using (var db = new HumaneSocietyDataContext())
            {
                var dietPlan = db.DietPlans.Where(d => d.food == food && d.amount == amount).FirstOrDefault();
                if(dietPlan == null)
                {
                    dietPlan = new DietPlan();
                    dietPlan.food = food;
                    dietPlan.amount = amount;

                    db.DietPlans.InsertOnSubmit(dietPlan);
                    db.SubmitChanges();
                    dietPlan = db.DietPlans.Where(d => d.food == food && d.amount == amount).FirstOrDefault();
                }

                return dietPlan.ID;
            }
        }

        private static Employee GetEmployee(Employee employee)
        {
            try
            {
                using (HumaneSocietyDataContext db = new HumaneSocietyDataContext())
                {
                    var employeeResult = (from e in db.Employees where e.employeeNumber == employee.employeeNumber select e).FirstOrDefault();
                    if (employeeResult != null)
                    {
                        UserInterface.DisplayEmployeeInfo(employeeResult);
                        return employeeResult;
                    }
                    else
                    {
                        UserInterface.DisplayUserOptions($"Unable to locate employee # {employee.employeeNumber}.");
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static int GetLocation()
        {
            string building = UserInterface.GetStringData("building", "the animal's");
            string room = UserInterface.GetStringData("room", "the animal's");

            using (var db = new HumaneSocietyDataContext())
            {
                var location = db.Rooms.Where(r => r.building == building && r.name == room).FirstOrDefault();
                if (location == null)
                {
                    location = new Room();
                    location.building = building;
                    location.name = room;

                    db.Rooms.InsertOnSubmit(location);
                    db.SubmitChanges();
                    location = db.Rooms.Where(r => r.building == building && r.name == room).FirstOrDefault();
                }

                return location.ID;
            }
        }

        public static IQueryable<ClientAnimalJunction> GetPendingAdoptions()
        {
            using (var db = new HumaneSocietyDataContext())
            {
                return db.ClientAnimalJunctions.Where(c => c.approvalStatus == "pending");
            }
        }

        public static IQueryable<AnimalShotJunction> GetShots(Animal animal)
        {
            using (var db = new HumaneSocietyDataContext())
            {
                return db.AnimalShotJunctions.Where(a => a.Animal_ID == animal.ID);
            }
        }

        public static USState GetStateByID(int iD)
        {
            try
            {
                using (var db = new HumaneSocietyDataContext())
                {
                    return db.USStates.Where(s => s.ID == iD).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public static IQueryable<USState> GetStates()
        {
            try
            {
                using (var db = new HumaneSocietyDataContext())
                {
                    return db.USStates;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<ClientAnimalJunction> GetUserAdoptionStatus(Client client)
        {
            try
            {
                using (HumaneSocietyDataContext db = new HumaneSocietyDataContext())
                {
                    var clientAnimalResult = db.ClientAnimalJunctions.Where(c => c.Client1.ID == client.ID);
                    return clientAnimalResult;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IQueryable<Client> RetrieveClients()
        {
            try
            {
                using (var db = new HumaneSocietyDataContext())
                {
                    return db.Clients;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            using (var db = new HumaneSocietyDataContext())
            {
                return db.Employees.Where(e => e.employeeNumber == employeeNumber && e.email == email).First();
            }
        }

        public static Employee RunEmployeeQueries(Employee employee, string queryType)
        {
            Func<Employee, Employee> queryMethod;

            switch (queryType)
            {
                case "read":
                    queryMethod = GetEmployee;
                    break;

                case "update":
                    queryMethod = UpdateEmployee;
                    break;

                case "create":
                    queryMethod = CreateEmployee;
                    break;

                default:
                    throw new Exception($"'{queryType}' is not a valid RunEmployeeQuery option.");
            }

            return queryMethod(employee);
        }

        public static void UpdateAddress(Client client)
        {
            using (var db = new HumaneSocietyDataContext())
            {
                Client clientResult = db.Clients.Where(c => c.ID == client.ID).First();
                clientResult.UserAddress1 = client.UserAddress1;
                db.SubmitChanges();
            }
        }

        public static void UpdateAdoption (bool decision, ClientAnimalJunction clientAnimalJunction)
        {
            using (var db = new HumaneSocietyDataContext())
            {
                var clientAnimalJunctionResult = db.ClientAnimalJunctions.Where(c => c.Client1.ID == clientAnimalJunction.Client1.ID && c.Animal1.ID == clientAnimalJunction.Animal1.ID).FirstOrDefault();
                clientAnimalJunctionResult.approvalStatus = decision ? "approved" : "denied";
                db.SubmitChanges();
            }
        }

        public static void updateClient(Client client)
        {
            using (var db = new HumaneSocietyDataContext())
            {
                var clientResult = db.Clients.Where(c => c.ID == client.ID).FirstOrDefault();
                clientResult.firstName = client.firstName;
                clientResult.lastName = client.lastName;
                clientResult.email = client.email;
                clientResult.homeSize = client.homeSize;
                clientResult.income = client.income;
                clientResult.kids = client.kids;
                clientResult.pass = client.pass;
                clientResult.userAddress = client.userAddress;
                clientResult.userName = client.userName;

                db.SubmitChanges();
            }
        }

        public static void UpdateEmail(Client client)
        {
            using (var db = new HumaneSocietyDataContext())
            {
                Client clientResult = db.Clients.Where(c => c.ID == client.ID).First();
                clientResult.email = client.email;
                db.SubmitChanges();
            }
        }

        private static Employee UpdateEmployee(Employee employee)
        {
            try
            {
                using (HumaneSocietyDataContext db = new HumaneSocietyDataContext())
                {
                    var employeeResult = db.Employees.Where(e => e.employeeNumber == employee.employeeNumber).FirstOrDefault();

                    employeeResult.Animals = employee.Animals;
                    employeeResult.email = employee.email;
                    employeeResult.firsttName = employee.firsttName;
                    employeeResult.lastName = employee.lastName;
                    employeeResult.pass = employee.pass;
                    employeeResult.userName = employee.userName;

                    try
                    {
                        db.SubmitChanges();
                        return employeeResult;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                    
            }
            catch(Exception)
            {
                return null;
            }
        }

        public static void UpdateFirstName(Client client)
        {
            using (var db = new HumaneSocietyDataContext())
            {
                Client clientResult = db.Clients.Where(c => c.ID == client.ID).First();
                clientResult.firstName = client.firstName;
                db.SubmitChanges();
            }
        }

        public static void UpdateLastName(Client client)
        {
            using (var db = new HumaneSocietyDataContext())
            {
                Client clientResult = db.Clients.Where(c => c.ID == client.ID).First();
                clientResult.lastName = client.lastName;
                db.SubmitChanges();
            }
        }

        public static void UpdateShot(string type, Animal animal)
        {
            using (var db = new HumaneSocietyDataContext())
            {
                var shot = db.Shots.Where(s => s.name == type).FirstOrDefault();
                if (shot == null)
                {
                    AddNewShot(type);
                    shot = db.Shots.Where(s => s.name == type).FirstOrDefault();
                }

                var animalShotJunction = db.AnimalShotJunctions.Where(a => a.Animal.ID == animal.ID && a.Shot.ID == shot.ID).FirstOrDefault();
                if(animalShotJunction == null)
                {
                    animalShotJunction = new AnimalShotJunction();
                    animalShotJunction.Animal = animal;
                    animalShotJunction.Shot = shot;
                    animalShotJunction.dateRecieved = DateTime.Now;
                    db.AnimalShotJunctions.InsertOnSubmit(animalShotJunction);
                }
                else
                {
                    animalShotJunction.dateRecieved = DateTime.Now;
                }

                db.SubmitChanges();
                

                
            }
        }

        public static void UpdateUsername(Client client)
        {
            using (var db = new HumaneSocietyDataContext())
            {
                Client clientResult = db.Clients.Where(c => c.ID == client.ID).First();
                clientResult.userName = client.userName;
                db.SubmitChanges();
            }
        }

    }
}
