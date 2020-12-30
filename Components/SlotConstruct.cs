﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace WFCToolset
{

    public class ComponentConstructSlot : GH_Component
    {
        public ComponentConstructSlot() : base("WFC Construct Slot", "WFCConstSlot",
            "Construct a WFC Slot.",
            "WaveFunctionCollapse", "Slot")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Slot Point", "P", "Point inside the slot", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Base plane",
                                       "B",
                                       "Grid space base plane. Defines orientation of the grid.",
                                       GH_ParamAccess.item,
                                       Plane.WorldXY);
            pManager.AddVectorParameter(
               "Grid Slot Diagonal",
               "D",
               "World grid slot diagonal vector specifying single grid slot dimension in base-plane-aligned XYZ axes",
               GH_ParamAccess.item,
               new Vector3d(1.0, 1.0, 1.0)
               );
            pManager.AddBooleanParameter("Allows Everything",
                                         "E",
                                         "Initiate the slot with all modules allowed. False = Allow nothing, True = Allow everything.",
                                         GH_ParamAccess.item,
                                         true);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new SlotParameter(), "Slot", "S", "WFC Slot", GH_ParamAccess.item);
        }

        /// <summary>
        /// Wrap input geometry into module cages.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var point = new Point3d();
            var basePlane = new Plane();
            var diagonal = new Vector3d();
            var allowEverything = true;


            if (!DA.GetData(0, ref point))
            {
                return;
            }

            if (!DA.GetData(1, ref basePlane))
            {
                return;
            }

            if (!DA.GetData(2, ref diagonal))
            {
                return;
            }

            if (!DA.GetData(3, ref allowEverything))
            {
                return;
            }

            if (diagonal.X <= 0 || diagonal.Y <= 0 || diagonal.Z <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "One or more slot dimensions are not larger than 0.");
                return;
            }

            // Scale down to unit size
            var normalizationTransform = Transform.Scale(basePlane, 1 / diagonal.X, 1 / diagonal.Y, 1 / diagonal.Z);
            // Orient to the world coordinate system
            var worldAlignmentTransform = Transform.PlaneToPlane(basePlane, Plane.WorldXY);
            point.Transform(normalizationTransform);
            point.Transform(worldAlignmentTransform);
            // Round point location
            // Slot dimension is for the sake of this calculation 1,1,1
            var slotCenterPoint = new Point3i(point);

            var slot = new Slot(basePlane,
                                slotCenterPoint,
                                diagonal,
                                allowEverything,
                                new List<string>(),
                                new List<string>(),
                                0);

            DA.SetData(0, slot);
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
                Properties.Resources.S;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("7235D63E-8E6E-4BAE-BEFC-D6AFDFBE5357");
    }
}
