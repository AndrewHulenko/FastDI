using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FastDI
{
    public class BinderContainer
    {
        private readonly BinderContext _context;
        private readonly List<object> _dependentInstances;

        public BinderContainer(BinderContext context)
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
                    string message = instance.GetType().Name + ".cs - is already registered in the context: " + _context;
                    BinderUtils.PrintError(message, instance);
                    return;
                }
                _dependentInstances.Add(instance);
                _dependentInstances.ForEach(FindFieldsWithInjectAttribute);
            }
        }

        public void Remove(object instance)
        {
            lock (_dependentInstances)
            {
                if (!_dependentInstances.Contains(instance))
                {
                    string message = instance.GetType().Name + ".cs - is not registered in the context: " + _context;
                    BinderUtils.PrintWarning(message, instance);
                    return;
                }                
                _dependentInstances.Remove(instance);
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
            List<object> foundObjects = new List<object>();
            
            Type fieldType = field.FieldType;
            if (fieldType.IsInterface)
            {
                foundObjects = _dependentInstances.FindAll(dependent => 
                    dependent.GetType().GetInterfaces().ToList().Contains(fieldType));
            }
            else if(fieldType.IsClass)
            {
                foundObjects = _dependentInstances.FindAll(dependent => dependent.GetType() == fieldType);    
            }
            
            if (foundObjects.Count > 1)
            {
                string message = instance.GetType().Name + ".cs, field: " + field + " - found more then one object: " 
                                 + fieldType + " in context: " + _context;
                BinderUtils.PrintWarning(message, instance);
            }
            
            object foundObject = foundObjects.FirstOrDefault();
            field.SetValue(instance, foundObject);
        }
    }
}