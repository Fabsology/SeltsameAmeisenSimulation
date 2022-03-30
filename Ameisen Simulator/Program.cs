// 
// 30.03.2022
// fabianmueller1207@gmail.com (Germany)
//
// URL  : paitorocxon.github.io
// Infos: MetaInfo.txt
// Tags : 
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Ameisen_Simulator
{
	class Program
	{
		
		static Graphics graphics;
        static BufferedGraphics bufferedGraphics;
        static Ameise ameischen;
		
		public static void Main(string[] args)
		{
			
            Random r = new Random();
            int WeltX = 600;
            int WeltY = 400;
            
            int SPRUNG = 10 *1000; // SEKUNDEN *1000 für Sekunden
            int MESSUNGBIS = 30;
            int BILDRATE = 200;
            
            int statistikPunkt = 0;
            int letzteMessungZahl = 0;
            string Messungen = "Start der Messung:" + '\n';
            DateTime letzteMessungZeitpunkt = DateTime.Now;
            
			int anzahlDerAmeisen = 100;
			System.Console.WriteLine("Bitte die Anzahl der Ameisen angeben:");
			int.TryParse(System.Console.ReadLine(),out anzahlDerAmeisen);
			System.Console.WriteLine("Weltbreite:");
			int.TryParse(System.Console.ReadLine(),out WeltX);
			System.Console.WriteLine("Weltlänge:");
			int.TryParse(System.Console.ReadLine(),out WeltY);
			System.Console.WriteLine("Messungspunkte Anzahl:");
			int.TryParse(System.Console.ReadLine(),out MESSUNGBIS);
			System.Console.WriteLine("Messunge alle N-Millisekunden:");
			int.TryParse(System.Console.ReadLine(),out SPRUNG);
			System.Console.WriteLine("Millisekunden bis Bilderneuerung:");
			int.TryParse(System.Console.ReadLine(),out BILDRATE);
			
            Messungen += " Initialmesswerte:" + '\n';
            Messungen += "  Ameisen:\t\t" + anzahlDerAmeisen +'\n';
            Messungen += "  Weltengröße:\t\t" + WeltX + "x" + WeltY + '\n';
            Messungen += "  Messpunkte:\t\t" + MESSUNGBIS + " á " +  (SPRUNG/1000) + "s" + '\n';
            Messungen += "  Gesamte Messzeit:\t" + (float)(MESSUNGBIS * SPRUNG) / 1000 + "s" + '\n';
            Messungen += "============================================";
            
			
			
			
            Point WELTGROESSE = new Point(WeltX,WeltY);
			
			Ameise[] ameisenKolonie = new Ameise[anzahlDerAmeisen];
			for (int anzahlZähler = 0; anzahlZähler < anzahlDerAmeisen; anzahlZähler++){
				ameisenKolonie[anzahlZähler] = new Ameise(r.Next(0,WeltX - 32),r.Next(0,WeltY - 32));
			}
			
            Console.CursorVisible = false;
            Process process = Process.GetCurrentProcess();
            graphics = Graphics.FromHdc(GetDC(process.MainWindowHandle));
            BufferedGraphicsContext context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(Console.WindowWidth, Console.WindowHeight);
            bufferedGraphics = context.Allocate(graphics, new Rectangle(0, 0, WELTGROESSE.X, WELTGROESSE.Y));
			
            long start = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			while (true)
            {
				
				
				if (letzteMessungZeitpunkt.Ticks / 10000 < (DateTime.Now.Ticks / 10000) - SPRUNG) {
					Messungen += "\n" + statistikPunkt + "\t" +  SPRUNG + ": \t+" +  (ameisenKolonie.Length - letzteMessungZahl);
					letzteMessungZahl = ameisenKolonie.Length;
					statistikPunkt++;
					letzteMessungZeitpunkt = DateTime.Now;
					if (statistikPunkt > MESSUNGBIS) {
						System.Console.Clear();
						System.Console.WriteLine(Messungen);
						System.Console.WriteLine(" Summe der Ameisen jetzt: " + ameisenKolonie.Length);
						System.Console.WriteLine(" Differenz: " + (ameisenKolonie.Length - anzahlDerAmeisen));
						
						System.Console.ReadLine();
						Environment.Exit(0);
					}
					
				}
				long DTNT = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
				double timespan = 1000 / ((DTNT - milliseconds)+0.01);
				
				bufferedGraphics.Graphics.Clear(Color.FromArgb(255,200,150,0));
				//bufferedGraphics.Graphics.Clear(Color.FromArgb(255,200,150,0));
				for (int anzahlZähler = 0; anzahlZähler < ameisenKolonie.Length; anzahlZähler++){
					Ameise ameischen = ameisenKolonie[anzahlZähler];
					//ameischen.bewegeNachVorne(1,WELTGROESSE, ref ameisenKolonie);
					ameischen.bewegeNachVorneMitDistanz(1,WELTGROESSE, ref ameisenKolonie,bufferedGraphics.Graphics);
					
					
					
					ameischen.drehen(r.Next(-20,20));
					
					if (DTNT - start > BILDRATE ) {
	                	ameischen.Draw(bufferedGraphics.Graphics);
					}
				}
                
                
				
				milliseconds = DTNT;
				
				if (DTNT - start > BILDRATE ) {
				bufferedGraphics.Graphics.DrawString((DTNT - start).ToString() + " FP/s", new Font("Arial", 16),new SolidBrush(Color.Blue),new Point(0,0));
				//bufferedGraphics.Graphics.DrawString(Math.Round(timespan,2).ToString() + " FP/s", new Font("Arial", 16),new SolidBrush(Color.Blue),new Point(0,0));
				bufferedGraphics.Graphics.DrawString(ameisenKolonie.Length.ToString() + " Ameisen", new Font("Arial", 14),new SolidBrush(Color.Black),new Point(121,0));
				bufferedGraphics.Graphics.DrawString(ameisenKolonie.Length.ToString() + " Ameisen", new Font("Arial", 14),new SolidBrush(Color.Black),new Point(120,1));
				bufferedGraphics.Graphics.DrawString(ameisenKolonie.Length.ToString() + " Ameisen", new Font("Arial", 14),new SolidBrush(Color.FromArgb(255,150,200,100)),new Point(120,0));
                Program.bufferedGraphics.Render(Program.graphics);
					start = DTNT;
				}
            }
			
			
			
			Console.ReadKey(true);
		}
		
			

				
			
			
			
		// Für mehr Information, siehe https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdc
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
	    public static extern IntPtr GetDC(IntPtr hWnd);
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	class Ameise
	{
        
		bool debug = true;
		
        public double x;					// Koordinate X der Ameise
        public double y;					// Koordinate Y der Ameise
        public int alter = 0;
        public float blickRichtung = 0;
        
        public readonly Bitmap OriginalBild = new Bitmap("ameise.png");
        public readonly Bitmap OriginalBildSchatten = new Bitmap("schatten.png");
        public Bitmap ameiseBild = new Bitmap("ameise.png");
        public Bitmap ameiseSchatten = new Bitmap("schatten.png");
	
        public Ameise(double x, double y) // Konstruktor unserer Ameise
        {
        	Random r = new Random();
            this.x = x;
            this.y = y;
            this.alter = r.Next(0,300);
        }
	
        public void Bewege(int x, int y, Point weltenGroesse, ref Ameise[] ameisenKolonie) // Konstruktor unserer Ameise
        {
        	Rectangle ameisenRechteck = new Rectangle(Convert.ToInt32(this.x) + x,Convert.ToInt32(this.y) + y,this.OriginalBild.Width,this.OriginalBild.Height);
        	Point weltenRechteck = new Point(weltenGroesse.X, weltenGroesse.Y);
        	//if (ameiseBefindetSichInFeld(ameisenRechteck,weltenRechteck)) {
        	if (ameiseBefindetSichInWelt(ameisenRechteck,weltenGroesse)) {
        		this.x += x;
        		this.y += y;
        	}
        }
        
        public void drehen(float deg){
        	this.blickRichtung += deg;
        	while (this.blickRichtung<0) {
        		blickRichtung += 360;
        	}
        	while (this.blickRichtung>360) {
        		blickRichtung -= 360;
        	}
        	
        	
	
			this.ameiseBild = bildDrehen(this.OriginalBild, this.blickRichtung);
			this.ameiseSchatten = bildDrehen(this.OriginalBildSchatten, this.blickRichtung);
        }
        
        public void bewegeNachVorneMitDistanz(int schritte, Point weltenGroesse,ref Ameise[] ameisenKolonie, Graphics g){
        	for (int schritt = 0; schritt < schritte; schritt++){
        		this.alter++;
        		double echterWinkel = this.blickRichtung * (Math.PI / 180);
        		
        		double neuePositionX = this.x + Convert.ToInt32(Math.Cos(echterWinkel));
        		double neuePositionY = this.y + Convert.ToInt32(Math.Sin(echterWinkel));
        		Rectangle ameisenRechteck = new Rectangle(Convert.ToInt32(neuePositionX) ,Convert.ToInt32(neuePositionY) ,this.OriginalBild.Width,this.OriginalBild.Height);
	        	if (ameiseBefindetSichInWelt(ameisenRechteck, weltenGroesse)) {
        			this.x = neuePositionX;
        			this.y = neuePositionY;
	        	}
        		int ameisenAnzahlOriginial = ameisenKolonie.Length;
        		for(int i = 0; i < ameisenAnzahlOriginial; i++){
        			if (ameisenKolonie[i] != this){
        				double distanz = Math.Sqrt((Math.Pow(ameisenKolonie[i].x - this.x, 2) + Math.Pow(ameisenKolonie[i].y - this.y, 2)));
        				if (debug) {
	        				if ( distanz < 70 && distanz > 60) {
		        				g.DrawLine(new Pen(Brushes.Red),Convert.ToInt32(ameisenKolonie[i].x), Convert.ToInt32(ameisenKolonie[i].y),Convert.ToInt32(this.x), Convert.ToInt32(this.y));
		        			} else if ( distanz < 60 && distanz > 40) {
		        				g.DrawLine(new Pen(Brushes.Yellow),Convert.ToInt32(ameisenKolonie[i].x), Convert.ToInt32(ameisenKolonie[i].y),Convert.ToInt32(this.x), Convert.ToInt32(this.y));
		        			} else if ( distanz < 40) {
		        				g.DrawLine(new Pen(Brushes.LightGreen),Convert.ToInt32(ameisenKolonie[i].x), Convert.ToInt32(ameisenKolonie[i].y),Convert.ToInt32(this.x), Convert.ToInt32(this.y));
		        			}
        				}
	        			if (distanz < 20 && this.alter > 100 && ameisenKolonie[i].alter > 100) {
        					Random RR = new Random();
        					if (RR.Next(0,5)> 3) {
        					//if (true){
	        					ameisenKolonie[i].alter = 0;
	        					this.alter = 0;
		        				Array.Resize(ref ameisenKolonie, ameisenKolonie.Length+1);
		        				ameisenKolonie[ameisenKolonie.Length-1] = new Ameise(this.x,this.y);
        					}
	        			}	
        			}
        			
        		}
        		
        		
        	}
        }
        
        
        
        public void bewegeNachVorne(int schritte, Point weltenGroesse,ref Ameise[] ameisenKolonie){
        	for (int schritt = 0; schritt < schritte; schritt++){
        		
        		double echterWinkel = this.blickRichtung * (Math.PI / 180);
        		
        		double neuePositionX = this.x + Convert.ToInt32(Math.Cos(echterWinkel));
        		double neuePositionY = this.y + Convert.ToInt32(Math.Sin(echterWinkel));
        		Rectangle ameisenRechteck = new Rectangle(Convert.ToInt32(neuePositionX) ,Convert.ToInt32(neuePositionY) ,this.OriginalBild.Width,this.OriginalBild.Height);
	        	if (ameiseBefindetSichInWelt(ameisenRechteck, weltenGroesse)) {
        			this.x = neuePositionX;
        			this.y = neuePositionY;
	        	}
        	}
        }

        
        public bool ameiseBefindetSichInFeld(Rectangle Feld1,Rectangle Feld2)
		{
        	bool kollidiert = false;
        	if(Rectangle.Intersect(Feld1, Feld2) != Rectangle.Empty)
			{
			    kollidiert = true;
			}
        	
        	return kollidiert;
		}
        
        
        public bool ameiseBefindetSichInWelt(Rectangle ameisenRechteck,Point weltenGroesse)
		{
        	bool istInWelt = true;
        	// Kollisionsabfrage auf der X-Achse
        	if (ameisenRechteck.Left < 0 || ameisenRechteck.Right > weltenGroesse.X) {
        		istInWelt = false;
        	}
        	// Kollisionsabfrage auf der Y-Achse
        	if (ameisenRechteck.Top < 0 || ameisenRechteck.Bottom > weltenGroesse.Y) {
        		istInWelt = false;
        	}
        	
        	return istInWelt;
		}
        
        public void Draw(Graphics g)
        {
        	Pen standFarbe = new Pen(Brushes.Blue);
        	if(this.alter > 400) {
        		standFarbe.Brush = Brushes.Red;
        	} else if(this.alter > 300) {
        		standFarbe.Brush = Brushes.Green;
        	}
        	if (debug) {
        		g.DrawRectangle(standFarbe, Convert.ToInt32(this.x), Convert.ToInt32(this.y),this.OriginalBild.Width, this.OriginalBild.Height);
        	}
        	g.DrawImage(ameiseSchatten, new Point(Convert.ToInt32(this.x+1), Convert.ToInt32(this.y+1)));
        	g.DrawImage(ameiseBild, new Point(Convert.ToInt32(this.x), Convert.ToInt32(this.y)));
        }
        
        
        
        public static Bitmap bildDrehen(Bitmap b, float angle)
		{
		  // Neue leere Bitmap
		  Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
		  // Graphics-Objekt anhand der neuen Bitmap erstellen
		  using(Graphics g = Graphics.FromImage(returnBitmap)) 
		  {
		      // Rotationspunkt in die Mitte
		      g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
		      // Rotieren um angle°
		      g.RotateTransform(angle);
		      // Bild wieder zurück bewegen auf null/0
		      g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
		      // das gedrehte Bild auf das Objekt übertragen
		      g.DrawImage(b, new Point(0, 0)); 
		  }
		  return returnBitmap;
		}
        
	}
	
}