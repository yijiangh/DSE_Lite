using System;
using System.IO;
using System.Collections.Generic;
using Grasshopper.Kernel;
using DSECommon;
using System.Drawing;

namespace Capture
{
    class CaptureComponentAttributes : Grasshopper.Kernel.Attributes.GH_ComponentAttributes
    {
        CaptureComponent MyComponent;

        public CaptureComponentAttributes(IGH_Component component)
            : base(component)
        {
            MyComponent = (CaptureComponent)component;
        }

        [STAThread]
        public override Grasshopper.GUI.Canvas.GH_ObjectResponse RespondToMouseDoubleClick(Grasshopper.GUI.Canvas.GH_Canvas sender, Grasshopper.GUI.GH_CanvasMouseEvent e)
        {
            // Reset list of objective values
            MyComponent.DataWritten = "Data not written";
            MyComponent.ImagesWritten = "Image not written";
            MyComponent.ObjValues      = new List<List<double>>();
            MyComponent.PropertyValues = new List<List<IConvertible>>();
            MyComponent.FirstRead = true;
            MyComponent.Iterating = true;
            this.Iterate();
            MyComponent.Iterating = false;
            Grasshopper.Instances.ActiveCanvas.Document.NewSolution(true,GH_SolutionMode.Silent);

            return base.RespondToMouseDoubleClick(sender, e);
        }

        private void Iterate()
        {
            // Create directories if not existed yet
            if (MyComponent.Mode == CaptureComponent.CaptureMode.SaveScreenshot || MyComponent.Mode == CaptureComponent.CaptureMode.Both)
            {
                if (!Directory.Exists(MyComponent.SSDir))
                {
                    try
                    {
                        Directory.CreateDirectory(MyComponent.SSDir);
                    }
                    catch (Exception e)
                    {
                        MyComponent.AddRuntimeMessage((GH_RuntimeMessageLevel)20, e.ToString());
                        throw e;
                    }
                }
            }
            if (MyComponent.Mode == CaptureComponent.CaptureMode.SaveCSV || MyComponent.Mode == CaptureComponent.CaptureMode.Both)
            {
                if (!Directory.Exists(MyComponent.CSVDir))
                {
                    try
                    {
                        Directory.CreateDirectory(MyComponent.CSVDir);
                    }
                    catch (Exception e)
                    {
                        MyComponent.AddRuntimeMessage((GH_RuntimeMessageLevel)20, e.ToString());
                        throw e;
                    }
                }
            }

            int i = 1;
            int total_num = MyComponent.DesignMap.Count;

            foreach (List<double> sample in MyComponent.DesignMap)
            {
                // Trigger Sliders change
                GHUtilities.ChangeSliders(MyComponent.SlidersList, sample);

                // If we're taking screen shots, this happens here.
                if (MyComponent.Mode == CaptureComponent.CaptureMode.SaveScreenshot || MyComponent.Mode == CaptureComponent.CaptureMode.Both)
                {
                    BeforeScreenShots();
                    ScreenShot(i, total_num);
                    AfterScreenShots();
                }

                // Write intermediate Screenshots
                if (MyComponent.Mode == CaptureComponent.CaptureMode.SaveCSV || MyComponent.Mode == CaptureComponent.CaptureMode.Both)
                {

                    if (MyComponent.SaveFreq > 0)
                    {
                        if (i % MyComponent.SaveFreq == 0)
                        {
                            WriteOutputToFile(MyComponent.AssembleDMO(MyComponent.DesignMap, MyComponent.ObjValues), MyComponent.PropertyValues,
                                MyComponent.CSVDir, MyComponent.CSVFilename, ".csv", progress_i: i);
                            int Last = i - MyComponent.SaveFreq;
                            System.IO.File.Delete(MyComponent.CSVDir + MyComponent.CSVFilename + "_progress_" + Last.ToString() + ".csv");
                        }
                    }
                }
                i++;
            }

            // If we're saving a CSV, this happens here.
            if (MyComponent.Mode == CaptureComponent.CaptureMode.SaveCSV || MyComponent.Mode == CaptureComponent.CaptureMode.Both)
            {
                WriteOutputToFile(MyComponent.AssembleDMO(MyComponent.DesignMap, MyComponent.ObjValues), MyComponent.PropertyValues,
                    MyComponent.CSVDir, MyComponent.CSVFilename, ".csv");
            }
        }

