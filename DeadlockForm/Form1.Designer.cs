
namespace DeadlockForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Form1";


            this.btDeadlock = new System.Windows.Forms.Button();
            this.btNoLock = new System.Windows.Forms.Button();
            this.btNoLockCorrect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btDeadlock
            // 
            this.btDeadlock.Location = new System.Drawing.Point(48, 81);
            this.btDeadlock.Name = "btDeadlock";
            this.btDeadlock.Size = new System.Drawing.Size(206, 23);
            this.btDeadlock.TabIndex = 0;
            this.btDeadlock.Text = "deadlock";
            this.btDeadlock.UseVisualStyleBackColor = true;
            this.btDeadlock.Click += new System.EventHandler(this.btnLockForSure);
            // 
            // btNoLock
            // 
            this.btNoLock.Location = new System.Drawing.Point(48, 152);
            this.btNoLock.Name = "btNoLock";
            this.btNoLock.Size = new System.Drawing.Size(206, 23);
            this.btNoLock.TabIndex = 1;
            this.btNoLock.Text = "nolock";
            this.btNoLock.UseVisualStyleBackColor = true;
            this.btNoLock.Click += new System.EventHandler(this.btNoLock_Click);
            // 
            // btNoLockCorrect
            // 
            this.btNoLockCorrect.Location = new System.Drawing.Point(48, 211);
            this.btNoLockCorrect.Name = "btNoLockCorrect";
            this.btNoLockCorrect.Size = new System.Drawing.Size(206, 23);
            this.btNoLockCorrect.TabIndex = 2;
            this.btNoLockCorrect.Text = "nolock correct";
            this.btNoLockCorrect.UseVisualStyleBackColor = true;
            this.btNoLockCorrect.Click += new System.EventHandler(this.btNoLockCorrect_Click);
            // 
            // Form1
            // 
            this.Controls.Add(this.btNoLockCorrect);
            this.Controls.Add(this.btNoLock);
            this.Controls.Add(this.btDeadlock);



        }

        #endregion




        private System.Windows.Forms.Button btDeadlock;
        private System.Windows.Forms.Button btNoLock;
        private System.Windows.Forms.Button btNoLockCorrect;
    }
}

