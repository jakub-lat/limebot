using System;

namespace LimeBot.Bot.Attributes
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
