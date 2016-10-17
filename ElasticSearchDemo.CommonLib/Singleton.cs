using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchDemo.CommonLib
{
    public abstract class Singleton<T>
    {
        private static readonly Lazy<T> _instance
          = new Lazy<T>(() =>
          {
              var ctors = typeof(T).GetConstructors(
                  BindingFlags.Instance
                  | BindingFlags.NonPublic
                  | BindingFlags.Public);
              if (ctors.Count() != 1)
                  throw new InvalidOperationException($"Type {typeof (T)} must have exactly one constructor.");
              var ctor = ctors.SingleOrDefault(c => !c.GetParameters().Any() && c.IsPrivate);
              if (ctor == null)
                  throw new InvalidOperationException(
                      $"The constructor for {typeof (T)} must be private and take no parameters.");
              return (T)ctor.Invoke(null);
          });

        public static T Instance => _instance.Value;
    }
}
