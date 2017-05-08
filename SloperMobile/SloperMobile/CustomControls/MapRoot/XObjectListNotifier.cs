using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;

namespace SloperMobile.CustomControls.MapRoot
{
    public class XObjectListNotifier<T> : List<T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        public delegate void CollectionChangedHandler(object sender, List<T> list);

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event CollectionChangedHandler CollectionChangedInternal;

        private object _lockObject = new object();

        public XObjectListNotifier() : base()
        {
        }

        protected void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            if (property == null || PropertyChanged == null)
                return;

            var expression = property.Body as MemberExpression;

            if (expression == null)
                return;

            string name = expression.Member.Name;   // property.Name;
            NotifyPropertyChanged(name);
        }

        protected void NotifyPropertyChanged(string property_name)
        {
            if (PropertyChanged == null || string.IsNullOrEmpty(property_name))
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property_name));
        }

        protected void NotifyCollectionChanged(object sender)
        {
            NotifyCollectionReset(sender);

            //if(CollectionChanged != null)
            //	CollectionChanged.Invoke(sender, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, this));
            // error: Range actions are not supported. (i.e. cannot raise event with multiple items!)

            if (CollectionChangedInternal == null)
                return;

            CollectionChangedInternal.Invoke(sender, this); // new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, (IList) this));
        }


        protected void NotifyCollectionReset(object sender)
        {
            if (CollectionChanged != null)
            {
                try
                {
                    CollectionChanged.Invoke(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
                catch (Exception)
                {
                    return;
                }
            }


            //if( CollectionChangedInternal == null)
            //	return;
            //CollectionChangedInternal.Invoke( sender, this);
        }



        public new void Add(T item)
        {
            if (item == null)
                return;

            lock (_lockObject)
            {
                base.Add(item);
                NotifyCollectionChanged(this);
            }
        }

        public new void Clear()
        {
            lock (_lockObject)
            {
                base.Clear();
                NotifyCollectionReset(this);
            }
        }

        public new void AddRange(IEnumerable<T> list)
        {
            if (list == null || list == this)
                return;

            lock (_lockObject)
            {
                base.AddRange(list);
                NotifyCollectionChanged(this);
            }
        }

        public virtual bool CopyOther(IEnumerable<T> other)
        {
            if (other == null || other == this)
                return false;

            lock (_lockObject)
            {
                this.Clear();

                foreach (T item in other)
                    this.Add(item);
            }

            return true;
        }


        //event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        //{
        //	add { throw new NotImplementedException(); }
        //	remove { throw new NotImplementedException(); }
        //}
    }
}
