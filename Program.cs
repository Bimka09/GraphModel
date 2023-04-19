using Graph;

var graph = new GraphModel();

var v0 = new Vertex(0);
graph.AddVertex(v0);
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

graph.AddEdge(v0, v1, 2);
graph.AddEdge(v1, v2, 3);
graph.AddEdge(v0, v2, 4);
graph.AddEdge(v2, v3, 5);
graph.AddEdge(v1, v3, 6);
graph.AddEdge(v3, v4, 2);
graph.AddEdge(v3, v5, 1);
graph.AddEdge(v4, v5, 3);


//var matrix = graph.GetMatrix();
//var vector = graph.GetVector();
//var components = graph.Kosaraju();
//var sortedVertexes = graph.DemukronSort();
//var edgesKruskal = graph.Kruskal();
var shortestPath = graph.Dijkstra();




