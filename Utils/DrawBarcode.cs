using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace SGcombo.Utils
{


////////////////////////////////////////////////////////////////////////////
//	Copyright 2005 - 2017 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//        
//             https://github.com/Vladimir-Novick/CSharp-Utility-Classes
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////


    public class DrawBarcode
    {

       
        Font plainTextF = new Font("Arial", 13, FontStyle.Regular, GraphicsUnit.Pixel);



        public static System.Drawing.FontFamily LoadFontFamily(string fileName, out PrivateFontCollection fontCollection)
        {
            fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(fileName);
            return fontCollection.Families[0];
        }

		/// <summary>
		/// Convert String to Barcode image
		/// </summary>
		/// <param name="inputString">input string</param>
        /// <param name="barcodeFont">font name</param>
        /// <param name="fontSize">font size, default = 30 </param>
		/// <returns>image</returns>
        public Image Draw(string inputString,String barcodeFont,int fontSize = 30)
        {

            PrivateFontCollection fonts;
            FontFamily family = LoadFontFamily(barcodeFont, out fonts);
            Font barCodeF = new Font(family, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            Bitmap bmp = new Bitmap(1, 1);

            try
            {
        

				string text = inputString.Trim();


				Graphics graphics = Graphics.FromImage(bmp);



				int barCodewidth = (int)graphics.MeasureString(text, barCodeF).Width;
				int barCodeHeight = (int)graphics.MeasureString(text, barCodeF).Height;

				int plainTextWidth = (int)graphics.MeasureString(text, plainTextF).Width;
				int plainTextHeight = (int)graphics.MeasureString(text, plainTextF).Height;


				if (barCodewidth > plainTextWidth)
				{
					bmp = new Bitmap(bmp,
									 new Size(barCodewidth, barCodeHeight + plainTextHeight));
				}
				else
				{
					bmp = new Bitmap(bmp,
									 new Size(plainTextWidth, barCodeHeight + plainTextHeight));
				}
				graphics = Graphics.FromImage(bmp);


				graphics.Clear(Color.White);
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				graphics.TextRenderingHint = TextRenderingHint.AntiAlias;


				if (barCodewidth > plainTextWidth)
				{

					graphics.DrawString(text,
										barCodeF,
										new SolidBrush(Color.Black),
										0,
										0);


					graphics.DrawString(text,
										plainTextF,
										new SolidBrush(Color.Black),
										(barCodewidth - plainTextWidth) / 2,
										barCodeHeight);
				}
				else
				{

					graphics.DrawString(text,
										barCodeF,
										new SolidBrush(Color.Black),
										(plainTextWidth - barCodewidth) / 2,
										0);

					graphics.DrawString(text,
										plainTextF,
										new SolidBrush(Color.Black),
										0,
										barCodeHeight);
				}



				graphics.Flush();
				graphics.Dispose();
				graphics = null;

            }
            catch (Exception)
            {
            }
            finally
            {
                if (barCodeF != null)
                {
                  
                    barCodeF.Dispose();
                    barCodeF = null;
                }
                if (family != null)
                {
                    
                    family.Dispose();
                    family = null;
                }
            }


            return bmp;
        }
    }
}
