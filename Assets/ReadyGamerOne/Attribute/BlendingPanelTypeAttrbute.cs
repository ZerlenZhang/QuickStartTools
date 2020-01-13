using System;

namespace ReadyGamerOne.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BlendingPanelTypeAttribute:System.Attribute
    {
        public Type panelType;

        public BlendingPanelTypeAttribute(Type type)
        {
            panelType = type;
        }
    }
}