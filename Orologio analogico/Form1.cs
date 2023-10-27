using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Orologio_analogico
{
    public partial class Form1 : Form
    {
        Timer timer = new Timer();
        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.Black;
            this.Paint += new PaintEventHandler(paint);
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void paint(object sender, PaintEventArgs e)
        {
            int raggio = Math.Min(ClientSize.Width, ClientSize.Height) / 2 - 10;
            // raggio si adatta alle dimensioni del form, utilizzando la metà del lato più piccolo del form come raggio

            int centroX = ClientSize.Width / 2;
            int centroY = ClientSize.Height / 2;
            Graphics g = e.Graphics;

            // disegno il cerchio di base che sarà la cornice dell'orologio
            Color colore = Color.Gray;
            e.Graphics.FillEllipse(new SolidBrush(colore), centroX - raggio, centroY - raggio, 2 * raggio, 2 * raggio);

            // disegno le tacchette delle ore
            for (int i = 0; i < 12; i++)
            {
                double angolo = Math.PI / 6 * i; // pi/6 = 30°, ogni ora la lancetta delle ore si sposta di 30°
                int x = (int)(centroX + (raggio - 10) * Math.Sin(angolo)); // il -10 serve per spostare i punti verso l'interno
                int y = (int)(centroY - (raggio - 10) * Math.Cos(angolo));
                g.FillRectangle(Brushes.White, x - 2, y - 2, 4, 4);
                // FillRectangle perché la lancetta delle ore deve essere ingrossata, e non deve essere solo una linea
            }

            // ottengo l'orario corrente in ore, minuti e secondi
            DateTime adesso = DateTime.Now;
            int ore = adesso.Hour % 12;
            int minuti = adesso.Minute;
            int secondi = adesso.Second;

            // disegno le lancette
            freccia(e.Graphics, centroX, centroY, raggio * 0.5, (ore + minuti / 60.0) * 30, Pens.Orange, 6); // lanc. ore

            /* (ore + minuti / 60.0) calcola l'ora effettiva, considerando la frazione dei minuti e
             moltiplica l'ora effettiva per 30, poiché ci sono 30 gradi tra 
            ogni ora sull'orologio (360 gradi diviso per 12 ore) */

            freccia(g, centroX, centroY, raggio * 0.8, minuti * 6, Pens.Red, 4); // lanc. minuti
            freccia(g, centroX, centroY, raggio * 0.9, secondi * 6, Pens.Cyan, 2); // lanc. secondi

            int centerSize = 10;
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;
            e.Graphics.FillEllipse(Brushes.White, centerX - centerSize / 2, centerY - centerSize / 2, centerSize, centerSize);

        }

        private void freccia(Graphics g, int x, int y, double length, double angle, Pen pen, int width)
        {
            double radianti = Math.PI / 180 * angle;
            int x2 = (int)(x + length * Math.Sin(radianti));
            int y2 = (int)(y - length * Math.Cos(radianti));
            g.DrawLine(new Pen(pen.Color, width), x, y, x2, y2);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}

