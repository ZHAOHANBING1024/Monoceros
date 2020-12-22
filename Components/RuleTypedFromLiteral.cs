﻿using System;
using Grasshopper.Kernel;

namespace WFCToolset
{

    public class ComponentRuleTypedFromLiteral : GH_Component
    {
        public ComponentRuleTypedFromLiteral() : base("WFC Create typed rule from literal", "WFCRuleTypLit",
            "Create a typed (connector-to-all-same-type-connecors) WFC Rule from module name, connector number and connector type.",
            "WaveFunctionCollapse", "Rule")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Module", "M", "Module name", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Connector", "C", "Connector number", GH_ParamAccess.item);
            pManager.AddTextParameter("Type", "T", "Connector type", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new RuleParameter(), "Rule", "R", "WFC Rule", GH_ParamAccess.item);
        }

        /// <summary>
        /// Wrap input geometry into module cages.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string moduleName = "";
            int connector = 0;
            string type = "";


            if (!DA.GetData(0, ref moduleName)) return;
            if (!DA.GetData(1, ref connector)) return;
            if (!DA.GetData(2, ref type)) return;


            var rule = new Rule(moduleName, connector, type);

            DA.SetData(0, rule);
        }


        /// <summary>
        /// The Exposure property controls where in the panel a component icon 
        /// will appear. There are seven possible locations (primary to septenary), 
        /// each of which can be combined with the GH_Exposure.obscure flag, which 
        /// ensures the component will only be visible on panel dropdowns.
        /// </summary>
        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon =>
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                Properties.Resources.C;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("1C74EDBE-C2DC-4C3B-922F-7E6C662340BC");
    }
}