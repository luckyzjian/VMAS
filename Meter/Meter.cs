using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;

namespace Chart
{
    public partial class Meter : UserControl
    {
        public Meter()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            DrawBackImg();
            pic.Invalidate();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;
        }
        #region 字段 属性
        int _diameter = 150;//默认
        double _maxValue = 100;//最大
        double _changeValue = 0;//改变量
        int _PinLen = 60;//默认
        Color _pinColor = Color.Black;//指针颜色
        Color _frameColor = Color.Black;//框架颜色
        string _unitStr = "";//单位
        StringFormat strFormat = new StringFormat();
        bool _isMaxValueChange = false;
        /// <summary>
        /// 直径
        /// </summary>
        public int Diameter
        {
            get { return _diameter; }
            set
            {
                if (value > 10 || value < 500)
                {
                    _diameter = value - 1;
                }
                this.Width = this.Height = value;
                DrawBackImg();
                pic.Invalidate();
            }
        }
        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxValue
        {
            get { return _maxValue; }
            set
            {
                if (value >= 1)
                {
                    _maxValue = value;

                    ValueSpan = value / 270.0;

                    DrawBackImg();
                }
            }
        }
        public double ChangeValue
        {
            get { return _changeValue; }
            set
            {
                //改变最大值
                if (_isMaxValueChange && value >= _maxValue)
                {
                    //完成自适应性
                    MaxValue = value;
                }

                this.tmChangvalue = value;
                startDraw();
            }
        }
        public int PinLen
        {
            get { return _PinLen; }
            set { _PinLen = value; pic.Invalidate(); }
        }
        public int NxPinLen
        {
            get { return (int)(_PinLen / 5); }
        }
        public Color PinColor
        {
            get { return _pinColor; }
            set { _pinColor = value; pic.Invalidate(); DrawBackImg(); }
        }
        public Color FrameColor
        {
            get { return _frameColor; }
            set { _frameColor = value; DrawBackImg(); }
        }
        public string UnitStr
        {
            get { return _unitStr; }
            set { _unitStr = value; DrawBackImg(); pic.Invalidate(); }
        }

        private bool _isDrawBackGround = false;
        /// <summary>
        /// 是否绘制背景颜色
        /// </summary>
        public bool IsDrawBackGround
        {
            get { return _isDrawBackGround; }
            set { _isDrawBackGround = value; DrawBackImg(); }
        }

        private Color _centerColor = Color.White;

