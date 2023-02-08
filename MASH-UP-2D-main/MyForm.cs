using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace Pt2pt
{
    public partial class MyForm : Form
    {
        static Figure f;
        Point ptX, ptY, mouse;
        Bitmap bmpX, bmpY;
        Graphics gX, gY;
        bool IsMouseDownX = false;
        bool IsMouseDownY = false;
        Canvas canvas;
        float deltaX = 0;
        float deltaY = 1;
        Scene scene;
        bool isMouseDown = false;
        float counterTimer = 0;
        int counterFigures = 0;

        Dictionary<int, List<PointF>> placedPointsFig1 = new Dictionary<int, List<PointF>>();
        Dictionary<int, List<PointF>> placedPointsFig2 = new Dictionary<int, List<PointF>>();
        Dictionary<int, List<PointF>> placedPointsFig3 = new Dictionary<int, List<PointF>>();

        const int WM_KEYUP = 0x0101;
        const int WM_KEYDOWN = 0x0100;
        private int nFigures;

        PointF centroid0F1 = new PointF(0, 0);
        PointF centroid1F1 = new PointF(0, 0);
        PointF centroid2F1 = new PointF(0, 0);
        PointF centroid3F1 = new PointF(0, 0);
        PointF centroid4F1 = new PointF(0, 0);
        PointF centroid5F1 = new PointF(0, 0);

        PointF centroid0F2 = new PointF(0, 0);
        PointF centroid1F2 = new PointF(0, 0);
        PointF centroid2F2 = new PointF(0, 0);
        PointF centroid3F2 = new PointF(0, 0);
        PointF centroid4F2 = new PointF(0, 0);
        PointF centroid5F2 = new PointF(0, 0);

        PointF centroid0F3 = new PointF(0, 0);
        PointF centroid1F3 = new PointF(0, 0);
        PointF centroid2F3 = new PointF(0, 0);
        PointF centroid3F3 = new PointF(0, 0);
        PointF centroid4F3 = new PointF(0, 0);
        PointF centroid5F3 = new PointF(0, 0);

        public MyForm()
        {
            InitializeComponent();
            Init();
            IsMouseDownX = false;            
        }

        private void Init()
        {   
            bmpX = new Bitmap(PCT_SLIDEER_X.Width, PCT_SLIDEER_X.Height);
            bmpY = new Bitmap(PCT_SLIDEER_Y.Width, PCT_SLIDEER_Y.Height);

            gX = Graphics.FromImage(bmpX);
            gY = Graphics.FromImage(bmpY);

            PCT_SLIDEER_X.Image = bmpX;
            PCT_SLIDEER_Y.Image = bmpY;

            gX.DrawLine(Pens.DimGray, 0, bmpX.Height / 2, bmpX.Width, bmpX.Height / 2);
            gX.FillEllipse(Brushes.Aquamarine, bmpX.Width / 2, bmpX.Height / 4, bmpX.Height / 2, bmpX.Height / 2);

            gY.DrawLine(Pens.DimGray, bmpY.Width / 2, 0,  bmpY.Width / 2, bmpY.Height);
            gY.FillEllipse(Brushes.Aquamarine, bmpY.Width / 4, bmpY.Height / 2, bmpX.Height / 2, bmpX.Height / 2);

            scene = new Scene();
            Figure fig = new Figure();
            fig.Add(new PointF(0, 0));
            fig.Add(new PointF(0, 0));
            scene.Figures.Add(fig);

            timer2.Stop();
            //timer1.Stop();
            nFigures = 0;
            
        }
        private void PCT_SLIDEER_Y_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDownY = false;
            gY.Clear(Color.Transparent);
            gY.DrawLine(Pens.DimGray, bmpY.Width / 2, 0, bmpY.Width / 2, bmpY.Height);
            gY.FillEllipse(Brushes.Aquamarine, bmpY.Width / 4, bmpY.Height / 2, bmpX.Height / 2, bmpX.Height / 2);

            PCT_SLIDEER_Y.Invalidate();
        }

        private void PCT_SLIDEER_Y_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDownY)
            {
                gY.Clear(Color.Transparent);
                gY.DrawLine(Pens.DimGray, bmpY.Width / 2, 0, bmpY.Width / 2, bmpY.Height);
                gY.FillEllipse(Brushes.Aquamarine, bmpY.Width / 4, e.Y, bmpX.Height / 2, bmpX.Height / 2);

                PCT_SLIDEER_Y.Invalidate();
                deltaY += (float)(ptY.Y - e.Location.Y) / 500;//------------------
                ptY.Y = e.Location.Y;
            }
        }

        private void PCT_SLIDEER_Y_MouseDown(object sender, MouseEventArgs e)
        {
            ptY = e.Location;
            IsMouseDownY = true;
        }

        private void PCT_SLIDEER_X_MouseDown(object sender, MouseEventArgs e)
        {
            ptX = e.Location;
            IsMouseDownX = true;
        }

        private void PCT_SLIDEER_X_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDownX)
            {
                gX.Clear(Color.Transparent);
                gX.DrawLine(Pens.DimGray, 0, PCT_SLIDEER_X.Height / 2, PCT_SLIDEER_X.Width, PCT_SLIDEER_X.Height / 2);
                gX.FillEllipse(Brushes.Aquamarine, e.X, PCT_SLIDEER_X.Height / 4, PCT_SLIDEER_X.Height / 2, PCT_SLIDEER_X.Height / 2);

                PCT_SLIDEER_X.Invalidate();
                deltaX += (float)(e.Location.X - ptX.X) / 3;//------------------
                ptX.X = e.Location.X;
            }
        }

        private void PCT_SLIDEER_X_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDownX = false;
            gX.Clear(Color.Transparent);
            gX.DrawLine(Pens.DimGray, 0, PCT_SLIDEER_X.Height / 2, PCT_SLIDEER_X.Width, PCT_SLIDEER_X.Height / 2);
            gX.FillEllipse(Brushes.Aquamarine, PCT_SLIDEER_X.Width / 2, PCT_SLIDEER_X.Height / 4, PCT_SLIDEER_X.Height / 2, PCT_SLIDEER_X.Height / 2);

            PCT_SLIDEER_X.Invalidate();
        }

        private void MyForm_Resize(object sender, EventArgs e)
        {
            canvas = new Canvas(PCT_CANVAS);
        }

        private void TIMER_Tick(object sender, EventArgs e)
        {
            if (f != null && (IsMouseDownX|| IsMouseDownY))
            {
                f.TranslateToOrigin();
                f.Scale(deltaY);
                f.Rotate(deltaX);                
                f.TranslatePoints(f.Centroid);             
            }
            deltaX = 0;
            deltaY = 1;
            canvas.Render(scene);
            if (counterFigures == 3)
            {
                BTN_EXE.Visible = false;
            }
        }

        private void PCT_CANVAS_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                f.UpdateAttributes();
        }

        private void BTN_EXE_Click(object sender, EventArgs e)
        {
            counterFigures++;
            if (counterFigures != 4)
            {
                f = new Figure();
                scene.Figures.Add(f);
                TreeNode node = new TreeNode("Fig" + (TREE.Nodes.Count + 1));
                node.Tag = f;
                TREE.Nodes.Add(node);
            }
        }   

        private void TREE_AfterSelect(object sender, TreeViewEventArgs e)
        {
            f = (Figure)TREE.SelectedNode.Tag;
            BTN_EXE.Select();
        }
        
        public static bool IsControlDown()
        {
            return (Control.ModifierKeys & Keys.Control) == Keys.Control;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (f == null)
                return false;
            
            switch (keyData)
            {
                case Keys.Left:
                    f.Centroid.X -= 3;                  
                    break;
                case Keys.Right:
                    f.Centroid.X += 3;
                    break;
                case Keys.Up:
                    f.Centroid.Y += -3;
                    break;
                case Keys.Down:
                    f.Centroid.Y += 3;
                    break;
                case Keys.Space:
                    break;
            }
            PCT_CANVAS.Select();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void PCT_CANVAS_MouseDown(object sender, MouseEventArgs e)
        {
            mouse = e.Location;
            isMouseDown = true;
        }

        private void PCT_CANVAS_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            PCT_CANVAS.Select();
        }

        private void PCT_CANVAS_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                mouse.X -= e.X;
                mouse.Y -= e.Y;
                f.TranslatePoints(new Point(-mouse.X,-mouse.Y));
                mouse = e.Location;
            }            
        }

        private void PCT_CANVAS_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (f != null)
            {
                canvas.DrawPixel(e.X, e.Y, Color.White);
                f.Add(new PointF(e.X, e.Y));
            }
        }

        private void TREE_KeyPress(object sender, KeyPressEventArgs e)
        {
            return;
        }
        

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            LBL_STATUS.Text = " ::: " + (float)trackBar1.Value / 100;            
            f.Follow(scene.Figures[0].Pts[0], scene.Figures[0].Pts[1], (float)trackBar1.Value / 100);
        }

        private void MyForm_Load(object sender, EventArgs e)
        {

        }
        private void trackBarTime_Scroll(object sender, EventArgs e)
        {
            //f.Follow()
            switch (trackBarTime.Value)
            {
                case 0:
                    if (placedPointsFig1.Count > 0)
                    {
                        
                        for (int i = 0; i < scene.Figures[1].Pts.Count; i++)
                        {
                            scene.Figures[1].Pts[i] = new PointF(placedPointsFig1.ElementAt(0).Value[i].X, placedPointsFig1.ElementAt(0).Value[i].Y);
                            scene.Figures[1].UpdateAttributes();
                            centroid0F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                        }
                    }
                    if(placedPointsFig2.Count > 0)
                    {
                        for (int i = 0; i < scene.Figures[1].Pts.Count; i++)
                        {
                            scene.Figures[1].Pts[i] = new PointF(placedPointsFig1.ElementAt(0).Value[i].X, placedPointsFig1.ElementAt(0).Value[i].Y);
                            scene.Figures[1].UpdateAttributes();
                            centroid0F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                        }

                        for (int i = 0; i < scene.Figures[2].Pts.Count; i++)
                        {
                            scene.Figures[2].Pts[i] = new PointF(placedPointsFig2.ElementAt(0).Value[i].X, placedPointsFig2.ElementAt(0).Value[i].Y);
                            scene.Figures[2].UpdateAttributes();
                            centroid0F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                        }
                    }
                    if(placedPointsFig3.Count > 0)
                    {
                        for (int i = 0; i < scene.Figures[3].Pts.Count; i++)
                        {
                            scene.Figures[3].Pts[i] = new PointF(placedPointsFig3.ElementAt(0).Value[i].X, placedPointsFig3.ElementAt(0).Value[i].Y);
                            scene.Figures[3].UpdateAttributes();
                            centroid0F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                        }
                    }
                    break;
                case 1:
                    if (placedPointsFig1.Count > 1)
                    {
                        for (int i = 0; i < scene.Figures[1].Pts.Count; i++)
                        {
                            scene.Figures[1].Pts[i] = new PointF(placedPointsFig1.ElementAt(1).Value[i].X, placedPointsFig1.ElementAt(1).Value[i].Y);
                            scene.Figures[1].UpdateAttributes();
                            centroid1F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                        }
                    }
                    if (placedPointsFig2.Count > 1)
                    {
                        for (int i = 0; i < scene.Figures[2].Pts.Count; i++)
                        {
                            scene.Figures[2].Pts[i] = new PointF(placedPointsFig2.ElementAt(1).Value[i].X, placedPointsFig2.ElementAt(1).Value[i].Y);
                            scene.Figures[2].UpdateAttributes();
                            centroid1F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                        }
                    }
                    if(placedPointsFig3.Count > 1)
                    {
                        for (int i = 0; i < scene.Figures[3].Pts.Count; i++)
                        {
                            scene.Figures[3].Pts[i] = new PointF(placedPointsFig3.ElementAt(1).Value[i].X, placedPointsFig3.ElementAt(1).Value[i].Y);
                            scene.Figures[3].UpdateAttributes();
                            centroid1F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                        }
                    }
                    break;
                case 2:
                    if (placedPointsFig1.Count > 2)
                    {
                        for (int i = 0; i < scene.Figures[1].Pts.Count; i++)
                        {
                            scene.Figures[1].Pts[i] = new PointF(placedPointsFig1.ElementAt(2).Value[i].X, placedPointsFig1.ElementAt(2).Value[i].Y);
                            scene.Figures[1].UpdateAttributes();
                            centroid2F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                        }
                    }
                    if (placedPointsFig2.Count > 2)
                    {
                        for (int i = 0; i < scene.Figures[2].Pts.Count; i++)
                        {
                            scene.Figures[2].Pts[i] = new PointF(placedPointsFig2.ElementAt(2).Value[i].X, placedPointsFig2.ElementAt(2).Value[i].Y);
                            scene.Figures[2].UpdateAttributes();
                            centroid2F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                        }
                    }
                    if(placedPointsFig3.Count > 2)
                    {
                        for (int i = 0; i < scene.Figures[3].Pts.Count; i++)
                        {
                            scene.Figures[3].Pts[i] = new PointF(placedPointsFig3.ElementAt(2).Value[i].X, placedPointsFig3.ElementAt(2).Value[i].Y);
                            scene.Figures[3].UpdateAttributes();
                            centroid2F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                        }
                    }
                    break;
                case 3:
                    if (placedPointsFig1.Count > 3)
                    {
                        for (int i = 0; i < scene.Figures[1].Pts.Count; i++)
                        {
                            scene.Figures[1].Pts[i] = new PointF(placedPointsFig1.ElementAt(3).Value[i].X, placedPointsFig1.ElementAt(3).Value[i].Y);
                            scene.Figures[1].UpdateAttributes();
                            centroid3F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                        }
                    }
                    if (placedPointsFig2.Count > 3)
                    {
                        for (int i = 0; i < scene.Figures[2].Pts.Count; i++)
                        {
                            scene.Figures[2].Pts[i] = new PointF(placedPointsFig2.ElementAt(3).Value[i].X, placedPointsFig2.ElementAt(3).Value[i].Y);
                            scene.Figures[2].UpdateAttributes();
                            centroid3F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                        }
                    }
                    if(placedPointsFig3.Count > 3)
                    {
                        for (int i = 0; i < scene.Figures[3].Pts.Count; i++)
                        {
                            scene.Figures[3].Pts[i] = new PointF(placedPointsFig3.ElementAt(3).Value[i].X, placedPointsFig3.ElementAt(3).Value[i].Y);
                            scene.Figures[3].UpdateAttributes();
                            centroid3F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                        }
                    }
                    break;
                case 4:
                    if (placedPointsFig1.Count > 4)
                    {
                        for (int i = 0; i < scene.Figures[1].Pts.Count; i++)
                        {
                            scene.Figures[1].Pts[i] = new PointF(placedPointsFig1.ElementAt(4).Value[i].X, placedPointsFig1.ElementAt(4).Value[i].Y);
                            scene.Figures[1].UpdateAttributes();
                            centroid4F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                        }
                    }
                    if (placedPointsFig2.Count > 4)
                    {
                        for (int i = 0; i < scene.Figures[2].Pts.Count; i++)
                        {
                            scene.Figures[2].Pts[i] = new PointF(placedPointsFig2.ElementAt(4).Value[i].X, placedPointsFig2.ElementAt(4).Value[i].Y);
                            scene.Figures[2].UpdateAttributes();
                            centroid4F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                        }
                    }
                    if(placedPointsFig3.Count > 4)
                    {
                        for (int i = 0; i < scene.Figures[3].Pts.Count; i++)
                        {
                            scene.Figures[3].Pts[i] = new PointF(placedPointsFig3.ElementAt(4).Value[i].X, placedPointsFig3.ElementAt(4).Value[i].Y);
                            scene.Figures[3].UpdateAttributes();
                            centroid4F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                        }
                    }
                    break;
                case 5:
                    if (placedPointsFig1.Count > 5)
                    {
                        for (int i = 0; i < scene.Figures[1].Pts.Count; i++)
                        {
                            scene.Figures[1].Pts[i] = new PointF(placedPointsFig1.ElementAt(5).Value[i].X, placedPointsFig1.ElementAt(5).Value[i].Y);
                            scene.Figures[1].UpdateAttributes();
                            centroid5F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                        }
                    }
                    if (placedPointsFig2.Count > 5)
                    {
                        for (int i = 0; i < scene.Figures[2].Pts.Count; i++)
                        {
                            scene.Figures[2].Pts[i] = new PointF(placedPointsFig2.ElementAt(5).Value[i].X, placedPointsFig2.ElementAt(5).Value[i].Y);
                            scene.Figures[2].UpdateAttributes();
                            centroid5F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                        }
                    }
                    if(placedPointsFig3.Count > 5)
                    {
                        for (int i = 0; i < scene.Figures[3].Pts.Count; i++)
                        {
                            scene.Figures[3].Pts[i] = new PointF(placedPointsFig3.ElementAt(5).Value[i].X, placedPointsFig3.ElementAt(5).Value[i].Y);
                            scene.Figures[3].UpdateAttributes();
                            centroid5F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                        }
                    }
                    break;
            }
        }

        private void puntosFiguras()
        {
            List<PointF> figurePoints1 = new List<PointF>();
            List<PointF> figurePoints2 = new List<PointF>();
            List<PointF> figurePoints3 = new List<PointF>();
            Figure figuraRec = new Figure();
            if(counterFigures == 1)
            {
                for (int i = 0; i < scene.Figures[1].Pts.Count; i++)
                {
                    figuraRec.Add(scene.Figures[1].Pts[i]);
                    figurePoints1.Add(scene.Figures[1].Pts[i]);
                }
            }
            if(counterFigures == 2)
            {
                for (int i = 0; i < scene.Figures[1].Pts.Count; i++)
                {
                    figuraRec.Add(scene.Figures[1].Pts[i]);
                    figurePoints1.Add(scene.Figures[1].Pts[i]);
                }

                for (int i = 0; i < scene.Figures[2].Pts.Count; i++)
                {
                    figuraRec.Add(scene.Figures[2].Pts[i]);
                    figurePoints2.Add(scene.Figures[2].Pts[i]);
                }
            }
            if(counterFigures == 3)
            {
                for (int i = 0; i < scene.Figures[1].Pts.Count; i++)
                {
                    figuraRec.Add(scene.Figures[1].Pts[i]);
                    figurePoints1.Add(scene.Figures[1].Pts[i]);
                }

                for (int i = 0; i < scene.Figures[2].Pts.Count; i++)
                {
                    figuraRec.Add(scene.Figures[2].Pts[i]);
                    figurePoints2.Add(scene.Figures[2].Pts[i]);
                }

                for (int i = 0; i < scene.Figures[3].Pts.Count; i++)
                {
                    figuraRec.Add(scene.Figures[3].Pts[i]);
                    figurePoints3.Add(scene.Figures[3].Pts[i]);
                }
            }

            if (figuraRec.Pts.Count > 2)
                scene.RepFigures.Add(figuraRec);
            switch (trackBarTime.Value)
            {
                case 0:
                    if(counterFigures == 1)
                    {
                        if (placedPointsFig1.ContainsKey(0)){
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(0).Key);
                        }
                        placedPointsFig1.Add(0, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid0F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                    }

                    //Figure 2
                    if (counterFigures == 2)
                    {
                        if (placedPointsFig1.ContainsKey(0))
                        {
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(0).Key);
                        }
                        placedPointsFig1.Add(0, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid0F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);

                        if (placedPointsFig2.ContainsKey(0))
                        {
                            placedPointsFig2.Remove(placedPointsFig2.ElementAt(0).Key);
                        }
                        placedPointsFig2.Add(0, figurePoints2);
                        scene.Figures[2].UpdateAttributes();
                        centroid0F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                    }

                    //Figure 3
                    if(counterFigures == 3)
                    {
                        if (placedPointsFig1.ContainsKey(0))
                        {
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(0).Key);
                        }
                        placedPointsFig1.Add(0, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid0F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);

                        if (placedPointsFig2.ContainsKey(0))
                        {
                            placedPointsFig2.Remove(placedPointsFig2.ElementAt(0).Key);
                        }
                        placedPointsFig2.Add(0, figurePoints2);
                        scene.Figures[2].UpdateAttributes();
                        centroid0F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);

                        if (placedPointsFig3.ContainsKey(0))
                        {
                            placedPointsFig3.Remove(placedPointsFig3.ElementAt(0).Key);
                        }
                        placedPointsFig3.Add(0, figurePoints3);
                        scene.Figures[3].UpdateAttributes();
                        centroid0F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                    }
                    break;
                case 1:
                    if(counterFigures == 1)
                    {
                        if (placedPointsFig1.ContainsKey(1)){
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(1).Key);
                        }
                        placedPointsFig1.Add(1, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid1F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                    }

                    //Figure 2
                    if (counterFigures == 2)
                    {
                        if (placedPointsFig1.ContainsKey(1))
                        {
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(1).Key);
                        }
                        placedPointsFig1.Add(1, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid1F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);

                        if (placedPointsFig2.ContainsKey(1))
                        {
                            placedPointsFig2.Remove(placedPointsFig2.ElementAt(1).Key);
                        }
                        placedPointsFig2.Add(1, figurePoints2);
                        scene.Figures[2].UpdateAttributes();
                        centroid1F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                    }

                    //Figure 3
                    if(counterFigures == 3)
                    {
                        if (placedPointsFig1.ContainsKey(1))
                        {
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(1).Key);
                        }
                        placedPointsFig1.Add(1, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid1F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);

                        if (placedPointsFig2.ContainsKey(1))
                        {
                            placedPointsFig2.Remove(placedPointsFig2.ElementAt(1).Key);
                        }
                        placedPointsFig2.Add(1, figurePoints2);
                        scene.Figures[2].UpdateAttributes();
                        centroid1F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);

                        if (placedPointsFig3.ContainsKey(1))
                        {
                            placedPointsFig3.Remove(placedPointsFig3.ElementAt(1).Key);
                        }
                        placedPointsFig3.Add(1, figurePoints3);
                        scene.Figures[3].UpdateAttributes();
                        centroid1F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                    }
                    break;
                case 2:
                    if(counterFigures == 1)
                    {
                        if (placedPointsFig1.ContainsKey(2)){
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(2).Key);
                        }
                        placedPointsFig1.Add(2, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid2F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                    }

                    //Figure 2
                    if(counterFigures == 2)
                    {
                        if (placedPointsFig1.ContainsKey(2))
                        {
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(2).Key);
                        }
                        placedPointsFig1.Add(2, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid2F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);

                        if (placedPointsFig2.ContainsKey(2))
                        {
                            placedPointsFig2.Remove(placedPointsFig2.ElementAt(2).Key);
                        }
                        placedPointsFig2.Add(2, figurePoints2);
                        scene.Figures[2].UpdateAttributes();
                        centroid2F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                    }

                    //Figure 3
                    if(counterFigures == 3)
                    {
                        if (placedPointsFig1.ContainsKey(2))
                        {
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(2).Key);
                        }
                        placedPointsFig1.Add(2, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid2F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);

                        if (placedPointsFig2.ContainsKey(2))
                        {
                            placedPointsFig2.Remove(placedPointsFig2.ElementAt(2).Key);
                        }
                        placedPointsFig2.Add(2, figurePoints2);
                        scene.Figures[2].UpdateAttributes();
                        centroid2F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);

                        if (placedPointsFig3.ContainsKey(2))
                        {
                            placedPointsFig3.Remove(placedPointsFig3.ElementAt(2).Key);
                        }
                        placedPointsFig3.Add(2, figurePoints3);
                        scene.Figures[3].UpdateAttributes();
                        centroid2F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                    }
                    break;
                case 3:
                    if (counterFigures == 1)
                    {
                        if (placedPointsFig1.ContainsKey(3)){
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(3).Key);
                        }
                        placedPointsFig1.Add(3, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid3F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                    }
                    
                    //Figure 2
                    if(counterFigures == 2)
                    {
                        if (placedPointsFig1.ContainsKey(3))
                        {
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(3).Key);
                        }
                        placedPointsFig1.Add(3, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid3F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);

                        if (placedPointsFig2.ContainsKey(3))
                        {
                            placedPointsFig2.Remove(placedPointsFig2.ElementAt(3).Key);
                        }
                        placedPointsFig2.Add(3, figurePoints2);
                        scene.Figures[2].UpdateAttributes();
                        centroid3F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                    }

                    //Figure 3
                    if(counterFigures == 3)
                    {
                        if (placedPointsFig1.ContainsKey(3))
                        {
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(3).Key);
                        }
                        placedPointsFig1.Add(3, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid3F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);

                        if (placedPointsFig2.ContainsKey(3))
                        {
                            placedPointsFig2.Remove(placedPointsFig2.ElementAt(3).Key);
                        }
                        placedPointsFig2.Add(3, figurePoints2);
                        scene.Figures[2].UpdateAttributes();
                        centroid3F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);

                        if (placedPointsFig3.ContainsKey(3))
                        {
                            placedPointsFig3.Remove(placedPointsFig3.ElementAt(3).Key);
                        }
                        placedPointsFig3.Add(3, figurePoints3);
                        scene.Figures[3].UpdateAttributes();
                        centroid3F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                    }
                    break;
                case 4:
                    if(counterFigures == 1)
                    {
                        if (placedPointsFig1.ContainsKey(4)){
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(4).Key);
                        }
                        placedPointsFig1.Add(4, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid4F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                    }

                    //Figure 2
                    if(counterFigures == 2)
                    {
                        if (placedPointsFig1.ContainsKey(4))
                        {
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(4).Key);
                        }
                        placedPointsFig1.Add(4, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid4F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);

                        if (placedPointsFig2.ContainsKey(4))
                        {
                            placedPointsFig2.Remove(placedPointsFig2.ElementAt(4).Key);
                        }
                        placedPointsFig2.Add(4, figurePoints2);
                        scene.Figures[2].UpdateAttributes();
                        centroid4F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                    }

                    //Figure 3
                    if(counterFigures == 3)
                    {
                        if (placedPointsFig1.ContainsKey(4))
                        {
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(4).Key);
                        }
                        placedPointsFig1.Add(4, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid4F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);

                        if (placedPointsFig2.ContainsKey(4))
                        {
                            placedPointsFig2.Remove(placedPointsFig2.ElementAt(4).Key);
                        }
                        placedPointsFig2.Add(4, figurePoints2);
                        scene.Figures[2].UpdateAttributes();
                        centroid4F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);

                        if (placedPointsFig3.ContainsKey(4))
                        {
                            placedPointsFig3.Remove(placedPointsFig3.ElementAt(4).Key);
                        }
                        placedPointsFig3.Add(4, figurePoints3);
                        scene.Figures[3].UpdateAttributes();
                        centroid4F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                    }
                    break;
                case 5:
                    if(counterFigures == 1)
                    {
                        if (placedPointsFig1.ContainsKey(5))
                        {
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(5).Key);
                        }
                        placedPointsFig1.Add(5, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid5F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                    }

                    //Figure 2
                    if(counterFigures == 2)
                    {
                        if (placedPointsFig1.ContainsKey(5))
                        {
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(5).Key);
                        }
                        placedPointsFig1.Add(5, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid5F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);

                        if (placedPointsFig2.ContainsKey(5))
                        {
                            placedPointsFig2.Remove(placedPointsFig2.ElementAt(5).Key);
                        }
                        placedPointsFig2.Add(5, figurePoints2);
                        scene.Figures[2].UpdateAttributes();
                        centroid5F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                    }

                    //Figure 3
                    if(counterFigures == 3)
                    {
                        if (placedPointsFig1.ContainsKey(5))
                        {
                            placedPointsFig1.Remove(placedPointsFig1.ElementAt(5).Key);
                        }
                        placedPointsFig1.Add(5, figurePoints1);
                        scene.Figures[1].UpdateAttributes();
                        centroid5F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);

                        if (placedPointsFig2.ContainsKey(5))
                        {
                            placedPointsFig2.Remove(placedPointsFig2.ElementAt(5).Key);
                        }
                        placedPointsFig2.Add(5, figurePoints2);
                        scene.Figures[2].UpdateAttributes();
                        centroid5F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);

                        if (placedPointsFig3.ContainsKey(5))
                        {
                            placedPointsFig3.Remove(placedPointsFig3.ElementAt(5).Key);
                        }
                        placedPointsFig3.Add(5, figurePoints3);
                        scene.Figures[3].UpdateAttributes();
                        centroid5F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                    }
                    break;
            }
        }

        // Boton para reproducir la animacion
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();


            System.Diagnostics.Debug.WriteLine("placedPoints1");
            foreach (List<PointF> x in placedPointsFig1.Values)
            {
                for (int i = 0; i < x.Count; i++)
                {
                    System.Diagnostics.Debug.WriteLine(x[i]);
                }
            }
            System.Diagnostics.Debug.WriteLine("placedPoints2");
            foreach (List<PointF> x in placedPointsFig2.Values)
            {
                for (int i = 0; i < x.Count; i++)
                {
                    System.Diagnostics.Debug.WriteLine(x[i]);
                }
            }
            System.Diagnostics.Debug.WriteLine("placedPoints3");
            foreach (List<PointF> x in placedPointsFig3.Values)
            {
                for (int i = 0; i < x.Count; i++)
                {
                    System.Diagnostics.Debug.WriteLine(x[i]);
                }
            }
        }

        //Boton para grabar
        private void button1_Click(object sender, EventArgs e)
        {
            puntosFiguras();
            //start = DateTime.Now;
            //timer1.Start();
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            if(counterFigures == 1)
            {
                timer1.Interval = 50;
            } else if(counterFigures == 2)
            {
                timer1.Interval = 100;
            } else if(counterFigures == 3)
            {
                timer1.Interval = 150;
            }
            switch (trackBarTime.Value)
            {
                case 0:
                    if (centroid0F1 != new PointF(0, 0) && centroid1F1 != new PointF(0, 0))
                    {
                        if(centroid0F1.X != centroid1F1.X && centroid0F1.Y != centroid1F1.Y)
                        {
                            scene.Figures[1].Follow(centroid0F1, centroid1F1, (float)0.1 + counterTimer);
                            counterTimer+=(float) 0.1;
                            scene.Figures[1].UpdateAttributes();
                            centroid0F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                        } else
                        {
                            counterTimer = 0;
                            trackBarTime.Value++;
                        }
                    }
                    if(counterFigures > 1)
                    {
                        if (centroid0F2 != new PointF(0, 0) && centroid1F2 != new PointF(0, 0))
                        {
                            if (centroid0F2.X != centroid1F2.X && centroid0F2.Y != centroid1F2.Y)
                            {
                                scene.Figures[2].Follow(centroid0F2, centroid1F2, (float)0.1 + counterTimer);
                                counterTimer += (float)0.1;
                                scene.Figures[2].UpdateAttributes();
                                centroid0F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                            }
                            else
                            {
                                counterTimer = 0;
                                trackBarTime.Value++;
                            }
                        }
                    }
                    if(counterFigures > 2)
                    {
                        if (centroid0F3 != new PointF(0, 0) && centroid1F3 != new PointF(0, 0))
                        {
                            if (centroid0F3.X != centroid1F3.X && centroid0F3.Y != centroid1F3.Y)
                            {
                                scene.Figures[3].Follow(centroid0F3, centroid1F3, (float)0.1 + counterTimer);
                                counterTimer += (float)0.1;
                                scene.Figures[3].UpdateAttributes();
                                centroid0F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                            }
                            else
                            {
                                counterTimer = 0;
                                trackBarTime.Value++;
                            }
                        }
                    }
                    break;
                case 1:
                    if(centroid1F1 != new PointF(0, 0) && centroid2F1 != new PointF(0, 0))
                    {
                        if (centroid1F1.X != centroid2F1.X && centroid1F1.Y != centroid2F1.Y)
                        {
                            scene.Figures[1].Follow(centroid1F1, centroid2F1, (float)0.1 + counterTimer);
                            counterTimer += (float)0.1;
                            scene.Figures[1].UpdateAttributes();
                            centroid1F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                        }
                        else
                        {
                            counterTimer = 0;
                            trackBarTime.Value++;
                        }
                    }
                    if(counterFigures > 1)
                    {
                        if (centroid1F2 != new PointF(0, 0) && centroid2F2 != new PointF(0, 0))
                        {
                            if (centroid1F2.X != centroid2F2.X && centroid1F2.Y != centroid2F2.Y)
                            {
                                scene.Figures[2].Follow(centroid1F2, centroid2F2, (float)0.1 + counterTimer);
                                counterTimer += (float)0.1;
                                scene.Figures[2].UpdateAttributes();
                                centroid1F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                            }
                            else
                            {
                                counterTimer = 0;
                                trackBarTime.Value++;
                            }
                        }
                    }
                    if(counterFigures > 2)
                    {
                        if (centroid1F3 != new PointF(0, 0) && centroid2F3 != new PointF(0, 0))
                        {
                            if (centroid1F3.X != centroid2F3.X && centroid1F3.Y != centroid2F3.Y)
                            {
                                scene.Figures[3].Follow(centroid1F3, centroid2F3, (float)0.1 + counterTimer);
                                counterTimer += (float)0.1;
                                scene.Figures[3].UpdateAttributes();
                                centroid1F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                            }
                            else
                            {
                                counterTimer = 0;
                                trackBarTime.Value++;
                            }
                        }
                    }
                    break;
                case 2:
                    if(centroid2F1 != new PointF(0, 0) && centroid3F1 != new PointF(0, 0))
                    {
                        if (centroid2F1.X != centroid3F1.X && centroid2F1.Y != centroid3F1.Y)
                        {
                            scene.Figures[1].Follow(centroid2F1, centroid3F1, (float)0.1 + counterTimer);
                            counterTimer += (float)0.1;
                            scene.Figures[1].UpdateAttributes();
                            centroid2F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                        }
                        else
                        {
                            counterTimer = 0;
                            trackBarTime.Value++;
                        }
                    }
                    if(counterFigures > 1)
                    {
                        if (centroid2F2 != new PointF(0, 0) && centroid3F2 != new PointF(0, 0))
                        {
                            if (centroid2F2.X != centroid3F2.X && centroid2F2.Y != centroid3F2.Y)
                            {
                                scene.Figures[2].Follow(centroid2F2, centroid3F2, (float)0.1 + counterTimer);
                                counterTimer += (float)0.1;
                                scene.Figures[2].UpdateAttributes();
                                centroid2F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                            }
                            else
                            {
                                counterTimer = 0;
                                trackBarTime.Value++;
                            }
                        }
                    }
                    if(counterFigures > 2)
                    {
                        if (centroid2F3 != new PointF(0, 0) && centroid3F3 != new PointF(0, 0))
                        {
                            if (centroid2F3.X != centroid3F3.X && centroid2F3.Y != centroid3F3.Y)
                            {
                                scene.Figures[3].Follow(centroid2F3, centroid3F3, (float)0.1 + counterTimer);
                                counterTimer += (float)0.1;
                                scene.Figures[3].UpdateAttributes();
                                centroid2F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                            }
                            else
                            {
                                counterTimer = 0;
                                trackBarTime.Value++;
                            }
                        }
                    }
                    break;
                case 3:
                    if(centroid3F1 != new PointF(0, 0) && centroid4F1 != new PointF(0, 0))
                    {
                        if (centroid3F1.X != centroid4F1.X && centroid3F1.Y != centroid4F1.Y)
                        {
                            scene.Figures[1].Follow(centroid3F1, centroid4F1, (float)0.1 + counterTimer);
                            counterTimer += (float)0.1;
                            scene.Figures[1].UpdateAttributes();
                            centroid3F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                        }
                        else
                        {
                            counterTimer = 0;
                            trackBarTime.Value++;
                        }
                    }
                    if(counterFigures > 1)
                    {
                        if (centroid3F2 != new PointF(0, 0) && centroid4F2 != new PointF(0, 0))
                        {
                            if (centroid3F2.X != centroid4F2.X && centroid3F2.Y != centroid4F2.Y)
                            {
                                scene.Figures[2].Follow(centroid3F2, centroid4F2, (float)0.1 + counterTimer);
                                counterTimer += (float)0.1;
                                scene.Figures[2].UpdateAttributes();
                                centroid3F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                            }
                            else
                            {
                                counterTimer = 0;
                                trackBarTime.Value++;
                            }
                        }
                    }
                    if(counterFigures > 2)
                    {
                        if (centroid3F3 != new PointF(0, 0) && centroid4F3 != new PointF(0, 0))
                        {
                            if (centroid3F3.X != centroid4F3.X && centroid3F3.Y != centroid4F3.Y)
                            {
                                scene.Figures[3].Follow(centroid3F3, centroid4F3, (float)0.1 + counterTimer);
                                counterTimer += (float)0.1;
                                scene.Figures[3].UpdateAttributes();
                                centroid3F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                            }
                            else
                            {
                                counterTimer = 0;
                                trackBarTime.Value++;
                            }
                        }
                    }
                    break;
                case 4:
                    if(centroid4F1 != new PointF(0, 0) && centroid5F1 != new PointF(0, 0))
                    {
                        if (centroid4F1.X != centroid5F1.X && centroid4F1.Y != centroid5F1.Y)
                        {
                            scene.Figures[1].Follow(centroid4F1, centroid5F1, (float)0.1 + counterTimer);
                            counterTimer += (float)0.1;
                            scene.Figures[1].UpdateAttributes();
                            centroid4F1 = new PointF(scene.Figures[1].Centroid.X, scene.Figures[1].Centroid.Y);
                        }
                        else
                        {
                            counterTimer = 0;
                            trackBarTime.Value++;
                        }
                    }
                    if(counterFigures > 1)
                    {
                        if (centroid4F2 != new PointF(0, 0) && centroid5F2 != new PointF(0, 0))
                        {
                            if (centroid4F2.X != centroid5F2.X && centroid4F2.Y != centroid5F2.Y)
                            {
                                scene.Figures[2].Follow(centroid4F2, centroid5F2, (float)0.1 + counterTimer);
                                counterTimer += (float)0.1;
                                scene.Figures[2].UpdateAttributes();
                                centroid4F2 = new PointF(scene.Figures[2].Centroid.X, scene.Figures[2].Centroid.Y);
                            }
                            else
                            {
                                counterTimer = 0;
                                trackBarTime.Value++;
                            }
                        }
                    }
                    if(counterFigures > 2)
                    {
                        if (centroid4F3 != new PointF(0, 0) && centroid5F3 != new PointF(0, 0))
                        {
                            if (centroid4F3.X != centroid5F3.X && centroid4F3.Y != centroid5F3.Y)
                            {
                                scene.Figures[3].Follow(centroid4F3, centroid5F3, (float)0.1 + counterTimer);
                                counterTimer += (float)0.1;
                                scene.Figures[3].UpdateAttributes();
                                centroid4F3 = new PointF(scene.Figures[3].Centroid.X, scene.Figures[3].Centroid.Y);
                            }
                            else
                            {
                                counterTimer = 0;
                                trackBarTime.Value++;
                            }
                        }
                    }
                    break;
            }
            if (trackBarTime.Value == 5)
            {
                timer1.Stop();
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (scene.RepFigures.Count > nFigures)
            {
                canvas.RenderFigsGr(scene, nFigures);
                nFigures++;
            }
            else
            {
                timer2.Stop(); 
            }

            //tReproduce.Text = ((DateTime.Now - startRepro).TotalSeconds).ToString(); 
        }

        //private void recTime() //Funcion para contar los 5 segundos de la grabacion
        //{
        //    seg.Text = ((DateTime.Now - start).TotalSeconds).ToString();

        //    if ((DateTime.Now - start).TotalSeconds >= 10.00) // Cuando llegue a los 5 segundos de grabacion
        //    {
        //        //timer1.Stop(); //Detener timer para grabacion
        //    }
        //}

    }
}
