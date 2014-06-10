using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;

namespace NTS.WEB.Common
{
    public class VerifyImage
    {
        private string _code;
        private Random _random;
        private Pen _borderColor = Pens.DarkGray;
        private Color _backColor = Color.White;
        private int _width = 66;
        private int _height = 24;
        //验证码字体个数
        private int _numberSize = 4;
        //验证码字体大小
        private int _fontSize = 11;

        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }
        public Pen BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }
        public string _Code
        {
            get { return _code; }
            set { _code = value; }
        }
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        public Random _Random
        {
            get { return _random; }
            set { _random = value; }
        }
        public int NumberSize
        {
            get { return _numberSize; }
            set { _numberSize = value; }
        }

        private int _brushNameIndex;

        public VerifyImage() { }

        public VerifyImage(int width, int height, Color backColor, Pen borderColor, int numberSize)
        {
            this.Width = width;
            this.Height = height;
            this.BackColor = backColor;
            this.BorderColor = borderColor;
            this.NumberSize = numberSize;
        }

        #region 随机样式
        static string[] FontItems = new string[] {
                                                  "Verdana"
                                              };

        static Brush[] BrushItems = new Brush[] {Brushes.OliveDrab,
                                                 Brushes.ForestGreen,
                                                 Brushes.DarkCyan,
                                                 Brushes.LightSlateGray,
                                                 Brushes.RoyalBlue,
                                                 Brushes.SlateBlue,
                                                 Brushes.DarkViolet,
                                                 Brushes.MediumVioletRed,
                                                 Brushes.IndianRed,
                                                 Brushes.Firebrick,
                                                 Brushes.Chocolate,
                                                 Brushes.Peru,
                                                 Brushes.Goldenrod
                                            };

        static string[] BrushName = new string[] {"OliveDrab",
                                                  "ForestGreen",
                                                  "DarkCyan",
                                                  "LightSlateGray",
                                                  "RoyalBlue",
                                                  "SlateBlue",
                                                  "DarkViolet",
                                                  "MediumVioletRed",
                                                  "IndianRed",
                                                  "Firebrick",
                                                  "Chocolate",
                                                  "Peru",
                                                  "Goldenrod"
                                             };
        #endregion

        /// <summary>
        /// 取得一个 4 位的随机码
        /// </summary>
        /// <returns></returns>
        //public string GetRandomCode()
        //{
        //    return Guid.NewGuid().ToString().Substring(0, NumberSize);
        //}

        private char[] constant = 
        {
            '2','3','4','5','6','7','8','9',
            'a','b','c','d','e','f','g','h','i','j','k','m','n','p','q','r','s','t','u','v','w','x','y','z'
        };
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public string GetRandomCode()
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder();
            Random rd = new Random();
            for (int i = 0; i < NumberSize; i++)
            {
                newRandom.Append(constant[rd.Next(constant.Length - 1)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 随机取一个字体
        /// </summary>
        /// <returns></returns>
        private Font GetFont()
        {
            int fontIndex = _random.Next(0, FontItems.Length);
            FontStyle fontStyle = GetFontStyle(_random.Next(0, 2));
            return new Font(FontItems[fontIndex], _fontSize, fontStyle);
        }

        /// <summary>
        /// 取一个字体的样式
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private FontStyle GetFontStyle(int index)
        {

            return FontStyle.Bold;

        }

        /// <summary>
        /// 随机取一个笔刷
        /// </summary>
        /// <returns></returns>
        private Brush GetBrush()
        {
            int brushIndex = _random.Next(0, BrushItems.Length);
            _brushNameIndex = brushIndex;
            return BrushItems[brushIndex];
        }

        public Bitmap GetVerifyImage()
        {
            Bitmap objBitmap = null;
            Graphics g = null;
            objBitmap = new Bitmap(Width, Height);
            g = Graphics.FromImage(objBitmap);

            Paint_Background(g);
            Paint_Text(g);
            Paint_TextStain(objBitmap);
            Paint_Border(g);

            if (null != g)
                g.Dispose();

            return objBitmap;
        }
        /// <summary>
        /// 绘画背景颜色
        /// </summary>
        /// <param name="g"></param>
        private void Paint_Background(Graphics g)
        {
            g.Clear(BackColor);
        }

        /// <summary>
        /// 绘画边框
        /// </summary>
        /// <param name="g"></param>
        private void Paint_Border(Graphics g)
        {
            //   g.DrawRectangle(BorderColor, 0, 0, Width - 1, Height - 1);
        }

        /// <summary>
        /// 绘画文字
        /// </summary>
        /// <param name="g"></param>
        private void Paint_Text(Graphics g)
        {
            Font f = GetFont();
            g.DrawString(_code, f, GetBrush(), (this.Width - f.Size * NumberSize) / 2, (this.Height - f.Height) / 2);
        }

        /// <summary>
        /// 绘画文字噪音点
        /// </summary>
        /// <param name="g"></param>
        private void Paint_TextStain(Bitmap b)
        {
            for (int n = 0; n < 20; n++)
            {
                int x = _random.Next(Width);
                int y = _random.Next(Height);
                b.SetPixel(x, y, Color.FromName(BrushName[_brushNameIndex]));
            }

        }
    }
}
