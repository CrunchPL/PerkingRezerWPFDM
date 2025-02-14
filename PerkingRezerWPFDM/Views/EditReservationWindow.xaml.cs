using System;
using System.Windows;
using PerkingRezerWPFDM.ViewModels;
using PerkingRezerWPFDM.Models;

namespace PerkingRezerWPFDM.Views
{
    public partial class EditReservationWindow : Window
    {
        public int NewStartHour { get; private set; }
        public int NewStartMinute { get; private set; }
        public int NewEndHour { get; private set; }
        public int NewEndMinute { get; private set; }

        public EditReservationWindow(ReservationDisplayModel reservation)
        {
            InitializeComponent();
            StartHourComboBox.ItemsSource = GetHours();
            StartMinuteComboBox.ItemsSource = GetMinutes();
            EndHourComboBox.ItemsSource = GetHours();
            EndMinuteComboBox.ItemsSource = GetMinutes();

            StartHourComboBox.SelectedItem = int.Parse(reservation.StartTime.Split(':')[0]);
            StartMinuteComboBox.SelectedItem = int.Parse(reservation.StartTime.Split(':')[1]);
            EndHourComboBox.SelectedItem = int.Parse(reservation.EndTime.Split(':')[0]);
            EndMinuteComboBox.SelectedItem = int.Parse(reservation.EndTime.Split(':')[1]);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            NewStartHour = (int)StartHourComboBox.SelectedItem;
            NewStartMinute = (int)StartMinuteComboBox.SelectedItem;
            NewEndHour = (int)EndHourComboBox.SelectedItem;
            NewEndMinute = (int)EndMinuteComboBox.SelectedItem;
            DialogResult = true;
        }

        private int[] GetHours() => new int[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 };
        private int[] GetMinutes() => new int[] { 0, 10, 20, 30, 40, 50 };
    }
}
