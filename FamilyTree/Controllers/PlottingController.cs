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
            //Dimensions of the box
            int height = 60;
            int width = 175;

            IList <Individual> indList = _treeService.GetIndividuals(fid);
            //int numberOfMembers = indList.Count();
            int numberOfGenerations = 2;
            int numberOfChildren;
            int bigW = 450;
            int bigH = 175 * numberOfGenerations;
            float xChild = (bigW/2);
            //COUPLE TABLE HAS NUMBER OF CHILDREN IN
            //Working out width of bitmap depending on how many children individuals have
            //In a nuclear family there are 2 parents, therefore each child will add 250 to the total width
            foreach (Individual person in indList) 
            {
                if (person.isParent == 1)
                {
                    numberOfChildren = _treeService.GetNumberOfChildren(person.individualID);
                    if (numberOfChildren <= 2)
                    {
                        bigW = bigW;
                        xChild = xChild;
                    }
                    else
                    {
                        bigW = bigW + (numberOfChildren * 125);
                        xChild = (bigW / 2) - ((numberOfChildren / 2) * 100);
                    }
                }
                else
                {
                    bigW = bigW;
                }
            }
            //int bigW = _treeService.GetNumberOfChildren(pid);
            int alpha = 100;
            int red = 204;
            int green = 102;
            int blue = 0;

            int xCoordinate = 10;
            int yCoordinate = 30;

            string individualName;
            string dateBirth;
            string dateDeath;
            string familyName = _treeService.GetFamily(fid).familyName;
            float titleLocation = (bigW / 2) - ((familyName.Length*8)/2); //A letter in a string takes up approx 8 pixels
            int parentGap = 75; //x coordinate space between parent rectangles
            int childGap = 50; //x coordingate space between child rectangles
            int yAddition = 150; //y space between parent and child rows

            int xParent = xCoordinate;
            int yParent = yCoordinate;
            int yChild = yCoordinate + yAddition;



            using (Bitmap bmp = new Bitmap(bigW, bigH))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Bisque);
                    g.DrawRectangle(Pens.Azure, (bigW/2), 1, 1, bigH); //Draws a line down the middle of the page, currently only testing for Johnson
                    g.DrawLine(Pens.Black, (xParent + width), (yParent + (height/ 2)), (xParent + width + parentGap), (yParent + (height / 2)));
                    g.DrawLine(Pens.Black, (bigW / 2), (yParent + (height / 2)), (bigW / 2), (yParent + (height / 2) + 70));

                    foreach (Individual person in indList)
                    {
                        individualName = person.fullName;
                        dateBirth = person.dateOfBirth.ToString();
                        dateDeath = person.dateOfDeath.ToString();
                        g.DrawString(familyName, new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(titleLocation, 10),
                                new StringFormat());
                        //Workig on nucear family - will have to refactor for extended family
                        if (person.isParent == 1) //Draw box for parent
                        {
                            //Draws a rectangle and fills it with the information retrieved from the database
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


                            g.DrawRectangle(Pens.Brown, xParent, yParent, width, height);
                            //Writes the Name at 20, yCoordinate
                            g.DrawString(individualName,
                                new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xParent + 5, yParent + 5),
                                new StringFormat());

                            //Writes the Date of Birth
                            g.DrawString(dateBirth,
                                new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xParent + 5, yParent + 20),
                                new StringFormat());

                            g.DrawString(dateDeath, new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xParent + 5, yParent + 35),
                                new StringFormat());

                            

                            g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, red,
                                green, blue)), xParent, yParent, width, height);
                            xParent = xParent + width + parentGap;
                        }
                        else //You must be a child, therefore drawn further down on the yAxis
                        {
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


                            g.DrawRectangle(Pens.Brown, xChild, yChild, width, height);

                            //Writes the Name at 20, yCoordinate
                            g.DrawString(individualName,
                                new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xChild + 5, yChild + 5),
                                new StringFormat());

                            //Writes the Date of Birth
                            g.DrawString(dateBirth,
                                new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xChild + 5, yChild + 20),
                                new StringFormat());

                            g.DrawString(dateDeath, new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xChild + 5, yChild + 35),
                                new StringFormat());



                            g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, red,
                                green, blue)), xChild, yChild, width, height);
                            xChild = xChild + width + childGap;
                        }
                        //switch (numberOfChildren = _treeService.GetNumberOfChildren(person.isParent = 1))
                        //{
                        //    case 1:
                        //        g.DrawLine(Pens.Black, (bigW / 2), (yParent + (height / 2)), (bigW / 2), (yParent + (height / 2) + 125);
                        //}


                        ////Draws a rectangle and fills it with the information retrieved from the database
                        //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        

                        //g.DrawRectangle(Pens.Brown, xCoordinate, yCoordinate, width, height);

                        ////Writes the Name at 20, yCoordinate
                        //g.DrawString(individualName, 
                        //    new Font("Arial", 10, FontStyle.Bold),
                        //    SystemBrushes.WindowText, 
                        //    new PointF(20, yCoordinate), 
                        //    new StringFormat());

                        ////Writes the Date of Birth
                        //g.DrawString(dateBirth, 
                        //    new Font("Arial", 10, FontStyle.Bold),
                        //    SystemBrushes.WindowText, 
                        //    new PointF(20, yCoordinate + 15), 
                        //    new StringFormat());

                        //g.DrawString(dateDeath, new Font("Arial", 10, FontStyle.Bold),
                        //    SystemBrushes.WindowText, 
                        //    new PointF(20, yCoordinate + 30), 
                        //    new StringFormat());

                        //g.DrawString(familyName, new Font("Arial", 10, FontStyle.Bold),
                        //    SystemBrushes.WindowText,
                        //    new PointF(titleLocation, 10),
                        //    new StringFormat());

                        //g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, red,
                        //    green, blue)), xCoordinate, yCoordinate, width, height);

                    }
                    // Saves it?? Outputs an image??
                    string filename = Server.MapPath("/") + Guid.NewGuid().ToString("N");
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
