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

namespace GTMS.Views
{
    public partial class GroupsList : Form
    {
        public List<Panel> GroupPanels { get; set; }

        public GroupsList(List<Group> groups)
        {
            InitializeComponent();

            GroupPanels = new List<Panel>();
            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];
                var panel = new Panel
                {
                    Size = new Size(240, 50),
                    Name = "groupBox2" + i.ToString(),
                    Location = new Point(10, 20 + 60 * i),
                    Text = "grupa",
                };

                panel.Controls.Add(new Label { AutoSize = true, Location = new Point(0, 20), Text = group.Name });
                panel.Controls.Add(new Label { AutoSize = true, Location = new Point(80, 20), Text = group.TotalValue.ToString() + "$" });
                panel.Controls.Add(new Button { Text = "Details", Size = new Size(60, 24), Location = new Point(164, 16) });

                GroupPanels.Add(panel);
            }

            groupBox1.Controls.AddRange(GroupPanels.ToArray());
        }

    }
}
