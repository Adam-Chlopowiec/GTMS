using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using mongo.Repos;
using mongo.Models.AppModels;
using MongoDB.Driver;

namespace Tests
{
    [TestClass]
    public class TestExpenseRepository
    {
        private static RepositoryBuilder builder = new RepositoryBuilder();
        private static TourRepository tourRepository = builder.GetTourRepository("TestTours");
        private static GroupRepository groupRepository = builder.GetGroupRepository("TestGroups");
        private static PersonRepository personRepository = builder.GetPersonRepository("TestPersons");
        private static ExpenseRepository expenseRepository = builder.GetExpenseRepository("TestExpenses");
        private static List<Expense> expenses = CreateExpenses();

        public async Task DropCollections()
        {
            var client = new MongoClient(
                "mongodb://127.0.0.1:27017/?directConnection=true&serverSelectionTimeoutMS=2000&appName=mongosh+1.1.9");
            var database = client.GetDatabase("GTMS");
            await database.DropCollectionAsync("TestTours");
            await database.DropCollectionAsync("TestGroups");
            await database.DropCollectionAsync("TestPersons");
            await database.DropCollectionAsync("TestExpenses");
        }

        [TestMethod]
        public async Task TestDropCollections()
        {
            await DropCollections();
            Assert.IsTrue(true);
        }

        public async Task AddToExpenseRepository()
        {
            foreach (var expense in expenses)
            {
                await expenseRepository.Add(expense);
            }
        }

        public async Task AddToPersonRepository()
        {
            foreach (var expense in expenses)
            {
                foreach (var person in expense.Payments.Keys)
                {
                    try
                    {
                        await personRepository.Add(person);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        [TestMethod]
        public async Task TestAddToExpenseRepository()
        {
            await DropCollections();
            foreach (var expense in expenses)
            {
                var expense_ = await expenseRepository.Add(expense);
                Assert.IsNotNull(expense_.Id);
            }
        
            var oldIds = new List<Guid>();
            foreach (var expense in expenses)
            {
                oldIds.Add(expense.Id);
            }
        
            foreach (var expense in expenses)
            {
                await Assert.ThrowsExceptionAsync<MongoWriteException>(async () => await expenseRepository.Add(expense));
            }
        
            for (var i = 0; i < expenses.Count; i++)
            {
                Assert.AreEqual(oldIds[i], expenses[i].Id);
            }
        }
        
        [TestMethod]
        public async Task TestEditExpenseRepository()
        {
            await DropCollections();
            await AddToExpenseRepository();
            var newNames = new List<string>
            {
                "A", "B", "C", "D", "E"
            };
            for (var i = 0; i < expenses.Count; i++)
            {
                expenses[i].Name = newNames[i];
                var result = await expenseRepository.Edit(expenses[i]);
                Assert.IsTrue(result);
            }
        
            var fakeExpense = new Expense
            {
                Name = "fake",
                Payments = new Dictionary<Person, double>()
            };
            Assert.IsFalse(await expenseRepository.Edit(fakeExpense));
        }
        
        [TestMethod]
        public async Task TestGetByIdExpenseRepository()
        {
            await DropCollections();
            await AddToExpenseRepository();
            await AddToPersonRepository();
            foreach (var expense in expenses)
            {
                var dbExpense = await expenseRepository.GetById(expense.Id);
                Assert.AreEqual(expense.Id, dbExpense.Id);
            }
        
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () =>
                await expenseRepository.GetById(Guid.NewGuid()));
        }
        
        [TestMethod]
        public async Task TestGetAllExpenseRepository()
        {
            await DropCollections();
            await AddToExpenseRepository();
            await AddToPersonRepository();
            var dbExpenses = (await expenseRepository.GetAll()).ToList();
            Assert.AreEqual(expenses.Count, dbExpenses.Count);
        
            for (var i = 0; i < dbExpenses.Count; i++)
            {
                Assert.AreEqual(expenses[i].Id, dbExpenses[i].Id);
            }
        }
        
        [TestMethod]
        public async Task TestDeleteFromExpenseRepository()
        {
            await DropCollections();
            await AddToExpenseRepository();

            foreach (var expense in expenses)
            {
                var result = await expenseRepository.Delete(expense.Id);
                Assert.IsTrue(result);
            }
        
            foreach (var expense in expenses)
            {
                var result = await expenseRepository.Delete(expense.Id);
                Assert.IsFalse(result);
            }
        }

        public static List<Expense> CreateExpenses()
        {
            var persons = CreateMockPersons();
            var payments = CreateMockPayments(persons);
            var expenses_ = new List<Expense>();
            var names = new List<string>
            {
                "first", "second", "third", "fourth", "fifth"
            };
            for (var i = 0; i < 5; i++)
            {
                var expense = new Expense
                {
                    Name = names[i],
                    Payments = payments[i]
                };
                expenses_.Add(expense);
            }

            return expenses_;
        }

        // Returns 5 PersonDB objects with no groups
        public static List<Person> CreateMockPersons()
        {
            var persons = new List<Person>();
            // Name, lastname, id, groups
            var names = new List<string>
            {
                "Adam", "Jan", "Adrian", "Aleksow", "Konrad"
            };
            var lastnames = new List<string>
            {
                "Chłopowiec", "Wielgus", "Chłopowiec", "Buk", "Kossack"
            };
            for (var i = 0; i < 5; i++)
            {
                var person = new Person
                {
                    Name = names[i],
                    Lastname = lastnames[i],
                    Groups = new List<Group>(),
                    Id = Guid.NewGuid()
                };
                persons.Add(person);
                _ = personRepository.Add(person).Result;
            }

            return persons;
        }

        // Returns 5 payment dictionaries with 3 kv pairs each
        public static List<Dictionary<Person, double>> CreateMockPayments(List<Person> persons)
        {
            var payments = new List<Dictionary<Person, double>>();
            var values = new List<double>
            {
                10.59, 69.42, 21.37, 4.20, 9.11
            };
            var valueCombos = new List<(double, double, double)>
            {
                (values[0], values[1], values[2]),
                (values[1], values[3], values[4]),
                (values[2], values[0], values[4]),
                (values[3], values[1], values[3]),
                (values[4], values[2], values[0])
            };
            var personCombos = new List<(Person, Person, Person)>
            {
                (persons[0], persons[1], persons[2]),
                (persons[1], persons[3], persons[4]),
                (persons[2], persons[0], persons[4]),
                (persons[3], persons[1], persons[2]),
                (persons[4], persons[2], persons[0])
            };
            for (var i = 0; i < 5; i++)
            {
                var payment = new Dictionary<Person, double>
                {
                    {personCombos[i].Item1, valueCombos[i].Item1},
                    {personCombos[i].Item2, valueCombos[i].Item2},
                    {personCombos[i].Item3, valueCombos[i].Item3},
                };
                payments.Add(payment);
            }

            return payments;
        }

    }
}