namespace _VX_ACS_Administration
{
    partial class FormActions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormActions));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.ActionsDataGrid = new System.Windows.Forms.DataGridView();
            this.ActionColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.actionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.LayoutColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.layoutBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.MonitorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CameraColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CellColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecondsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PresetColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PatternColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ActionsDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.BackColor = System.Drawing.Color.Transparent;
            this.buttonOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonOK.Image = ((System.Drawing.Image)(resources.GetObject("buttonOK.Image")));
            this.buttonOK.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonOK.Location = new System.Drawing.Point(581, 344);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(78, 36);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "   &OK";
            this.buttonOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonCancel.Image = ((System.Drawing.Image)(resources.GetObject("buttonCancel.Image")));
            this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCancel.Location = new System.Drawing.Point(668, 344);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(78, 36);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // ActionsDataGrid
            // 
            this.ActionsDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionsDataGrid.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ActionsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ActionsDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ActionColumn,
            this.LayoutColumn,
            this.MonitorColumn,
            this.CameraColumn,
            this.CellColumn,
            this.SecondsColumn,
            this.PresetColumn,
            this.PatternColumn,
            this.DescriptionColumn});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ActionsDataGrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.ActionsDataGrid.Location = new System.Drawing.Point(12, 14);
            this.ActionsDataGrid.Name = "ActionsDataGrid";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ActionsDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ActionsDataGrid.Size = new System.Drawing.Size(734, 324);
            this.ActionsDataGrid.TabIndex = 6;
            this.ActionsDataGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ActionsDataGrid_CellEndEdit);
            this.ActionsDataGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.ActionsDataGrid_CellValidating);
            this.ActionsDataGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ActionsDataGrid_CellValueChanged);
            this.ActionsDataGrid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.ActionsDataGrid_RowEnter);
            this.ActionsDataGrid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.ActionsDataGrid_RowsAdded);
            // 
            // ActionColumn
            // 
            this.ActionColumn.DataSource = this.actionsBindingSource;
            this.ActionColumn.HeaderText = "Action";
            this.ActionColumn.Name = "ActionColumn";
            this.ActionColumn.Width = 160;
            // 
            // LayoutColumn
            // 
            this.LayoutColumn.DataSource = this.layoutBindingSource;
            this.LayoutColumn.HeaderText = "Layout";
            this.LayoutColumn.Name = "LayoutColumn";
            this.LayoutColumn.Width = 80;
            // 
            // MonitorColumn
            // 
            this.MonitorColumn.HeaderText = "Monitor";
            this.MonitorColumn.Name = "MonitorColumn";
            this.MonitorColumn.Width = 50;
            // 
            // CameraColumn
            // 
            this.CameraColumn.HeaderText = "Camera";
            this.CameraColumn.Name = "CameraColumn";
            this.CameraColumn.Width = 50;
            // 
            // CellColumn
            // 
            this.CellColumn.HeaderText = "Cell";
            this.CellColumn.Name = "CellColumn";
            this.CellColumn.Width = 35;
            // 
            // SecondsColumn
            // 
            this.SecondsColumn.HeaderText = "Previous Seconds";
            this.SecondsColumn.Name = "SecondsColumn";
            this.SecondsColumn.Width = 55;
            // 
            // PresetColumn
            // 
            this.PresetColumn.HeaderText = "Preset";
            this.PresetColumn.Name = "PresetColumn";
            this.PresetColumn.Width = 90;
            // 
            // PatternColumn
            // 
            this.PatternColumn.HeaderText = "Pattern";
            this.PatternColumn.Name = "PatternColumn";
            this.PatternColumn.Width = 90;
            // 
            // DescriptionColumn
            // 
            this.DescriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DescriptionColumn.HeaderText = "Description";
            this.DescriptionColumn.Name = "DescriptionColumn";
            // 
            // FormActions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(758, 392);
            this.Controls.Add(this.ActionsDataGrid);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormActions";
            this.Text = "Actions";
            ((System.ComponentModel.ISupportInitialize)(this.ActionsDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.Button buttonOK;
        internal System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridView ActionsDataGrid;
        private System.Windows.Forms.BindingSource actionsBindingSource;
        private System.Windows.Forms.BindingSource layoutBindingSource;
        private System.Windows.Forms.DataGridViewComboBoxColumn ActionColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn LayoutColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MonitorColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CameraColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CellColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecondsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PresetColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PatternColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescriptionColumn;
    }
}