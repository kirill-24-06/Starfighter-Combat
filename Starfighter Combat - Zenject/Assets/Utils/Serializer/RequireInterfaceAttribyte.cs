using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils.Serializer
{
  
    [Serializable]
    public class InterfaceReference<TInterface, TObject> where TObject : Object where TInterface : class
    {
        [SerializeField] TObject _value;

        public TInterface Value
        {
            get => _value switch
            {
                null => null,
                TInterface @interface => @interface,
                _ => throw new InvalidOperationException($"{_value} needs to implement interface {nameof(TInterface)}")
            };

            set => _value = value switch
            {
                null => null,
                TObject newValue => newValue,
                _ => throw new ArgumentException($"{value} needs to be of type {typeof(TObject)}", string.Empty)
            };
        }

        public TObject ValueNoChecks
        {
            get => _value;
            set => _value = value;
        }

        public InterfaceReference() { }

        public InterfaceReference(TObject @object) => _value = @object;

        public InterfaceReference(TInterface @interface) => _value = @interface as TObject;
    }

    [Serializable]
    public class InterfaceReference<TInterface> : InterfaceReference<TInterface, Object> where TInterface : class { }


}
