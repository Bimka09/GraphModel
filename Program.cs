using Graph;

var graph = new GraphModel();

var v1 = new Vertex(1);
graph.AddVertex(v1);
var v2 = new Vertex(2);
graph.AddVertex(v2);
var v3 = new Vertex(3);
graph.AddVertex(v3);
var v4 = new Vertex(4);
graph.AddVertex(v4);
var v5 = new Vertex(5);
graph.AddVertex(v5);

graph.AddEdge(v1, v2);
graph.AddEdge(v2, v3);
graph.AddEdge(v2, v4);
graph.AddEdge(v3, v4);
graph.AddEdge(v5, v5);

var matrix = graph.GetMatrix();
Console.Write(" ");
for (int i = 0; i < graph.VertexCount; i++)
{
    Console.Write(" | " + i + " | ");
}
Console.WriteLine();
Console.WriteLine("------------------------------------");
for (int i = 0; i < graph.VertexCount; i++)
{
    Console.Write(i + 1);
    for(int j = 0; j < graph.VertexCount; j++)
    {
        Console.Write(" | " + matrix[i, j] + " | ");
    }
    Console.WriteLine();
}