        /// <summary>
        /// 渐变中心颜色
        /// </summary>
        public Color CenterColor
        {
            get { return _centerColor; }
            set { _centerColor = value; DrawBackImg(); }
        }
        private Color _borderColor = Color.White;
        /// <summary>
        /// 扩展颜色
        /// </summary>
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; DrawBackImg(); }
        }

        /// <summary>
        /// 最大值是否可变
        /// </summary>
        public bool IsMaxValueChange
        {
            get { return _isMaxValueChange; }
            set { _isMaxValueChange = value; }
        }
        #endregion

        public void Clear()
        {
            _changeValue = 0;
            this.Invalidate();
        }

        #region 画针
        double tmChangvalue = 0;
        double ValueSpan = 0;


        private void startDraw()
        {
            Thread threadDraw = new Thread(new ThreadStart(drawTrhead));
            threadDraw.IsBackground = false;
            threadDraw.Start();
        }
        private void drawTrhead()
        {
            //改变量
            double tmp = tmChangvalue - _changeValue;
            if (tmp > 0)//变大
            {
                for (double j = 0; j <= tmp; )
                {
                    j += ValueSpan;
                    if (_changeValue + ValueSpan < _maxValue)
                    {
                        _changeValue += ValueSpan;
                    }
                    else if (_isMaxValueChange && _changeValue <= _maxValue && _changeValue + ValueSpan >= _maxValue)
                    {
                        _changeValue = _maxValue;
                    }

                    Thread.Sleep(1);
                    this.Invalidate();
                }
            }
            else
            {
                for (double j = 0; j < -tmp; )
                {

                    j += ValueSpan;
                    if (_changeValue - ValueSpan > 0)
                    {
                        _changeValue -= ValueSpan;
                    }
                    else if (_changeValue >= 0 && _changeValue - ValueSpan <= 0)
                    {
                        _changeValue = 0;
                    }

                    Thread.Sleep(1);
                    this.Invalidate();
                }
            }
        }

        private void DrawForeImg(Graphics gp)
        {
            Bitmap bit = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(bit);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //画针
            DrawPin(g);
            DrawString(g);
            gp.DrawImage(bit, new Point(0, 0));
            g.Dispose();

        }
        //画针
        private void DrawPin(Graphics g)
        {
            int cer = _diameter / 2;
            float start = 135;
            float sweepShot = (float)(_changeValue / _maxValue * 270.0);

            Pen linePen = new Pen(_pinColor, 1);
            Pen NxPen = new Pen(_pinColor, 2);
            Pen xPen = new Pen(_pinColor, 5);
            double rad = (sweepShot + start) * Math.PI / 180;
            float radius = _diameter / 2 - 5;
            int dx = (int)(cer + (_PinLen) * Math.Cos(rad));
            int dy = (int)(cer + (_PinLen) * Math.Sin(rad));

            int px = (int)(cer + (_PinLen * 0.4) * Math.Cos(rad));
            int py = (int)(cer + (_PinLen * 0.4) * Math.Sin(rad));

            int nx = (int)(cer - (NxPinLen) * Math.Sin(rad));
            int ny = (int)(cer - (NxPinLen) * Math.Cos(rad));
            g.DrawLine(linePen, new Point(cer, cer), new Point(dx, dy));
            g.DrawLine(NxPen, new Point(cer, cer), new Point(px, py));
            g.DrawLine(xPen, new Point(cer, cer), new Point(ny, nx));
        }

        private void DrawString(Graphics g)
        {
            int cer = _diameter / 2;
            string str = _changeValue.ToString("F2");
            g.DrawString(str, new Font("宋体", 10), new SolidBrush(_pinColor), new PointF(cer, (float)(cer + cer * 0.4)), strFormat);
        }
        #endregion


        #region 画框架

        /// <summary>
        /// 绘制背景
        /// </summary>
        private void DrawBackImg()
        {
            Bitmap bit = new Bitmap(this.Width, this.Height);
            Graphics gp = Graphics.FromImage(bit);
            gp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            #region 在这里可以扩展需要绘制的背景项目
            if (_isDrawBackGround)
            {
                //背景渐变色
                drawBachground(gp);
            }
            //外框
            drawFrame(gp);

            // 画刻度
            DrawRuling(gp);
            //画点
            drawPoint(gp);

            DrawUnitStr(gp);

            #endregion

            this.BackgroundImage = bit;
        }

        private void DrawUnitStr(Graphics gp)
        {
            int cer = _diameter / 2;
            gp.DrawString(_unitStr, new Font("宋体", 10), new SolidBrush(_frameColor), new PointF(cer, (float)(cer - cer * 0.3)), strFormat);

        }

        private void DrawLevel(Graphics g)
        {

        }

        /// <summary>
        /// 画外框
        /// </summary>
        /// <param name="gp"></param>
        private void drawFrame(Graphics gp)
        {
            Pen pen = new Pen(_frameColor, 2);
            Rectangle rec = new Rectangle(5, 5, _diameter - 10, _diameter - 10);
            gp.DrawEllipse(pen, rec);
        }

        /// <summary>
        /// 画刻度
        /// </summary>
        /// <param name="gp"></param>
        private void DrawRuling(Graphics gp)
        {
            //刻度
            int cerX = _diameter / 2;
            int cerY = _diameter / 2;
            float start = 135;
            float sweepShot = 0;
            int dx = 0;
            int dy = 0;
            int soildLenght = 8;
            Pen linePen = new Pen(_frameColor, 1);
            float span = (float)(_maxValue / 30);
            float sp = 0;
            //用于右边数字右对齐
            StringFormat stf = new StringFormat();
            stf.Alignment = StringAlignment.Far;

            StringFormat stfMid = new StringFormat();
            stfMid.Alignment = StringAlignment.Center;
            stfMid.LineAlignment = StringAlignment.Center;
            for (int i = 0; i <= 30; i++)
            {
                double rad = (sweepShot + start) * Math.PI / 180;
                float radius = _diameter / 2 - 5;
                int px = (int)(cerX + radius * Math.Cos(rad));
                int py = (int)(cerY + radius * Math.Sin(rad));
                if (sweepShot % 15 == 0)
                {
                    linePen.Width = 2;
                    dx = (int)(cerX + (radius - soildLenght) * Math.Cos(rad));
                    dy = (int)(cerY + (radius - soildLenght) * Math.Sin(rad));
                    string str = sp.ToString("f0");
                    if (sweepShot <= 45)
                    {
                        gp.DrawString(str, new Font("宋体", 9), new SolidBrush(_frameColor), new PointF(dx, dy - 5));
                    }
                    else if (sweepShot > 45 && sweepShot < 135)
                    {
                        gp.DrawString(str, new Font("宋体", 9), new SolidBrush(_frameColor), new PointF(dx, dy));
                    }
                    else if (sweepShot == 135)
                    {
                        gp.DrawString(str, new Font("宋体", 9), new SolidBrush(_frameColor), new PointF(dx, dy + 10), stfMid);
                    }
                    else if (sweepShot > 135 && sweepShot < 225)
                    {
                        gp.DrawString(str, new Font("宋体", 9), new SolidBrush(_frameColor), new PointF(dx, dy), stf);
                    }
                    else if (sweepShot >= 225)
                    {
                        gp.DrawString(str, new Font("宋体", 9), new SolidBrush(_frameColor), new PointF(dx, dy - 5), stf);
                    }

                }
                else
                {
                    linePen.Width = 1;
                    dx = (int)(cerX + (radius - soildLenght + 2) * Math.Cos(rad));
                    dy = (int)(cerY + (radius - soildLenght + 2) * Math.Sin(rad));
                }
                gp.DrawLine(linePen, new Point(px, py), new Point(dx, dy));
                sp += span;
                sweepShot += 9;
            }
        }
        //画中间的点
        private void drawPoint(Graphics gp)
        {
            Pen p = new Pen(_pinColor);
            int tmpWidth = 6;
            int px = _diameter / 2 - tmpWidth;

            gp.DrawEllipse(p, new Rectangle(px, px, 2 * tmpWidth, 2 * tmpWidth));
            gp.FillEllipse(new SolidBrush(_pinColor), new Rectangle(px + 2, px + 2, 2 * tmpWidth - 4, 2 * tmpWidth - 4));
        }

        //画背景渐变色
        private void drawBachground(Graphics gp)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(3, 3, _diameter - 6, _diameter - 6);

            PathGradientBrush pthGrBrush = new PathGradientBrush(path);
            pthGrBrush.CenterColor = _centerColor;
            Color[] colors = { _borderColor };
            pthGrBrush.SurroundColors = colors;
            gp.FillEllipse(pthGrBrush, 3, 3, _diameter - 6, _diameter - 6);
        }

        #endregion

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            DrawForeImg(e.Graphics);
        }

        private void Meter_SizeChanged(object sender, EventArgs e)
        {
            Diameter = this.Width = this.Height;
        }

        private void pic_Paint_1(object sender, PaintEventArgs e)
        {
            DrawForeImg(e.Graphics);
        }
    }
}
