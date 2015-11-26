namespace FractalBrowser
{
    partial class JuliaEditor
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
            this.MainPanelOfJuliaEditor = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.CreateButton = new System.Windows.Forms.Button();
            this.ImaginePartEdit = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.RealPartEdit = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.BottomEdgeEdit = new System.Windows.Forms.TextBox();
            this.TopEdgeEdit = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.RightEdgeEdit = new System.Windows.Forms.TextBox();
            this.RightEdgeLabel = new System.Windows.Forms.Label();
            this.LeftEdgeLabel = new System.Windows.Forms.Label();
            this.LeftEdgeEdit = new System.Windows.Forms.TextBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.IterCountLabel = new System.Windows.Forms.Label();
            this.EditDescriptor = new System.Windows.Forms.ToolTip(this.components);
            this.MainPanelOfJuliaEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // MainPanelOfJuliaEditor
            // 
            this.MainPanelOfJuliaEditor.Controls.Add(this.button2);
            this.MainPanelOfJuliaEditor.Controls.Add(this.CreateButton);
            this.MainPanelOfJuliaEditor.Controls.Add(this.ImaginePartEdit);
            this.MainPanelOfJuliaEditor.Controls.Add(this.label7);
            this.MainPanelOfJuliaEditor.Controls.Add(this.RealPartEdit);
            this.MainPanelOfJuliaEditor.Controls.Add(this.label6);
            this.MainPanelOfJuliaEditor.Controls.Add(this.BottomEdgeEdit);
            this.MainPanelOfJuliaEditor.Controls.Add(this.TopEdgeEdit);
            this.MainPanelOfJuliaEditor.Controls.Add(this.label5);
            this.MainPanelOfJuliaEditor.Controls.Add(this.label4);
            this.MainPanelOfJuliaEditor.Controls.Add(this.RightEdgeEdit);
            this.MainPanelOfJuliaEditor.Controls.Add(this.RightEdgeLabel);
            this.MainPanelOfJuliaEditor.Controls.Add(this.LeftEdgeLabel);
            this.MainPanelOfJuliaEditor.Controls.Add(this.LeftEdgeEdit);
            this.MainPanelOfJuliaEditor.Controls.Add(this.numericUpDown1);
            this.MainPanelOfJuliaEditor.Controls.Add(this.IterCountLabel);
            this.MainPanelOfJuliaEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.MainPanelOfJuliaEditor.Location = new System.Drawing.Point(12, 12);
            this.MainPanelOfJuliaEditor.Name = "MainPanelOfJuliaEditor";
            this.MainPanelOfJuliaEditor.Size = new System.Drawing.Size(320, 250);
            this.MainPanelOfJuliaEditor.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(162, 208);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(135, 32);
            this.button2.TabIndex = 15;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.CancelReturn);
            // 
            // CreateButton
            // 
            this.CreateButton.Location = new System.Drawing.Point(7, 208);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(135, 32);
            this.CreateButton.TabIndex = 14;
            this.CreateButton.Text = "Создать";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.ReturnEditedData);
            // 
            // ImaginePartEdit
            // 
            this.ImaginePartEdit.Location = new System.Drawing.Point(162, 176);
            this.ImaginePartEdit.Name = "ImaginePartEdit";
            this.ImaginePartEdit.Size = new System.Drawing.Size(135, 26);
            this.ImaginePartEdit.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(158, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 20);
            this.label7.TabIndex = 12;
            this.label7.Text = "Мнимая часть:";
            // 
            // RealPartEdit
            // 
            this.RealPartEdit.Location = new System.Drawing.Point(7, 176);
            this.RealPartEdit.Name = "RealPartEdit";
            this.RealPartEdit.Size = new System.Drawing.Size(135, 26);
            this.RealPartEdit.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 153);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(149, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Реальная часть:";
            // 
            // BottomEdgeEdit
            // 
            this.BottomEdgeEdit.Location = new System.Drawing.Point(162, 124);
            this.BottomEdgeEdit.Name = "BottomEdgeEdit";
            this.BottomEdgeEdit.Size = new System.Drawing.Size(135, 26);
            this.BottomEdgeEdit.TabIndex = 9;
            // 
            // TopEdgeEdit
            // 
            this.TopEdgeEdit.Location = new System.Drawing.Point(7, 124);
            this.TopEdgeEdit.Name = "TopEdgeEdit";
            this.TopEdgeEdit.Size = new System.Drawing.Size(135, 26);
            this.TopEdgeEdit.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(159, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = "Низняя граница:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Верхняя граница:";
            // 
            // RightEdgeEdit
            // 
            this.RightEdgeEdit.Location = new System.Drawing.Point(162, 72);
            this.RightEdgeEdit.Name = "RightEdgeEdit";
            this.RightEdgeEdit.Size = new System.Drawing.Size(135, 26);
            this.RightEdgeEdit.TabIndex = 5;
            // 
            // RightEdgeLabel
            // 
            this.RightEdgeLabel.AutoSize = true;
            this.RightEdgeLabel.Location = new System.Drawing.Point(158, 49);
            this.RightEdgeLabel.Name = "RightEdgeLabel";
            this.RightEdgeLabel.Size = new System.Drawing.Size(150, 20);
            this.RightEdgeLabel.TabIndex = 4;
            this.RightEdgeLabel.Text = "Правая граница:";
            // 
            // LeftEdgeLabel
            // 
            this.LeftEdgeLabel.AutoSize = true;
            this.LeftEdgeLabel.Location = new System.Drawing.Point(3, 49);
            this.LeftEdgeLabel.Name = "LeftEdgeLabel";
            this.LeftEdgeLabel.Size = new System.Drawing.Size(139, 20);
            this.LeftEdgeLabel.TabIndex = 3;
            this.LeftEdgeLabel.Text = "Левая граница:";
            // 
            // LeftEdgeEdit
            // 
            this.LeftEdgeEdit.Location = new System.Drawing.Point(7, 72);
            this.LeftEdgeEdit.Name = "LeftEdgeEdit";
            this.LeftEdgeEdit.Size = new System.Drawing.Size(135, 26);
            this.LeftEdgeEdit.TabIndex = 2;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown1.Location = new System.Drawing.Point(59, 20);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(190, 26);
            this.numericUpDown1.TabIndex = 1;
            this.EditDescriptor.SetToolTip(this.numericUpDown1, "От количества итераций зависить точность построения фрактала, \r\nи также количеств" +
        "о итераций может влият на некоторые цветовые режимы.\r\nДля обыкновенной джулии ре" +
        "комендуеться 10000 итераций.");
            // 
            // IterCountLabel
            // 
            this.IterCountLabel.AutoSize = true;
            this.IterCountLabel.Location = new System.Drawing.Point(55, -3);
            this.IterCountLabel.Name = "IterCountLabel";
            this.IterCountLabel.Size = new System.Drawing.Size(194, 20);
            this.IterCountLabel.TabIndex = 0;
            this.IterCountLabel.Text = "Количество итераций";
            this.EditDescriptor.SetToolTip(this.IterCountLabel, "От количества итераций зависить точность построения фрактала, \r\nи также количеств" +
        "о итераций может влият на некоторые цветовые режимы.");
            // 
            // EditDescriptor
            // 
            this.EditDescriptor.AutoPopDelay = 15000;
            this.EditDescriptor.InitialDelay = 500;
            this.EditDescriptor.ReshowDelay = 100;
            // 
            // JuliaEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 268);
            this.Controls.Add(this.MainPanelOfJuliaEditor);
            this.Name = "JuliaEditor";
            this.Text = "JuliaEditor";
            this.Load += new System.EventHandler(this.JuliaEditor_Load);
            this.MainPanelOfJuliaEditor.ResumeLayout(false);
            this.MainPanelOfJuliaEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainPanelOfJuliaEditor;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label IterCountLabel;
        private System.Windows.Forms.Label LeftEdgeLabel;
        private System.Windows.Forms.TextBox LeftEdgeEdit;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.TextBox ImaginePartEdit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox RealPartEdit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox BottomEdgeEdit;
        private System.Windows.Forms.TextBox TopEdgeEdit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox RightEdgeEdit;
        private System.Windows.Forms.Label RightEdgeLabel;
        private System.Windows.Forms.ToolTip EditDescriptor;
    }
}