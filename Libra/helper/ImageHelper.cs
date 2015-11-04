using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;

namespace Libra.helper
{
    public class ImageHelper
    {
        public static void DoGetImage(string url, string path)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            req.ServicePoint.Expect100Continue = false;
            req.Method = "GET";
            req.KeepAlive = true;

            req.ContentType = "image/jpg";
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();

            System.IO.Stream stream = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                System.Drawing.Image.FromStream(stream).Save(path);
            }
            finally
            {
                // 释放资源
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }

        /// <summary>
        /// 以逆时针为方向对图像进行旋转
        /// </summary>
        /// <param name="img">位图流</param>
        /// <param name="angle">旋转角度[0,360](前台给的)</param>
        /// <returns></returns>
        public static Bitmap Rotate(Bitmap img, int angle, bool dispose = true)
        {
            angle = angle % 360;
            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //原图的宽和高
            int w = img.Width;
            int h = img.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));
            //目标位图
            Bitmap image = new Bitmap(W, H);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //计算偏移量
            Point Offset = new Point((W - w) / 2, (H - h) / 2);
            //构造图像显示区域：让图像的中心与窗口的中心点一致
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);
            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(img, rect);
            //重至绘图的所有变换
            g.ResetTransform();
            g.Save();
            g.Dispose();

            if (dispose)
            {
                img.Dispose();
            }

            //dsImage.Save("yuancd.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            return image;
        }

        //public static List<Rectangle> GetRectangle(Bitmap bitmap)
        //{
        //    List<Rectangle> rectList = new List<Rectangle>();

        //    int w = bitmap.Width;
        //    int h = bitmap.Height;
        //    Color color;
        //    for (int row = 0; row < h; row++)
        //    {
        //        for (int col = 0; col < w; col++)
        //        {
        //            color = bitmap.GetPixel(col, row);
        //            if (color.A > 0)
        //            {

        //            }
        //        }
        //    }

        //    return rectList;
        //}

        /// <summary>
        /// 根据图片得到一个图片非透明部分的区域
        /// </summary>
        /// <param name="bckImage"></param>
        /// <returns></returns>
        public static unsafe Region GetRegion(Bitmap bckImage)
        {
            GraphicsPath path = new GraphicsPath();
            int w = bckImage.Width;
            int h = bckImage.Height;
            BitmapData bckdata = null;
            try
            {
                bckdata = bckImage.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                uint* bckInt = (uint*)bckdata.Scan0;
                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        if ((*bckInt & 0xff000000) != 0)
                        {
                            path.AddRectangle(new Rectangle(i, j, 1, 1));
                        }
                        bckInt++;
                    }
                }
                bckImage.UnlockBits(bckdata); bckdata = null;
            }
            catch
            {
                if (bckdata != null)
                {
                    bckImage.UnlockBits(bckdata);
                    bckdata = null;
                }
            }
            Region region = new Region(path);
            path.Dispose(); path = null;
            return region;
        }


    }
}
