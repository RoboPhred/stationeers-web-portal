using System;

namespace WebAPI.JsonTranslation
{
    [AttributeUsage(AttributeTargets.Class)]
    public class JsonTranslatorTargetAttribute : Attribute
    {
        public Type TargetType { get; }
        public JsonTranslatorTargetAttribute(Type targetType)
        {
            this.TargetType = targetType;
        }
    }
}