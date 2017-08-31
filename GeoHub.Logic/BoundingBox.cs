using System;

namespace GeoHub.Logic
{
    /// <summary>
    /// Specification for a Bounding Box
    /// </summary>
    public class BoundingBox:IEquatable<BoundingBox>
    {
        public static bool operator ==(BoundingBox left, BoundingBox right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BoundingBox left, BoundingBox right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// The point/coordinates at center of the bounding box
        /// </summary>
        public Coordinates Point { get; } = null;

        /// <summary>
        /// THe radius (box) around Point, in km
        /// </summary>
        public int Radius { get; } 


        public BoundingBox(Coordinates coordinates, int radius) 
        {
            if (coordinates == null)
            {
                throw  new ArgumentNullException(nameof(coordinates));
            }

            Point = coordinates;

            if (radius < 0)
            {
                throw new ArgumentException(nameof(radius));
            }

            Radius = radius;
        }

        public bool Equals(BoundingBox other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Point, other.Point) && Radius == other.Radius;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BoundingBox) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Point != null ? Point.GetHashCode() : 0) * 397) ^ Radius;
            }
        }
    }
}