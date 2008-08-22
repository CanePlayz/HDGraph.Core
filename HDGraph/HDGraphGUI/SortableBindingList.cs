using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace HDGraph
{
    /// <summary>
    /// Default implementation of BindingList does not support sorting.
    /// This class fixes that problem. However it does not support filtering.
    /// </summary>
    /// <typeparam name="T">Type of object that will be stored</typeparam>
    public class SortableBindingList<T> : BindingList<T>
    {

        #region Private Variables

        private bool _Sorted = false;
        private PropertyDescriptor _sortProperty = null;
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;

        #endregion

        #region Constructors

        public SortableBindingList()
            : base()
        {
        }

        public SortableBindingList(IList<T> list)
            : base(list)
        {
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Returns whether this object supports Sorting
        /// </summary>
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        /// <summary>
        /// Whether the collection has been sorted.
        /// </summary>
        protected override bool IsSortedCore
        {
            get { return _Sorted; }
        }

        /// <summary>
        /// Direction in which sorting was last performed (Ascending/Descending).
        /// </summary>
        protected override ListSortDirection SortDirectionCore
        {
            get { return _sortDirection; }
        }

        /// <summary>
        /// Property on which the sort has been last performed
        /// </summary>
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return _sortProperty; }
        }

        /// <summary>
        /// Sort the collection.
        /// </summary>
        /// <param name="prop">Property on which the sort will be performed.</param>
        /// <param name="direction">The direction in which the collection should be sorted.</param>
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            // Get list to sort
            List<T> items = this.Items as List<T>;

            // Apply and set the sort
            if (items != null)
            {
                PropertyComparer<T> pc = new PropertyComparer<T>(prop, direction);
                items.Sort(pc);
                _Sorted = true;

                _sortProperty = prop;
                _sortDirection = direction;

                // Let bound controls know they should refresh their views
                //OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
            else
            {
                _Sorted = false;
            }
        }

        protected override void RemoveSortCore()
        {
            throw new NotSupportedException();
            //_Sorted = false;
            //this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }


        #endregion

    }

    public class PropertyComparer<T> : System.Collections.Generic.IComparer<T>
    {

        // The following code contains code implemented by Rockford Lhotka:
        // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnadvnet/html/vbnet01272004.asp

        private PropertyDescriptor _property;
        private ListSortDirection _direction;

        public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
        {
            _property = property;
            _direction = direction;
        }

        #region IComparer<T>

        public int Compare(T xWord, T yWord)
        {
            // Get property values
            object xValue = GetPropertyValue(xWord, _property.Name);
            object yValue = GetPropertyValue(yWord, _property.Name);

            // Determine sort order
            if (_direction == ListSortDirection.Ascending)
            {
                return CompareAscending(xValue, yValue);
            }
            else
            {
                return CompareDescending(xValue, yValue);
            }
        }

        public bool Equals(T xWord, T yWord)
        {
            return xWord.Equals(yWord);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        #endregion

        // Compare two property values of any type
        private int CompareAscending(object xValue, object yValue)
        {
            int result;

            // If values implement IComparer
            if (xValue is IComparable)
            {
                result = ((IComparable)xValue).CompareTo(yValue);
            }
            // If values don't implement IComparer but are equivalent
            else if (xValue.Equals(yValue))
            {
                result = 0;
            }
            // Values don't implement IComparer and are not equivalent, so compare as string values
            else result = xValue.ToString().CompareTo(yValue.ToString());

            // Return result
            return result;
        }

        private int CompareDescending(object xValue, object yValue)
        {
            // Return result adjusted for ascending or descending sort order ie
            // multiplied by 1 for ascending or -1 for descending
            return CompareAscending(xValue, yValue) * -1;
        }

        private object GetPropertyValue(T value, string property)
        {
            // Get property
            PropertyInfo propertyInfo = value.GetType().GetProperty(property);

            // Return value
            return propertyInfo.GetValue(value, null);
        }
    }

}
