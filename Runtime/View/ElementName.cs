namespace Haikara.Runtime.View
{
    public struct ElementName
    {
        public string Name { get; }

        private ElementName(string name)
        {
            Name = name;
        }

        public static implicit operator ElementName(string name)
        {
            return new ElementName(name);
        }

        public static implicit operator string(ElementName self)
        {
            return self.Name;
        }
    }
}