        private Color currentColor;
        private bool grid, axes, worldAxes;

        private void BeforeScreenShots()
        {
            // Change Rhino appearance settings for best screenshot properties, and remember settings so we can change them back.

            currentColor = Rhino.ApplicationSettings.AppearanceSettings.ViewportBackgroundColor;
            grid = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.ConstructionGridVisible;
            axes = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.ConstructionAxesVisible;
            worldAxes = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.WorldAxesVisible;

            Rhino.ApplicationSettings.AppearanceSettings.ViewportBackgroundColor = Color.White;
            Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.ConstructionGridVisible = false;
            Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.ConstructionAxesVisible = false;
            Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.WorldAxesVisible = false;
        }

        private void AfterScreenShots()
        {
            // Change Rhino appearance settings back after screen shots.

            Rhino.ApplicationSettings.AppearanceSettings.ViewportBackgroundColor = currentColor;
            Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.ConstructionGridVisible = grid;
            Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.ConstructionAxesVisible = axes;
            Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.WorldAxesVisible = worldAxes;
        }

        private void ScreenShot(int i, int total_num)
        {
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            Rhino.Display.RhinoView view = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView;
            if (view == null)
            {
                return;
            }

            if (!Directory.Exists(MyComponent.SSDir))
            {
                MyComponent.AddRuntimeMessage((GH_RuntimeMessageLevel)20, "No valid screenshot directory exists:" + MyComponent.SSDir + ". Please add valid directory");
                //throw new Exception("No valid screenshot directory exists:" + MyComponent.SSDir + ". Please add valid directory");
            }

            // add zero paddings in front of the index (useful when listing images as a sequence)
            //string fileName = @"" + MyComponent.SSDir + MyComponent.SSFilename + "-" + 
            //    i.ToString("D" + total_num.ToString().Length) + ".png";
            string fileName = Path.Combine(MyComponent.SSDir, MyComponent.SSFilename + "-" +
                i.ToString("D" + total_num.ToString().Length) + ".png");

            // TODO let user specify resolutions
            Bitmap image = view.CaptureToBitmap();

            if (image == null)
            {
                return;
            }

            image.Save(fileName);
            image = null;
            MyComponent.ImagesWritten = "Image written to " + fileName;
        }

        private void WriteOutputToFile(List<List<double>> num_output, List<List<IConvertible>> generic_output, string path, string filename, string extension, 
                int progress_i=-1)
        {
            string file_suffix = progress_i >= 0 ? "_progress_" + progress_i.ToString() : "";
            string csv_filepath = Path.Combine(path, filename + file_suffix + extension);
                // @"" + path + filename + file_suffix + extension;

            if (!Directory.Exists(path))
            {
                MyComponent.AddRuntimeMessage((GH_RuntimeMessageLevel)20,
                    "No valid directory exists for saving CSV files:" + path + ". Please add valid directory");
                //throw new Exception("No valid directory exists for saving CSV files:" + path + ". Please add valid directory");
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(csv_filepath))
            {
                // number outputs is gathered before properties are gathered when saving intermediate results
                for (int i = 0; i < generic_output.Count; i++)
                {
                    string b = null;
                    for (int j = 0; j < num_output[i].Count; j++)
                    {
                        b = b + num_output[i][j] + ",";
                    }
                    for (int j = 0; j < generic_output[i].Count - 1; j++)
                    {
                        b = b + generic_output[i][j] + ",";
                    }
                    b = b + generic_output[i][generic_output[i].Count - 1];

                    file.WriteLine(b);
                }
            }
            MyComponent.DataWritten = "Data written to " + csv_filepath;
        }

    }
}
