using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using PerkingRezerWPFDM.Models;

namespace PerkingRezerWPFDM.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Employee> _employees;

        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public ICommand AddEmployeeCommand { get; }

        public EmployeeViewModel()
        {
            Employees = new ObservableCollection<Employee>();
            AddEmployeeCommand = new RelayCommand(AddEmployee);
        }

        private void AddEmployee()
        {
            Employees.Add(new Employee
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                RegistrationNumber = "123456",
                Department = "IT"
            });
            OnPropertyChanged(nameof(Employees));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
