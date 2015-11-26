﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
/*Тестовый прогон*/
namespace FractalBrowser
{
    public partial class MainForm : Form
    {
        /*___________________________________________________________________________Фрактальная_часть_____________________________________________________________*/
        #region Fractal part of Form
        private FractalDataHandlerControler FractalControler;
        private FractalPictureBox MainFractalPictureBox;
        #region Julia Handlers
        /// <summary>
        /// Джулия с комплексной константой -0.8+0.156i
        /// </summary>
        private FractalDataHandler FirstJulia;
        private FractalDataHandler SecondJulia;
        private FractalDataHandler ThirdJulia;
        private FractalDataHandler FourthJulia;
        private FractalDataHandler FifthJulia;
        private FractalDataHandler SixthJulia;
        #endregion /Julia handlers
        private FractalDataHandler MandelbrotHandler;
        private FractalDataHandler AmoebaLasVegas;
        private FractalDataHandler CustomJulia;
        #endregion /Fractal part of Form
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            FractalControler = new FractalDataHandlerControler();
            MainFractalPictureBox = new FractalPictureBox();
            MainFractalPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            MainPanel.Controls.Add(MainFractalPictureBox);
            #region Julia creating
            FirstJulia = new FractalDataHandler(this, new JuliaWithClouds(FractalStaticData.RecomendJuliaIterationsCount, -1.523D, 1.523D, -0.9D, 0.9D, new Complex(-0.8D, 0.156D)),MainFractalPictureBox,new CycleGradientColorMode(),new Size(960,640),FractalControler);
            FirstJulia.ConnectToolStripProgressBar(toolStripProgressBar1);
            FirstJulia.ConnectShowToMenuItem(первыйФракталToolStripMenuItem, FractalControler,32,32);
            FirstJulia.ConnectStandartResetToMenuItem(новыйСтандартногоРазмераToolStripMenuItem, FractalControler);
            FirstJulia.ConnectResetWithSizeFromWindowToMenuItem(новыйСЗаданнымРазмеромToolStripMenuItem, FractalControler);
            FirstJulia.ConntectToStatusLabel(toolStripStatusLabel1);
            SecondJulia = new FractalDataHandler(this, new JuliaWithClouds(FractalStaticData.RecomendJuliaIterationsCount, -0.91D, 0.91D, -1.12D, 1.12D, new Complex(0.285D, 0.0126D)), MainFractalPictureBox, new My2DClassicColorMode(), new Size(960, 640), FractalControler);
            SecondJulia.ConnectToolStripProgressBar(toolStripProgressBar1);
            SecondJulia.ConnectShowToMenuItem(второйФракталToolStripMenuItem, FractalControler,32,32);
            SecondJulia.ConnectStandartResetToMenuItem(новыйСтандартногоРазмераToolStripMenuItem1,FractalControler);
            SecondJulia.ConnectResetWithSizeFromWindowToMenuItem(новыйСЗаданнымРазмеромToolStripMenuItem1,FractalControler);
            SecondJulia.ConntectToStatusLabel(toolStripStatusLabel1);
            ThirdJulia = new FractalDataHandler(this, new JuliaWithClouds(FractalStaticData.RecomendJuliaIterationsCount, -1.35D, 1.35D, -1.12D, 1.12D, new Complex(-0.0085D, 0.71D)), MainFractalPictureBox, new My2DClassicColorMode(1.1D, 1.1D, 1.1D), new Size(960, 640), FractalControler);
            ThirdJulia.ConnectToolStripProgressBar(toolStripProgressBar1);
            ThirdJulia.ConnectShowToMenuItem(третийToolStripMenuItem, FractalControler, 32, 32);
            ThirdJulia.ConnectStandartResetToMenuItem(новыйСтандартногоРазмераToolStripMenuItem2, FractalControler);
            ThirdJulia.ConnectResetWithSizeFromWindowToMenuItem(новыйСЗаданнымРазмеромToolStripMenuItem2,FractalControler);
            ThirdJulia.ConntectToStatusLabel(toolStripStatusLabel1);
            FourthJulia = new FractalDataHandler(this, new JuliaWithClouds(FractalStaticData.RecomendJuliaIterationsCount, -0.88D, 0.88D, -1.12D, 1.12D, new Complex(0.285D, 0.01D)), MainFractalPictureBox, new My2DClassicColorMode(), new Size(960, 640), FractalControler);
            FourthJulia.ConnectToolStripProgressBar(toolStripProgressBar1);
            FourthJulia.ConnectShowToMenuItem(четвёртыйФракталToolStripMenuItem, FractalControler, 32, 32);
            FourthJulia.ConnectStandartResetToMenuItem(новыйСтандартногоРазмераToolStripMenuItem3, FractalControler);
            FourthJulia.ConnectResetWithSizeFromWindowToMenuItem(новыйСЗаданнымРазмеромToolStripMenuItem3, FractalControler);
            FourthJulia.ConntectToStatusLabel(toolStripStatusLabel1);
            FifthJulia = new FractalDataHandler(this, new JuliaWithClouds(FractalStaticData.RecomendJuliaIterationsCount, -1.505D, 1.505D, -0.9D, 0.9D, new Complex(-0.74534D, 0.11301D)), MainFractalPictureBox, new My2DClassicColorMode(), new Size(960, 640), FractalControler);
            FifthJulia.ConnectToolStripProgressBar(toolStripProgressBar1);
            FifthJulia.ConnectShowToMenuItem(пятыйФракталToolStripMenuItem, FractalControler, 32, 32);
            FifthJulia.ConnectStandartResetToMenuItem(новыйСтандартногоРазмераToolStripMenuItem4, FractalControler);
            FifthJulia.ConnectResetWithSizeFromWindowToMenuItem(новыйСЗаданнымРазмеромToolStripMenuItem4, FractalControler);
            FifthJulia.ConntectToStatusLabel(toolStripStatusLabel1);
            SixthJulia = new FractalDataHandler(this, new JuliaWithClouds(FractalStaticData.RecomendJuliaIterationsCount, -1.5D, 1.5D, -1D, 1D, new Complex(-0.75D,-0.03125D)), MainFractalPictureBox, new Simple2DFractalColorMode(), new Size(960, 640),FractalControler);
            SixthJulia.ConnectShowToMenuItem(шестойФракталToolStripMenuItem, FractalControler, 32, 32);
            SixthJulia.ConnectStandartResetToMenuItem(новыйСтандартногоРазмераToolStripMenuItem7, FractalControler);
            SixthJulia.ConnectResetWithSizeFromWindowToMenuItem(новыйСЗаданнымРазмеромToolStripMenuItem7, FractalControler);
            SixthJulia.ConntectToStatusLabel(toolStripStatusLabel1);
            SixthJulia.ConnectToolStripProgressBar(toolStripProgressBar1);
            #endregion /Julia creating
            MandelbrotHandler = new FractalDataHandler(this, new MandelbrotWithClouds(), MainFractalPictureBox, new CosColorMode(), new Size(960, 640), FractalControler);
            MandelbrotHandler.ConnectToolStripProgressBar(toolStripProgressBar1);
            MandelbrotHandler.ConnectShowToMenuItem(обыкновенныйToolStripMenuItem,FractalControler,32,32);
            MandelbrotHandler.ConnectStandartResetToMenuItem(новыйСтандартногоРазмераToolStripMenuItem5, FractalControler);
            MandelbrotHandler.ConnectResetWithSizeFromWindowToMenuItem(новыйСЗаданнымРазмеромToolStripMenuItem5,FractalControler);
            MandelbrotHandler.ConntectToStatusLabel(toolStripStatusLabel1);
            AmoebaLasVegas = new FractalDataHandler(this, new AmoebaLasVegas(), MainFractalPictureBox, new TrioArgsCosColorMode(), new Size(900, 900),FractalControler);
            AmoebaLasVegas.ConnectToolStripProgressBar(toolStripProgressBar1);
            AmoebaLasVegas.ConnectShowToMenuItem(amoebaLasVegasToolStripMenuItem,FractalControler,16,16);
            
