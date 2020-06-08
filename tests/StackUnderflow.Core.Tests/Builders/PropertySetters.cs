namespace StackUnderflow.Core.Tests.Builders
{
    public static class PropertySetter
    {
        public static void SetProperty(this object target, string propertyName, object value)
        {
            var propertyInfo = target.GetType().GetProperty(propertyName);
            propertyInfo.SetValue(target, value);
        }
    }
}