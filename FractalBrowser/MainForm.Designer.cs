﻿namespace FractalBrowser
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.фракталыДжулииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.первыйФракталToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.новыйСтандартногоРазмераToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.новыйСЗаданнымРазмеромToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.второйФракталToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.третийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.четвёртыйФракталToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пятыйФракталToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.множествоМандельбротаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обыкновенныйToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.действияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.получитьВремяВычисленияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьИзображениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.отменитьМашстабированиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.AutoScroll = true;
            this.MainPanel.Location = new System.Drawing.Point(12, 27);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(920, 549);
            this.MainPanel.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.фракталыДжулииToolStripMenuItem,
            this.множествоМандельбротаToolStripMenuItem,
            this.действияToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(944, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // фракталыДжулииToolStripMenuItem
            // 
            this.фракталыДжулииToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.первыйФракталToolStripMenuItem,
            this.второйФракталToolStripMenuItem,
            this.третийToolStripMenuItem,
            this.четвёртыйФракталToolStripMenuItem,
            this.пятыйФракталToolStripMenuItem});
            this.фракталыДжулииToolStripMenuItem.Name = "фракталыДжулииToolStripMenuItem";
            this.фракталыДжулииToolStripMenuItem.Size = new System.Drawing.Size(128, 20);
            this.фракталыДжулииToolStripMenuItem.Text = "Множество джулии";
            // 
            // первыйФракталToolStripMenuItem
            // 
            this.первыйФракталToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.новыйСтандартногоРазмераToolStripMenuItem,
            this.новыйСЗаданнымРазмеромToolStripMenuItem});
            this.первыйФракталToolStripMenuItem.Name = "первыйФракталToolStripMenuItem";
            this.первыйФракталToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.первыйФракталToolStripMenuItem.Text = "Первый фрактал";
            // 
            // новыйСтандартногоРазмераToolStripMenuItem
            // 
            this.новыйСтандартногоРазмераToolStripMenuItem.Name = "новыйСтандартногоРазмераToolStripMenuItem";
            this.новыйСтандартногоРазмераToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.новыйСтандартногоРазмераToolStripMenuItem.Text = "Новый стандартного размера";
            // 
            // новыйСЗаданнымРазмеромToolStripMenuItem
            // 
            this.новыйСЗаданнымРазмеромToolStripMenuItem.Name = "новыйСЗаданнымРазмеромToolStripMenuItem";
            this.новыйСЗаданнымРазмеромToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.новыйСЗаданнымРазмеромToolStripMenuItem.Text = "Новый с заданным размером";
            // 
            // второйФракталToolStripMenuItem
            // 
            this.второйФракталToolStripMenuItem.Name = "второйФракталToolStripMenuItem";
            this.второйФракталToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.второйФракталToolStripMenuItem.Text = "Второй фрактал";
            // 
            // третийToolStripMenuItem
            // 
            this.третийToolStripMenuItem.Name = "третийToolStripMenuItem";
            this.третийToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.третийToolStripMenuItem.Text = "Третий фрактал";
            // 
            // четвёртыйФракталToolStripMenuItem
            // 
            this.четвёртыйФракталToolStripMenuItem.Name = "четвёртыйФракталToolStripMenuItem";
            this.четвёртыйФракталToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.четвёртыйФракталToolStripMenuItem.Text = "Четвёртый фрактал";
            // 
            // пятыйФракталToolStripMenuItem
            // 
            this.пятыйФракталToolStripMenuItem.Name = "пятыйФракталToolStripMenuItem";
            this.пятыйФракталToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.пятыйФракталToolStripMenuItem.Text = "Пятый фрактал";
            // 
            // множествоМандельбротаToolStripMenuItem
            // 
            this.множествоМандельбротаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.обыкновенныйToolStripMenuItem});
            this.множествоМандельбротаToolStripMenuItem.Name = "множествоМандельбротаToolStripMenuItem";
            this.множествоМандельбротаToolStripMenuItem.Size = new System.Drawing.Size(165, 20);
            this.множествоМандельбротаToolStripMenuItem.Text = "Множество мандельброта";
            // 
            // обыкновенныйToolStripMenuItem
            // 
            this.обыкновенныйToolStripMenuItem.Name = "обыкновенныйToolStripMenuItem";
            this.обыкновенныйToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.обыкновенныйToolStripMenuItem.Text = "Обыкновенный";
            // 
            // действияToolStripMenuItem
            // 
            this.действияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.получитьВремяВычисленияToolStripMenuItem,
            this.сохранитьИзображениеToolStripMenuItem});
            this.действияToolStripMenuItem.Name = "действияToolStripMenuItem";
            this.действияToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.действияToolStripMenuItem.Text = "Действия";
            // 
            // получитьВремяВычисленияToolStripMenuItem
            // 
            this.получитьВремяВычисленияToolStripMenuItem.Name = "получитьВремяВычисленияToolStripMenuItem";
            this.получитьВремяВычисленияToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.получитьВремяВычисленияToolStripMenuItem.Text = "Получить время вычисления";
            this.получитьВремяВычисленияToolStripMenuItem.Click += new System.EventHandler(this.получитьВремяВычисленияToolStripMenuItem_Click);
            // 
            // сохранитьИзображениеToolStripMenuItem
            // 
            this.сохранитьИзображениеToolStripMenuItem.Name = "сохранитьИзображениеToolStripMenuItem";
            this.сохранитьИзображениеToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.сохранитьИзображениеToolStripMenuItem.Text = "Сохранить изображение";
            this.сохранитьИзображениеToolStripMenuItem.Click += new System.EventHandler(this.сохранитьИзображениеToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 579);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(944, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.отменитьМашстабированиеToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(235, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // отменитьМашстабированиеToolStripMenuItem
            // 
            this.отменитьМашстабированиеToolStripMenuItem.Name = "отменитьМашстабированиеToolStripMenuItem";
            this.отменитьМашстабированиеToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.отменитьМашстабированиеToolStripMenuItem.Text = "Отменить масштабирование";
            this.отменитьМашстабированиеToolStripMenuItem.Click += new System.EventHandler(this.отменитьМашстабированиеToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 601);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Фрактальный обозреватель";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this._on_size_of_MainForm_changed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem фракталыДжулииToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem первыйФракталToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripMenuItem второйФракталToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem третийToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem новыйСтандартногоРазмераToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem четвёртыйФракталToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пятыйФракталToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem новыйСЗаданнымРазмеромToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem действияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem получитьВремяВычисленияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem множествоМандельбротаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обыкновенныйToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьИзображениеToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem отменитьМашстабированиеToolStripMenuItem;
    }
}

