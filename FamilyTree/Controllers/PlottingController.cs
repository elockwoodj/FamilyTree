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
            //Dimensions of the box, all distances should be measured in these unit distances - helps keep consistancy
            int height = 60;
            int width = 175;

            
            IList <Individual> indList = _treeService.GetIndividuals(fid);
            //int numberOfMembers = indList.Count();
            int numberOfGenerations = 2;
            int numberOfChildren;
            int bigW = 4 * width;
            int bigH = 175 * numberOfGenerations;
            float xChild = (bigW / 2);
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
                    }
                    else
                    {
                        if (numberOfChildren == 4)
                        {
                            bigW = 6 * width + width / 2;
                        }
                        //bigW = bigW + ((numberOfChildren - 2) * width)/2;
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

            string individualName;
            string dateBirth;
            string dateDeath;
            string familyName = _treeService.GetFamily(fid).familyName;
            float titleLocation = (bigW / 2) - ((familyName.Length*8)/2); //A letter in a string takes up approx 8 pixels
            float parentGap = width; //x coordinate space between parent rectangles
            float childGap = width; //x coordingate space between child rectangles
            float yAddition = height; //y space between parent and child rows

            float xParent = ((bigW / 2) - width) - (width / 2);
            float yParent = height;
            float yChild = yAddition;
            float pageMid = bigW / 2;


            using (Bitmap bmp = new Bitmap(bigW, bigH))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Bisque);
                    g.DrawRectangle(Pens.Azure, (bigW/2), 1, 1, bigH); //Draws a line down the middle of the page, currently only testing for Johnson



                    //Draws line between top two parent nodes
                    g.DrawLine(Pens.Black, (xParent + width), (yParent + (height/ 2)), (xParent + width + parentGap), (yParent + (height / 2)));
                    

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

                            xParent = xParent + 2 * width;
                            numberOfChildren = _treeService.GetNumberOfChildren(person.individualID);
                            switch (numberOfChildren)
                            {
                                case 0: // No children, no line required
                                    break;
                                case 1: //One Child, therefore one line required
                                    //Draws from middle of parent bus straight down 70 pixels
                                        g.DrawLine(Pens.Black, pageMid, //Half way point on x-axis
                                            (yParent + (height / 2)), //Halfway down the y-axis of the parent box
                                            pageMid, //Stays on half way point
                                            (yParent + 2 * height)); //
                                    xChild = (bigW - width) / 2;
                                    yChild = yParent + 2 * height;
                                    break;
                                case 2:
                                    g.DrawLine(Pens.Black, pageMid, (yParent + (height / 2)), pageMid, (yParent + (height / 2) + height)); //Draws line straight down, from this point horizontal line must be drawn
                                    g.DrawLine(Pens.Black, (pageMid - width), yParent + (height / 2) + height, (pageMid + width), yParent + (height / 2) + height); //Same y coordinates as where previous line stops, x coordinates change
                                    g.DrawLine(Pens.Black, (pageMid - width), yParent + (height / 2) + height, (pageMid - width), yParent + (2 * height));
                                    g.DrawLine(Pens.Black, (pageMid + width), yParent + (height / 2) + height, (pageMid + width), yParent + (2 * height));
                                    xChild = (pageMid - width) - (width / 2);
                                    yChild = yParent + 2 * height;
                                    break;
                                case 3:
                                    g.DrawLine(Pens.Black, pageMid, (yParent + (height / 2)), pageMid, (yParent + (height / 2) + height)); //Draws line straight down, from this point horizontal line must be drawn
                                    g.DrawLine(Pens.Black, (pageMid - 2 * width), yParent + (height / 2) + height, (pageMid + 2 * width), yParent + (height / 2) + height); //Same y coordinates as where previous line stops, x coordinates change
                                    g.DrawLine(Pens.Black, (pageMid - 2 * width), yParent + (height / 2) + height, (pageMid - 2 * width), yParent + (2 * height)); //middle of first child node
                                    g.DrawLine(Pens.Black, pageMid, yParent + ((3 / 2) * height), pageMid, yParent + (2 * height)); //middle of second child node 
                                    g.DrawLine(Pens.Black, (pageMid + 2 * width), yParent + (height / 2) + height, (pageMid + 2 * width), yParent + (2 * height));//middle of third child node
                                    xChild = (pageMid - width) - (width / 2) - width; //Make the x location of the child node correct
                                    yChild = yParent + 2 * height; //make y location correct
                                    break;
                                case 4:
                                    g.DrawLine(Pens.Black, pageMid, (yParent + (height / 2)), pageMid, (yParent + (height / 2) + height)); //Draws line straight down, from this point horizontal line must be drawn
                                    g.DrawLine(Pens.Black, pageMid - 2 * width - width / 4, yParent + (height / 2) + height, pageMid + 2 * width + width / 4, yParent + (height / 2) + height);//Same y coordinates as where previous line stops, x coordinates change
                                    g.DrawLine(Pens.Purple, pageMid - 2 * width - width / 4, yParent + (height / 2) + height, pageMid - 2 * width - width / 4, yParent + (2 * height)); //Middle of first child node
                                    g.DrawLine(Pens.Green, pageMid - width + (width / 4), yParent + (height / 2) + height, pageMid - width + (width / 4), yParent + (2 * height)); //Second Node
                                    g.DrawLine(Pens.Red, pageMid + 2 * width + width / 4, yParent + (height / 2) + height, pageMid + 2 * width + width / 4, yParent + (2 * height)); //Third Node
                                    g.DrawLine(Pens.Orange, pageMid + width - (width / 4), yParent + (height / 2) + height, pageMid + width - (width / 4), yParent + (2 * height)); //Fourth Node
                                    childGap = width / 2;
                                    xChild = width / 2;
                                    yChild = yParent + 2 * height;
                                    break;
                            }
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
