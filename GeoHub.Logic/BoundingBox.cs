using System;

namespace GeoHub.Logic
{
    /// <summary>
    /// Specification for a Bounding Box
    /// </summary>
    public class BoundingBox:IEquatable<BoundingBox>
    {
        private Coordinates _coordinates = null;
        private int _radius = -1;

        /// <summary>
        /// The point/coordinates at center of the bounding box
        /// </summary>
        public Coordinates Point
        {
            get { return _coordinates; }
        }

        /// <summary>
        /// THe radius (box) around Point, in km
        /// </summary>
        public int Radius
        {
            get { return _radius; }
        }

        
        public BoundingBox(Coordinates coordinates, int radius) 
        {
            if (coordinates == null)
            {
                throw  new ArgumentNullException(nameof(coordinates));
            }

            _coordinates = coordinates;

            if (radius < 0)
            {
                throw new ArgumentException(nameof(radius));
            }

            _radius = radius;
        }

        public bool Equals(BoundingBox other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                
                return Point.Equals(other.Point) && Radius == other.Radius;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BoundingBox);
        }

        public override int GetHashCode()
        {
            return Point.GetHashCode() + Radius.GetHashCode();
        }
    }
}