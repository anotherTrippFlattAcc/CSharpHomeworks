using System.Drawing.Drawing2D;

namespace GDILogo;

public static class LogoGenerator
    {
        public static Image CreateCompanyLogo(int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                
                g.Clear(Color.Transparent);
                
                int iconSize = (int)(height * 0.7);
                int iconX = 20;
                int iconY = (height - iconSize) / 2;
                
                using (LinearGradientBrush iconBrush = new LinearGradientBrush(
                    new Point(iconX, iconY), 
                    new Point(iconX + iconSize, iconY + iconSize),
                    Color.FromArgb(0, 114, 255),
                    Color.FromArgb(0, 198, 255)))
                {
                    Point[] trianglePoints = new Point[]
                    {
                        new Point(iconX + iconSize / 2, iconY),
                        new Point(iconX + iconSize, iconY + iconSize),
                        new Point(iconX + iconSize / 2, iconY + (int)(iconSize * 0.75)),
                        new Point(iconX, iconY + iconSize)
                    };
                    g.FillPolygon(iconBrush, trianglePoints);
                }
                
                string companyText = "APEX";
                
                using (GraphicsPath textPath = new GraphicsPath())
                {
                    FontFamily fontFamily = new FontFamily("Arial Black");
                    int emSize = (int)(height * 0.45);
                    int textX = iconX + iconSize + 25;
                    int textY = (height / 2) - (emSize / 2) - 30;
                    
                    textPath.AddString(
                        companyText, 
                        fontFamily, 
                        (int)FontStyle.Bold, 
                        g.DpiY * emSize / 72,
                        new Point(textX, textY), 
                        StringFormat.GenericDefault
                    );
                    
                    using (Pen outlinePen = new Pen(Color.FromArgb(20, 30, 45), 8f) { LineJoin = LineJoin.Round })
                    {
                        g.DrawPath(outlinePen, textPath);
                    }
                    
                    using (Brush textBrush = new SolidBrush(Color.White))
                    {
                        g.FillPath(textBrush, textPath);
                    }
                }
            }

            return bitmap;
        }
    }