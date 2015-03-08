using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using MathExtensions.TwoD;

namespace MathExtensions
{
    [DebuggerDisplay("A = {A} Y = {Y}")]
    [DataContract()]
    public struct AnglePoint
    {
        public static readonly AnglePoint Empty = new AnglePoint(0, 0);

        private decimal m_a;
        private decimal m_y;

        /// <summary>
        /// Creates a new instance of the angle point. 
        /// </summary>
        /// <param name="a">The angle in degrees.</param>
        /// <param name="y">The distance from the origin.</param>
        /// <remarks></remarks>
        public AnglePoint(decimal a, decimal y)
        {
            m_a = a;
            m_y = y;
        }

        [DataMember]
        public decimal A
        {
            get { return m_a; }
            set { m_a = value; }
        }
        public decimal ARad
        {
            get { return MathExt.ToRad(m_a); }
            set { m_a = MathExt.ToDeg(value); }
        }
        [DataMember]
        public decimal Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AnglePoint)) throw new Exception("Can't compare to type '" + obj.GetType().Name + "'.");

            var _with1 = (AnglePoint)obj;
            return ((m_a == _with1.A) && (m_y == _with1.Y));
        }

        public static bool Equals(object objA, object objB)
        {
            if (!(objA is AnglePoint)) throw new Exception("Can't compare to type '" + objA.GetType().Name + "'.");
            if (!(objB is AnglePoint)) throw new Exception("Can't compare to type '" + objB.GetType().Name + "'.");

            return ((AnglePoint)objA).Equals(objB);
        }

        public bool IsEmpty
        {
            get { return this.Equals(Empty); }
        }

        public static AnglePoint operator -(AnglePoint pt1, AnglePoint pt2)
        {
            return new AnglePoint(pt1.A - pt2.A, pt1.Y - pt2.Y);
        }

        public static AnglePoint operator +(AnglePoint pt1, AnglePoint pt2)
        {
            return new AnglePoint(pt1.A + pt2.A, pt1.Y + pt2.Y);
        }

        public static bool operator !=(AnglePoint p1, AnglePoint p2)
        {
            return ((p1.A != p2.A) || (p1.Y != p2.Y));
        }

        public static bool operator ==(AnglePoint p1, AnglePoint p2)
        {
            return ((p1.A == p2.A) && (p1.Y == p2.Y));
        }

        public override string ToString()
        {
            return string.Format("{{A={0}, Y={1}}}", A, Y);
        }

        public static implicit operator Point2D(AnglePoint p)
        {
            return new Point2D(p.A, p.Y);
        }
    }
}