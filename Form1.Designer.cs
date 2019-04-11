namespace WebCam
{
    partial class Form1
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
            this.mDisplay = new WebCam.Dispaly();
            this.mClearButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mDisplay
            // 
            this.mDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mDisplay.Location = new System.Drawing.Point(12, 41);
            this.mDisplay.Name = "mDisplay";
            this.mDisplay.Size = new System.Drawing.Size(697, 424);
            this.mDisplay.TabIndex = 0;
            this.mDisplay.Text = "dispaly1";
            this.mDisplay.Click += new System.EventHandler(this.mDisplay_Click);
            // 
            // mClearButton
            // 
            this.mClearButton.Location = new System.Drawing.Point(12, 12);
            this.mClearButton.Name = "mClearButton";
            this.mClearButton.Size = new System.Drawing.Size(75, 23);
            this.mClearButton.TabIndex = 1;
            this.mClearButton.Text = "Clear";
            this.mClearButton.UseVisualStyleBackColor = true;
            this.mClearButton.Click += new System.EventHandler(this.mClearButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 477);
            this.Controls.Add(this.mClearButton);
            this.Controls.Add(this.mDisplay);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Dispaly mDisplay;
        private System.Windows.Forms.Button mClearButton;
    }
}

