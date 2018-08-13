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
using System.Windows.Forms;

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


        // ---------------- This is the first iteration of the plotting application -------------------
        public FileContentResult PlotOne(int fid) //Plots for nuclear families, no extended families.
        {
            //Dimensions of the box, all distances should be measured in these unit distances - helps keep consistancy
            int height = 60;
            int width = 175;


            IList<Individual> indList = _treeService.GetIndividuals(fid);
            //int numberOfMembers = indList.Count();
            int numberOfGenerations = 2;
            int numberOfChildren;
            int bigW = 4 * width;
            int bigH = 175 * numberOfGenerations;
            float xChild = (bigW / 2);

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
                    else if (numberOfChildren == 3)
                    {
                        bigW = 5 * width;
                    }
                    else if (numberOfChildren == 4)
                    {
                        bigW = 6 * width + width / 2;
                    }
                    else if (numberOfChildren == 5)
                    {
                        bigW = 8 * width;
                    }
                    else if (numberOfChildren == 6)
                    {
                        bigW = 9 * width;
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
            float titleLocation = (bigW / 2) - ((familyName.Length * 8) / 2); //A letter in a string takes up approx 8 pixels
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
                    g.DrawRectangle(Pens.Azure, (bigW / 2), 1, 1, bigH); //Draws a line down the middle of the page, currently only testing for Johnson



                    //Draws line between top two parent nodes
                    g.DrawLine(Pens.Black, (xParent + width), (yParent + (height / 2)), (xParent + width + parentGap), (yParent + (height / 2)));


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
                                        (yParent + 2 * height)); //Half a height of distance to the node
                                    xChild = (bigW - width) / 2;
                                    yChild = yParent + 2 * height;
                                    break;
                                case 2:
                                    g.DrawLine(Pens.Black, pageMid, (yParent + (height / 2)), pageMid, (yParent + (height / 2) + height)); //Draws line straight down, from this point horizontal line must be drawn
                                    g.DrawLine(Pens.Black, (pageMid - width), yParent + (height / 2) + height, (pageMid + width), yParent + (height / 2) + height); //Same y coordinates as where previous line stops, x coordinates change
                                    //Busses down to nodes
                                    g.DrawLine(Pens.Black, (pageMid - width), yParent + (height / 2) + height, (pageMid - width), yParent + (2 * height));

                                    g.DrawLine(Pens.Black, (pageMid + width), yParent + (height / 2) + height, (pageMid + width), yParent + (2 * height));
                                    xChild = (pageMid - width) - (width / 2);
                                    yChild = yParent + 2 * height;
                                    break;
                                case 3:
                                    g.DrawLine(Pens.Black, pageMid, (yParent + (height / 2)), pageMid, (yParent + (height / 2) + height)); //Draws line straight down, from this point horizontal line must be drawn
                                    g.DrawLine(Pens.Black, (pageMid - 2 * width), yParent + (height / 2) + height, (pageMid + 2 * width), yParent + (height / 2) + height); //Same y coordinates as where previous line stops, x coordinates change
                                    //Busses down to nodes
                                    g.DrawLine(Pens.Black, (pageMid - 2 * width), yParent + (height / 2) + height, (pageMid - 2 * width), yParent + (2 * height)); //middle of first child node

                                    g.DrawLine(Pens.Black, pageMid, yParent + ((3 / 2) * height), pageMid, yParent + (2 * height)); //middle of second child node 

                                    g.DrawLine(Pens.Black, (pageMid + 2 * width), yParent + (height / 2) + height, (pageMid + 2 * width), yParent + (2 * height));//middle of third child node
                                    xChild = (pageMid - width) - (width / 2) - width; //Make the x location of the child node correct
                                    yChild = yParent + 2 * height; //make y location correct
                                    break;
                                case 4:
                                    g.DrawLine(Pens.Black, pageMid, (yParent + (height / 2)), pageMid, (yParent + (height / 2) + height)); //Draws line straight down, from this point horizontal line must be drawn
                                    g.DrawLine(Pens.Black, pageMid - 2 * width - width / 4, yParent + (height / 2) + height, pageMid + 2 * width + width / 4, yParent + (height / 2) + height);//Same y coordinates as where previous line stops, x coordinates change
                                    //Busses down to nodes
                                    g.DrawLine(Pens.Purple, pageMid - 2 * width - width / 4, yParent + (height / 2) + height, pageMid - 2 * width - width / 4, yParent + (2 * height)); //Middle of first child node
                                    g.DrawLine(Pens.Green, pageMid - width + (width / 4), yParent + (height / 2) + height, pageMid - width + (width / 4), yParent + (2 * height)); //Second Node

                                    g.DrawLine(Pens.Orange, pageMid + width - (width / 4), yParent + (height / 2) + height, pageMid + width - (width / 4), yParent + (2 * height)); //Third Node
                                    g.DrawLine(Pens.Red, pageMid + 2 * width + width / 4, yParent + (height / 2) + height, pageMid + 2 * width + width / 4, yParent + (2 * height)); //Fourth Node

                                    childGap = width / 2;
                                    xChild = width / 2;
                                    yChild = yParent + 2 * height;
                                    break;
                                case 5:
                                    g.DrawLine(Pens.Black, pageMid, (yParent + (height / 2)), pageMid, (yParent + (height / 2) + height)); //Draws line straight down, from this point horizontal line must be drawn
                                    g.DrawLine(Pens.Black, pageMid - 3 * width, yParent + (height / 2) + height, pageMid + 3 * width, yParent + (height / 2) + height);//Same y coordinates as where previous line stops, x coordinates change
                                    //Busses down to nodes
                                    g.DrawLine(Pens.Black, pageMid - 3 * width, yParent + (height / 2) + height, pageMid - 3 * width, yParent + 2 * height);
                                    g.DrawLine(Pens.Black, pageMid - width - (width / 2), yParent + (height / 2) + height, pageMid - width - (width / 2), yParent + 2 * height);

                                    g.DrawLine(Pens.Black, pageMid, yParent + (height / 2) + height, pageMid, yParent + 2 * height);

                                    g.DrawLine(Pens.Black, pageMid + width + (width / 2), yParent + (height / 2) + height, pageMid + width + (width / 2), yParent + 2 * height);
                                    g.DrawLine(Pens.Black, pageMid + 3 * width, yParent + (height / 2) + height, pageMid + 3 * width, yParent + 2 * height);
                                    childGap = width / 2;
                                    xChild = width / 2;
                                    yChild = yParent + 2 * height;
                                    break;
                                case 6:
                                    g.DrawLine(Pens.Black, pageMid, (yParent + (height / 2)), pageMid, (yParent + (height / 2) + height)); //Draws line straight down, from this point horizontal line must be drawn
                                    g.DrawLine(Pens.Black, pageMid - 3 * width - ((3 / 4) * width), yParent + (height / 2) + height, pageMid + 3 * width + ((3 / 4) * width), yParent + (height / 2) + height); //Same y coordinates as where previous line stops, x coordinates change
                                    //Busses down to nodes
                                    g.DrawLine(Pens.Black, pageMid - 3 * width - ((3 / 4) * width), yParent + (height / 2) + height, pageMid - 3 * width - ((3 / 4) * width), yParent + 2 * height);
                                    g.DrawLine(Pens.Black, pageMid - 2 * width - ((1 / 4) * width), yParent + (height / 2) + height, pageMid - 2 * width - ((1 / 4) * width), yParent + 2 * height);
                                    g.DrawLine(Pens.Black, pageMid - width + (width / 4), yParent + (height / 2) + height, pageMid - width + (width / 4), yParent + 2 * height);

                                    g.DrawLine(Pens.Black, pageMid + ((3 / 4) * width), yParent + (height / 2) + height, pageMid + ((3 / 4) * width), yParent + 2 * height);
                                    g.DrawLine(Pens.Black, pageMid + 2 * width + ((1 / 4) * width), yParent + (height / 2) + height, pageMid + 2 * width + ((1 / 4) * width), yParent + 2 * height);
                                    g.DrawLine(Pens.Black, pageMid + 3 * width + ((3 / 4) * width), yParent + (height / 2) + height, pageMid + 3 * width + ((3 / 4) * width), yParent + 2 * height);
                                    childGap = width / 2;
                                    xChild = width / 4;
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


        //public ActionResult PlotIndividual(int pid)
        //{
        //    ViewBag.pid = pid;
        //    return View();
        //}
        //Plotting from a specific person in your family, will show parents, partner and children
        public FileContentResult PlotIndividual(int pid)
        {
            //Dimensions of the box, all distances should be measured in these unit distances - helps keep consistancy
            int height = 60;
            int width = 175;

            // Collect information on individual and relatives
            Individual mainIndividual = _treeService.GetIndividual(pid);
            IList<Relationship> relList = _treeService.GetRelationships(pid);
            int numberOfChildren = _treeService.GetNumberOfChildren(pid);       
            string mainName = mainIndividual.fullName.ToString();

            
            int bigW = _treeService.GetPlotWidth(pid) * width * 2 + (width / 4);
            int bigH = height * 6; //Give a border of a height either side around the plot, assi

            //Used for colour plotting
            int alpha = 100;
            int red = 204;
            int green = 102;
            int blue = 0;


            string individualName;
            string dateBirth;
            string dateDeath;
            float titleLocation = (bigW / 2) - ((mainIndividual.fullName.Length * 8) / 2); //A letter in a string takes up approx 8 pixels


            //Three Rows of boxes, x and y values for these boxes updated as plotting is done

            float xRowOne = width / 4;
            float yRowOne = height + height / 2;
            float xRowTwo = width; // Room for a parent node either side above you
            float yRowTwo = 3 * height;
            float xRowThree = width / 4;
            float yRowThree = 5 * height;

            //If the individual plotted has siblings, these will need to be used, otherwise they won't be used
            float siblingX = 0;
            float siblingY = 0;
            //Plot point for Children
            float childX = 0;
            float childY = 0;

            using (Bitmap bmp = new Bitmap(bigW, 500))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {

                    //Store individual name, first node will be the main individual
                    individualName = mainIndividual.fullName.ToString();
                    dateBirth = mainIndividual.dateOfBirth.ToString();
                    dateDeath = mainIndividual.dateOfDeath.ToString();

                    //Colour the background of the bitmap
                    g.Clear(Color.Bisque);

                    //Title the bitmap with the main individuals name
                    g.DrawString(individualName + "'s Family Tree",
                        new Font("Arial", 10, FontStyle.Bold),
                        SystemBrushes.WindowText,
                        new PointF(titleLocation, 10),
                        new StringFormat());

                    //Plot Initial Person, 
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.DrawRectangle(Pens.Brown, xRowTwo, yRowTwo, width, height);

                    //Fill in the information of the individual within the confised of the rectangle fill
                    g.DrawString(individualName,
                        new Font("Arial", 10, FontStyle.Bold),
                        SystemBrushes.WindowText,
                        new PointF(xRowTwo + 5, yRowTwo + 5),
                        new StringFormat());
                    g.DrawString(dateBirth,
                        new Font("Arial", 10, FontStyle.Bold),
                        SystemBrushes.WindowText,
                        new PointF(xRowTwo + 5, yRowTwo + 20),
                        new StringFormat());
                    g.DrawString(dateDeath, new Font("Arial", 10, FontStyle.Bold),
                        SystemBrushes.WindowText,
                        new PointF(xRowTwo + 5, yRowTwo + 35),
                        new StringFormat());
                    g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, red,
                        green, blue)), xRowTwo, yRowTwo, width, height);

                    //Check if the individual has parents in the database
                    //If there are, prepare the bus to accept two parent boxes, 
                    bool Check = relList.Any(p => p.relationshipTypeID == 2);
                    if (Check == true) 
                    {
                        g.DrawLine(Pens.Black, xRowTwo + width / 2, yRowTwo, xRowTwo + width / 2, yRowTwo - height); //Draws a T shape above your node, ready for addition of parent nodes
                        g.DrawLine(Pens.Black, xRowTwo + width / 4, yRowTwo - height, xRowTwo + width - width / 4, yRowTwo - height);

                        //Store the location that siblings will need to bus to
                        siblingX = xRowTwo + width / 2;
                        siblingY = yRowTwo - height / 4;
                    }


                    //Update x location
                    xRowTwo = xRowTwo + 2 * width; 

                    //Check if the individual has any children, if they do draw a bus for their nodes to attach to and save the x and y coordinates of that location
                    bool kidCheck = relList.Any(c => c.relationshipTypeID == 3); 
                    if (kidCheck == true)
                    {
                        g.DrawLine(Pens.Black, xRowTwo - width / 2, (yRowTwo + (height / 2)), xRowTwo - width / 2, (yRowTwo + (height / 2) + height));
                    }
                    childX = xRowTwo - width / 2;
                    childY = yRowTwo + height + height / 2;

                    foreach (var relative in relList)
                    {

                        //------------------------------- Check for Marriage Relationship -----------------------------
                        if (relative.relationshipTypeID == 4)
                        {
                            //Load partners information
                            individualName = _treeService.GetIndividual(relative.relativeID).fullName.ToString();
                            dateBirth = _treeService.GetIndividual(relative.relativeID).dateOfBirth.ToString();
                            dateDeath = _treeService.GetIndividual(relative.relativeID).dateOfDeath.ToString();


                            //Fill the node with their partners information and fill the node with the colour
                            g.DrawString(individualName,
                                new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xRowTwo + 5, yRowTwo + 5),
                                new StringFormat());
                            g.DrawString(dateBirth,
                                new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xRowTwo + 5, yRowTwo + 20),
                                new StringFormat());
                            g.DrawString(dateDeath, new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xRowTwo + 5, yRowTwo + 35),
                                new StringFormat());
                            g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, red,
                                green, blue)), xRowTwo, yRowTwo, width, height);

                            //Draw Line to Partner
                            g.DrawLine(Pens.Black, (xRowTwo), (yRowTwo + (height / 2)), (xRowTwo - width), (yRowTwo + (height / 2)));

                            //Update x coordinates
                            xRowTwo = xRowTwo + 2 * width;

                        }
                        // ****************************** COMMENT FROM HERE **************************************

                        /// ---------------- Check For Parent Relationship -----------------
                        else if (relative.relationshipTypeID == 2)//relationship type is parent, therefore plot box above you
                        {
                            individualName = _treeService.GetIndividual(relative.relativeID).fullName.ToString();
                            dateBirth = _treeService.GetIndividual(relative.relativeID).dateOfBirth.ToString();
                            dateDeath = _treeService.GetIndividual(relative.relativeID).dateOfDeath.ToString();

                            //Draw Node for Parent
                            g.DrawString(individualName,
                                new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xRowOne + 5, yRowOne + 5),
                                new StringFormat());
                            //Writes the Date of Birth
                            g.DrawString(dateBirth,
                                new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xRowOne + 5, yRowOne + 20),
                                new StringFormat());
                            g.DrawString(dateDeath, new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xRowOne + 5, yRowOne + 35),
                                new StringFormat());
                            g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, red,
                                green, blue)), xRowOne, yRowOne, width, height);

                            var parCheck = _treeService.GetRelationships(relative.relativeID);
                            bool Checker = parCheck.Any(p => p.relationshipTypeID == 2); //Check if any of the relationships are parents
                            if (Checker == true) //If there are parent relationships, draw a line out of your box
                            {
                                g.DrawLine(Pens.Black, xRowOne + width / 2, yRowOne, xRowOne + width / 2, yRowOne - (height / 2)); //Position the horizontal line, ready for addition of parent nodes, these will not contain information
                                g.DrawString(individualName + "'s Parents",
                                    new Font("Arial", 10, FontStyle.Bold),
                                    SystemBrushes.WindowText,
                                    new PointF(xRowOne + 5, yRowOne - height),
                                    new StringFormat());

                                g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, red,
                                green, blue)), xRowOne, yRowOne - height, width, height / 2);
                            }

                            xRowOne = xRowOne + width / 2 + width;

                        }



                        //----------------------  Check for Child Relationship ------------------------
                        else if (relative.relationshipTypeID == 3)//relationship type is child, therefore plot box below you
                        {
                            individualName = _treeService.GetIndividual(relative.relativeID).fullName.ToString();
                            dateBirth = _treeService.GetIndividual(relative.relativeID).dateOfBirth.ToString();
                            dateDeath = _treeService.GetIndividual(relative.relativeID).dateOfDeath.ToString();

                            //Draw Node for Parent
                            g.DrawString(individualName,
                                new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xRowThree + 5, yRowThree + 5),
                                new StringFormat());
                            //Writes the Date of Birth
                            g.DrawString(dateBirth,
                                new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xRowThree + 5, yRowThree + 20),
                                new StringFormat());
                            g.DrawString(dateDeath, new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xRowThree + 5, yRowThree + 35),
                                new StringFormat());
                            g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, red,
                                green, blue)), xRowThree, yRowThree, width, height);



                            g.DrawLine(Pens.Black, xRowThree + width / 2, yRowThree, xRowThree + width / 2, yRowThree - height / 2);
                            g.DrawLine(Pens.Black, xRowThree + width / 2, childY, childX, childY);
                            xRowThree = xRowThree + width + width;
                            var partCheck = _treeService.GetRelationships(relative.relativeID); //Load up childs relationships
                            bool partnerCheck = partCheck.Any(par => par.relationshipTypeID == 4); //Check if any of the relationships match the marriage type
                            if (partnerCheck == true) //If they do, plot the marriage 
                            {
                                g.DrawLine(Pens.Black, xRowThree - width, yRowThree + height / 2, xRowThree, yRowThree + height / 2);
                                bool kidChecker = partCheck.Any(c => c.relationshipTypeID == 3); //Check if your children have children, if they do show boxes that can be filled 

                                if (kidChecker == true)
                                {
                                    g.DrawLine(Pens.Black, xRowThree - width + width / 2, (yRowThree + (height / 2)), xRowThree - width + width / 2, (yRowThree + (height / 2) + height));
                                    g.DrawString(individualName + "'s Children",
                                        new Font("Arial", 10, FontStyle.Bold),
                                        SystemBrushes.WindowText,
                                        new PointF(xRowThree + 5 - width, yRowThree + height + height / 2),
                                        new StringFormat());

                                    g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, red,
                                        green, blue)), xRowThree - width, yRowThree + height + height / 2, width, height / 2);
                                }

                                foreach (var person in partCheck)
                                {

                                    if (person.relationshipTypeID == 4)
                                    {
                                        individualName = _treeService.GetIndividual(person.relativeID).fullName.ToString();
                                        dateBirth = _treeService.GetIndividual(person.relativeID).dateOfBirth.ToString();
                                        dateDeath = _treeService.GetIndividual(person.relativeID).dateOfDeath.ToString();
                                        g.DrawString(individualName,
                                            new Font("Arial", 10, FontStyle.Bold),
                                            SystemBrushes.WindowText,
                                            new PointF(xRowThree + 5, yRowThree + 5),
                                            new StringFormat());
                                        //Writes the Date of Birth
                                        g.DrawString(dateBirth,
                                            new Font("Arial", 10, FontStyle.Bold),
                                            SystemBrushes.WindowText,
                                            new PointF(xRowThree + 5, yRowThree + 20),
                                            new StringFormat());
                                        g.DrawString(dateDeath, new Font("Arial", 10, FontStyle.Bold),
                                            SystemBrushes.WindowText,
                                            new PointF(xRowThree + 5, yRowThree + 35),
                                            new StringFormat());
                                        g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, red,
                                            green, blue)), xRowThree, yRowThree, width, height);
                                        xRowThree = xRowThree + width + width;
                                    }
                                }
                            }
                        }
                    }

                    //Plot Siblings after all transformations have been made by previous foreach
                    foreach (var relative in relList)
                    {
                        if (relative.relationshipTypeID == 1)
                        {
                            individualName = _treeService.GetIndividual(relative.relativeID).fullName.ToString();
                            dateBirth = _treeService.GetIndividual(relative.relativeID).dateOfBirth.ToString();
                            dateDeath = _treeService.GetIndividual(relative.relativeID).dateOfDeath.ToString();

                            g.DrawString(individualName,
                                new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xRowTwo + 5, yRowTwo + 5),
                                new StringFormat());

                            //Writes the Date of Birth
                            g.DrawString(dateBirth,
                                new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xRowTwo + 5, yRowTwo + 20),
                                new StringFormat());

                            g.DrawString(dateDeath, new Font("Arial", 10, FontStyle.Bold),
                                SystemBrushes.WindowText,
                                new PointF(xRowTwo + 5, yRowTwo + 35),
                                new StringFormat());

                            g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, red,
                                green, blue)), xRowTwo, yRowTwo, width, height);

                            //Draw lines back to the parent bus
                            g.DrawLine(Pens.Black, xRowTwo + width / 2, yRowTwo, xRowTwo + width / 2, siblingY);
                            g.DrawLine(Pens.Black, xRowTwo + width / 2, siblingY, siblingX, siblingY);

                            xRowTwo = xRowTwo + 2 * width;

                            var partCheck = _treeService.GetRelationships(relative.relativeID); //Load up siblings relationships
                            bool partnerCheck = partCheck.Any(par => par.relationshipTypeID == 4); //Check if any of the relationships match the marriage type
                            if (partnerCheck == true) //If they do, plot the marriage 
                            {
                                g.DrawLine(Pens.Black, xRowTwo - width, yRowTwo + height / 2, xRowTwo, yRowTwo + height / 2);
                                bool kidChecker = partCheck.Any(c => c.relationshipTypeID == 3); //Check if your children have children, if they do show boxes that can be filled 
                                if (kidChecker == true)
                                {
                                    g.DrawLine(Pens.Black, xRowTwo - width + width / 2, (yRowTwo + (height / 2)), xRowTwo - width + width / 2, (yRowTwo + (height / 2) + height));
                                    g.DrawString(individualName + "'s Children",
                                        new Font("Arial", 10, FontStyle.Bold),
                                        SystemBrushes.WindowText,
                                        new PointF(xRowTwo + 5 - width, yRowTwo + height + height / 2),
                                        new StringFormat());
                                    g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, red,
                                        green, blue)), xRowTwo - width, yRowTwo + height + height / 2, width, height / 2);
                                }

                                foreach (var person in partCheck)
                                {

                                    if (person.relationshipTypeID == 4)
                                    {
                                        individualName = _treeService.GetIndividual(person.relativeID).fullName.ToString();
                                        dateBirth = _treeService.GetIndividual(person.relativeID).dateOfBirth.ToString();
                                        dateDeath = _treeService.GetIndividual(person.relativeID).dateOfDeath.ToString();
                                        g.DrawString(individualName,
                                            new Font("Arial", 10, FontStyle.Bold),
                                            SystemBrushes.WindowText,
                                            new PointF(xRowTwo + 5, yRowTwo + 5),
                                            new StringFormat());
                                        //Writes the Date of Birth
                                        g.DrawString(dateBirth,
                                            new Font("Arial", 10, FontStyle.Bold),
                                            SystemBrushes.WindowText,
                                            new PointF(xRowTwo + 5, yRowTwo + 20),
                                            new StringFormat());
                                        g.DrawString(dateDeath, new Font("Arial", 10, FontStyle.Bold),
                                            SystemBrushes.WindowText,
                                            new PointF(xRowTwo + 5, yRowTwo + 35),
                                            new StringFormat());
                                        g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, red,
                                            green, blue)), xRowTwo, yRowTwo, width, height);
                                        xRowTwo = xRowTwo + width + width;
                                    }
                                }


                            }
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
