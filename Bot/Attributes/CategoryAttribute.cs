using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    class CategoryAttribute : Attribute
    {
        public string Name { get; private set; }
        public CategoryAttribute(string name)
        {
            Name = name;
        }
    }
}
