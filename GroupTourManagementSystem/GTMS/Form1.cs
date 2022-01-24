// ***********************************************************************
// Assembly         : GTMS
// Author           : Adam
// Created          : 01-24-2022
//
// Last Modified By : Adam
// Last Modified On : 01-24-2022
// ***********************************************************************
// <copyright file="Form1.cs" company="">
//     Copyright ©  2022
// </copyright>
// <summary></summary>
// ***********************************************************************
using GTMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTMS.Views;

namespace GTMS
{
    /// <summary>
    /// Class Form1.
    /// Implements the <see cref="System.Windows.Forms.Form" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class Form1 : Form
    {
        private List<Group> Groups { get; set; } = new List<Group>();

        public List<Panel> TourPanels { get; set; } = new List<Panel>();

        public List<Panel> GroupPanels { get; set; } = new List<Panel>();

        public List<Panel> ExpensesPanels { get; set; } = new List<Panel>();

        public List<Label> GroupMemberLabels { get; set; } = new List<Label>();

        public List<CheckBox> CheckboxesAddGroup { get; set; } = new List<CheckBox>();

        public List<CheckBox> CheckboxesAddExpense { get; set; } = new List<CheckBox>();

        public List<TextBox> TextBoxesAddExpense { get; set; } = new List<TextBox>();

        public Group SelectedGroup { get; set; }

        public List<Expense> Expenses { get; set; } = new List<Expense>();

        public Form1()
        {
            InitializeComponent();

            var persons = new List<Person>
            {
                new Person { Id = 1, Name = "Adam", Lastname = "Chłopowiec" },
                new Person { Id = 2, Name = "Jan", Lastname = "Wielgus" },
                new Person { Id = 3, Name = "Adrian", Lastname = "Chłopowiec" },
            };


            var expense1 = new Expense { Id = 1, Name = "Wypad w góry" };
            expense1.Payments.Add(persons[2], 34.56);
            expense1.Payments.Add(persons[1], 11.56);
            var expense2 = new Expense { Id = 2, Name = "Kebsy" };
            expense2.Payments.Add(persons[0], 44.96);
            var expense3 = new Expense { Id = 3, Name = "Nowy" };
            var expenses = new List<Expense> { expense1, expense2, expense3 };
            Expenses.AddRange(expenses);

            Groups.Add(new Group { Name = "Madlads", Id = 1, Captain = persons[0] });
            Groups.Add(new Group { Name = "Grupa2", Id = 2, Captain = persons[0] });
            Groups[0].Members.AddRange(persons);
            Groups[0].Expenses.AddRange(expenses);

            Groups[1].Members.Add(persons[0]);

            var tour1 = new Tour
            {
                Id = 1,
                Name = "Trip Around the World",
                Captain = persons[0],
                Members = new List<Person> { persons[0], persons[2] },
            };

            var tourExpense1 = new Expense { Id = 4, Name = "statek" };
            tourExpense1.Payments.Add(persons[2], 2000);
            tour1.Expenses.Add(tourExpense1);
            Groups[0].Tours.Add(tour1);

            //koniec mocku

            
            for (int i = 0; i < Groups.Count; i++) 
            {
                var group = Groups[i];
                var panel = new Panel
                {
                    Size = new Size(240, 50),
                    Name = "groupBox2" + i.ToString(),
                    Location = new Point(10, 20 + 60*i),
                    Text = "grupa",
                };

                panel.Controls.Add(new Label { AutoSize = true, Location = new Point(0, 20), Text = group.Name });
                panel.Controls.Add(new Label { AutoSize = true, Location = new Point(80, 20), Text = group.TotalValue.ToString() + "$" });
                var button = new Button { Text = "Details", Size = new Size(60, 24), Location = new Point(164, 16), Name = "but" + i.ToString() };
                button.Click += new System.EventHandler(this.groupDetails);
                panel.Controls.Add(button);

                GroupPanels.Add(panel);
            }

            this.groupBox1.Controls.AddRange(GroupPanels.ToArray());

            
            
            for (int i = 0; i < Groups[0].Members.Count; i++)
            {
                var member = Groups[0].Members[i];
                
                var checkbox = new CheckBox
                {
                    AutoSize = true,
                    Location = new Point(30, 60 + 20 * i),
                    Text = member.Name + " " + member.Lastname,
                    UseVisualStyleBackColor = true
                };
                CheckboxesAddGroup.Add(checkbox);
            }

            groupBox6.Controls.AddRange(CheckboxesAddGroup.ToArray());

            
            

        }


        private void generateGroupTours(Group group)
        {
            for (int i = 0; i < group.Tours.Count; i++)
            {
                var tour = group.Tours[i];
                var panel = new Panel
                {
                    Size = new Size(220, 50),
                    Location = new Point(0, 0 + 60 * i),
                };
                panel.Controls.Add(new Label { AutoSize = true, Location = new Point(0, 5), Text = tour.Name });
                panel.Controls.Add(new Label { AutoSize = true, Location = new Point(10, 20), Text = "total value: " + tour.TotalExpense.ToString() + "$" });
                var button = new Button { Text = "Details", Size = new Size(60, 24), Location = new Point(150, 5) };
                panel.Controls.Add(button);

                TourPanels.Add(panel);
            }
        }

        private void generateExpenses(Group group)
        {
            for (int i = 0; i < group.Expenses.Count; i++)
            {
                var exp = group.Expenses[i];
                var panel = new Panel
                {
                    Size = new Size(220, 40),
                    Location = new Point(0, 6 + 40 * i),
                };

                panel.Controls.Add(new Label { AutoSize = true, Location = new Point(10, 0), Text = exp.Name, Font = new Font(label1.Font.FontFamily, 9.0f, label1.Font.Style, label1.Font.Unit) });
                panel.Controls.Add(new Label { AutoSize = true, Location = new Point(10, 20), Text = "total value: " + exp.TotalValue.ToString() + "$" });
                panel.Controls.Add(new Button { Text = "Details", Size = new Size(60, 24), Location = new Point(150, 5) });

                ExpensesPanels.Add(panel);
            }
        }

        private void generateGroupMembers(Group group)
        {
            for (int i = 0; i < group.Members.Count; i++)
            {
                var member = group.Members[i];
                var label = new Label
                {
                    AutoSize = true,
                    Location = new Point(20, 30 + 20 * i),
                    Text = member.Name + " " + member.Lastname,
                };

                GroupMemberLabels.Add(label);
            }
        }

        private void generateAddExpense(Group group)
        {
            for (int i = 0; i < group.Members.Count; i++)
            {
                var member = group.Members[i];

                var checkbox = new CheckBox
                {
                    AutoSize = true,
                    Location = new Point(30, 60 + 25 * i),
                    Text = member.Name + " " + member.Lastname,
                    UseVisualStyleBackColor = true,
                    Name = "check" + i.ToString(),
                };
                checkbox.CheckedChanged += new System.EventHandler(this.addExpenseCheckboxChecked);
                CheckboxesAddExpense.Add(checkbox);

                var textBox = new TextBox
                {
                    AutoSize = true,
                    Location = new Point(150, 60 + 25 * i),
                    Text = "0,00",
                    Size = new Size(50, 12),
                    Enabled = false,
                    Name = "text" + i.ToString(),
                };
                textBox.TextChanged += new System.EventHandler(this.addExpenseValueChanged);
                TextBoxesAddExpense.Add(textBox);
            }
        }

        private void addExpenseCheckboxChecked(object sender, EventArgs e)
        {
            int i = Int32.Parse((sender as CheckBox).Name.Substring(5));
            TextBoxesAddExpense[i].Enabled = CheckboxesAddExpense[i].Checked;
            label4.Text = "sum: "
                + TextBoxesAddExpense.Sum(tb =>
                    tb.Enabled & tb.Text.Length > 0 ? double.Parse(tb.Text) : 0)
                + "$";
        }
        
        private void addExpenseValueChanged(object sender, EventArgs e)
        {
            int i = Int32.Parse((sender as TextBox).Name.Substring(4));
            label4.Text = "sum: " 
                + TextBoxesAddExpense.Sum(tb => 
                    tb.Enabled & tb.Text.Length > 0
                        ? double.Parse(tb.Text.Replace('.', ','))
                        : 0)
                + "$";
        }

        //Add new group
        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void groupDetails(object sender, EventArgs e)
        {
            int i = Int32.Parse((sender as Button).Name.Substring(3));
            SelectedGroup = Groups[i];
            GroupPanels.ForEach(gp => groupBox1.Controls.Remove(gp));
            groupBox1.Visible = false;
            groupBox2.Visible = true;
            groupBox2.Location = groupBox1.Location;

            generateGroupTours(Groups[i]);
            this.panel2.Controls.AddRange(TourPanels.ToArray());

            generateExpenses(Groups[i]);
            this.panel1.Controls.AddRange(ExpensesPanels.ToArray());

            generateGroupMembers(Groups[i]);
            this.groupBox3.Controls.AddRange(GroupMemberLabels.ToArray());
            if (GroupMemberLabels.Count > 0)
                button2.Location = new Point(button2.Location.X, GroupMemberLabels[GroupMemberLabels.Count - 1].Location.Y + 20);

            label1.Text = "Group " + Groups[i].Name;
        }

        private void addExpense(object sender, EventArgs e)
        {
            ExpensesPanels.ForEach(ep => panel1.Controls.Remove(ep));
            TourPanels.ForEach(tp => panel2.Controls.Remove(tp));
            GroupMemberLabels.ForEach(gml => groupBox3.Controls.Remove(gml));

            ExpensesPanels.Clear();
            TourPanels.Clear();
            GroupMemberLabels.Clear();

            groupBox2.Visible = false;

            groupBox7.Visible = true;
            groupBox7.Location = groupBox1.Location;

            generateAddExpense(SelectedGroup);
            groupBox7.Controls.AddRange(CheckboxesAddExpense.ToArray());
            groupBox7.Controls.AddRange(TextBoxesAddExpense.ToArray());
        }

        //Add new Expense
        private void button6_Click(object sender, EventArgs e)
        {
            var sum = TextBoxesAddExpense.Sum(tb =>
                    tb.Enabled & tb.Text.Length > 0
                        ? double.Parse(tb.Text.Replace('.', ','))
                        : 0);

            var payments = new Dictionary<Person, double>();
            for (int i = 0; i < SelectedGroup.Members.Count; i++)
            {
                if (CheckboxesAddExpense[i].Checked)
                    payments.Add(SelectedGroup.Members[i],
                        double.Parse(TextBoxesAddExpense[i].Text.Replace('.', ',')));
            }
            
            var expense = new Expense
            { 
                Name = textBox2.Text,
                Payments = payments,
                Id = Expenses.Max(exp => exp.Id) + 1,
            };

            Expenses.Add(expense);
            SelectedGroup.Expenses.Add(expense);

            //closing addExpense view
            CheckboxesAddExpense.ForEach(c => groupBox7.Controls.Remove(c));
            TextBoxesAddExpense.ForEach(c => groupBox7.Controls.Remove(c));

            CheckboxesAddExpense.Clear();
            TextBoxesAddExpense.Clear();

            groupBox7.Visible = false;
            groupBox2.Visible = true;

            generateGroupTours(SelectedGroup);
            this.panel2.Controls.AddRange(TourPanels.ToArray());

            generateExpenses(SelectedGroup);
            this.panel1.Controls.AddRange(ExpensesPanels.ToArray());

            generateGroupMembers(SelectedGroup);
            this.groupBox3.Controls.AddRange(GroupMemberLabels.ToArray());
            if (GroupMemberLabels.Count > 0)
                button2.Location = new Point(button2.Location.X, GroupMemberLabels[GroupMemberLabels.Count - 1].Location.Y + 20);

            label1.Text = "Group " + SelectedGroup.Name;
        }
    }
}
