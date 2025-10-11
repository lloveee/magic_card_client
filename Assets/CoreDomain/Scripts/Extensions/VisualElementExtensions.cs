using UnityEngine.UIElements;

namespace CoreDomain.Scripts.Extensions
{
    public static class VisualElementExtensions
    {
        public static VisualElement CreateChild(this VisualElement parent, params string[] classes)
        {
            var child = new VisualElement();
            child.AddClass(classes).AddTo(parent);
            return child;
        }
        
        public static VisualElement CreateChildWithName(this VisualElement parent, string childName, params string[] classes)
        {
            var child = new VisualElement { name = childName};
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        public static T CreateChild<T>(this VisualElement parent, params string[] classes) where T : VisualElement, new()
        {
            var child = new T();
            child.AddClass(classes).AddTo(parent);
            return child;
        }
        
        public static T CreateChildWithName<T>(this VisualElement parent, string childName, params string[] classes) where T : VisualElement, new()
        {
            var child = new T { name = childName};
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        public static T AddTo<T>(this T child, VisualElement parent) where T : VisualElement
        {
            parent.Add(child);
            return child;
        }

        public static T AddClass<T>(this T visualElement, params string[] classes) where T : VisualElement
        {
            foreach (var @class in classes)
            {
                if (!string.IsNullOrEmpty(@class))
                {
                    visualElement.AddToClassList(@class);
                }
            }

            return visualElement;
        }
    }
}