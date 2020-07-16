using System.Drawing;
using System.Drawing.Drawing2D;

namespace LimeBot.Bot.Utils
{
    public static class GraphicsHelper
    {
        public static void DrawRoundedRectangle(this Graphics gfx, RectangleF bounds, int cornerRadius, Color fillColor)
        {
            GraphicsPath gfxPath = new GraphicsPath();
            gfxPath.AddArc(bounds.X, bounds.Y, cornerRadius, cornerRadius, 180, 90);
            gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y, cornerRadius, cornerRadius, 270, 90);
            gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            gfxPath.AddArc(bounds.X, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            gfxPath.CloseAllFigures();

            gfx.FillPath(new SolidBrush(fillColor), gfxPath);
        }

        public static Image OvalImage(Image img)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddEllipse(0, 0, img.Width, img.Height);
                using (Graphics gr = Graphics.FromImage(bmp))
                {
                    gr.SetClip(gp);
                    gr.DrawImage(img, Point.Empty);
                }
            }
            return bmp;
        }
    }
}
