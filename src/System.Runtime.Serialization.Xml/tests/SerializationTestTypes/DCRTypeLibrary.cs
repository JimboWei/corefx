using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SerializationTestTypes
{
    [DataContract]
    public class ObjectContainer
    {
        [DataMember]
        private object _data;

        [DataMember]
        private object _data2;

        public ObjectContainer(object input)
        {
            _data = input;
            _data2 = _data;
        }

        public object Data
        {
            get { return _data; }
        }

        public object Data2
        {
            get { return _data2; }
        }
    }

    [Serializable]
    public class CollOfString : CollOfBoolBase, ICollection<string>
    {
        private string _stringField = "stringField";

        public void Add(string item)
        {
        }

        public bool Contains(string item)
        {
            return true;
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
        }

        public bool Remove(string item)
        {
            return true;
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return new List<string>() { "stringReturnsFromGetEnumerator" }.GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            var otherCollOfString = obj as CollOfString;
            return (otherCollOfString == null) ? false : otherCollOfString._stringField == _stringField;
        }

        public override int GetHashCode()
        {
            return (_stringField == null) ? 0 : _stringField.GetHashCode();
        }
    }

    public class CollOfBoolBase : ICollection<bool>
    {
        public void Add(bool item)
        {
        }

        public void Clear()
        {
        }

        public bool Contains(bool item)
        {
            return true;
        }

        public void CopyTo(bool[] array, int arrayIndex)
        {
        }

        public bool Remove(bool item)
        {
            return true;
        }

        public int Count
        {
            get
            {
                return 0;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IEnumerator<bool> GetEnumerator()
        {
            return new List<bool>() { true, false }.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new List<bool>() { true, false, true, false, true, false }.GetEnumerator();
        }
    }

    [Serializable]
    public sealed class DerivedFromBaseList : BaseList<string>
    {
        // no default constructor
        public DerivedFromBaseList(string str)
            : base(str)
        {
        }

        public override bool Equals(object obj)
        {
            var other = obj as DerivedFromBaseList;
            return other == null ? false : base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [Serializable]
    public class BaseList<T> : List<T>
    {
        private string _str;

        internal BaseList(string str)
        {
            _str = str;
        }

        public override bool Equals(object obj)
        {
            var other = obj as BaseList<T>;
            return other == null ? false : other._str == _str;
        }

        public override int GetHashCode()
        {
            return _str.GetHashCode();
        }
    }
}
