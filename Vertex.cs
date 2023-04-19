namespace Graph
{
    public class Vertex
    {
        public int Number { get; set; }
        public int Component { get; set; }
        public int TransactionCounter { get; set; }
        private Vertex _parent;

        public Vertex(int number)
        {
            Number = number;
        }
        public override string ToString()
        {
            return Number.ToString();
        }
        public Vertex SetParent(Vertex parent)
        {
            if (_parent == null || _parent == this)
            {
                return _parent = parent;
            }
            return _parent.SetParent(parent);
        }
        public Vertex GetParent(Vertex vertex)
        {

            if(_parent == this)
            {
                return _parent;
            }
            return _parent.GetParent(vertex);

        }
    }
}
