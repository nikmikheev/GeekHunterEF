using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GeekHunterEF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Candidate newCandidate { get; set; }
        public Skill newSkill { get; set; }

        GeekHunterConnection context = new GeekHunterConnection();
        CollectionViewSource candidateView;
        CollectionViewSource candidateSkillView;

        public MainWindow()
        {
            InitializeComponent();
            newCandidate = new Candidate();
            newSkill = new Skill();
            candidateView = ((CollectionViewSource)(FindResource("CandidateViewSource")));
            candidateSkillView = ((CollectionViewSource)(FindResource("CandidateSkillsViewSource")));
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                context.Candidates.Load();
                // Load data by setting the CollectionViewSource.Source property:
                candidateView.Source = context.Candidates.Local;
                //            candidateSkillView.Source = context.Skills.Local;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }

        }

        private void LastCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            candidateView.View.MoveCurrentToLast();
        }

        private void PreviousCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            candidateView.View.MoveCurrentToPrevious();
        }

        private void NextCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            candidateView.View.MoveCurrentToNext();
        }

        private void FirstCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            candidateView.View.MoveCurrentToFirst();
        }

        private void DeleteCandidateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            // If existing window is visible, then delete the Candidate and all their orders.  
            // In a real application, you should add warnings and allow a user to cancel the operation.  
            var cur = candidateView.View.CurrentItem as Candidate;

            var cust = (from c in context.Candidates
                        where c.Id == cur.Id
                        select c).FirstOrDefault();

            if (cust != null)
            {
                foreach (var ord in cust.Skills.ToList())
                {
                    DeleteCandidateSkill(ord);
                }
                context.Candidates.Remove(cust);
            }
            context.SaveChanges();
            candidateView.View.Refresh();
        }

        // Commit changes from the new Candidate form, the new skill form,  
        // or edits made to the existing Candidate form.  
        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (newCandidateGrid.IsVisible)
            {
                // Create a new object because the old one  
                // is being tracked by EF now.  
                newCandidate = new Candidate();
                newCandidate.Id = Convert.ToInt32(add_idTextBox.Text);
                newCandidate.FirstName = add_firstNameTextBox.Text;
                newCandidate.LastName = add_lastNameTextBox.Text;
                newCandidate.EnteredDate = add_enteredDateDatePicker.DisplayDate;

                // Perform very basic validation  
                //if (newCandidate.Id.Length == 5)
                //{
                // Insert the new Candidate at correct position:  
                int len = context.Candidates.Local.Count();
                int pos = len;
                for (int i = 0; i < len; ++i)
                {
                    if (newCandidate.Id < context.Candidates.Local[i].Id) 
                    {
                        pos = i;
                        break;
                    }
                }
                context.Candidates.Local.Insert(pos, newCandidate);
                    candidateView.View.Refresh();
                    candidateView.View.MoveCurrentTo(newCandidate);
                //}
                //else
                //{
                //    MessageBox.Show("Id must have 5 characters.");
                //}

                newCandidateGrid.Visibility = Visibility.Collapsed;
                existingCandidateGrid.Visibility = Visibility.Visible;
            }
            else if (newSkillGrid.IsVisible)
            {
                // Skill ID is auto-generated so we don't set it here.  
                // For CandidateID, address, etc we use the values from current Candidate.  
                // User can modify these in the datagrid after the skill is entered.  

                //newSkill.OrderDate = add_orderDatePicker.SelectedDate;
                //newSkill.RequiredDate = add_requiredDatePicker.SelectedDate;
                //newSkill.ShippedDate = add_shippedDatePicker.SelectedDate;
                //try
                //{
                //    newSkill.Freight = Convert.ToDecimal(add_freightTextBox.Text);
                //}
                //catch
                //{
                //    MessageBox.Show("Freight must be convertible to decimal.");
                //    return;
                //}

                // Add the skill into the EF model  
                context.Skills.Add(newSkill);
                candidateSkillView.View.Refresh();
            }

            // Save the changes, either for a new Candidate, a new skill  
            // or an edit in an existing Candidate or skill  
            context.SaveChanges();
        }

        // Sets up the form so that user can enter data. Data is later  
        // saved when user clicks Commit.  
        private void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            existingCandidateGrid.Visibility = Visibility.Collapsed;
            newSkillGrid.Visibility = Visibility.Collapsed;
            newCandidateGrid.Visibility = Visibility.Visible;

            // Clear all the text boxes before adding a new Candidate.  
            foreach (var child in newCandidateGrid.Children)
            {
                var tb = child as TextBox;
                if (tb != null)
                {
                    tb.Text = "";
                }
            }
        }

        private void EditCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void NewCandidateSkill_click(object sender, RoutedEventArgs e)
        {
            var tmpCandidate = candidateView.View.CurrentItem as Candidate;
            if (tmpCandidate == null)
            {
                MessageBox.Show("No Candidate selected.");
                return;
            }

            newSkill.Id = tmpCandidate.Id;

            // Get address and other mostly constant fields from   
            // an existing skill, if one exists  
            var coll = candidateView.Source as IEnumerable<Candidate>;
            var lastOrder = (from c in coll
                             from ord in c.Skills
                             select ord).LastOrDefault();
            if (lastOrder != null)
            {
                //newSkill.ShipAddress = lastOrder.ShipAddress;
                //newSkill.ShipCity = lastOrder.ShipCity;
            }

            existingCandidateGrid.Visibility = Visibility.Collapsed;
            newCandidateGrid.Visibility = Visibility.Collapsed;
            newSkillGrid.UpdateLayout();
            newSkillGrid.Visibility = Visibility.Visible;
        }

        // Cancels any input into the new Candidate form  
        private void CancelCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            add_idTextBox.Text = "";
            add_firstNameTextBox.Text = "";
            add_lastNameTextBox.Text = "";
            add_enteredDateDatePicker.DisplayDate = DateTime.Now;

            existingCandidateGrid.Visibility = Visibility.Visible;
            newCandidateGrid.Visibility = Visibility.Collapsed;
            newSkillGrid.Visibility = Visibility.Collapsed;
        }

        private void DeleteCandidateSkill(Skill skill)
        {
            // Find the skill in the EF model.  
            var ord = (from o in context.Skills.Local
                       where o.Id == skill.Id
                select o).FirstOrDefault();

            // delete the skill.  
            context.Skills.Remove(ord);
            context.SaveChanges();

            // Update the data grid.  
            candidateSkillView.View.Refresh();
        }

        private void DeleteCandidateSkillCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            // Get the Skill in the row in which the Delete button was clicked.  
            Skill obj = e.Parameter as Skill;
            DeleteCandidateSkill(obj);
        }
    }
}
