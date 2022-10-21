namespace Graph
{
    public class GraphModel
    {
        List<Vertex> Vertexes = new List<Vertex>();
        List<Edge> Edges = new List<Edge>();

        public int VertexCount => Vertexes.Count;
        public int EdgeCount => Edges.Count;

        public void AddVertex(Vertex vertex)
        {
            Vertexes.Add(vertex);
        }

        public void AddEdge(Vertex from, Vertex to)
        {
            var edge = new Edge(from, to);
            Edges.Add(edge);
        }
        public int[,] GetMatrix()
        {
            var matrix = new int[VertexCount, VertexCount];
            foreach(var edge in Edges)
            {
                var row = edge.From.Number;
                var column = edge.To.Number;

                matrix[row - 1, column - 1] = edge.Weight;
                if(row == column)
                {
                    matrix[column - 1, row - 1] = edge.Weight * 2;
                }
                else
                {
                    matrix[column - 1, row - 1] = edge.Weight;
                }
            }
            return matrix;
        }
    }
}
