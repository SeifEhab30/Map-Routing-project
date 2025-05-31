using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Drawing;
using System.Windows.Forms;
using static WinFormsApp1.Program;
using System.Windows.Forms.VisualStyles;
using System.Runtime.InteropServices;
namespace WinFormsApp1
{//    /*
 //     * 
 //     * 
 //     * used data structure are arrays, List<type> (in c# is a dynamic array) and priority queue 
 //     * inializing arrays is O(n) to write zeros or set to null but newer versions of .net apply bulk zeroing which make it more faster , accessing array is big-theta of (1)
 //     * creating list is O(1) , accesing in O(1) , adding in most cases when there is a space in dynamic array O(1) , in some cases when there is no space it become O(n) but in average ~O(1)
 //     * priority queue uses min heap so enqueue/ dequeue are equivlent to add remove to heap in O(log n)
 //     * 
 //     * 
 //     * */
    internal static class Program
    {
        static double length = 0;
        static StreamReader rm;
        static StreamReader rq;
        static double min_x = double.MaxValue, min_y = double.MaxValue, max_x = 0, max_y = 0;
        static Stopwatch SW = new Stopwatch();
        static List<List<int>> paths = new List<List<int>>();
        public struct vertex
        {
            public int id;
            public double x;
            public double y;
            public vertex(int id1, double xx, double yy)
            {
                id = id1;
                x = xx;
                y = yy;
            }
        }
        public struct vertex1
        {

            public double x;
            public double y;
            public vertex1(double xx, double yy)
            {

                x = xx;
                y = yy;
            }
        }

        public struct inp// query
        {
            public vertex1 start;
            public vertex1 end;
            public double r;
            public inp(vertex1 ss, vertex1 enn, double wd)
            {
                start = ss;
                end = enn;
                r = wd;
            }
        }
        public struct node
        {
            public int id;
            public int Pid;
            public double weight;
            public double distance;
            public bool done;
            public double neighdist;
            public node(int i, double w, double dis)
            {
                id = i;
                Pid = -1;
                weight = w;
                distance = dis;
                done = false;
            }
        }

        static inp[] input;//queries array
        static vertex[] v;//vertex array
        static List<node>[] map;//map of edges
        static List<int>[,] vert_map;
        static void readm(StreamReader r) // big theta (V+E)  (vertex count+edge count)
        {

            double x, y;              
            int id;                  
            string line;             

            line = r.ReadLine();    
            int n = Convert.ToInt32(line); 

            v = new vertex[n];       // O(V) - array of refrences ->therotacly O(V) but after search we found that newer versions of .NET can do bulk zeroing to initialize 
            map = new List<node>[n];  

            for (int i = 0; i < n; i++) // O(V) -big theta (V).
            {
                line = r.ReadLine();  
                string[] t = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries); 

                SW.Start();           
                x = Convert.ToDouble(t[1]);  
                y = Convert.ToDouble(t[2]); 
                id = Convert.ToInt32(t[0]); 
                v[id] = new vertex(id, x, y); 
                map[i] = new List<node>(); 
                if (x > max_x)        
                    max_x = x;       
                if (x < min_x)       
                    min_x = x;      
                if (y > max_y)       
                    max_y = y;       
                if (y < min_y)        
                    min_y = y;       
                SW.Stop();

            }

            int m = Convert.ToInt32(r.ReadLine());

            for (int i = 0; i < m; i++) // O(E) - big theta(E)
            {
                line = r.ReadLine(); 
                string[] t = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries); 

