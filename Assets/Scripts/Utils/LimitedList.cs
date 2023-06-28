using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List with a limited size and queue style methods.
/// This is used for the Request queue.
/// </summary>
/// <typeparam name="T"></typeparam>
public class LimitedList<T> : IList<T>
{
    private readonly List<T> _list = new List<T>();
    private readonly int _maxSize;

    public LimitedList(int maxSize)
    {
        _maxSize = maxSize;
    }

    public LimitedList(LimitedList<T> CopyFrom, int maxSize)
    {
        _maxSize = maxSize;
        
        for (int i = 0; (i < _maxSize && i < CopyFrom.Count); ++i)
        {
            _list.Add(CopyFrom[i]);
        }
    }

    public int Count => _list.Count;
    public int MaxSize => _maxSize;
    public bool IsReadOnly => false;

    public T this[int index]
    {
        get => _list[index];
        set => _list[index] = value;
    }

    public void Add(T item)
    {
        if (_list.Count >= _maxSize)
        {
            throw new InvalidOperationException("Cannot Add to LimitedList. LimitedList is full!");
        }

        _list.Add(item);
    }

    public bool TryPeek(out T item)
    {
        if (_list.Count <= 0)
        {
            item = default(T);
            return false;
        }

        item = _list[0];
        return true;
    }

    public bool TryDequeue(out T item)
    {
        if (!TryPeek(out item))
        {
            return false;
        }
        RemoveAt(0);
        return true;
    }

    public T Dequeue()
    {
        T temp = this[0];
        RemoveAt(0);
        return temp;
    }

    public void Clear() => _list.Clear();
    public bool Contains(T item) => _list.Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
    public int IndexOf(T item) => _list.IndexOf(item);
    public void Insert(int index, T item) => _list.Insert(index, item);
    public bool Remove(T item) => _list.Remove(item);
    public void RemoveAt(int index) => _list.RemoveAt(index);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
