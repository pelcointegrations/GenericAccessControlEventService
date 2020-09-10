using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _VX_ACS_Administration
{
    public partial class FormActions : Form
    {
        private List<XMLUtilities.Action> _actions = null;
        private List<string> _layoutNames;
        private List<string> _actionNames;

        public FormActions(string title, ref List<XMLUtilities.Action> actions, List<string> actionList, List<string> layoutList)
        {
            InitializeComponent();
            this.Text = title;
            _actions = actions;
            _actionNames = actionList;
            _layoutNames = layoutList;
            actionsBindingSource.DataSource = _actionNames;
            layoutBindingSource.DataSource = _layoutNames;

            ActionsDataGrid.DefaultCellStyle.SelectionForeColor = Color.Black;
            ActionsDataGrid.DefaultCellStyle.ForeColor = Color.Black;

            // populate action grid
            foreach(var action in _actions)
            {
                ActionsDataGrid.Rows.Add(action.Name, action.Layout, action.Monitor, action.Camera, action.Cell, action.PreviousSeconds, action.Preset, action.Pattern, action.Description);
            }
        }

        public List<XMLUtilities.Action> Actions
        {
            get { return _actions; }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in ActionsDataGrid.Rows)
            {
                string missingParam = string.Empty;
                if ((row.Cells["ActionColumn"].Value != null)&&(!ValidateActionParameters(row, ref missingParam)))
                {
                    DialogResult result = MessageBox.Show("Row " + (row.Index + 1) + " required parameter " + missingParam + " is missing.  Please fix errors before continuing.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None; // keep from closing
                    return;
                }
            }

            List<XMLUtilities.Action> actions = new List<XMLUtilities.Action>();
            foreach(DataGridViewRow row in ActionsDataGrid.Rows)
            {
                // only consider valid if action is selected (prevents issues with last row also)
                if (row.Cells["ActionColumn"].Value != null)
                {
                    XMLUtilities.Action action = new XMLUtilities.Action();
                    action.Name = row.Cells["ActionColumn"].Value.ToString();

                    if (row.Cells["LayoutColumn"].Value != null)
                    {
                        action.Layout = row.Cells["LayoutColumn"].Value.ToString();
                    }
                    if (row.Cells["MonitorColumn"].Value != null)
                    {
                        action.Monitor = row.Cells["MonitorColumn"].Value.ToString();
                    }
                    if (row.Cells["CameraColumn"].Value != null)
                    {
                        action.Camera = row.Cells["CameraColumn"].Value.ToString();
                    }
                    if (row.Cells["CellColumn"].Value != null)
                    {
                        action.Cell = row.Cells["CellColumn"].Value.ToString();
                    }
                    if (row.Cells["PresetColumn"].Value != null)
                    {
                        action.Preset = row.Cells["PresetColumn"].Value.ToString();
                    }
                    if (row.Cells["PatternColumn"].Value != null)
                    {
                        action.Pattern = row.Cells["PatternColumn"].Value.ToString();
                    }
                    if (row.Cells["DescriptionColumn"].Value != null)
                    {
                        action.Description = row.Cells["DescriptionColumn"].Value.ToString();
                    }
                    actions.Add(action);
                }
            }
            _actions = actions;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // just exit, nothing to do
        }

        private void ActionsDataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string headerText = 
                ActionsDataGrid.Columns[e.ColumnIndex].HeaderText;

            //// Abort validation if number is not unique
            //if (headerText.Equals("Number"))
            //{
            //    ActionsDataGrid.Rows[e.RowIndex].ErrorText =
            //        "The action number must be unique.";
            //    //e.Cancel = true;
            //}
        }

        private void ActionsDataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Clear the row error in case the user presses ESC.   
            ActionsDataGrid.Rows[e.RowIndex].ErrorText = String.Empty;
            EnableDisableColumns(ActionsDataGrid.Rows[e.RowIndex]);
        }

        private bool ValidateActionParameters(DataGridViewRow row, ref string missingParam)
        {
            bool valid = false;
            if (row.Cells["ActionColumn"].Value != null)
            {
                string action = row.Cells["ActionColumn"].Value.ToString();
                if (action == "SetLayout")
                {
                    if (row.Cells["LayoutColumn"].Value == null)
                    {
                        missingParam = "Layout";
                    }
                    else valid = true;
                }
                else if (action == "DisplayCamera")
                {
                    if (row.Cells["MonitorColumn"].Value == null)
                    {
                        missingParam = "Monitor";
                    }
                    else if (row.Cells["CameraColumn"].Value == null)
                    {
                        missingParam = "Camera";
                    }
                    else if (row.Cells["CellColumn"].Value == null)
                    {
                        missingParam = "Cell";
                    }
                    else valid = true;
                }
                else if (action == "DisconnectCamera")
                {
                    if (row.Cells["MonitorColumn"].Value == null)
                    {
                        missingParam = "Monitor";
                    }
                    else if (row.Cells["CellColumn"].Value == null)
                    {
                        missingParam = "Cell";
                    }
                    else valid = true;
                }
                else if (action == "GotoPreset")
                {
                    if (row.Cells["PresetColumn"].Value == null)
                    {
                        missingParam = "Preset";
                    }
                    else if (row.Cells["CameraColumn"].Value == null)
                    {
                        missingParam = "Camera";
                    }
                    else valid = true;
                }
                else if (action == "RunPattern")
                {
                    if (row.Cells["PatternColumn"].Value == null)
                    {
                        missingParam = "Pattern";
                    }
                    else if (row.Cells["CameraColumn"].Value == null)
                    {
                        missingParam = "Camera";
                    }
                    else valid = true;
                }
                else if (action == "BookMark")
                {
                    if (row.Cells["CameraColumn"].Value == null)
                    {
                        missingParam = "Camera";
                    }
                    else valid = true;
                }
            }
            return valid;
        }

        private void ActionsDataGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = ActionsDataGrid.Rows[e.RowIndex];
            if (row != null)
            {
                EnableDisableColumns(row);
            }
        }

        private void EnableDisableColumns(DataGridViewRow row)
        {
            if (row.Cells["ActionColumn"].Value != null)
            {
                string action = row.Cells["ActionColumn"].Value.ToString();
                if (action == "SetLayout")
                {
                    SetColumnsReadOnly(row.Cells, false /*layout*/, true, true, true, true, true, true, true);
                }
                else if (action == "DisplayCamera")
                {
                    SetColumnsReadOnly(row.Cells, true, false /*monitor*/, false /*camera*/, false /*cell*/, true, true, true, true);
                }
                else if (action == "DisconnectCamera")
                {
                    SetColumnsReadOnly(row.Cells, true, false /*monitor*/, true, false /*cell*/, true, true, true, true);
                }
                else if (action == "GotoPreset")
                {
                    SetColumnsReadOnly(row.Cells, true, true, false /*camera*/, true, true, false /*preset*/, true, true);
                }
                else if (action == "RunPattern")
                {
                    SetColumnsReadOnly(row.Cells, true, true, false /*camera*/, true, true, true, false /*pattern*/, true);
                }
                else if (action == "BookMark")
                {
                    SetColumnsReadOnly(row.Cells, true, true, false /*camera*/, true, true, true, true, false /*description*/);
                }
            }
            else // disable all
            {
                SetColumnsReadOnly(row.Cells, true, true, true, true, true, true, true, true);
            }
        }

        private void SetColumnsReadOnly(DataGridViewCellCollection cells, bool layout, bool monitor, bool camera, bool cell, bool seconds, bool preset, bool pattern, bool description)
        {
            if (cells != null)
            {
                cells["LayoutColumn"].ReadOnly = layout;
                cells["MonitorColumn"].ReadOnly = monitor;
                cells["CameraColumn"].ReadOnly = camera;
                cells["CellColumn"].ReadOnly = cell;
                cells["SecondsColumn"].ReadOnly = seconds;
                cells["PresetColumn"].ReadOnly = preset;
                cells["PatternColumn"].ReadOnly = pattern;
                cells["DescriptionColumn"].ReadOnly = description;
                if (layout)
                {
                    cells["LayoutColumn"].Style.BackColor = Color.Black;
                }
                else
                {
                    cells["LayoutColumn"].Style.BackColor = Color.White;
                }
                if (monitor)
                {
                    cells["MonitorColumn"].Style.BackColor = Color.Black;
                }
                else
                {
                    cells["MonitorColumn"].Style.BackColor = Color.White;
                }
                if (camera)
                {
                    cells["CameraColumn"].Style.BackColor = Color.Black;
                }
                else
                {
                    cells["CameraColumn"].Style.BackColor = Color.White;
                }
                if (cell)
                {
                    cells["CellColumn"].Style.BackColor = Color.Black;
                }
                else
                {
                    cells["CellColumn"].Style.BackColor = Color.White;
                }
                if (seconds)
                {
                    cells["SecondsColumn"].Style.BackColor = Color.Black;
                }
                else
                {
                    cells["SecondsColumn"].Style.BackColor = Color.White;
                }
                if (preset)
                {
                    cells["PresetColumn"].Style.BackColor = Color.Black;
                }
                else
                {
                    cells["PresetColumn"].Style.BackColor = Color.White;
                }
                if (pattern)
                {
                    cells["PatternColumn"].Style.BackColor = Color.Black;
                }
                else
                {
                    cells["PatternColumn"].Style.BackColor = Color.White;
                }
                if (description)
                {
                    cells["DescriptionColumn"].Style.BackColor = Color.Black;
                }
                else
                {
                    cells["DescriptionColumn"].Style.BackColor = Color.White;
                }
            }
        }

        private void ActionsDataGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = ActionsDataGrid.Rows[e.RowIndex];
            if (row != null)
            {
                EnableDisableColumns(row);
            }
        }

        private void ActionsDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 0)&&(e.RowIndex != -1))
            {
                DataGridViewRow row = ActionsDataGrid.Rows[e.RowIndex];
                if (row != null)
                {
                    EnableDisableColumns(row);
                }
            }
        }

        private void ActionsDataGrid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 0)&&(e.RowIndex != -1))
            {
                DataGridViewRow row = ActionsDataGrid.Rows[e.RowIndex];
                if (row != null)
                {
                    EnableDisableColumns(row);
                }
            }
        }
    }
}