            _differenсe_in_width = this.Width - MainPanel.Width;
            _difference_in_height = this.Height - MainPanel.Height;
            FractalDataHandler.UseSafeZoom = true;
            FractalDataHandler.MaxGlobalPercent = toolStripProgressBar1.Maximum;
            MainFractalPictureBox.OpenMenuEvent += () => { contextMenuStrip1.Show(Cursor.Position); };
            MainFractalPictureBox.SelectionPen = null;
        }







        /*___________________________________________________________________________Прочие_данные_формы___________________________________________________________*/
        #region Other data
        private int _differenсe_in_width;
        private int _difference_in_height;
        

        #endregion /Other data

        /*_________________________________________________________________________Прочие_обработки_событий________________________________________________________*/
        #region Other event workers
        private void _on_size_of_MainForm_changed(object sender,EventArgs e)
        {
            MainPanel.Size = new Size(this.Width - _differenсe_in_width, this.Height - _difference_in_height);
        }
        #endregion /Other event workers

        private void получитьВремяВычисленияToolStripMenuItem_Click(object sender, EventArgs e)
        {   FractalDataHandler[] fhs= FractalControler.GetFractalDataHandlers(true);
        if (fhs.Length < 1)
        {
            MessageBox.Show("Вы еще не строили фракталы!");
            return;
        }
            FractalAssociationParametrs fap = fhs[0].FractalAssociationParameters;
            MessageBox.Show(fap.TimeOfCalculating.ToString());
        }

