using System;
using System.IO;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Sampler 
{
    class SamplerComponentAttributes : Grasshopper.Kernel.Attributes.GH_ComponentAttributes
    {
        // Component
        SamplerComponent MyComponent;

        // Variables Declaration

        // Constructor
        public SamplerComponentAttributes(IGH_Component component) : base(component)
        {
            MyComponent = (SamplerComponent)component;
        }

        [STAThread]
        public override Grasshopper.GUI.Canvas.GH_ObjectResponse RespondToMouseDoubleClick(Grasshopper.GUI.Canvas.GH_Canvas sender, Grasshopper.GUI.GH_CanvasMouseEvent e)
        {
            MyComponent.FilesWritten = "File not written";
            if (MyComponent.Seed != 0) { MyComponent.MyRand = new Random(MyComponent.Seed); } // reset Random to give same result each time.
            MyComponent.Util.Sample();

            if (MyComponent.Dir != "None")
            {
                PrintAllSolutions();
            }
            Grasshopper.Instances.ActiveCanvas.Document.NewSolution(true);
            return base.RespondToMouseDoubleClick(sender, e);
        }

        public void PrintAllSolutions()
        {
            if (!Directory.Exists(MyComponent.Dir))
            {
                try
                {
                    Directory.CreateDirectory(MyComponent.Dir);
                }
                catch (Exception e)
                {
                    MyComponent.AddRuntimeMessage((GH_RuntimeMessageLevel)20, e.ToString());
                    throw e;
                }
            }

            string csv_filepath = @"" + MyComponent.Dir + MyComponent.Filename + ".csv";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(csv_filepath))
            {
                for (int i = 0; i < MyComponent.Output.Count; i++)
                {
                    string design = "";
                    List<double> currentDesign = MyComponent.Output[i];
                    for (int j = 0; j < currentDesign.Count - 1; j++)
                    {
                        design = design + currentDesign[j] + ",";
                    }

                    design = design + currentDesign[currentDesign.Count - 1];

                    file.WriteLine(design);
                }
            }
            MyComponent.FilesWritten = "File written to " + csv_filepath;
        }

    }
}
