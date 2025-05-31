Route Finder & Visualization Tool
Overview
This C# WinForms application implements a shortest path finding algorithm over a weighted graph representing a map. It supports loading various map datasets, running shortest path queries between locations considering walking distance constraints, and visualizing the resulting routes on a graphical interface.

Features
Graph Data Structure: Uses arrays and adjacency lists to represent vertices (nodes) and edges of a weighted undirected graph.

Shortest Path Algorithm: Implements Dijkstra's algorithm with a priority queue for efficient shortest path calculation.

Multi-Query Support: Handles multiple route queries with start and end coordinates, including walking distance constraints.

Distance Filtering: Finds candidate vertices near query points within a maximum walking distance.

Graph Visualization: Draws the entire graph and highlights shortest paths with distinct colors using Windows Forms.

Performance Monitoring: Measures time taken for loading data and running queries using Stopwatch.

User Interaction: Console menu for selecting map files, entering queries, and choosing visualization options.

Data Structures
vertex: Represents a graph node with ID and coordinates (x, y).

node: Represents an edge's neighbor with an ID, weight, distance, and tracking fields for shortest path computation.

inp: Stores query input with start/end coordinates and a maximum walking distance radius.

Algorithmic Details
Dijkstra's Algorithm:

Time Complexity: O(E log V) where E is edges, V is vertices.

Uses a min-priority queue implemented via PriorityQueue<int, double>.

Searches shortest path among candidate vertices near start/end query points.

Candidate Filtering:

Finds vertices within a given radius around query start/end points for realistic walking constraints.

Reduces search space to nearby nodes, improving efficiency.

File Structure & Input
Map Files (map1.txt ... NAMap.txt):

Contain vertex data (ID, x, y) followed by edges (two vertex IDs, length, speed).

Query Files (queries1.txt ... NAQueries.txt):

Contain queries with start and end coordinates and walking distance constraints.

Usage Instructions
Run the application.

Choose a map dataset by entering a number from the console menu.

For options 1-10, queries are read automatically from predefined files.

For option 11, enter custom query details (start_x start_y end_x end_y max_walking_distance_meters).

The application computes shortest paths considering walking distance, travel time, and visualizes the results.

Output:

Shortest path nodes printed to Temp.txt.

Travel time, total distance, walking distance printed alongside paths.

Visualization window displays the map graph and highlights computed paths with different colors.

Dependencies
.NET Framework (version compatible with WinForms and PriorityQueue)

Uses standard libraries:

System.Windows.Forms

System.Drawing

System.Collections.Generic

System.Diagnostics

System.Runtime.InteropServices (for console allocation)

Code Highlights
Efficient adjacency list storage for edges.

Custom vertex filtering using Euclidean distance squared for optimization.

Interactive console menu with input validation.

Graphical rendering scales points to window size and draws paths distinctly.

Uses Windows Forms PictureBox and Graphics for smooth drawing.

