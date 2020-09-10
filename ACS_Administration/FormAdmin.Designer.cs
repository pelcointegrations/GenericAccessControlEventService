namespace _VX_ACS_Administration
{
    partial class FormAdmin
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdmin));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cameraAssociationGridView = new System.Windows.Forms.DataGridView();
            this.CameraNameColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cameraBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.PeripheralColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.feenicsPeripheralsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.EventNameColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.feenicsEventsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.ACSSettingGroupBox = new System.Windows.Forms.GroupBox();
            this.AxConnectionPictureBox = new System.Windows.Forms.PictureBox();
            this.buttonConnectACS = new System.Windows.Forms.Button();
            this.textBoxACSPassword = new System.Windows.Forms.TextBox();
            this.labelACSPassword = new System.Windows.Forms.Label();
            this.textBoxACSUserName = new System.Windows.Forms.TextBox();
            this.labelACSUserName = new System.Windows.Forms.Label();
            this.textBoxACSAddress = new System.Windows.Forms.TextBox();
            this.labelACSIPAddress = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.ACSServerGroupBox = new System.Windows.Forms.GroupBox();
            this.ACSuseSSLCheckBox = new System.Windows.Forms.CheckBox();
            this.textBoxACSPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxACSIpAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.VideoXpertSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.VxConnectionPictureBox = new System.Windows.Forms.PictureBox();
            this.textBoxVxPort = new System.Windows.Forms.TextBox();
            this.labelVxPort = new System.Windows.Forms.Label();
            this.buttonConnectVx = new System.Windows.Forms.Button();
            this.textBoxVxPassword = new System.Windows.Forms.TextBox();
            this.labelVxPassword = new System.Windows.Forms.Label();
            this.textBoxVxUserName = new System.Windows.Forms.TextBox();
            this.labelVxUsername = new System.Windows.Forms.Label();
            this.textBoxVxIpAddress = new System.Windows.Forms.TextBox();
            this.labelVxAddress = new System.Windows.Forms.Label();
            this.loggingGroupBox = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDownRetention = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBoxError = new System.Windows.Forms.CheckBox();
            this.checkBoxOperation = new System.Windows.Forms.CheckBox();
            this.tabPageCameraAssociations = new System.Windows.Forms.TabPage();
            this.tabPageEventMap = new System.Windows.Forms.TabPage();
            this.eventInjectionGroupBox = new System.Windows.Forms.GroupBox();
            this.eventMapDataGrid = new System.Windows.Forms.DataGridView();
            this.DirectionColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.SituationColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.situationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ACSEventColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.RunScriptsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AckScriptsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageSituations = new System.Windows.Forms.TabPage();
            this.situationsGroupBox = new System.Windows.Forms.GroupBox();
            this.companyTextBox = new System.Windows.Forms.TextBox();
            this.companyLabel = new System.Windows.Forms.Label();
            this.customSituationDataGrid = new System.Windows.Forms.DataGridView();
            this.TypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SeverityColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LogColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.NotifyColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.BannerColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ExpandBannerColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.AudibleColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.NeedAckColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.AutoAckColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SnoozeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageScripts = new System.Windows.Forms.TabPage();
            this.scriptsDataGrid = new System.Windows.Forms.DataGridView();
            this.DBNumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScriptNumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScriptNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActionsColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.buttonReload = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panelProgress = new System.Windows.Forms.Panel();
            this.labelProgress = new System.Windows.Forms.Label();
            this.buttonCancelProgress = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cameraAssociationGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cameraBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.feenicsPeripheralsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.feenicsEventsBindingSource)).BeginInit();
            this.ACSSettingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AxConnectionPictureBox)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.ACSServerGroupBox.SuspendLayout();
            this.VideoXpertSettingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VxConnectionPictureBox)).BeginInit();
            this.loggingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRetention)).BeginInit();
            this.tabPageCameraAssociations.SuspendLayout();
            this.tabPageEventMap.SuspendLayout();
            this.eventInjectionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventMapDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.situationBindingSource)).BeginInit();
            this.tabPageSituations.SuspendLayout();
            this.situationsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customSituationDataGrid)).BeginInit();
            this.tabPageScripts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scriptsDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panelProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.cameraAssociationGridView);
            this.groupBox1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(684, 390);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Camera Event Associations";
            // 
            // cameraAssociationGridView
            // 
            this.cameraAssociationGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cameraAssociationGridView.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cameraAssociationGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.cameraAssociationGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cameraAssociationGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CameraNameColumn,
            this.PeripheralColumn,
            this.EventNameColumn});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cameraAssociationGridView.DefaultCellStyle = dataGridViewCellStyle1;
            this.cameraAssociationGridView.Location = new System.Drawing.Point(11, 17);
            this.cameraAssociationGridView.Name = "cameraAssociationGridView";
            this.cameraAssociationGridView.RowHeadersWidth = 21;
            this.cameraAssociationGridView.Size = new System.Drawing.Size(664, 367);
            this.cameraAssociationGridView.TabIndex = 5;
            this.cameraAssociationGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.cameraAssociationGridView_CellValueChanged);
            this.cameraAssociationGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.cameraAssociationGridView_RowsRemoved);
            this.cameraAssociationGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cameraAssociationGridView_MouseClick);
            // 
            // CameraNameColumn
            // 
            this.CameraNameColumn.DataSource = this.cameraBindingSource;
            this.CameraNameColumn.HeaderText = "Camera Name";
            this.CameraNameColumn.Name = "CameraNameColumn";
            this.CameraNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CameraNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CameraNameColumn.ToolTipText = "Name to identify Camera";
            this.CameraNameColumn.Width = 225;
            // 
            // PeripheralColumn
            // 
            this.PeripheralColumn.DataSource = this.feenicsPeripheralsBindingSource;
            this.PeripheralColumn.HeaderText = "Door";
            this.PeripheralColumn.Name = "PeripheralColumn";
            this.PeripheralColumn.Width = 250;
            // 
            // EventNameColumn
            // 
            this.EventNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EventNameColumn.DataSource = this.feenicsEventsBindingSource;
            this.EventNameColumn.HeaderText = "Event Name";
            this.EventNameColumn.Name = "EventNameColumn";
            this.EventNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.EventNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.EventNameColumn.ToolTipText = "Name to identify Event";
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
            this.buttonOK.Location = new System.Drawing.Point(434, 446);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(78, 36);
            this.buttonOK.TabIndex = 2;
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
            this.buttonCancel.Location = new System.Drawing.Point(623, 446);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(78, 36);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.BackColor = System.Drawing.Color.Transparent;
            this.buttonApply.Enabled = false;
            this.buttonApply.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonApply.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonApply.Image = ((System.Drawing.Image)(resources.GetObject("buttonApply.Image")));
            this.buttonApply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonApply.Location = new System.Drawing.Point(529, 446);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(78, 36);
            this.buttonApply.TabIndex = 3;
            this.buttonApply.Text = "Appl&y ";
            this.buttonApply.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonApply.UseVisualStyleBackColor = false;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // ACSSettingGroupBox
            // 
            this.ACSSettingGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ACSSettingGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.ACSSettingGroupBox.Controls.Add(this.AxConnectionPictureBox);
            this.ACSSettingGroupBox.Controls.Add(this.buttonConnectACS);
            this.ACSSettingGroupBox.Controls.Add(this.textBoxACSPassword);
            this.ACSSettingGroupBox.Controls.Add(this.labelACSPassword);
            this.ACSSettingGroupBox.Controls.Add(this.textBoxACSUserName);
            this.ACSSettingGroupBox.Controls.Add(this.labelACSUserName);
            this.ACSSettingGroupBox.Controls.Add(this.textBoxACSAddress);
            this.ACSSettingGroupBox.Controls.Add(this.labelACSIPAddress);
            this.ACSSettingGroupBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ACSSettingGroupBox.Location = new System.Drawing.Point(6, 6);
            this.ACSSettingGroupBox.Name = "ACSSettingGroupBox";
            this.ACSSettingGroupBox.Padding = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.ACSSettingGroupBox.Size = new System.Drawing.Size(684, 116);
            this.ACSSettingGroupBox.TabIndex = 5;
            this.ACSSettingGroupBox.TabStop = false;
            this.ACSSettingGroupBox.Text = "Access Control System Settings";
            // 
            // AxConnectionPictureBox
            // 
            this.AxConnectionPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.AxConnectionPictureBox.Image = global::_VX_ACS_Administration.Properties.Resources.Red_Dot;
            this.AxConnectionPictureBox.InitialImage = global::_VX_ACS_Administration.Properties.Resources.Red_Dot;
            this.AxConnectionPictureBox.Location = new System.Drawing.Point(654, 16);
            this.AxConnectionPictureBox.MaximumSize = new System.Drawing.Size(20, 20);
            this.AxConnectionPictureBox.Name = "AxConnectionPictureBox";
            this.AxConnectionPictureBox.Size = new System.Drawing.Size(20, 20);
            this.AxConnectionPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.AxConnectionPictureBox.TabIndex = 9;
            this.AxConnectionPictureBox.TabStop = false;
            // 
            // buttonConnectACS
            // 
            this.buttonConnectACS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConnectACS.BackColor = System.Drawing.Color.Transparent;
            this.buttonConnectACS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonConnectACS.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonConnectACS.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonConnectACS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConnectACS.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonConnectACS.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonConnectACS.Location = new System.Drawing.Point(573, 71);
            this.buttonConnectACS.Name = "buttonConnectACS";
            this.buttonConnectACS.Size = new System.Drawing.Size(77, 32);
            this.buttonConnectACS.TabIndex = 4;
            this.buttonConnectACS.Text = "Connect";
            this.buttonConnectACS.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonConnectACS.UseVisualStyleBackColor = false;
            this.buttonConnectACS.Click += new System.EventHandler(this.buttonConnectACS_Click);
            // 
            // textBoxACSPassword
            // 
            this.textBoxACSPassword.Location = new System.Drawing.Point(123, 83);
            this.textBoxACSPassword.Name = "textBoxACSPassword";
            this.textBoxACSPassword.PasswordChar = '*';
            this.textBoxACSPassword.Size = new System.Drawing.Size(186, 20);
            this.textBoxACSPassword.TabIndex = 3;
            this.textBoxACSPassword.TextChanged += new System.EventHandler(this.textBoxPassword_TextChanged);
            // 
            // labelACSPassword
            // 
            this.labelACSPassword.Location = new System.Drawing.Point(53, 85);
            this.labelACSPassword.Name = "labelACSPassword";
            this.labelACSPassword.Size = new System.Drawing.Size(59, 14);
            this.labelACSPassword.TabIndex = 6;
            this.labelACSPassword.Text = "&Password:";
            this.labelACSPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxACSUserName
            // 
            this.textBoxACSUserName.Location = new System.Drawing.Point(123, 51);
            this.textBoxACSUserName.Name = "textBoxACSUserName";
            this.textBoxACSUserName.Size = new System.Drawing.Size(186, 20);
            this.textBoxACSUserName.TabIndex = 2;
            this.textBoxACSUserName.TextChanged += new System.EventHandler(this.textBoxUserName_TextChanged);
            // 
            // labelACSUserName
            // 
            this.labelACSUserName.Location = new System.Drawing.Point(42, 53);
            this.labelACSUserName.Name = "labelACSUserName";
            this.labelACSUserName.Size = new System.Drawing.Size(70, 14);
            this.labelACSUserName.TabIndex = 4;
            this.labelACSUserName.Text = "&User Name:";
            this.labelACSUserName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxACSAddress
            // 
            this.textBoxACSAddress.Location = new System.Drawing.Point(123, 19);
            this.textBoxACSAddress.Name = "textBoxACSAddress";
            this.textBoxACSAddress.Size = new System.Drawing.Size(186, 20);
            this.textBoxACSAddress.TabIndex = 1;
            // 
            // labelACSIPAddress
            // 
            this.labelACSIPAddress.Location = new System.Drawing.Point(24, 21);
            this.labelACSIPAddress.Name = "labelACSIPAddress";
            this.labelACSIPAddress.Size = new System.Drawing.Size(88, 14);
            this.labelACSIPAddress.TabIndex = 1;
            this.labelACSIPAddress.Text = "IP Address/URL:";
            this.labelACSIPAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageSettings);
            this.tabControl1.Controls.Add(this.tabPageCameraAssociations);
            this.tabControl1.Controls.Add(this.tabPageEventMap);
            this.tabControl1.Controls.Add(this.tabPageSituations);
            this.tabControl1.Controls.Add(this.tabPageScripts);
            this.tabControl1.Location = new System.Drawing.Point(6, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(704, 428);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabPageSettings.BackgroundImage")));
            this.tabPageSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPageSettings.Controls.Add(this.ACSServerGroupBox);
            this.tabPageSettings.Controls.Add(this.VideoXpertSettingsGroupBox);
            this.tabPageSettings.Controls.Add(this.loggingGroupBox);
            this.tabPageSettings.Controls.Add(this.ACSSettingGroupBox);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(696, 402);
            this.tabPageSettings.TabIndex = 1;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // ACSServerGroupBox
            // 
            this.ACSServerGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ACSServerGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.ACSServerGroupBox.Controls.Add(this.ACSuseSSLCheckBox);
            this.ACSServerGroupBox.Controls.Add(this.textBoxACSPort);
            this.ACSServerGroupBox.Controls.Add(this.label1);
            this.ACSServerGroupBox.Controls.Add(this.textBoxACSIpAddress);
            this.ACSServerGroupBox.Controls.Add(this.label4);
            this.ACSServerGroupBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ACSServerGroupBox.Location = new System.Drawing.Point(7, 253);
            this.ACSServerGroupBox.Name = "ACSServerGroupBox";
            this.ACSServerGroupBox.Size = new System.Drawing.Size(683, 81);
            this.ACSServerGroupBox.TabIndex = 9;
            this.ACSServerGroupBox.TabStop = false;
            this.ACSServerGroupBox.Text = "Access Control Server (MiddleWare PC) Settings";
            // 
            // ACSuseSSLCheckBox
            // 
            this.ACSuseSSLCheckBox.AutoSize = true;
            this.ACSuseSSLCheckBox.Location = new System.Drawing.Point(343, 34);
            this.ACSuseSSLCheckBox.Name = "ACSuseSSLCheckBox";
            this.ACSuseSSLCheckBox.Size = new System.Drawing.Size(66, 17);
            this.ACSuseSSLCheckBox.TabIndex = 10;
            this.ACSuseSSLCheckBox.Text = "use SSL";
            this.ACSuseSSLCheckBox.UseVisualStyleBackColor = true;
            this.ACSuseSSLCheckBox.Visible = false;
            this.ACSuseSSLCheckBox.CheckedChanged += new System.EventHandler(this.ACSuseSSLCheckBox_CheckedChanged);
            // 
            // textBoxACSPort
            // 
            this.textBoxACSPort.Location = new System.Drawing.Point(267, 32);
            this.textBoxACSPort.Name = "textBoxACSPort";
            this.textBoxACSPort.Size = new System.Drawing.Size(39, 20);
            this.textBoxACSPort.TabIndex = 6;
            this.textBoxACSPort.TextChanged += new System.EventHandler(this.textBoxACSPort_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(238, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 14);
            this.label1.TabIndex = 9;
            this.label1.Text = "Port:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxACSIpAddress
            // 
            this.textBoxACSIpAddress.Location = new System.Drawing.Point(120, 31);
            this.textBoxACSIpAddress.Name = "textBoxACSIpAddress";
            this.textBoxACSIpAddress.Size = new System.Drawing.Size(116, 20);
            this.textBoxACSIpAddress.TabIndex = 5;
            this.textBoxACSIpAddress.TextChanged += new System.EventHandler(this.textBoxACSIpAddress_TextChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(36, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 14);
            this.label4.TabIndex = 1;
            this.label4.Text = "IP Address:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // VideoXpertSettingsGroupBox
            // 
            this.VideoXpertSettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VideoXpertSettingsGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.VideoXpertSettingsGroupBox.Controls.Add(this.VxConnectionPictureBox);
            this.VideoXpertSettingsGroupBox.Controls.Add(this.textBoxVxPort);
            this.VideoXpertSettingsGroupBox.Controls.Add(this.labelVxPort);
            this.VideoXpertSettingsGroupBox.Controls.Add(this.buttonConnectVx);
            this.VideoXpertSettingsGroupBox.Controls.Add(this.textBoxVxPassword);
            this.VideoXpertSettingsGroupBox.Controls.Add(this.labelVxPassword);
            this.VideoXpertSettingsGroupBox.Controls.Add(this.textBoxVxUserName);
            this.VideoXpertSettingsGroupBox.Controls.Add(this.labelVxUsername);
            this.VideoXpertSettingsGroupBox.Controls.Add(this.textBoxVxIpAddress);
            this.VideoXpertSettingsGroupBox.Controls.Add(this.labelVxAddress);
            this.VideoXpertSettingsGroupBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.VideoXpertSettingsGroupBox.Location = new System.Drawing.Point(7, 128);
            this.VideoXpertSettingsGroupBox.Name = "VideoXpertSettingsGroupBox";
            this.VideoXpertSettingsGroupBox.Padding = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.VideoXpertSettingsGroupBox.Size = new System.Drawing.Size(683, 116);
            this.VideoXpertSettingsGroupBox.TabIndex = 8;
            this.VideoXpertSettingsGroupBox.TabStop = false;
            this.VideoXpertSettingsGroupBox.Text = "VideoXpert Settings";
            // 
            // VxConnectionPictureBox
            // 
            this.VxConnectionPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.VxConnectionPictureBox.Image = global::_VX_ACS_Administration.Properties.Resources.Red_Dot;
            this.VxConnectionPictureBox.Location = new System.Drawing.Point(653, 16);
            this.VxConnectionPictureBox.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.VxConnectionPictureBox.MaximumSize = new System.Drawing.Size(20, 20);
            this.VxConnectionPictureBox.Name = "VxConnectionPictureBox";
            this.VxConnectionPictureBox.Size = new System.Drawing.Size(20, 20);
            this.VxConnectionPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.VxConnectionPictureBox.TabIndex = 10;
            this.VxConnectionPictureBox.TabStop = false;
            // 
            // textBoxVxPort
            // 
            this.textBoxVxPort.Location = new System.Drawing.Point(267, 19);
            this.textBoxVxPort.Name = "textBoxVxPort";
            this.textBoxVxPort.Size = new System.Drawing.Size(39, 20);
            this.textBoxVxPort.TabIndex = 6;
            this.textBoxVxPort.TextChanged += new System.EventHandler(this.textBoxVxPort_TextChanged);
            // 
            // labelVxPort
            // 
            this.labelVxPort.Location = new System.Drawing.Point(238, 21);
            this.labelVxPort.Name = "labelVxPort";
            this.labelVxPort.Size = new System.Drawing.Size(31, 14);
            this.labelVxPort.TabIndex = 9;
            this.labelVxPort.Text = "Port:";
            this.labelVxPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonConnectVx
            // 
            this.buttonConnectVx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConnectVx.BackColor = System.Drawing.Color.Transparent;
            this.buttonConnectVx.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonConnectVx.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonConnectVx.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonConnectVx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConnectVx.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonConnectVx.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonConnectVx.Location = new System.Drawing.Point(572, 66);
            this.buttonConnectVx.Name = "buttonConnectVx";
            this.buttonConnectVx.Size = new System.Drawing.Size(77, 32);
            this.buttonConnectVx.TabIndex = 9;
            this.buttonConnectVx.Text = "Connect";
            this.buttonConnectVx.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonConnectVx.UseVisualStyleBackColor = false;
            this.buttonConnectVx.Click += new System.EventHandler(this.buttonConnectVx_Click);
            // 
            // textBoxVxPassword
            // 
            this.textBoxVxPassword.Location = new System.Drawing.Point(120, 82);
            this.textBoxVxPassword.Name = "textBoxVxPassword";
            this.textBoxVxPassword.PasswordChar = '*';
            this.textBoxVxPassword.Size = new System.Drawing.Size(186, 20);
            this.textBoxVxPassword.TabIndex = 8;
            this.textBoxVxPassword.TextChanged += new System.EventHandler(this.textBoxVxPassword_TextChanged);
            // 
            // labelVxPassword
            // 
            this.labelVxPassword.Location = new System.Drawing.Point(50, 84);
            this.labelVxPassword.Name = "labelVxPassword";
            this.labelVxPassword.Size = new System.Drawing.Size(59, 14);
            this.labelVxPassword.TabIndex = 6;
            this.labelVxPassword.Text = "&Password:";
            this.labelVxPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxVxUserName
            // 
            this.textBoxVxUserName.Location = new System.Drawing.Point(120, 50);
            this.textBoxVxUserName.Name = "textBoxVxUserName";
            this.textBoxVxUserName.Size = new System.Drawing.Size(186, 20);
            this.textBoxVxUserName.TabIndex = 7;
            this.textBoxVxUserName.TextChanged += new System.EventHandler(this.textBoxVxUserName_TextChanged);
            // 
            // labelVxUsername
            // 
            this.labelVxUsername.Location = new System.Drawing.Point(39, 52);
            this.labelVxUsername.Name = "labelVxUsername";
            this.labelVxUsername.Size = new System.Drawing.Size(70, 14);
            this.labelVxUsername.TabIndex = 4;
            this.labelVxUsername.Text = "&User Name:";
            this.labelVxUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxVxIpAddress
            // 
            this.textBoxVxIpAddress.Location = new System.Drawing.Point(120, 18);
            this.textBoxVxIpAddress.Name = "textBoxVxIpAddress";
            this.textBoxVxIpAddress.Size = new System.Drawing.Size(116, 20);
            this.textBoxVxIpAddress.TabIndex = 5;
            this.textBoxVxIpAddress.TextChanged += new System.EventHandler(this.textBoxVxIpAddress_TextChanged);
            // 
            // labelVxAddress
            // 
            this.labelVxAddress.Location = new System.Drawing.Point(36, 20);
            this.labelVxAddress.Name = "labelVxAddress";
            this.labelVxAddress.Size = new System.Drawing.Size(73, 14);
            this.labelVxAddress.TabIndex = 1;
            this.labelVxAddress.Text = "IP Address:";
            this.labelVxAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // loggingGroupBox
            // 
            this.loggingGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loggingGroupBox.Controls.Add(this.label11);
            this.loggingGroupBox.Controls.Add(this.numericUpDownRetention);
            this.loggingGroupBox.Controls.Add(this.label10);
            this.loggingGroupBox.Controls.Add(this.checkBoxError);
            this.loggingGroupBox.Controls.Add(this.checkBoxOperation);
            this.loggingGroupBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.loggingGroupBox.Location = new System.Drawing.Point(6, 342);
            this.loggingGroupBox.Name = "loggingGroupBox";
            this.loggingGroupBox.Size = new System.Drawing.Size(684, 54);
            this.loggingGroupBox.TabIndex = 7;
            this.loggingGroupBox.TabStop = false;
            this.loggingGroupBox.Text = "Logging";
            this.loggingGroupBox.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(549, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Days";
            // 
            // numericUpDownRetention
            // 
            this.numericUpDownRetention.Location = new System.Drawing.Point(465, 19);
            this.numericUpDownRetention.Name = "numericUpDownRetention";
            this.numericUpDownRetention.Size = new System.Drawing.Size(78, 20);
            this.numericUpDownRetention.TabIndex = 15;
            this.numericUpDownRetention.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownRetention.ValueChanged += new System.EventHandler(this.numericUpDownRetention_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(400, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Retentio&n:";
            // 
            // checkBoxError
            // 
            this.checkBoxError.AutoSize = true;
            this.checkBoxError.Checked = true;
            this.checkBoxError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxError.Location = new System.Drawing.Point(250, 22);
            this.checkBoxError.Name = "checkBoxError";
            this.checkBoxError.Size = new System.Drawing.Size(48, 17);
            this.checkBoxError.TabIndex = 11;
            this.checkBoxError.Text = "&Error";
            this.checkBoxError.UseVisualStyleBackColor = true;
            this.checkBoxError.CheckedChanged += new System.EventHandler(this.checkBoxError_CheckedChanged);
            // 
            // checkBoxOperation
            // 
            this.checkBoxOperation.AutoSize = true;
            this.checkBoxOperation.Location = new System.Drawing.Point(50, 22);
            this.checkBoxOperation.Name = "checkBoxOperation";
            this.checkBoxOperation.Size = new System.Drawing.Size(72, 17);
            this.checkBoxOperation.TabIndex = 10;
            this.checkBoxOperation.Text = "&Operation";
            this.checkBoxOperation.UseVisualStyleBackColor = true;
            this.checkBoxOperation.CheckedChanged += new System.EventHandler(this.checkBoxOperation_CheckedChanged);
            // 
            // tabPageCameraAssociations
            // 
            this.tabPageCameraAssociations.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabPageCameraAssociations.BackgroundImage")));
            this.tabPageCameraAssociations.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPageCameraAssociations.Controls.Add(this.groupBox1);
            this.tabPageCameraAssociations.Location = new System.Drawing.Point(4, 22);
            this.tabPageCameraAssociations.Name = "tabPageCameraAssociations";
            this.tabPageCameraAssociations.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCameraAssociations.Size = new System.Drawing.Size(696, 402);
            this.tabPageCameraAssociations.TabIndex = 0;
            this.tabPageCameraAssociations.Text = "CameraAssociations";
            this.tabPageCameraAssociations.UseVisualStyleBackColor = true;
            // 
            // tabPageEventMap
            // 
            this.tabPageEventMap.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabPageEventMap.BackgroundImage")));
            this.tabPageEventMap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPageEventMap.Controls.Add(this.eventInjectionGroupBox);
            this.tabPageEventMap.Location = new System.Drawing.Point(4, 22);
            this.tabPageEventMap.Name = "tabPageEventMap";
            this.tabPageEventMap.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEventMap.Size = new System.Drawing.Size(696, 402);
            this.tabPageEventMap.TabIndex = 2;
            this.tabPageEventMap.Text = "EventMap";
            this.tabPageEventMap.UseVisualStyleBackColor = true;
            // 
            // eventInjectionGroupBox
            // 
            this.eventInjectionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventInjectionGroupBox.Controls.Add(this.eventMapDataGrid);
            this.eventInjectionGroupBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.eventInjectionGroupBox.Location = new System.Drawing.Point(7, 6);
            this.eventInjectionGroupBox.Name = "eventInjectionGroupBox";
            this.eventInjectionGroupBox.Size = new System.Drawing.Size(683, 390);
            this.eventInjectionGroupBox.TabIndex = 0;
            this.eventInjectionGroupBox.TabStop = false;
            this.eventInjectionGroupBox.Text = "Event Injection";
            // 
            // eventMapDataGrid
            // 
            this.eventMapDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventMapDataGrid.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.eventMapDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.eventMapDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DirectionColumn,
            this.SituationColumn,
            this.ACSEventColumn,
            this.RunScriptsColumn,
            this.AckScriptsColumn});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.eventMapDataGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.eventMapDataGrid.Location = new System.Drawing.Point(13, 19);
            this.eventMapDataGrid.Name = "eventMapDataGrid";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.eventMapDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.eventMapDataGrid.Size = new System.Drawing.Size(661, 365);
            this.eventMapDataGrid.TabIndex = 3;
            this.eventMapDataGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.eventMapDataGrid_CellValueChanged);
            this.eventMapDataGrid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.eventMapDataGrid_RowsAdded);
            this.eventMapDataGrid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.eventMapDataGrid_RowsRemoved);
            this.eventMapDataGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.eventMapDataGrid_MouseClick);
            // 
            // DirectionColumn
            // 
            this.DirectionColumn.HeaderText = "Direction";
            this.DirectionColumn.Items.AddRange(new object[] {
            "To VideoXpert",
            "To ACS"});
            this.DirectionColumn.Name = "DirectionColumn";
            this.DirectionColumn.Width = 110;
            // 
            // SituationColumn
            // 
            this.SituationColumn.DataSource = this.situationBindingSource;
            this.SituationColumn.HeaderText = "VideoXpert Situation";
            this.SituationColumn.MinimumWidth = 240;
            this.SituationColumn.Name = "SituationColumn";
            this.SituationColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SituationColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.SituationColumn.ToolTipText = "Custom VideoXpert Situation to Inject into VideoXpert";
            this.SituationColumn.Width = 240;
            // 
            // ACSEventColumn
            // 
            this.ACSEventColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ACSEventColumn.DataSource = this.feenicsEventsBindingSource;
            this.ACSEventColumn.HeaderText = "Access Control Event";
            this.ACSEventColumn.MinimumWidth = 240;
            this.ACSEventColumn.Name = "ACSEventColumn";
            this.ACSEventColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ACSEventColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ACSEventColumn.ToolTipText = "Event from Access Control System to Match to a VideoXpert Situation";
            // 
            // RunScriptsColumn
            // 
            this.RunScriptsColumn.HeaderText = "Scripts";
            this.RunScriptsColumn.Name = "RunScriptsColumn";
            this.RunScriptsColumn.Width = 50;
            // 
            // AckScriptsColumn
            // 
            this.AckScriptsColumn.HeaderText = "Ack Scripts";
            this.AckScriptsColumn.Name = "AckScriptsColumn";
            this.AckScriptsColumn.Width = 50;
            // 
            // tabPageSituations
            // 
            this.tabPageSituations.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabPageSituations.BackgroundImage")));
            this.tabPageSituations.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPageSituations.Controls.Add(this.situationsGroupBox);
            this.tabPageSituations.Location = new System.Drawing.Point(4, 22);
            this.tabPageSituations.Name = "tabPageSituations";
            this.tabPageSituations.Size = new System.Drawing.Size(696, 402);
            this.tabPageSituations.TabIndex = 3;
            this.tabPageSituations.Text = "Custom Situations";
            this.tabPageSituations.UseVisualStyleBackColor = true;
            // 
            // situationsGroupBox
            // 
            this.situationsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.situationsGroupBox.Controls.Add(this.companyTextBox);
            this.situationsGroupBox.Controls.Add(this.companyLabel);
            this.situationsGroupBox.Controls.Add(this.customSituationDataGrid);
            this.situationsGroupBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.situationsGroupBox.Location = new System.Drawing.Point(4, 4);
            this.situationsGroupBox.Name = "situationsGroupBox";
            this.situationsGroupBox.Size = new System.Drawing.Size(687, 395);
            this.situationsGroupBox.TabIndex = 0;
            this.situationsGroupBox.TabStop = false;
            this.situationsGroupBox.Text = "Custom VideoXpert Situations";
            // 
            // companyTextBox
            // 
            this.companyTextBox.Enabled = false;
            this.companyTextBox.Location = new System.Drawing.Point(289, 27);
            this.companyTextBox.MaxLength = 64;
            this.companyTextBox.Name = "companyTextBox";
            this.companyTextBox.Size = new System.Drawing.Size(244, 20);
            this.companyTextBox.TabIndex = 5;
            this.companyTextBox.Text = "GenericACS";
            // 
            // companyLabel
            // 
            this.companyLabel.AutoSize = true;
            this.companyLabel.Location = new System.Drawing.Point(90, 30);
            this.companyLabel.Name = "companyLabel";
            this.companyLabel.Size = new System.Drawing.Size(187, 13);
            this.companyLabel.TabIndex = 4;
            this.companyLabel.Text = "Company Name (External Event Type)";
            // 
            // customSituationDataGrid
            // 
            this.customSituationDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customSituationDataGrid.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.customSituationDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customSituationDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TypeColumn,
            this.SeverityColumn,
            this.LogColumn,
            this.NotifyColumn,
            this.BannerColumn,
            this.ExpandBannerColumn,
            this.AudibleColumn,
            this.NeedAckColumn,
            this.AutoAckColumn,
            this.SnoozeColumn});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customSituationDataGrid.DefaultCellStyle = dataGridViewCellStyle4;
            this.customSituationDataGrid.Location = new System.Drawing.Point(6, 62);
            this.customSituationDataGrid.Name = "customSituationDataGrid";
            this.customSituationDataGrid.Size = new System.Drawing.Size(675, 327);
            this.customSituationDataGrid.TabIndex = 3;
            this.customSituationDataGrid.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.customSituationDataGrid_CellLeave);
            this.customSituationDataGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.customSituationDataGrid_CellValueChanged);
            this.customSituationDataGrid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.customSituationDataGrid_RowsAdded);
            this.customSituationDataGrid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.customSituationDataGrid_RowsRemoved);
            this.customSituationDataGrid.Leave += new System.EventHandler(this.eventMapDataGrid_Leave);
            this.customSituationDataGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.customSituationDataGrid_MouseClick);
            // 
            // TypeColumn
            // 
            this.TypeColumn.Frozen = true;
            this.TypeColumn.HeaderText = "External Situation Type";
            this.TypeColumn.MaxInputLength = 64;
            this.TypeColumn.MinimumWidth = 200;
            this.TypeColumn.Name = "TypeColumn";
            this.TypeColumn.ToolTipText = "Custom VideoXpert Situation";
            this.TypeColumn.Width = 200;
            // 
            // SeverityColumn
            // 
            this.SeverityColumn.HeaderText = "Severity";
            this.SeverityColumn.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.SeverityColumn.MinimumWidth = 50;
            this.SeverityColumn.Name = "SeverityColumn";
            this.SeverityColumn.ToolTipText = "Severity of Custom Situation";
            this.SeverityColumn.Width = 50;
            // 
            // LogColumn
            // 
            this.LogColumn.HeaderText = "Log";
            this.LogColumn.MinimumWidth = 30;
            this.LogColumn.Name = "LogColumn";
            this.LogColumn.ToolTipText = "Whether or not to Log Situation in VideoXpert";
            this.LogColumn.Width = 30;
            // 
            // NotifyColumn
            // 
            this.NotifyColumn.HeaderText = "Notify";
            this.NotifyColumn.MinimumWidth = 40;
            this.NotifyColumn.Name = "NotifyColumn";
            this.NotifyColumn.ToolTipText = "Whether or not to Notify users of Situation in VideoXpert";
            this.NotifyColumn.Width = 40;
            // 
            // BannerColumn
            // 
            this.BannerColumn.HeaderText = "Banner";
            this.BannerColumn.MinimumWidth = 50;
            this.BannerColumn.Name = "BannerColumn";
            this.BannerColumn.ToolTipText = "Whether or not to display a banner in VideoXpert";
            this.BannerColumn.Width = 50;
            // 
            // ExpandBannerColumn
            // 
            this.ExpandBannerColumn.HeaderText = "Expand Banner";
            this.ExpandBannerColumn.MinimumWidth = 50;
            this.ExpandBannerColumn.Name = "ExpandBannerColumn";
            this.ExpandBannerColumn.Width = 50;
            // 
            // AudibleColumn
            // 
            this.AudibleColumn.HeaderText = "Audible";
            this.AudibleColumn.MinimumWidth = 50;
            this.AudibleColumn.Name = "AudibleColumn";
            this.AudibleColumn.ToolTipText = "Whether or not to produce audible notification in VideoXpert";
            this.AudibleColumn.Width = 50;
            // 
            // NeedAckColumn
            // 
            this.NeedAckColumn.HeaderText = "Need Ack";
            this.NeedAckColumn.MinimumWidth = 40;
            this.NeedAckColumn.Name = "NeedAckColumn";
            this.NeedAckColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NeedAckColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.NeedAckColumn.ToolTipText = "Whether or not this Situation needs to be Acknowledged";
            this.NeedAckColumn.Width = 40;
            // 
            // AutoAckColumn
            // 
            this.AutoAckColumn.HeaderText = "AutoAck";
            this.AutoAckColumn.MinimumWidth = 50;
            this.AutoAckColumn.Name = "AutoAckColumn";
            this.AutoAckColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.AutoAckColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AutoAckColumn.ToolTipText = "Time in seconds to Automatically Acknowledge this Situation";
            this.AutoAckColumn.Width = 50;
            // 
            // SnoozeColumn
            // 
            this.SnoozeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SnoozeColumn.HeaderText = "Snooze Intervals";
            this.SnoozeColumn.MinimumWidth = 120;
            this.SnoozeColumn.Name = "SnoozeColumn";
            this.SnoozeColumn.ToolTipText = "Comma delimited string representing snooze intervals in minutes";
            // 
            // tabPageScripts
            // 
            this.tabPageScripts.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabPageScripts.BackgroundImage")));
            this.tabPageScripts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPageScripts.Controls.Add(this.scriptsDataGrid);
            this.tabPageScripts.Location = new System.Drawing.Point(4, 22);
            this.tabPageScripts.Name = "tabPageScripts";
            this.tabPageScripts.Size = new System.Drawing.Size(696, 402);
            this.tabPageScripts.TabIndex = 4;
            this.tabPageScripts.Text = "Scripts";
            this.tabPageScripts.UseVisualStyleBackColor = true;
            // 
            // scriptsDataGrid
            // 
            this.scriptsDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptsDataGrid.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.scriptsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.scriptsDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DBNumberColumn,
            this.ScriptNumberColumn,
            this.ScriptNameColumn,
            this.ActionsColumn});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.scriptsDataGrid.DefaultCellStyle = dataGridViewCellStyle5;
            this.scriptsDataGrid.Location = new System.Drawing.Point(18, 19);
            this.scriptsDataGrid.Name = "scriptsDataGrid";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.scriptsDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.scriptsDataGrid.Size = new System.Drawing.Size(661, 365);
            this.scriptsDataGrid.TabIndex = 4;
            this.scriptsDataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ScriptsDataGrid_CellContentClick);
            this.scriptsDataGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.scriptsDataGrid_CellEndEdit);
            this.scriptsDataGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.scriptsDataGrid_CellValueChanged);
            this.scriptsDataGrid.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.scriptsDataGrid_RowLeave);
            this.scriptsDataGrid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.scriptsDataGrid_RowsAdded);
            this.scriptsDataGrid.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.scriptsDataGrid_RowsRemoved);
            this.scriptsDataGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.scriptsDataGrid_MouseClick);
            // 
            // DBNumberColumn
            // 
            this.DBNumberColumn.HeaderText = "DBNumber";
            this.DBNumberColumn.Name = "DBNumberColumn";
            this.DBNumberColumn.Visible = false;
            // 
            // ScriptNumberColumn
            // 
            this.ScriptNumberColumn.HeaderText = "Number";
            this.ScriptNumberColumn.Name = "ScriptNumberColumn";
            this.ScriptNumberColumn.Width = 55;
            // 
            // ScriptNameColumn
            // 
            this.ScriptNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ScriptNameColumn.HeaderText = "Name";
            this.ScriptNameColumn.Name = "ScriptNameColumn";
            // 
            // ActionsColumn
            // 
            this.ActionsColumn.HeaderText = "Actions";
            this.ActionsColumn.Name = "ActionsColumn";
            this.ActionsColumn.Text = "Edit";
            this.ActionsColumn.UseColumnTextForButtonValue = true;
            this.ActionsColumn.Width = 50;
            // 
            // buttonReload
            // 
            this.buttonReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonReload.BackColor = System.Drawing.Color.Transparent;
            this.buttonReload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonReload.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonReload.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonReload.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonReload.Image = ((System.Drawing.Image)(resources.GetObject("buttonReload.Image")));
            this.buttonReload.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonReload.Location = new System.Drawing.Point(6, 446);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Size = new System.Drawing.Size(84, 36);
            this.buttonReload.TabIndex = 1;
            this.buttonReload.Text = "Re&fresh";
            this.buttonReload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonReload.UseVisualStyleBackColor = false;
            this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // panelProgress
            // 
            this.panelProgress.BackColor = System.Drawing.Color.Transparent;
            this.panelProgress.Controls.Add(this.labelProgress);
            this.panelProgress.Controls.Add(this.buttonCancelProgress);
            this.panelProgress.Controls.Add(this.progressBar1);
            this.panelProgress.Location = new System.Drawing.Point(96, 444);
            this.panelProgress.Name = "panelProgress";
            this.panelProgress.Size = new System.Drawing.Size(332, 42);
            this.panelProgress.TabIndex = 20;
            this.panelProgress.Visible = false;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(23, 15);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(68, 13);
            this.labelProgress.TabIndex = 10;
            this.labelProgress.Text = "Processing...";
            // 
            // buttonCancelProgress
            // 
            this.buttonCancelProgress.FlatAppearance.BorderSize = 0;
            this.buttonCancelProgress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancelProgress.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonCancelProgress.Image = ((System.Drawing.Image)(resources.GetObject("buttonCancelProgress.Image")));
            this.buttonCancelProgress.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCancelProgress.Location = new System.Drawing.Point(244, 4);
            this.buttonCancelProgress.Name = "buttonCancelProgress";
            this.buttonCancelProgress.Size = new System.Drawing.Size(74, 34);
            this.buttonCancelProgress.TabIndex = 9;
            this.buttonCancelProgress.Text = "C&ancel";
            this.buttonCancelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCancelProgress.UseVisualStyleBackColor = true;
            this.buttonCancelProgress.Click += new System.EventHandler(this.buttonCancelProgress_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 8);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(226, 26);
            this.progressBar1.TabIndex = 8;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FormAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(713, 494);
            this.Controls.Add(this.panelProgress);
            this.Controls.Add(this.buttonReload);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(729, 532);
            this.Name = "FormAdmin";
            this.Text = "AccessControl Integration Administration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAdmin_FormClosing_1);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cameraAssociationGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cameraBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.feenicsPeripheralsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.feenicsEventsBindingSource)).EndInit();
            this.ACSSettingGroupBox.ResumeLayout(false);
            this.ACSSettingGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AxConnectionPictureBox)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.ACSServerGroupBox.ResumeLayout(false);
            this.ACSServerGroupBox.PerformLayout();
            this.VideoXpertSettingsGroupBox.ResumeLayout(false);
            this.VideoXpertSettingsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VxConnectionPictureBox)).EndInit();
            this.loggingGroupBox.ResumeLayout(false);
            this.loggingGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRetention)).EndInit();
            this.tabPageCameraAssociations.ResumeLayout(false);
            this.tabPageEventMap.ResumeLayout(false);
            this.eventInjectionGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.eventMapDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.situationBindingSource)).EndInit();
            this.tabPageSituations.ResumeLayout(false);
            this.situationsGroupBox.ResumeLayout(false);
            this.situationsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customSituationDataGrid)).EndInit();
            this.tabPageScripts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scriptsDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panelProgress.ResumeLayout(false);
            this.panelProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Button buttonOK;
        internal System.Windows.Forms.Button buttonCancel;
        internal System.Windows.Forms.Button buttonApply;
        internal System.Windows.Forms.DataGridView cameraAssociationGridView;
        private System.Windows.Forms.GroupBox ACSSettingGroupBox;
        private System.Windows.Forms.TextBox textBoxACSPassword;
        private System.Windows.Forms.Label labelACSPassword;
        private System.Windows.Forms.TextBox textBoxACSUserName;
        private System.Windows.Forms.Label labelACSUserName;
        private System.Windows.Forms.TextBox textBoxACSAddress;
        private System.Windows.Forms.Label labelACSIPAddress;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.TabPage tabPageCameraAssociations;
        internal System.Windows.Forms.Button buttonReload;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.GroupBox loggingGroupBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericUpDownRetention;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBoxError;
        private System.Windows.Forms.CheckBox checkBoxOperation;
        internal System.Windows.Forms.Button buttonConnectACS;
        private System.Windows.Forms.Panel panelProgress;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Button buttonCancelProgress;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox textBoxVxPort;
        private System.Windows.Forms.TextBox textBoxVxPassword;
        private System.Windows.Forms.TextBox textBoxVxUserName;
        private System.Windows.Forms.TextBox textBoxVxIpAddress;
        private System.Windows.Forms.Label labelVxAddress;
        private System.Windows.Forms.Label labelVxUsername;
        private System.Windows.Forms.Label labelVxPassword;
        internal System.Windows.Forms.Button buttonConnectVx;
        private System.Windows.Forms.Label labelVxPort;
        private System.Windows.Forms.GroupBox VideoXpertSettingsGroupBox;
        private System.Windows.Forms.TabPage tabPageEventMap;
        private System.Windows.Forms.GroupBox eventInjectionGroupBox;
        private System.Windows.Forms.TabPage tabPageSituations;
        private System.Windows.Forms.GroupBox situationsGroupBox;
        private System.Windows.Forms.TextBox companyTextBox;
        private System.Windows.Forms.Label companyLabel;
        private System.Windows.Forms.DataGridView customSituationDataGrid;
        private System.Windows.Forms.GroupBox ACSServerGroupBox;
        private System.Windows.Forms.TextBox textBoxACSPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxACSIpAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox ACSuseSSLCheckBox;
        private System.Windows.Forms.BindingSource situationBindingSource;
        private System.Windows.Forms.BindingSource cameraBindingSource;
        private System.Windows.Forms.BindingSource feenicsEventsBindingSource;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.PictureBox AxConnectionPictureBox;
        private System.Windows.Forms.PictureBox VxConnectionPictureBox;
        private System.Windows.Forms.DataGridView eventMapDataGrid;
        private System.Windows.Forms.BindingSource feenicsPeripheralsBindingSource;
        private System.Windows.Forms.DataGridViewComboBoxColumn CameraNameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn PeripheralColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn EventNameColumn;
        private System.Windows.Forms.TabPage tabPageScripts;
        private System.Windows.Forms.DataGridView scriptsDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn DBNumberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScriptNumberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScriptNameColumn;
        private System.Windows.Forms.DataGridViewButtonColumn ActionsColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn DirectionColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn SituationColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn ACSEventColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RunScriptsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AckScriptsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn SeverityColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn LogColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn NotifyColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn BannerColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ExpandBannerColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn AudibleColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn NeedAckColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AutoAckColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SnoozeColumn;
    }
}

