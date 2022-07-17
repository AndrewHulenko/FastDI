using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace FastDI
{
    public class Container
    {
        private readonly Context _context;
        private readonly List<object> _dependentInstances;

        public Container(Context context)
        {
            _context = context;
            _dependentInstances = new List<object>();
        }
        
        public void Install(object instance)
        {
            lock (_dependentInstances)
            {
                if (_dependentInstances.Contains(instance))
                {
                    Print.Error(instance.GetType().Name + ".cs - is already registered in the context: " + _context, instance);
                    return;
                }
                _dependentInstances.Add(instance);
            }
            UpdateDependentReferences();
        }

        public void Remove(object instance)
        {
            lock (_dependentInstances)
            {
                if (!_dependentInstances.Contains(instance))
                {
                    Print.Warning(instance.GetType().Name + ".cs - is not registered in the context: " + _context, instance);
                    return;
                }                
                _dependentInstances.Remove(instance);   
            }
            
            UpdateDependentReferences();
        }
        
        private void UpdateDependentReferences()
        {
            lock(_dependentInstances) {
                _dependentInstances.ForEach(FindFieldsWithInjectAttribute);
            }
        }
        
        private void FindFieldsWithInjectAttribute(object instance)
        {
            Type instanceType = instance.GetType();
            
            FieldInfo[] fields = instanceType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                InjectAttribute [] injectAttributes = (InjectAttribute[]) field.GetCustomAttributes(typeof(InjectAttribute), true);
                if (injectAttributes.Length > 0)
                {
                    InjectInField(instance, field);
                }
            }
        }

        private void InjectInField(object instance, FieldInfo field)
        {
            ConcurrentBag<object> foundObjects = new ConcurrentBag<object>();
            
            Type fieldType = field.FieldType;
            if (fieldType.IsInterface)
            {
                Parallel.ForEach(_dependentInstances, dependent =>
                {
                    if (dependent.GetType().GetInterfaces().ToList().Contains(fieldType))
                    {
                        foundObjects.Add(dependent);
                    }
                });
            }
            else if(fieldType.IsClass)
            {
                Parallel.ForEach(_dependentInstances, dependent =>
                {
                    if (dependent.GetType() == fieldType)
                    {
                        foundObjects.Add(dependent);
                    }
                });                
            }
            
            if (foundObjects.Count > 1)
            {
                string typeName = instance.GetType().Name;
                Print.Warning(typeName + ".cs, field: " + field + " - found more then one object: " + fieldType + " in context: " + _context, instance);
            }
            
            object foundObject = foundObjects.FirstOrDefault();
            field.SetValue(instance, foundObject);
        }
    }
}