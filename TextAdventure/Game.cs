using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace TextAdventure
{
    public class Game
    {
        public Player Player;
        public List<Room> Rooms = new List<Room>();
        //public Enemy E1;
        //public Enemy E2;

        public static List<Enemy> enemies = new List<Enemy>();

        public static List<Enemy> _enemies = new List<Enemy>();


        public void BuildGame()
        {
            DataTable _data = GetTable();

            //Create rooms
            foreach (DataRow _row in _data.Rows)
            {
                string _name = _row["Name"].ToString();
                Rooms.Add(new Room() { Name = _name });
            }

            //Create doors
            int ir = 0;
            foreach (DataRow _row in _data.Rows) // reihen durchgehen 
            {
                // all rows
                string _direction = "";
                string _nextroom = "";
                Room _neighbour;

                // all directions n/s/e/w
                for (int i = 0; i < 4; i++)
                {
                    //get next room
                    switch (i)
                    {
                        case 0:
                            _direction = "North";
                            break;
                        case 1:
                            _direction = "South";
                            break;
                        case 2:
                            _direction = "East";
                            break;
                        case 3:
                            _direction = "West";
                            break;
                    }
                    _nextroom = _row[_direction].ToString();

                    if (_nextroom != "")
                    {
                        //search neighbour room in list
                        _neighbour = null;
                        for (int r = 0; r < Rooms.Count; r++)
                        {
                            if (Rooms[r].Name == _nextroom)
                            {
                                _neighbour = Rooms[r];
                            }
                        }

                        //set neighbours
                        switch (_direction)
                        {
                            case "North":
                                Rooms[ir].North = _neighbour;
                                break;
                            case "South":
                                Rooms[ir].South = _neighbour;
                                break;
                            case "East":
                                Rooms[ir].East = _neighbour;
                                break;
                            case "West":
                                Rooms[ir].West = _neighbour;
                                break;
                        }
                    }
                }
                //define Items in Rooms
                string _item1 = _row["Item1"].ToString();
                if (_item1 != "")
                {
                    Rooms[ir].RoomItems.Add(new Item { Name = _item1 });
                }
                string item2 = _row["Item2"].ToString();
                if (item2 != "")
                {
                    Rooms[ir].RoomItems.Add(new Item { Name = item2 });
                }
                ir++;
            }

            //Set room items
            Player = new Player("Spieler", Rooms[RandomRoom()]);


            var data = File.ReadAllText("enemies.json");

            enemies = JsonConvert.DeserializeObject<List<Enemy>>(data); // !!!! hier kann er enemies nicht als typ Enemy im Constructor erzeugen


            foreach (Enemy enemy in enemies)
            {
                Enemy _enemy = new Enemy(enemy.Name, Rooms[RandomRoom()]);

                foreach (Item item in enemy.PlayerItems)
                {
                    _enemy.PlayerItems.Add(new Item { Name = item.Name });
                }
                _enemies.Add( _enemy );
            }




            /*  E2 = new Enemy("Casino", Rooms[RandomRoom()]);
             E2.PlayerItems.Add(new Item { Name = "Brille" });
             E2.PlayerItems.Add(new Item { Name = "Handy" }); */

        }

        int RandomRoom()
        {
            Random _randomNumber = new Random();
            int _random = _randomNumber.Next(0, Rooms.Count);
            return _random;
        }

        int RandomMove()
        {
            Random _randomNumber = new Random();
            int _random = _randomNumber.Next(0, 4);
            return _random;
        }
        static DataTable GetTable()
        {
            //DataTable _table = new DataTable();

            //Define columns
            /*_table.Columns.Add("Name", typeof(string));
            _table.Columns.Add("North", typeof(string));
            _table.Columns.Add("South", typeof(string));
            _table.Columns.Add("East", typeof(string));
            _table.Columns.Add("West", typeof(string));
            _table.Columns.Add("Item1", typeof(string));
            _table.Columns.Add("Item2", typeof(string));


            //Define rows
            _table.Rows.Add("Deutschland", "","Italien","Russland","Frankreich","Bier","Brezel");
            _table.Rows.Add("Frankreich", "","Spanien","Deutschland","","Baguette","Wein");
            _table.Rows.Add("Russland", "","","","Deutschland","Wodka","Matrjoschka");
            _table.Rows.Add("Italien", "Deutschland","","","Spanien","Pizza","Espresso");
            _table.Rows.Add("Spanien", "Frankreich","","Italien","","Cerveza","Tapas");*/

            using (StreamReader r = new StreamReader("rooms.json"))
            {
                string json = r.ReadToEnd();
                DataTable _table = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));

                return _table;


            }

        }

        public void Play()
        {
            string _input = "";
            while (_input != "q")
            {
                if (_input == "n" || _input == "s" || _input == "e" || _input == "w")
                {
                    int _x = RandomRoom();
                    switch (_x)
                    {
                        case 0:

                            foreach (Enemy enemy in _enemies)
                            {
                                enemy.Move("n");
                            }
                            break;
                        case 1:
                            foreach (Enemy enemy in _enemies)
                            {
                                enemy.Move("n");
                            }
                            break;
                        case 2:
                            foreach (Enemy enemy in _enemies)
                            {
                                enemy.Move("n");
                            }
                            break;
                        case 3:
                            foreach (Enemy enemy in _enemies)
                            {
                                enemy.Move("n");
                            }
                            break;
                    }
                }

                Show(Player);
                _input = Console.ReadLine();

                try
                {
                    if (_input != "q")
                    {
                        switch (_input)
                        {
                            case "q":
                                break;
                            case "l":
                                Show(Player);
                                break;
                            case "i":
                                Player.Inventory();
                                break;
                            case "c":
                                Commands();
                                break;
                            case "d":
                                Drop(Player);
                                break;
                            case "t":
                                Take(Player);
                                break;
                            case "n":
                                Player.Move(_input);
                                break;
                            case "e":
                                Player.Move(_input);
                                break;
                            case "s":
                                Player.Move(_input);
                                break;
                            case "w":
                                Player.Move(_input);
                                break;
                            case "a":
                                Player.Attack();
                                break;
                            default:
                                Console.WriteLine("Falsche Eingabe");
                                break;
                        }
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Falsche Eingabe");
                }
                if (Player.Total <= 0)
                {
                    Console.WriteLine("Sie haben verloren");
                    break;
                }
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.Total <= 0 && enemy.Total <= 0)
                    {
                        Console.WriteLine("Sie haben gewonnen");
                        break;
                    }
                }

            }

            void Show(Player p)
            {
                Enemy _enemy = p.Position.GetEnemy();   // checking if enemy in room
                Console.WriteLine("");
                Console.WriteLine("Sie befinden sich in " + p.Position.Name + " und Ihr Kontostand beträgt " + p.Total + " Euro. Geben Sie Norden(n), Osten(e), Westen(w) oder Süden(s) ein um sich zu bewegen.");
                p.Position.Look();
                if (_enemy != null)
                {
                    Console.WriteLine("Sie treffen auf " + _enemy.Name + " der Items hat die Sie eventuell gebrauchen könnten. Dein Gegner hat ein Kontostand von " + _enemy.Total + " Euro. Wollen Sie gegen ihn mit <a> spielen um ein Item von ihm zu gewinnen? Seine Items:");
                    if (p.Position.GetEnemy().PlayerItems.Count != 0)
                    {
                        for (int i = 0; i < p.Position.GetEnemy().PlayerItems.Count; i++)
                        {
                            Console.WriteLine(p.Position.GetEnemy().PlayerItems[i].Name);
                        }
                    }
                }
            }

            void Take(Player p)
            {
                Console.WriteLine("Welches Item wollen Sie?");
                string _item = Console.ReadLine();
                Item _takeItem = p.Position.Take(_item);
                p.Insert(_takeItem);
            }

            void Drop(Player p)
            {
                Console.WriteLine("Welches Item wollen Sie ablegen?");
                string _item = Console.ReadLine();
                Item _dropItem = p.Delete(_item);
                p.Position.Drop(_dropItem);
            }

            void Commands()
            {
                Console.WriteLine("commands(c), look(l), inventory(i), take(t) item, drop(d) item, quit(q)");
            }
        }
    }
}