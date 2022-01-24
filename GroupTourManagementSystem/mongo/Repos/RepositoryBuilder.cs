using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongo.Repos
{
    public class RepositoryBuilder
    {
        private TourRepository tourRepository;
        private GroupRepository groupRepository;
        private PersonRepository personRepository;
        private ExpenseRepository expenseRepository;
        public RepositoryBuilder()
        {
            tourRepository = new TourRepository();
            groupRepository = new GroupRepository();
            personRepository = new PersonRepository();
            expenseRepository = new ExpenseRepository();

            tourRepository.SetExpenseRepository(expenseRepository);
            tourRepository.SetPersonRepository(personRepository);
            groupRepository.SetExpenseRepository(expenseRepository);
            groupRepository.SetPersonRepository(personRepository);
            groupRepository.SetTourRepository(tourRepository);
            personRepository.SetGroupRepository(groupRepository);
            expenseRepository.SetPersonRepository(personRepository);
        }

        public TourRepository GetTourRepository(string collection)
        {
            tourRepository.Connect(collection);
            return tourRepository;
        }

        public GroupRepository GetGroupRepository(string collection)
        {
            groupRepository.Connect(collection);
            return groupRepository;
        }

        public PersonRepository GetPersonRepository(string collection)
        {
            personRepository.Connect(collection);
            return personRepository;
        }

        public ExpenseRepository GetExpenseRepository(string collection)
        {
            expenseRepository.Connect(collection);
            return expenseRepository;
        }
    }
}
