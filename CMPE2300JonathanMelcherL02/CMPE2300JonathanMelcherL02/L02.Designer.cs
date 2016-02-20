namespace CMPE2300JonathanMelcherL02
{
    partial class L02
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
            this.statsUI = new System.Windows.Forms.GroupBox();
            this.killRatioUI = new System.Windows.Forms.TextBox();
            this.missileLaunchedUI = new System.Windows.Forms.TextBox();
            this.incomingMissileDestroyedUI = new System.Windows.Forms.TextBox();
            this.incomingMissileTotalUI = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.startUI = new System.Windows.Forms.Button();
            this.pauseUI = new System.Windows.Forms.Button();
            this.restartUI = new System.Windows.Forms.Button();
            this.controlsUI = new System.Windows.Forms.GroupBox();
            this.terrainVerticalScaleUI = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.incomingMissileUI = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.explosionRadiusUI = new System.Windows.Forms.NumericUpDown();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.statsUI.SuspendLayout();
            this.controlsUI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.terrainVerticalScaleUI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.incomingMissileUI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.explosionRadiusUI)).BeginInit();
            this.SuspendLayout();
            // 
            // statsUI
            // 
            this.statsUI.Controls.Add(this.killRatioUI);
            this.statsUI.Controls.Add(this.missileLaunchedUI);
            this.statsUI.Controls.Add(this.incomingMissileDestroyedUI);
            this.statsUI.Controls.Add(this.incomingMissileTotalUI);
            this.statsUI.Controls.Add(this.label7);
            this.statsUI.Controls.Add(this.label6);
            this.statsUI.Controls.Add(this.label5);
            this.statsUI.Controls.Add(this.label4);
            this.statsUI.Location = new System.Drawing.Point(12, 41);
            this.statsUI.Name = "statsUI";
            this.statsUI.Size = new System.Drawing.Size(260, 118);
            this.statsUI.TabIndex = 0;
            this.statsUI.TabStop = false;
            this.statsUI.Text = "Statistics";
            // 
            // killRatioUI
            // 
            this.killRatioUI.Location = new System.Drawing.Point(157, 88);
            this.killRatioUI.Name = "killRatioUI";
            this.killRatioUI.ReadOnly = true;
            this.killRatioUI.Size = new System.Drawing.Size(93, 20);
            this.killRatioUI.TabIndex = 7;
            // 
            // missileLaunchedUI
            // 
            this.missileLaunchedUI.Location = new System.Drawing.Point(157, 66);
            this.missileLaunchedUI.Name = "missileLaunchedUI";
            this.missileLaunchedUI.ReadOnly = true;
            this.missileLaunchedUI.Size = new System.Drawing.Size(93, 20);
            this.missileLaunchedUI.TabIndex = 6;
            // 
            // incomingMissileDestroyedUI
            // 
            this.incomingMissileDestroyedUI.Location = new System.Drawing.Point(157, 44);
            this.incomingMissileDestroyedUI.Name = "incomingMissileDestroyedUI";
            this.incomingMissileDestroyedUI.ReadOnly = true;
            this.incomingMissileDestroyedUI.Size = new System.Drawing.Size(93, 20);
            this.incomingMissileDestroyedUI.TabIndex = 5;
            // 
            // incomingMissileTotalUI
            // 
            this.incomingMissileTotalUI.Location = new System.Drawing.Point(157, 22);
            this.incomingMissileTotalUI.Name = "incomingMissileTotalUI";
            this.incomingMissileTotalUI.ReadOnly = true;
            this.incomingMissileTotalUI.Size = new System.Drawing.Size(93, 20);
            this.incomingMissileTotalUI.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Kill Ratio:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Missiles Launched:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Incoming Missiles Destroyed:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Incoming Missile Total:";
            // 
            // startUI
            // 
            this.startUI.Location = new System.Drawing.Point(12, 12);
            this.startUI.Name = "startUI";
            this.startUI.Size = new System.Drawing.Size(75, 23);
            this.startUI.TabIndex = 1;
            this.startUI.Text = "Start";
            this.startUI.UseVisualStyleBackColor = true;
            this.startUI.Click += new System.EventHandler(this.startUI_Click);
            // 
            // pauseUI
            // 
            this.pauseUI.Location = new System.Drawing.Point(105, 12);
            this.pauseUI.Name = "pauseUI";
            this.pauseUI.Size = new System.Drawing.Size(75, 23);
            this.pauseUI.TabIndex = 2;
            this.pauseUI.Text = "Pause";
            this.pauseUI.UseVisualStyleBackColor = true;
            this.pauseUI.Click += new System.EventHandler(this.pauseUI_Click);
            // 
            // restartUI
            // 
            this.restartUI.Location = new System.Drawing.Point(197, 12);
            this.restartUI.Name = "restartUI";
            this.restartUI.Size = new System.Drawing.Size(75, 23);
            this.restartUI.TabIndex = 3;
            this.restartUI.Text = "Restart";
            this.restartUI.UseVisualStyleBackColor = true;
            this.restartUI.Click += new System.EventHandler(this.restartUI_Click);
            // 
            // controlsUI
            // 
            this.controlsUI.Controls.Add(this.terrainVerticalScaleUI);
            this.controlsUI.Controls.Add(this.label3);
            this.controlsUI.Controls.Add(this.label2);
            this.controlsUI.Controls.Add(this.incomingMissileUI);
            this.controlsUI.Controls.Add(this.label1);
            this.controlsUI.Controls.Add(this.explosionRadiusUI);
            this.controlsUI.Location = new System.Drawing.Point(12, 165);
            this.controlsUI.Name = "controlsUI";
            this.controlsUI.Size = new System.Drawing.Size(260, 103);
            this.controlsUI.TabIndex = 4;
            this.controlsUI.TabStop = false;
            this.controlsUI.Text = "Controls";
            // 
            // terrainVerticalScaleUI
            // 
            this.terrainVerticalScaleUI.Location = new System.Drawing.Point(157, 71);
            this.terrainVerticalScaleUI.Name = "terrainVerticalScaleUI";
            this.terrainVerticalScaleUI.Size = new System.Drawing.Size(93, 20);
            this.terrainVerticalScaleUI.TabIndex = 5;
            this.terrainVerticalScaleUI.ValueChanged += new System.EventHandler(this.terrainVerticalScaleUI_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Terrain Vertical Scale:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Incoming Missiles:";
            // 
            // incomingMissileUI
            // 
            this.incomingMissileUI.Location = new System.Drawing.Point(157, 45);
            this.incomingMissileUI.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.incomingMissileUI.Name = "incomingMissileUI";
            this.incomingMissileUI.Size = new System.Drawing.Size(93, 20);
            this.incomingMissileUI.TabIndex = 2;
            this.incomingMissileUI.ValueChanged += new System.EventHandler(this.incomingMissilesUI_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Explosion Radius:";
            // 
            // explosionRadiusUI
            // 
            this.explosionRadiusUI.Location = new System.Drawing.Point(157, 19);
            this.explosionRadiusUI.Name = "explosionRadiusUI";
            this.explosionRadiusUI.Size = new System.Drawing.Size(93, 20);
            this.explosionRadiusUI.TabIndex = 0;
            this.explosionRadiusUI.ValueChanged += new System.EventHandler(this.explosionRadiusUI_ValueChanged);
            // 
            // timer
            // 
            this.timer.Interval = 50;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // L02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 276);
            this.Controls.Add(this.statsUI);
            this.Controls.Add(this.controlsUI);
            this.Controls.Add(this.restartUI);
            this.Controls.Add(this.pauseUI);
            this.Controls.Add(this.startUI);
            this.MaximumSize = new System.Drawing.Size(300, 315);
            this.MinimumSize = new System.Drawing.Size(300, 315);
            this.Name = "L02";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MissileCommand";
            this.Load += new System.EventHandler(this.L02_Load);
            this.statsUI.ResumeLayout(false);
            this.statsUI.PerformLayout();
            this.controlsUI.ResumeLayout(false);
            this.controlsUI.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.terrainVerticalScaleUI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.incomingMissileUI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.explosionRadiusUI)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox statsUI;
        private System.Windows.Forms.Button startUI;
        private System.Windows.Forms.Button pauseUI;
        private System.Windows.Forms.Button restartUI;
        private System.Windows.Forms.GroupBox controlsUI;
        private System.Windows.Forms.NumericUpDown terrainVerticalScaleUI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown incomingMissileUI;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown explosionRadiusUI;
        private System.Windows.Forms.TextBox killRatioUI;
        private System.Windows.Forms.TextBox missileLaunchedUI;
        private System.Windows.Forms.TextBox incomingMissileDestroyedUI;
        private System.Windows.Forms.TextBox incomingMissileTotalUI;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timer;
    }
}

