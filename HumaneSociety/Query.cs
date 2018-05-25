using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {

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
            catch
            {
                return null;
            }   
        }

        // Jake left off starting public static IEnumberable GetUserAdoptionStatus();

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
