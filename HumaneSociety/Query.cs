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

        public static int BreedSearch(string inputBreed, HumaneSocietyDataContext MyTable)    //have get breed call breed search within it once your donw wrtiing it 
        {
            var existingBreed = (from row in MyTable.Breeds where row.breed1 == inputBreed select row.ID).FirstOrDefault();
            return existingBreed;
        }

        //public static int GetBreed(string inputBreed)
        //{
        //    HumanSocietyDataContext MyTable = new HumanSocietyDataContext();
        //    int testBreed = BreedSearch(inputBreed, MyTable)



        //    if BreedSearch() returns null, call AddBreed() and then return that breed ID;
            
        //}



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

        public static USState GetStateByID(int iD)
        {
            try
            {
                using (var db = new HumaneSocietyDataContext)
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

    }
}
