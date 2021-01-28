using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace StreamLaunch
{
    public class RoundButton : Button
    {
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            GraphicsPath grPath = new GraphicsPath();
            grPath.AddEllipse(ClientSize.Width * .05f, ClientSize.Height * .05f, ClientSize.Width - (ClientSize.Width * .1f), ClientSize.Height - (ClientSize.Height * .1f));
            
            this.Region = new System.Drawing.Region(grPath);
            base.OnPaint(e);
        }
    }
}
