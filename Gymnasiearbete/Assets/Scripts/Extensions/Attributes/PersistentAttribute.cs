using System;

namespace ArenaShooter.Extensions.Attributes
{

    /// <summary>
    /// Marks this class as a persistent class (DontDestroyOnLoad).
    /// However, no logic is done with this attribute, it only acts as a mark.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    sealed class PersistentAttribute : Attribute
    {

        public PersistentAttribute() { }

    }

}
