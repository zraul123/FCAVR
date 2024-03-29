﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Ganter.Algorithm
{
    /// <summary>
    /// Class representing an attribute of a formal context.
    /// </summary>
    public class Attribute
    {
        private int _max;
        private int _min;

        /// <summary>
        /// The name of the attribute.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The lectic position of the attribute. If none is given, the formal context should assign a default value.
        /// </summary>
        public int LecticPosition { get; set; }
        /// <summary>
        /// The maximum value discovered during preprocessing. Serves to count the Step value.
        /// </summary>
        public int Max
        {
            get { return _max; }
            set
            {
                _max = value;
                CalculateDefaultStep();
            }
        }
        /// <summary>
        /// The minimal value discovered during preprocessing. Servers to count the Step value.
        /// </summary>
        public int Min
        {
            get { return _min; }
            set
            {
                _min = value;
                CalculateDefaultStep();
            }
        }
        /// <summary>
        /// The step, that will divide this attribute into several sub-attributes.
        /// </summary>
        public int Step { get; set; }
        /// <summary>
        /// If this is a sub-attribute, the reference for the original attribute is needed for creation of formal concept.
        /// </summary>
        public Attribute ParentAttribute { get; set; }

        /// <summary>
        /// Compares two formal attributes based on their lectic position.
        /// </summary>
        /// <param name="a">The first attribute to compare.</param>
        /// <param name="b">The second attribute to compare.</param>
        /// <returns>True, if the first attribute is smaller than the second. Otherwise returns false.</returns>
        public static bool operator <(Attribute a, Attribute b)
        {
            if (a == null || b == null) throw new ArgumentNullException();
            else return a.LecticPosition < b.LecticPosition;
        }

        /// <summary>
        /// Compares two formal attributes based on their lectic position.
        /// </summary>
        /// <param name="a">The first attribute to compare.</param>
        /// <param name="b">The second attribute to compare.</param>
        /// <returns>True, if the first attribute is larger than the second. Otherwise returns false.</returns>
        public static bool operator >(Attribute a, Attribute b)
        {
            if (a == null || b == null) throw new ArgumentNullException();
            else return a.LecticPosition > b.LecticPosition;
        }

        /// <summary>
        /// Compares two formal attributes based on their lectic position.
        /// </summary>
        /// <param name="a">The first attribute to compare.</param>
        /// <param name="b">The second attribute to compare.</param>
        /// <returns>True, if the first attribute is smaller or equal than the second. Otherwise returns false.</returns>
        public static bool operator <=(Attribute a, Attribute b)
        {
            if (a == null && b == null) return true;
            else if (a == null || b == null) return false;
            else return a.LecticPosition <= b.LecticPosition;
        }

        /// <summary>
        /// Compares two formal attributes based on their lectic position.
        /// </summary>
        /// <param name="a">The first attribute to compare.</param>
        /// <param name="b">The second attribute to compare.</param>
        /// <returns>True, if the first attribute is larger or equal than the second. Otherwise returns false.</returns>
        public static bool operator >=(Attribute a, Attribute b)
        {
            if (a == null && b == null) return true;
            else if (a == null || b == null) return false;
            else return a.LecticPosition >= b.LecticPosition;
        }

        /// <summary>
        /// Compares two formal attributes based on their lectic position.
        /// </summary>
        /// <param name="a">The first attribute to compare.</param>
        /// <param name="b">The second attribute to compare.</param>
        /// <returns>True, if both attributes have the same lectic position, or both are null. Otherwise returns false.</returns>
        public static bool operator ==(Attribute a, Attribute b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.LecticPosition == b.LecticPosition;
        }

        /// <summary>
        /// Compares two formal attributes based on their lectic position.
        /// </summary>
        /// <param name="a">The first attribute to compare.</param>
        /// <param name="b">The second attribute to compare.</param>
        /// <returns>True, if one attribute is null or they do not have the same lectic position. Otherwise returns false.</returns>
        public static bool operator !=(Attribute a, Attribute b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Determines, whether given number is larger than the minimal value, but smaller than the maximum.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool IsInRange(decimal number)
        {
            return number >= Min && number < Max;
        }

        /// <summary>
        /// Creates a closure (a derivation, according to Ganter) of this attribute and an attribute set, based on give formal context.
        /// The closure is formed as an intent of the extent of the union of this attribute and all attributes in the attribute set, that
        ///  have smaller lectic position.
        /// </summary>
        /// <param name="setA">The attribute set, to form a closure.</param>
        /// <param name="formalContext">The formal context, into which both the attribute and the attribute set belong.</param>
        /// <returns>The Ganter closure for given attribute, attribute set and formal context.</returns>
        public List<Attribute> Closure(List<Attribute> setA, FormalContext formalContext)
        {
            List<Attribute> lecticSet = FormLecticSet(setA);
            return formalContext.Intent(formalContext.Extent(lecticSet)).ToList();
        }

        /// <summary>
        /// Forms a "lectic set" - a union of this attribute and all attributes from the set that have smaller lectic position.
        /// </summary>
        /// <param name="setA">The attribute set, to form a closure.</param>
        /// <returns>A union of this attribute and all attributes from the set that have smaller lectic position.</returns>
        private List<Attribute> FormLecticSet(List<Attribute> setA)
        {
            List<Attribute> result = new List<Attribute>();
            result = setA.Where(a => a.LecticPosition < this.LecticPosition).ToList();
            result.Add(this);
            return result;
        }

        /// <summary>
        /// Compares this attribute to another object.
        /// </summary>
        /// <param name="obj">An object, that should be compared to this attribute.</param>
        /// <returns>True, if the object is an attribute and has the same lectic position, as this attribute.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() == typeof(Attribute))
            {
                return this.LecticPosition == (obj as Attribute).LecticPosition;
            }
            else return false;
        }

        /// <summary>
        /// Gets the hash code of the object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.LecticPosition;
        }

        /// <summary>
        /// Converts the object to its string representation.
        /// </summary>
        /// <returns>Returns the name of the attribute and its lectic position.</returns>
        public override string ToString()
        {
            return string.Format("{0} : {1}", Name, LecticPosition);
        }

        /// <summary>
        /// Calculates the default step value based on the Min and Max properties.
        /// </summary>
        private void CalculateDefaultStep()
        {
            int difference = Max - Min;

            if (difference <= 3)
            {
                Step = 1;
            }
            else
            {
                Step = difference / 4;
            }
        }
    }
}
