using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where  T : MonoBehaviour
{
    private readonly T _prototype;
    private readonly List<T> _elements;
    private readonly bool _autoExpand;
    private readonly int _elementsCount;
    private readonly Transform _parent;

    public ObjectPool(T prototype, bool autoExpand, int elementsCount, Transform parent, bool activeFromStart)
    {
        _prototype = prototype;
        _autoExpand = autoExpand;
        _elementsCount = elementsCount;
        _parent = parent;

        _elements = new List<T>();
        CreatePool(_elementsCount, activeFromStart);
    }

    public bool TryGetFreeElement(out T element)
    {
        if (_autoExpand)
        {
            element = GetFreeElement();
            element ??= AddElement(true);
            return element != null;
        }
        
        element = GetFreeElement();
        return element != null;
    }

    public List<T> GetAllFreeElements()
    {
        List<T> freeElements = new List<T>();
        foreach (var element in _elements)
        {
            if (element.gameObject.activeSelf == false)
                freeElements.Add(element);
        }

        return freeElements;
    }

    public List<T> GetAllElements() => 
        _elements;
    
    public bool HasFreeElement() => 
        GetFreeElement() != null;

    private T GetFreeElement()
    {
        foreach (var element in _elements)
        {
            if (element.gameObject.activeSelf == false)
                return element;
        }
        
        return null;
    }

    private void CreatePool(int elementsCount, bool activeFromStart)
    {
        for (int i = 0; i < _elementsCount; i++)
            AddElement(activeFromStart);
    }

    private T AddElement(bool active)
    {
        var element = Object.Instantiate(_prototype, _parent);
        _elements.Add(element);
        element.gameObject.SetActive(active);
        return element;
    }
}
