# Route Finder & Visualization Tool

## Overview

This C# WinForms application implements a shortest path-finding algorithm over a weighted graph that represents a map. It supports loading various map datasets, running multiple shortest path queries between coordinates with walking distance constraints, and visualizing the resulting routes through an interactive graphical interface.

---

## Features

- **Graph Data Structure**  
  Uses arrays and adjacency lists to represent vertices (nodes) and edges of a weighted, undirected graph.

- **Shortest Path Algorithm**  
  Implements Dijkstra's algorithm with a priority queue for efficient shortest path computation.

- **Multi-Query Support**  
  Handles multiple route queries with start and end coordinates, while honoring walking distance constraints.

- **Distance Filtering**  
  Narrows candidate vertices to those within a maximum walking distance from the given coordinates, reducing search space.

- **Graph Visualization**  
  Renders the map graph and highlights computed shortest paths using Windows Forms with distinct colors.

- **Performance Monitoring**  
  Measures and displays the time taken to load maps and compute queries using `Stopwatch`.

- **User Interaction**  
  Console-based menu for selecting datasets, entering queries, and choosing visualization modes.

---

## Data Structures

- **`vertex`**  
  Represents a graph node with ID and 2D coordinates (x, y).

- **`node`**  
  Represents an edge's neighbor, storing ID, weight, distance, and metadata for path computation.

- **`inp`**  
  Stores query inputs including start/end coordinates and the walking distance radius.

---

## Algorithmic Details

### Dijkstra's Algorithm
- **Time Complexity:** O(E log V)  
- Uses `PriorityQueue<int, double>` to process nodes by minimum cost.
- Operates on filtered vertices near the query start and end to reduce computation.

### Candidate Filtering
- Identifies candidate nodes within a user-defined radius using squared Euclidean distance.
- Optimizes performance by avoiding unnecessary computation over distant nodes.

---

## File Structure & Input

- **Map Files (e.g., `map1.txt`, `NAMap.txt`)**  
  Contain:
  - Vertex definitions: `id x y`
  - Edge definitions: `vertex1_id vertex2_id length speed`

- **Query Files (e.g., `queries1.txt`, `NAQueries.txt`)**  
  Contain:
  - Queries with: `start_x start_y end_x end_y max_walking_distance`

---

## Usage Instructions

1. Run the application.
2. Choose a map dataset by selecting a number in the console menu:
   - Options 1–10: Load predefined map and query files.
   - Option 11: Enter custom query manually:
     ```
     start_x start_y end_x end_y max_walking_distance_meters
     ```
3. The program will:
   - Load the selected map.
   - Process shortest path queries with walking constraints.
   - Output results to both the console and a visual window.

### Output

- **Text Output:**
  - Shortest path node IDs written to `Temp.txt`
  - Displays total distance, walking distance, and estimated travel time

- **Graphical Output:**
  - Map graph displayed using Windows Forms
  - Shortest paths highlighted in distinct colors for clarity

---

## Dependencies

- **.NET Framework** (compatible with WinForms and PriorityQueue)
- **Namespaces Used:**
  - `System.Windows.Forms`
  - `System.Drawing`
  - `System.Collections.Generic`
  - `System.Diagnostics`
  - `System.Runtime.InteropServices`

---

## Code Highlights

- Efficient graph storage using adjacency lists
- Walking radius filtering with Euclidean distance squared optimization
- Interactive and user-friendly console menu
- Real-time rendering of map and path with scaling to fit window size
- Clean use of `Graphics` and `PictureBox` for rendering

---

## Disclaimer

This project was built to simulate and visualize real-world routing using fundamental algorithms and Windows Forms. It serves as a demonstration of pathfinding, spatial filtering, and simple graphical rendering within a C# WinForms environment.
