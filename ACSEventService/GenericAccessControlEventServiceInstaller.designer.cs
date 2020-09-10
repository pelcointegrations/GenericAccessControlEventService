namespace GenericAccessControlEventService
{
    partial class GenericAccessControlEventServiceInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
            System.ServiceProcess.ServiceInstaller serviceInstaller1;
            serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            serviceProcessInstaller1.Password = "Pelco123";
            serviceProcessInstaller1.Username = ".\\DSNVSUser";
            // 
            // serviceInstaller1
            // 
            serviceInstaller1.Description = "Generic Access Control Event Service";
            serviceInstaller1.DisplayName = "GenericAccessControlEventService";
            serviceInstaller1.ServiceName = "GenericAccessControlEventService";
            serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // GenericAccessControlEventServiceInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            serviceProcessInstaller1,
            serviceInstaller1});

        }

        #endregion

    }
}