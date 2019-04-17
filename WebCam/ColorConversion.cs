using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace   WebCam
{
    class ColorConversion
    {
        public static Color ColorFromhsb(float h, float s, float b)
        {

            if (0f > h || 360f < h) return Color.White;
            if (0f > s || 1f < s) return Color.White;
            if (0f > b || 1f < b) return Color.White;
            if (0 == s)
            {
                return Color.FromArgb(255, Convert.ToInt32(b * 255),
                  Convert.ToInt32(b * 255), Convert.ToInt32(b * 255));
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < b)
            {
                fMax = b - (b * s) + s;
                fMin = b + (b * s) - s;
            }
            else
            {
                fMax = b + (b * s);
                fMin = b - (b * s);
            }

            iSextant = (int)Math.Floor(h / 60f);
            if (300f <= h)
            {
                h -= 360f;
            }
            h /= 60f;
            h -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = h * (fMax - fMin) + fMin;
            }
            else
            {
                fMid = fMin - h * (fMax - fMin);
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(255, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(255, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(255, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(255, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(255, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(255, iMax, iMid, iMin);
            }
        }
    }
}
