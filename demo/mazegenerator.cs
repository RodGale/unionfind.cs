using System; 
using System.Linq; 
using System.Collections.Generic; 

using Cell = System.Collections.Generic.KeyValuePair<int, int>;
using Edge = System.Collections.Generic.KeyValuePair<System.Collections.Generic.KeyValuePair<int, int>, System.Collections.Generic.KeyValuePair<int, int>>;

public class MazeGenerator {

  public static void DrawMaze(List<Edge> edges, int width, int height) {
    Console.WriteLine("\\begin{tikzpicture}[>=latex]");
    Console.WriteLine("  \\draw[thick](0,0) -- ({0},0);",width); 
    Console.WriteLine("  \\draw[thick](0,{0}) -- ({1},{0});",width, height); 
    Console.WriteLine("  \\draw[thick](0,0) -- (0,{0});",height-1);     
    Console.WriteLine("  \\draw[thick]({0},1) -- ({0},{1});",width, height); 
    foreach(var edge in edges) {
      var cell1 = edge.Key;
      var cell2 = edge.Value;
      if (cell1.Value == cell2.Value)
        Console.WriteLine("  \\draw({0},{1}) -- ({2},{3});", cell2.Key, cell1.Value, cell2.Key, cell1.Value+1);
      else 
        Console.WriteLine("  \\draw({0},{1}) -- ({2},{3});", cell1.Key, cell2.Value, cell1.Key+1, cell2.Value);
    }
    Console.WriteLine("\\end{tikzpicture}");
  }

  public static void Main(string[] args) {
  
    int width, height;
    if (!Int32.TryParse(args[0], out width)) width = 7;
    if (!Int32.TryParse(args[0], out height)) height = 7;

    var UF = new UnionFind<Cell>(); 
    var grid = Enumerable.Range(0, width*height).Select( x => new Cell(x%width,x/width));
    foreach (var cell in grid) // works! 
      UF.MakeSet(cell);
 
   var edges = new List<Edge>();
    foreach(var cell in grid) {
      if (cell.Key < width-1) edges.Add(new Edge(cell, new Cell(cell.Key+1, cell.Value)));
      if (cell.Value < height-1) edges.Add(new Edge(cell, new Cell(cell.Key, cell.Value+1)));
    }

    var random = new Random();
    int removedEdges = 0; 
    while (removedEdges < grid.Count()-1) {
      int next = random.Next(edges.Count);
      var edge = edges[next];
      if (UF.Find(edge.Key) != UF.Find(edge.Value)) {
        UF.Union(UF.Find(edge.Key),UF.Find(edge.Value));
        edges.RemoveAt(next);
        removedEdges++;
      }
    }
    DrawMaze(edges, width, height); 
  }
}