        private void сохранитьИзображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FractalDataHandler[] fdh = FractalControler.GetFractalDataHandlers(true);
            if (fdh.Length < 1) return;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = FractalImageSaver.Filter;
            if(sfd.ShowDialog(this)==DialogResult.OK)
            {
                Bitmap bmp = fdh[0].FractalColorMode.GetDrawnBitmap(fdh[0].FractalAssociationParameters);
                bmp.Save(sfd.FileName,FractalImageSaver.GetFormatFromIndex(sfd.FilterIndex));
                MessageBox.Show("Изображение сохранено.");
            }
            
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            FractalDataHandler[] fdh = FractalControler.GetFractalDataHandlers(true);
            if (fdh.Length < 1) { e.Cancel = true; return; }
            contextMenuStrip1.Items[0].Enabled=fdh[0].Fractal.CanBack();
        }

        private void отменитьМашстабированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FractalControler.FractalGetBack();
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void изменитьРазрешениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FractalControler.ChangeSize();
        }

        private void изменитьКоличествоИтерацийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FractalDataHandler[] fdhs= FractalControler.GetFractalDataHandlers(true);
            if (fdhs.Length == 0) return;
            FractalDataHandler fdh = fdhs[0];
            OneNumberEditor one = new OneNumberEditor(fdh.Fractal.Iterations, (decimal)ulong.MaxValue);
            if(one.ShowDialog(this)==DialogResult.Yes)
            {
                fdh.Fractal.Iterations = (ulong)one.value;
                
            }
        }

        private void изменитьЦвтовойРежимToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            FractalDataHandler[] fdhs = FractalControler.GetFractalDataHandlers(true);
            if (fdhs.Length < 1) return;
            FractalDataHandler fdh = fdhs[0];
            fdh.ChangeColorMode();
        }

        private void новыйСЗаданнымРазмеромToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            JuliaEditor je;
            if (CustomJulia == null) je = new JuliaEditor();
            else je = new JuliaEditor((Julia)CustomJulia.Fractal);
            if (je.ShowDialog(this) != DialogResult.Yes) return;
            if (CustomJulia == null) { CustomJulia = new FractalDataHandler(this, je.Julia, MainFractalPictureBox, new My2DClassicColorMode(), new Size(960, 640),FractalControler);
            CustomJulia.ConnectToolStripProgressBar(toolStripProgressBar1);
            CustomJulia.ConnectShowToMenuItem(настроенныйToolStripMenuItem, FractalControler, 32, 32);
            CustomJulia.ConntectToStatusLabel(toolStripStatusLabel1);
            }
            else CustomJulia.Fractal = je.Julia;
            CustomJulia.CreateInSize(FractalControler);
        }

        private void настроенныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CustomJulia == null) новыйСЗаданнымРазмеромToolStripMenuItem6.PerformClick();
        }

        private void новыйСтандартногоРазмераToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            JuliaEditor je;
            if (CustomJulia == null) je = new JuliaEditor();
            else je = new JuliaEditor((Julia)CustomJulia.Fractal);
            if (je.ShowDialog(this) != DialogResult.Yes) return;
            if (CustomJulia == null)
            {
                CustomJulia = new FractalDataHandler(this, je.Julia, MainFractalPictureBox, new My2DClassicColorMode(), new Size(960, 640),FractalControler);
                CustomJulia.ConnectToolStripProgressBar(toolStripProgressBar1);
                CustomJulia.ConnectShowToMenuItem(настроенныйToolStripMenuItem, FractalControler, 32, 32);
                CustomJulia.ConntectToStatusLabel(toolStripStatusLabel1);
            }
            else CustomJulia.Fractal = je.Julia;
            CustomJulia.Reset(960,640);
        }

        private void новыйСтандартногоРазмераToolStripMenuItem7_Click(object sender, EventArgs e)
        {

        }        
    }
}
