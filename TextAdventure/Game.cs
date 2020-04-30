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
        public List<Enemy> Enemies = new List<Enemy>();


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
                    Rooms[ir].Items.Add(new Item { Name = _item1 });
                }
                string _item2 = _row["Item2"].ToString();
                if (_item2 != "")
                {
                    Rooms[ir].Items.Add(new Item { Name = _item2 });
                }
                ir++;
            }

            Player = new Player("Spieler", Rooms[RandomRoom()]);
            List<Enemy> enemieshelp = new List<Enemy>();
            var _dataEnemies = File.ReadAllText("enemies.json");
            enemieshelp = JsonConvert.DeserializeObject<List<Enemy>>(_dataEnemies);

            foreach (Enemy enemy in enemieshelp)
            {
                Enemy _enemy = new Enemy(enemy.Name, Rooms[RandomRoom()]);

                foreach (Item item in enemy.Items)
                {
                    _enemy.Items.Add(new Item { Name = item.Name });
                }
                Enemies.Add(_enemy);
            }
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
            using (StreamReader r = new StreamReader("rooms.json"))
            {
                string _json = r.ReadToEnd();
                DataTable _table = (DataTable)JsonConvert.DeserializeObject(_json, (typeof(DataTable)));

                return _table;
            }
        }

        public void Play()
        {
            string _input = "";
            string[] _directions = { "n", "s", "e", "w" };
            string _randomDirection = "";
            while (_input != "q")
            {
                Show(Player);
                _input = Console.ReadLine();

                if (_input == "n" || _input == "s" || _input == "e" || _input == "w")
                {
                    foreach (Enemy Enemy in Enemies)
                    {
                        int _x = RandomMove();
                        _randomDirection = _directions[_x];
                        Enemy.Move(_randomDirection);
                    }
                }

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

                int _enemiesDead = 0;

                foreach (Enemy enemy in Enemies)
                {
                    if (enemy.Total <= 0)
                    {
                        _enemiesDead++;
                    }
                }

                if (Enemies.Count() == _enemiesDead)
                {
                    Console.WriteLine("!!! Sie haben alle Gegner besiegt und somit das Spiel gewonnen !!!");
                    break;
                }
            }


            void Show(Player p)
            {
                Enemy _enemy = p.Position.GetEnemy();   // checking if enemy in room
                Console.WriteLine("");
                Console.WriteLine(string.Empty.PadLeft(Console.WindowWidth - Console.CursorLeft, '─'));
                Console.WriteLine("Sie befinden sich in " + p.Position.Name + " und Ihr leben beträgt " + p.Total + ". Geben Sie Norden(n), Osten(e), Westen(w) oder Süden(s) ein um sich zu bewegen.");
                p.Position.Look();
                if (_enemy != null)
                {
                    Console.WriteLine("");
                    Console.WriteLine("DUM DUMM DUMM DUMMMMMMMMM");
                    Console.WriteLine("In ihrem Raum befindet sich der Gegner " + _enemy.Name + " der Items hat die Sie eventuell gebrauchen könnten. Dein Gegner hat noch " + _enemy.Total + " Lebenspunkte. Wollen Sie ihn mit <a> angreifen um ein Item von ihm zu gewinnen? Seine Items:");
                    if (p.Position.GetEnemy().Items.Count != 0)
                    {
                        for (int i = 0; i < p.Position.GetEnemy().Items.Count; i++)
                        {
                            Console.WriteLine("- " + p.Position.GetEnemy().Items[i].Name);
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