                SW.Start();          
                double len = Convert.ToDouble(t[2]); 
                double weight = len / Convert.ToDouble(t[3]); 
                int id1 = Convert.ToInt32(t[0]); 
                int id2 = Convert.ToInt32(t[1]); 
                node p1 = new node(id1, weight, len);  
                node p2 = new node(id2, weight, len);  
                map[id1].Add(p2);     
                map[id2].Add(p1);     
                SW.Stop();            

            }

        }
        static void readq(StreamReader r) //O(Q) big theta(Number of queries)
        {
            string line; 
            line = r.ReadLine(); 
            int n = Convert.ToInt32(line); 
            input = new inp[n];            
            for (int i = 0; i < n; i++)     //O(Q) - number of queries
            {
                line = r.ReadLine(); 
                string[] t = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries); 

                vertex1 st = new vertex1(Convert.ToDouble(t[0]), Convert.ToDouble(t[1]));   
                vertex1 en = new vertex1(Convert.ToDouble(t[2]), Convert.ToDouble(t[3]));   
                inp x = new inp(st, en, Convert.ToDouble(t[4]) / 1000);                     
                input[i] = x;                                                       

            }
        }

        public static List<vertex> GetPointsWithinDistance(vertex1 source, vertex[] points, double maxDistance)  //O(V) big theta (V)
        {
            List<vertex> result = new List<vertex>(); 
            double maxsqrsdist = (maxDistance * maxDistance);
            foreach (var point in points) //O(V) - big theta (V)
            {
                double xd = (point.x - source.x); 
                double yd = (point.y - source.y); 
                double distance = xd * xd + yd * yd; 

                if (distance <= maxsqrsdist)
                {
                    result.Add(point);//~O(1)
                }

            }
            return result;
        }



        public static (node[], bool) dij2(vertex start, List<vertex> dest) //O(v + E log v) and since in complete graph E is greater than V, then E*log(V) is extreamly bigger than V so O(E log V) is the final time complexity
        {

            int done = 0; 
            node[] res = new node[v.Length];//O(V)

            var pq = new PriorityQueue<int, double>();
            List<int> destId = new List<int>();
            for (int i = 0; i < dest.Count; i++) //O(F)
            {
                destId.Add(dest[i].id); 
            }
            for (int i = 0; i < v.Length; i++) //O(V)
            {
                res[i].id = v[i].id; 
                res[i].weight = double.MaxValue; 
                res[i].Pid = -1; 
                res[i].done = false; 
            }

            res[start.id].weight = 0; 
            res[start.id].neighdist = 0;
            pq.Enqueue(start.id, 0); //O(log V)


            while (pq.Count != 0 && done != dest.Count) //O(V)* O(adj(V) * O(log V)  each vertex is visited one time and check all its adjacent vertexs so V* adj(V) -> O(E), so final complexity is O(E* Log(V))
            {
                int min = pq.Dequeue(); //O(log V)
                if (res[min].done) continue; 

                res[min].done = true; 

                if (destId.Contains(min)) //O(F)
                {
                    done++;

                }

                foreach (node neighbor in map[min])//O( adj(min)) * O(log(v) )
                {

                    double newWeight = res[min].weight + neighbor.weight; 

                    if (!res[neighbor.id].done && newWeight < res[neighbor.id].weight) 
                    {
                        res[neighbor.id].weight = newWeight; 
                        res[neighbor.id].Pid = min; 
                        res[neighbor.id].neighdist = neighbor.distance; 
                        pq.Enqueue(neighbor.id, newWeight); 
                    }
                }
            }

            return (res, true);
        }

        public static List<int> GetPath(int endId, node[] res, bool tt)
        {
            length = 0;
            List<int> path = new List<int>();

            int current = endId;
            while (current != -1)
            {

                path.Add(current);

                length += res[current].neighdist;
                current = res[current].Pid;

            }

            if (tt)
                path.Reverse();
            return path;
        }
        public static double GetDistance(vertex1 x, vertex y)
        {
            return Math.Sqrt(Math.Pow(x.x - y.x, 2) + Math.Pow(x.y - y.y, 2));
        }
        public static void GraphDisplayForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int vertexRadius = 10;
            Font vertexLabelFont = new Font("Arial", 8);

            for (int i = 0; i < v.Length; i++)
            {
                List<node> edges = map[i];
                foreach (var edge in edges)
                {
                    int v1 = i;
                    int v2 = edge.id;

                    if (v1 != null && v2 != null)
                    {
                        g.DrawLine(Pens.Black, Convert.ToInt32((v[v1].x - min_x) * 100 / (max_x - min_x)), Convert.ToInt32((v[v1].y - min_y) * 100 / (max_y - min_y)), Convert.ToInt32((v[v2].x - min_x) * 100 / (max_x - min_x)), Convert.ToInt32((v[v2].y - min_y) * 100 / (max_y - min_y)));
                        
                    }
                }
            }


        }
        public static void VisualizeGraph(Form form)
        {
            // Create a new form for visualization
            Form graphForm = new Form();
            graphForm.Text = "Graph Visualization";
            graphForm.Width = 1000;
            graphForm.Height = 1000;

            // Create a PictureBox for drawing
            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;

            // Define colors for paths (supports up to 7 different paths)
            Color[] pathColors = new Color[] {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Purple,
            Color.Orange,
            Color.Magenta,
            Color.Cyan
            };

            pictureBox.Paint += (sender, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


                double scaleX = (pictureBox.Width) / (max_x - min_x);
                double scaleY = (pictureBox.Height) / (max_y - min_y);
                double scale = Math.Min(scaleX, scaleY);

                Point ToScreen(double x, double y) => new Point(
                    (int)((x - min_x) * scale),
                    (int)((max_y - y) * scale)
                );

                Pen defaultEdgePen = new Pen(Color.LightGray, 1);
                foreach (var vertex in v)
                {
                    Point source = ToScreen(vertex.x, vertex.y);

                    foreach (var edge in map[vertex.id])
                    {
                        var targetVertex = v[edge.id];
                        Point target = ToScreen(targetVertex.x, targetVertex.y);
                        g.DrawLine(defaultEdgePen, source, target);
                    }
                }

                // Highlight paths if they exist
                if (paths != null && paths.Count > 0)
                {
                    for (int i = 0; i < paths.Count; i++)
                    {

                        Color pathColor = pathColors[i % pathColors.Length];
                        Pen pathPen = new Pen(pathColor, 1);

                        // Draw the path
                        for (int j = 0; j < paths[i].Count - 1; j++)
                        {
                            int fromId = paths[i][j];
                            int toId = paths[i][j + 1];

                            Point from = ToScreen(v[fromId].x, v[fromId].y);
                            Point to = ToScreen(v[toId].x, v[toId].y);

                            g.DrawLine(pathPen, from, to);
                        }


                    }
                }
            };

            graphForm.Controls.Add(pictureBox);
            graphForm.ShowDialog();
        }
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        [STAThread]
        static void Main()
        {



            string[] map =
{
                "",
                "map1.txt",
                "map2.txt",
                "map3.txt",
                "map4.txt",
                "map5.txt",
                "map6.txt",
                "OLMap.txt",
                "TGMap.txt",
                "SFMap.txt",
                "NAMap.txt",
            };
            string[] query =
            {
                "",
                "queries1.txt",
                "queries2.txt",
                "queries3.txt",
                "queries4.txt",
                "queries5.txt",
                "queries6.txt",
                "OLQueries.txt",
                "TGQueries.txt",
                "SFQueries.txt",
                "NAQueries.txt",
            };
            AllocConsole();
            Console.WriteLine("choose the map you want to use:\n1-6:sample cases.\n7-8:Medium case(OLMap).\n9-10:-Large case(SFMap).\n11-visualize a path from a chosen map.");
            int ans = Convert.ToInt32(Console.ReadLine());
            while (ans < 1 || ans > 11)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 11.");
                ans = Convert.ToInt32(Console.ReadLine());
            }
            var totalStopwatch = System.Diagnostics.Stopwatch.StartNew();
            if (ans >= 1 && ans <= 10)
            {
                rq = new StreamReader(query[ans]);
                readq(rq);
                rm = new StreamReader(map[ans]);
            }
            else if (ans == 11)
            {
                Console.WriteLine("choose the map you want to use:\n1-6:sample cases.\n7-8:Medium case(OLMap/TGMap).\n9-10:Large case(SFMap/NAMap).");
                int ans1 = Convert.ToInt32(Console.ReadLine());
                while (ans1 < 1 || ans1 > 10)
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 10.");
                    ans1 = Convert.ToInt32(Console.ReadLine());
                }
                rm = new StreamReader(map[ans1]);
                input = new inp[1];
                Console.WriteLine("Enter the details (source x source y  destination x destination y   maximum walking distance in meters): ");
                string[] startInput = Console.ReadLine().Split(' ');
                vertex1 st = new vertex1(Convert.ToDouble(startInput[0]), Convert.ToDouble(startInput[1]));
                vertex1 en = new vertex1(Convert.ToDouble(startInput[2]), Convert.ToDouble(startInput[3]));
                inp x = new inp(st, en, Convert.ToDouble(startInput[4]) / 1000);
                input[0] = x;
            }
            readm(rm);
            StreamWriter r1 = new StreamWriter("Temp.txt");
            var queryStopwatch = System.Diagnostics.Stopwatch.StartNew();


            vertex minstart = v[0], minend = v[0];
            foreach (var q in input)
            {
                int n = 0, m = 0;
                bool st = true;
                List<vertex> startCandidates = GetPointsWithinDistance(q.start, v, q.r);
                List<vertex> endCandidates = GetPointsWithinDistance(q.end, v, q.r);
                node[] minpath = { };

                if (startCandidates.Count > 0 && endCandidates.Count > 0)
                {
                    double minDis = double.MaxValue;
                    if (startCandidates.Count <= endCandidates.Count)
                    {
                        n = startCandidates.Count;
                        m = endCandidates.Count;
                        st = true;
                    }
                    else
                    {
                        m = startCandidates.Count;
                        n = endCandidates.Count;
                        st = false;
                    }
                    for (int i = 0; i < n; i++) // O(S)*O(E log V)=>O(S*E*log V)
                    {
                        vertex vi;
                        double dis;
                        node[] result;
                        bool success;
                        if (st)
                        {
                            vi = startCandidates[i];
                            dis = GetDistance(q.start, vi);
                            (result, success) = dij2(vi, endCandidates);//O(E log V)
                        }
                        else
                        {
                            vi = endCandidates[i];
                            dis = GetDistance(q.end, vi);
                            (result, success) = dij2(vi, startCandidates); //O(E log V)
                        }
                        if (!success)
                            continue;
                        for (int j = 0; j < m; j++)
                        {
                            vertex vj;
                            double totaldis;
                            if (st)
                            {
                                vj = endCandidates[j];
                                totaldis = (dis + GetDistance(q.end, vj)) / 5 + result[vj.id].weight;
                                if (totaldis < minDis)
                                {
                                    minDis = totaldis;
                                    minstart = vi;
                                    minend = vj;
                                    minpath = result;
                                }
                            }
                            else
                            {
                                vj = startCandidates[j];
                                totaldis = (dis + GetDistance(q.start, vj)) / 5 + result[vj.id].weight;
                                if (totaldis < minDis)
                                {
                                    minDis = totaldis;
                                    minstart = vj;
                                    minend = vi;
                                    minpath = result;
                                }
                            }
                        }


                    }
                }

                List<int> path;
                double walkingDis = (GetDistance(q.start, minstart) + GetDistance(q.end, minend));
                double walkingTime = walkingDis * 60.0 / 5.0;
                double totalTime;
                if (st)
                {
                    path = GetPath(minend.id, minpath, st);
                    totalTime = minpath[minend.id].weight * 60 + walkingTime;
                }
                else
                {
                    path = GetPath(minstart.id, minpath, st);
                    totalTime = minpath[minstart.id].weight * 60 + walkingTime;
                }
                double totaldistence = length + walkingDis;
                r1.WriteLine(string.Join(" ", path));
                paths.Add(path);
                queryStopwatch.Stop();
                r1.WriteLine($"{totalTime:F2} mins");
                r1.WriteLine($"{totaldistence:F2} km");
                r1.WriteLine($"{walkingDis:F2} km");
                r1.WriteLine($"{length:F2} km\n");
                queryStopwatch.Start();
                


            }
            queryStopwatch.Stop();
            double queryTime = queryStopwatch.Elapsed.TotalMilliseconds + SW.Elapsed.TotalMilliseconds;
            Console.WriteLine($"{queryTime:F0} ms\n");
            r1.WriteLine($"{queryTime:F0} ms\n");


            totalStopwatch.Stop();
            Console.WriteLine($"{totalStopwatch.Elapsed.TotalMilliseconds:F0} ms");
            r1.WriteLine($"{totalStopwatch.Elapsed.TotalMilliseconds:F0} ms");
            r1.Flush();
            r1.Close();
            //to visualize all queries in a case comment out this if condition
            if (ans == 11)
                VisualizeGraph(new Form());
            Console.ReadKey();
            //to view your output visit the file just click on the bin->Debug->net8.0-windows and find the Temp.txt file
            // or edit the path in stream writer r1 to your desired location
        }
    }
}


