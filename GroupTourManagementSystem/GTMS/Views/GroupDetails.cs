// ***********************************************************************
// Assembly         : GTMS
// Author           : Adam
// Created          : 01-24-2022
//
// Last Modified By : Adam
// Last Modified On : 01-24-2022
// ***********************************************************************
// <copyright file="GroupDetails.cs" company="">
//     Copyright ©  2022
// </copyright>
// <summary></summary>
// ***********************************************************************
using GTMS.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTMS.Views
{
    /// <summary>
    /// Class GroupDetails.
    /// Implements the <see cref="System.Windows.Forms.GroupBox" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.GroupBox" />
    public class GroupDetails : GroupBox
    {
        public List<Panel> GroupPanels { get; set; }

        public GroupDetails(List<Group> groups)
        {
            //Controls.Add(this.button4);
            Name = "groupBox1";
            Size = new Size(260, 360);
            TabIndex = 0;
            TabStop = false;
            Text = "Twoje Grupy";
            Location = new Point(20, 20);

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

            Controls.AddRange(GroupPanels.ToArray());
        }
    }
}
