using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyTree.Data;
using FamilyTree.Data.BEANS;
using FamilyTree.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Drawing;

namespace FamilyTree.Controllers
{
    public class PlottingController : Controller
    {
        private Services.DAO.TreeService _treeService;
        public PlottingController()
        {
            _treeService = new FamilyTree.Services.DAO.TreeService();
        }
        public ActionResult PlotRectangle()
        {

            return View();
        }

        public FileContentResult CreateBitmap()
        {
            
            int height = 400;
            int width = 200;
            Random r = new Random();
            using (Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.Clear(System.Drawing.Color.LightGray);
                    g.DrawRectangle(System.Drawing.Pens.White, 1, 1, width - 3, height - 3);
                    g.DrawRectangle(System.Drawing.Pens.Gray, 2, 2, width - 3, height - 3);
                    g.DrawRectangle(System.Drawing.Pens.Black, 0, 0, width, height);
                    g.DrawString("Refresh Me!", new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold),
                        System.Drawing.SystemBrushes.WindowText, new System.Drawing.PointF(r.Next(50), r.Next(100)),
                        new System.Drawing.StringFormat(System.Drawing.StringFormatFlags.DirectionVertical));
                    g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(r.Next(100), r.Next(130),
                        r.Next(150), r.Next(200))), 20, 40, 60, 80);
                    int x = r.Next(width);
                    g.FillRectangle(new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.Point(x, 0),
                        new System.Drawing.Point(x + 75, 100), System.Drawing.Color.FromArgb(128, 0, 0, r.Next(255)),
                        System.Drawing.Color.FromArgb(255, r.Next(192, 255), r.Next(192, 255), r.Next(255))), x, r.Next(height), 75, 50);
                    string filename = Server.MapPath("/") + System.Guid.NewGuid().ToString("N");
                    bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] bytes;
                    using (System.IO.FileStream stream = new System.IO.FileStream(filename, System.IO.FileMode.Open))
                    {
                        bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                    }
                    System.IO.File.Delete(filename);
                    return new FileContentResult(bytes, "image/jpeg");
                }
            }
        }

        public FileContentResult PlotOne(int fid)
        {
            int height = 75;
            int width = 200;

            IList <Individual> indList = _treeService.GetIndividuals(fid);
            int numberOfMembers = indList.Count();
            int bigH = 150 * numberOfMembers;
            int bigW = 600;
            int alpha = 100;
            int red = 204;
            int green = 102;
            int blue = 0;
            int xCoordinate = 5;
            int yCoordinate = 0;

            string familyName = _treeService.GetFamily(fid).familyName;
            string individualName = _treeService.GetIndividual(1).fullName;
            string dateBirth = _treeService.GetIndividual(1).dateOfBirth.ToString();
            string dateDeath = _treeService.GetIndividual(1).dateOfDeath.ToString();



            using (Bitmap bmp = new Bitmap(bigW, bigH))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    foreach (Individual person in indList)
                    {
                        individualName = person.fullName;
                        dateBirth = person.dateOfBirth.ToString();
                        dateDeath = person.dateOfDeath.ToString();
                        yCoordinate = yCoordinate + 100;

                        //Draws a rectangle and fills it with the information retrieved from the database
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.Clear(Color.Bisque);

                        g.DrawRectangle(Pens.Brown, xCoordinate, yCoordinate, width, height);

                        g.DrawString(individualName, 
                            new Font("Arial", 10, FontStyle.Bold),
                            SystemBrushes.WindowText, 
                            new PointF(20, yCoordinate), 
                            new StringFormat());

                        g.DrawString(dateBirth, 
                            new Font("Arial", 10, FontStyle.Bold),
                            SystemBrushes.WindowText, 
                            new PointF(20, yCoordinate + 15), 
                            new StringFormat());

                        g.DrawString(dateDeath, new Font("Arial", 10, FontStyle.Bold),
                            SystemBrushes.WindowText, 
                            new PointF(20, yCoordinate + 30), 
                            new StringFormat());

                        g.DrawString(familyName, new Font("Arial", 10, FontStyle.Bold), 
                            SystemBrushes.WindowText, 
                            new PointF((bigW / 2) - (familyName.Length / 2), 10), 
                            new StringFormat());

                        g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, red,
                            green, blue)), xCoordinate, yCoordinate, width, height);

                        g.DrawRectangle(Pens.Azure, 300, 1, 1, 600); //Draws a line down the middle of the page, currently only testing for Johnson
                    }


                    // Saves it?? Outputs an image??
                    string filename = Server.MapPath("/") + System.Guid.NewGuid().ToString("N");
                    bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] bytes;
                    using (System.IO.FileStream stream = new System.IO.FileStream(filename, System.IO.FileMode.Open))
                    {
                        bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                    }
                    System.IO.File.Delete(filename);
                    return new FileContentResult(bytes, "image/jpeg");


                    


                }
            }




        }
    }
}
