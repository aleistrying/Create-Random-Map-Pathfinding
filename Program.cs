using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateRandomMap
{
    class Node
    {   
        /// <summary>
        /// X position of node
        /// </summary>
        public int X;
        /// <summary>
        /// Y position of node
        /// </summary>
        public int Y;
        /// <summary>
        /// Character name Of node
        /// </summary>
        public char Character;
        /// <summary>
        /// Collision state of node
        /// </summary>
        public bool Collision;
        /// <summary>
        /// Name of node
        /// </summary>
        public string Name;
        public bool Checked;
        public int Count;
        public void SetPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public char GetCharacter()
        {
            char c = this.Character;
            return c;
        }
        public bool GetCollisionState()
        {
            bool b = this.Collision;
            return b;
        }
        public string GetName()
        {
            string name = this.Name;
            return name;
        }
        /// <summary>
        /// Node above this Node
        /// </summary>
        public Node NodeUp;
        /// <summary>
        /// Node Below this Node
        /// </summary>
        public Node NodeDown;
        /// <summary>
        /// Node to the left of this Node
        /// </summary>
        public Node NodeLeft;
        /// <summary>
        ///  Node to the right of this Node
        /// </summary>
        public Node NodeRight;
        
    }
    class Program
    {

        static int x = Console.WindowWidth / 4;
        static int y = Console.WindowHeight * 2 / 4;
        static int posX = 0;
        static int posY = 0;
        static char[,] grid = new char[x, y];
        static Node[] NodeArray = new Node[grid.Length];
        static Node[] NodePath = null;

        static void Main(string[] args)
        {
            Random r = new Random();
            posX = r.Next(x);
            posY = r.Next(y);
            Node prevNode = null;
            ConsoleKey key = ConsoleKey.Enter;
            bool wonTag = false;
            bool started = false;
            //Node initialNode = null;
            int prevIndex = 0;
            //int initialIndex = 0;
            Console.SetCursorPosition(posX, posY);
            Console.Write("O");
            Console.CursorVisible = false;
            CreateNodeMap();
            prevNode = NodeArray[0];
            FixExit();

            DisplayNodes(NodeArray);

            CheckNodesAround(NodeArray, posX, posY);
            while (true)
            {
                Console.SetCursorPosition(posX, posY);
                Console.Write("O");

                Console.WindowWidth = x + 1;
                Console.WindowHeight = y;
                Console.SetBufferSize(x + 1, y);
                //FIND EXIT
                if (AIFindPath(NodeArray[posX + posY * x]))
                {
                    foreach (Node n in NodePath)
                    {
                        Console.SetCursorPosition(n.X, n.Y);
                        Console.Write("O");
                        ///speeeddddd
                        System.Threading.Thread.Sleep(500);
                    }

                    Console.WriteLine("Path was shown, press enter to show end result");

                    Console.ReadLine();
                    Console.Clear();
                    Random ranWin = new Random();
                    int redo = 0;
                    while (redo < 7)
                    {
                        Console.WriteLine("");
                        for (int i = 0; i < x / 2 - 3; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.WriteLine("YOU WIN");
                        redo++;
                    }
                    System.Threading.Thread.Sleep(500);
                }

                if (!wonTag)
                {
                    key = Console.ReadKey().Key;
                }
                else if (wonTag)
                {
                    key = ConsoleKey.Enter;
                    wonTag = false;
                }

                if (key == ConsoleKey.Enter)
                {
                    posX = r.Next(x);
                    posY = r.Next(y);
                    if (Console.CursorTop != 0)
                    {
                        Console.CursorTop = Console.CursorTop - 1;
                    }

                    CreateNodeMap();
                    FixExit();
                    //fix where you start
                    CheckNodesAround(NodeArray, posX, posY);

                }
                // user input gameplay
                else if (key == ConsoleKey.RightArrow || key == ConsoleKey.LeftArrow || key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow)
                {
                    prevNode = NodeArray[posX + posY * x];
                    prevIndex = posX + posY * x;
                    CheckUserInput(key);

                }
                else if (key != ConsoleKey.Backspace)
                {
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    Console.Write(' ');
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                }
                else
                {
                    Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                }
                //AI Developement
                prevNode = NodeArray[posX + posY * x];
                prevIndex = posX + posY * x;

                //NodeArray[posX + posY * x].Character = 'O';
                if (!started)
                {
                    NodeArray[prevIndex] = prevNode;
                    started = true;
                }
                //person win
                if (prevNode.Character == 'E' && wonTag == false)
                {
                    wonTag = true;
                    Console.Clear();
                    Random ranWin = new Random();
                    int redo = 0;
                    while (redo < 7)
                    {
                        Console.WriteLine("");
                        for (int i = 0; i < x / 2 - 3; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.WriteLine("YOU WIN");
                        redo++;
                    }
                    System.Threading.Thread.Sleep(2000);
                    //break;
                }
                DisplayNodes(NodeArray);

                //prevNode = NodeArray[posX + posY * x];
                //prevIndex = posX + posY * x;


                Console.SetCursorPosition(posX, posY);
                Console.Write("O");
            }
        }
        static bool found = false;
        static bool countTag = false;
        static bool AIFindPath(Node Node, int n = 0)
        {
            Node.Count++;
            if (Node.NodeRight.Count != null)
            {
                Node.NodeRight.Count = Node.Count + 1;

            }
            if (Node.NodeLeft.Count != null)
            {
                Node.NodeLeft.Count = Node.Count + 1;

            }
            if(Node.NodeUp.Count != null)
            {
               
                Node.NodeUp.Count = Node.Count + 1;
            }
            if(Node.NodeDown.Count != null)
            {
                
                Node.NodeDown.Count  = Node.Count + 1;
            }


            /*
            if (Node.Character == 'E' || found == true)
            {
                if(found == false)
                {
                    NodePath = new Node[n];
                }
                found = true;
                NodePath[n] = Node;
                return found;
            }
            Node.Checked = true;

            if(Node.NodeRight != null && Node.NodeRight.Character != 'X' && Node.NodeRight.Checked == false && Node.NodeRight.Count != 0 && Node.NodeRight.Count - 1 == Node.Count)
            {
                //Node.NodeRight.Count++;
                AIFindPath(Node.NodeRight, n++);
                n--;
            }
                
            if (Node.NodeLeft != null && Node.NodeLeft.Character != 'X' && Node.NodeLeft.Checked == false && Node.NodeLeft.Count != 0 && Node.NodeLeft.Count - 1 == Node.Count)
            {
                //Node.NodeLeft.Count++;
                AIFindPath(Node.NodeLeft, n++);
                n--;
            }
                
            if (Node.NodeUp != null && Node.NodeUp.Character != 'X' && Node.NodeUp.Checked == false && Node.NodeUp.Count != 0 && Node.NodeUp.Count - 1 == Node.Count)
            {
                //Node.NodeUp.Count++;
                AIFindPath(Node.NodeUp, n++);
                n--;
            }
                
            if (Node.NodeDown != null && Node.NodeDown.Character != 'X' && Node.NodeDown.Checked == false && Node.NodeDown.Count != 0 && Node.NodeDown.Count -1 == Node.Count)
            {
                //Node.NodeDown.Count++;
                AIFindPath(Node.NodeDown, n++);
                n--;
            }
                
            return found;
            
            /*
            if ((Node.Character == 'E' && Node != null && Node.Collision == false && Node.Character != 'X' && Node.Checked == false)|| found == true )
            {
                if (found == false)
                {
                    NodePath = new Node[n +1];
                }
                found = true;
                
                NodePath[n] = Node;
                //Node.Checked = true;
                
                return true;
            }
            Node.Checked = true;
            if (Node.NodeRight != null && Node.NodeRight.Collision == false && Node.NodeRight.Character != 'X' && Node.NodeRight.Checked == false && found == false)
            {
                
                n++;
                AIFindPath(Node.NodeRight,n);
                if (found)
                {
                    n--;
                    AIFindPath(Node.NodeRight, n);
                    //add nodeleft something something.
                }
                else
                {
                    //Node.NodeRight.Checked = true;
                    n--;
                }
                Node.NodeRight.Checked = false;
            }
            if (Node.NodeLeft != null && Node.NodeLeft.Collision == false && Node.NodeLeft.Character != 'X' && Node.NodeLeft.Checked == false && found == false)
            {
                n++;
                AIFindPath(Node.NodeLeft,n);
                if (found)
                {
                    n--;
                    AIFindPath(Node.NodeLeft, n);
                }
                else
                {
                    //Node.NodeLeft.Checked = true;
                    n--;
                }
                Node.NodeLeft.Checked = false;
            }
            if (Node.NodeUp != null && Node.NodeUp.Collision == false && Node.NodeUp.Character != 'X' && Node.NodeUp.Checked == false && found == false)
            {
                n++;
                AIFindPath(Node.NodeUp,n);
                //Node.NodeUp.Checked = true;
                if (found)
                {
                    n--;
                    AIFindPath(Node.NodeUp, n);
                }
                else
                {
                    n--;
                }
                Node.NodeUp.Checked = false;
            }
            if (Node.NodeDown != null && Node.NodeDown.Collision == false && Node.NodeDown.Character != 'X' && Node.NodeDown.Checked == false && found == false)
            {
                n++;
                AIFindPath(Node.NodeDown,n);
                //Node.NodeDown.Checked = true;
                if (found)
                {
                    n--;
                    AIFindPath(Node.NodeDown, n);
                }
                {
                    n--;
                }
                Node.NodeDown.Checked = false;
            }
            if (found)
            {
                return true;
            }
            return false;
            */
            return false;
        }
        static void CreateNodeMap()
        {
            CreateMap();
            CreateNodes();
            ConnectNodes();

        }
        static void CreateNodes()
        {
            //int number = 0;
            for (int j = 0; j < y; j++)
            {
                for (int i = 0; i < x; i++)
                {
                    //number = number + 1;
                    Node n = new Node();
                    //posicion
                    n.SetPosition(i, j);
                    //nombre
                    n.Name = "Node " + (i + j * x + 1);
                    //tipo de collision y character
                    if (grid[i, j] == ' ')
                    {
                        n.Character = ' ';
                        n.Collision = false;
                    }
                    else if(grid[i, j] == 'X')
                    {
                        n.Character = grid[i, j];
                        n.Collision = true;
                    }
                    else if (grid[i, j] == 'E')
                    {
                        n.Character = grid[i, j];
                        n.Collision = false;
                    }
                    n.Checked = false;
                    //set new Node into the array
                    NodeArray[i + j * x] = n;
                    //test for nodes to left right and up.
                    

                }
            }
            
        }
        static void ConnectNodes()
        {
            for(int j  = 0; j < y; j++)
            {
                for(int i  = 0; i < x; i++)
                {
                    if ((i + 1) < x)
                    {
                        NodeArray[i + j * x].NodeRight = NodeArray[(i + 1) + j * x];
                    }
                    else
                    {
                        NodeArray[i + j * x].NodeRight = null;
                    }
                    if ((i - 1) >= 0)
                    {
                        NodeArray[i + j * x].NodeLeft = NodeArray[(i - 1) + j * x];
                    }
                    else
                    {
                        NodeArray[i + j * x].NodeLeft = null;
                    }
                    if ((j + 1) < y)
                    {
                        NodeArray[i + j * x].NodeDown = NodeArray[i + (j + 1) * x];
                    }
                    else
                    {
                        NodeArray[i + j * x].NodeDown = null;
                    }
                    if ((j - 1) >= 0)
                    {
                        NodeArray[i + j * x].NodeUp = NodeArray[i + (j - 1) * x];
                    }
                    else
                    {
                        // up because remember 80 means below, and 0 means above
                        NodeArray[i + j * x].NodeUp = null;
                    }
                }
            }
        }
        static void CreateMap()
        {
            char[] charArray = { ' ', 'X' };
            Random r = new Random();
            double randomIndex = 0;
            char Exit = 'E';
            for (int j = 0; j < y; j++)
            {
                for (int i = 0; i < x; i++)
                {
                    randomIndex = r.Next(5, 71) / (double)100;
                    randomIndex = Math.Round(randomIndex);

                    grid[i, j] = charArray[(int)randomIndex];
                }
            }
            grid[r.Next(0, x), r.Next(0, y)] = Exit;
        }
        static void DisplayNodes(Node[] nodes)
        {
            
            for (int j = 0; j < y; j++)
            {
                for (int i = 0; i < x; i++)
                {
                    Console.SetCursorPosition(i,j);
                    Console.Write(nodes[i + j * x].Character);
                }
            }

        }
        static void CheckUserInput(ConsoleKey keyPressed)
        {
            try
            {
                if (keyPressed == ConsoleKey.RightArrow && NodeArray[(posX + 1) + posY * x].Character != 'X')
                {
                    posX++;
                }
                else if (keyPressed == ConsoleKey.LeftArrow && NodeArray[(posX - 1) + posY * x].Character != 'X')
                {
                    posX--;
                }
                else if (keyPressed == ConsoleKey.UpArrow && NodeArray[posX + (posY - 1) * x].Character != 'X')
                {
                    posY--;
                }
                else if (keyPressed == ConsoleKey.DownArrow && NodeArray[posX + (posY + 1) * x].Character != 'X')
                {
                    posY++;
                }
            }
            catch { }
             if (posY >= y)
            {
                posY--;
            }
            else if (posY < 0)
            {
                posY++;
            }
            else if (posX >= x)
            {
                posX--;
            }
            else if (posX < 0)
            {
                posX++;
            }
        }
        static void FixExit()
        {
            bool breakNow = false;
            int index;
            for (int j = 0; j < y; j++)
            {
                for (int i = 0; i < x; i++)
                {
                    index = i + j * x;
                    if(NodeArray[index].Character == 'E')
                    {
                        CheckNodesAround(NodeArray,i, j);
                        breakNow = true;
                        break;
                    }
                }
                if (breakNow)
                {
                    break;
                }
            }
        }
        static void CheckNodesAround(Node[] nArray, int xNode, int yNode)
        {
            int count = 0;
            //topleft
            if ((xNode - 1) >= 0 && (yNode - 1) >= 0 && nArray[((xNode - 1) + (yNode - 1) * x)].Character == 'X')
            {
                count++;
            }
            //top
            if ((yNode - 1) >= 0 && nArray[((xNode) + (yNode - 1) * x)].Character == 'X')
            {
                count++;
            }
            //topright
            if ((xNode + 1) < x && (yNode - 1) >= 0 && nArray[((xNode + 1) + (yNode - 1) * x)].Character == 'X')
            {
                count++;
            }
            //left
            if ((xNode - 1) >= 0 && nArray[((xNode - 1) + (yNode) * x)].Character == 'X')
            {
                count++;
            }
            //right
            if ((xNode + 1) < x && nArray[((xNode + 1) + (yNode) * x)].Character == 'X')
            {
                count++;
            }
            //bottom left
            if ((xNode - 1) >= 0 && (yNode + 1) < y && nArray[((xNode - 1) + (yNode + 1) * x)].Character == 'X')
            {
                count++;
            }
            //bottom
            if ((yNode + 1) < y && nArray[((xNode) + (yNode + 1) * x)].Character == 'X')
            {
                count++;
            }
            //bottom right
            if ((xNode + 1) < x && (yNode + 1) < y && nArray[((xNode + 1) + (yNode + 1) * x)].Character == 'X')
            {
                count++;
            }
            Random r = new Random();
            while (count >= 4)
            {
                int xRan = r.Next(-1, 2);
                int yRan = r.Next(-1, 2);
                if (yRan == -1 && yNode == 0)
                {
                    yRan++;
                }
                if (xRan == -1 && xNode == 0)
                {
                    xRan++;
                }
                if (yRan == 1 && yNode >= 14)
                {
                    yRan--;
                }
                //if (xRan == 1 && xNode == 30)
                //{
                //    xRan--;
                //}
                int newxNode = xNode + xRan;
                int newyNode = yNode + yRan;
                
                if ((xRan != 0 || yRan != 0) && (nArray[((newxNode) + (newyNode) * x)] != null && (nArray[((newxNode) + (newyNode) * x)].Character != 'O' && nArray[((newxNode) + (newyNode) * x)].Character != ' ')))
                {
                    nArray[((newxNode) + (newyNode) * x)].Character = ' ';
                    count--;
                }
            }
        }
    }
}
