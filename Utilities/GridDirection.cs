﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Rhino.Geometry;

namespace WFCToolset
{
    /// <summary>
    /// Submodule face axis within a grid: <c>X</c> or <c>Y</c> or <c>Z</c>.
    /// </summary>
    public enum Axis
    {
        X,
        Y,
        Z
    }

    /// <summary>
    /// Submodule face orientation within a grid: <c>Positive</c> or <c>Negative</c>.
    /// </summary>
    public enum Orientation
    {
        Positive,
        Negative
    }

    /// <summary>
    /// Submodule face direction consisting of <see cref="Axis"/> and <see cref="Orientation"/>.
    /// </summary>
    public struct Direction
    {
        public Axis _axis;
        public Orientation _orientation;

        /// <summary>
        /// Determines whether the other <see cref="Direction"/> is opposite to the current.
        /// A <see cref="Direction"/> is opposite when the <see cref="Axis"/> equals and <see cref="Orientation"/> does not.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>True if opposite.</returns>
        public bool IsOpposite(Direction other)
        {
            return _axis == other._axis &&
                _orientation != other._orientation;
        }

        /// <summary>
        /// Returns flipped <see cref="Direction"/> - the <see cref="Axis"/> remains the same, the <see cref="Orientation"/> flips.
        /// </summary>
        /// <returns>A flipped Direction.</returns>
        public Direction ToFlipped()
        {
            var flipped = new Direction()
            {
                _axis = _axis,
                _orientation = _orientation == Orientation.Positive ? Orientation.Negative : Orientation.Positive
            };
            return flipped;
        }


        /// <summary>
        /// Converts the <see cref="Direction"/> to a <see cref="Vector3d"/> in Cartesian coordinate system.
        /// </summary>
        /// <returns>A Vector3d.</returns>
        public Vector3d ToVector()
        {
            if (_axis == Axis.X && _orientation == Orientation.Positive)
            {
                return Vector3d.XAxis;
            }
            if (_axis == Axis.Y && _orientation == Orientation.Positive)
            {
                return Vector3d.YAxis;
            }
            if (_axis == Axis.Z && _orientation == Orientation.Positive)
            {
                return Vector3d.ZAxis;
            }
            if (_axis == Axis.X && _orientation == Orientation.Negative)
            {
                var directionVector = Vector3d.XAxis;
                directionVector.Reverse();
                return directionVector;
            }
            if (_axis == Axis.Y && _orientation == Orientation.Negative)
            {
                var directionVector = Vector3d.YAxis;
                directionVector.Reverse();
                return directionVector;
            }
            if (_axis == Axis.Z && _orientation == Orientation.Negative)
            {
                var directionVector = Vector3d.ZAxis;
                directionVector.Reverse();
                return directionVector;
            }
            return Vector3d.Unset;
        }

        /// <summary>
        /// Converts the <see cref="Direction"/> to a submodule connector index, according to the convention:
        /// (submoduleIndex * 6) + faceIndex, where faceIndex is X=0, Y=1, Z=2, -X=3, -Y=4, -Z=5.
        /// This method is the source of truth.
        /// </summary>
        /// <returns>Submodule connector index.</returns>
        public int ToConnectorIndex()
        {
            // Connector numbering convention: (submoduleIndex * 6) + faceIndex, where faceIndex is X=0, Y=1, Z=2, -X=3, -Y=4, -Z=5
            if (_axis == Axis.X && _orientation == Orientation.Positive)
            {
                return 0;
            }
            if (_axis == Axis.Y && _orientation == Orientation.Positive)
            {
                return 1;
            }
            if (_axis == Axis.Z && _orientation == Orientation.Positive)
            {
                return 2;
            }
            if (_axis == Axis.X && _orientation == Orientation.Negative)
            {
                return 3;
            }
            if (_axis == Axis.Y && _orientation == Orientation.Negative)
            {
                return 4;
            }
            if (_axis == Axis.Z && _orientation == Orientation.Negative)
            {
                return 5;
            }
            // Never
            return -1;
        }

    }
}
