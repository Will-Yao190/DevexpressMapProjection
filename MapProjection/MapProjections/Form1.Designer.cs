namespace MapProjections {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.mapControl = new DevExpress.XtraMap.MapControl();
            this.lbProjection = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.mapControl)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mapControl
            // 
            this.mapControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapControl.Location = new System.Drawing.Point(5, 5);
            this.mapControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.mapControl.Name = "mapControl";
            this.mapControl.Size = new System.Drawing.Size(683, 630);
            this.mapControl.TabIndex = 0;
            // 
            // lbProjection
            // 
            this.lbProjection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbProjection.FormattingEnabled = true;
            this.lbProjection.ItemHeight = 16;
            this.lbProjection.Location = new System.Drawing.Point(5, 22);
            this.lbProjection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbProjection.Name = "lbProjection";
            this.lbProjection.Size = new System.Drawing.Size(257, 608);
            this.lbProjection.TabIndex = 1;
            this.lbProjection.SelectedIndexChanged += new System.EventHandler(this.lbProjection_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Projection class:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbProjection);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(688, 5);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.panel1.Size = new System.Drawing.Size(267, 630);
            this.panel1.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 640);
            this.Controls.Add(this.mapControl);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mapControl)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraMap.MapControl mapControl;
        private System.Windows.Forms.ListBox lbProjection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
    }
}

