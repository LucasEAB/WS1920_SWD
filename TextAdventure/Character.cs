using System;
using System.Collections.Generic;
using System.Data;

namespace TextAdventure
{
    public class Character
    {
        public int Total = 100;
        public string Name;
        public List<Item> Items = new List<Item>();
        public Room Position;

        public Character(string _name, Room _position) //constructor
        {
            Position = _position;
            Name = _name;
            try
            {
                Position.Entry(this);
            }
            catch
            {

            }
        }

        public void Inventory()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Console.WriteLine("Ihr Item: " + Items[i].Name);
            }
        }

        public void Insert(Item item)
        {
            Items.Add(item);
        }

        public Item Delete(string item)
        {
            Item _drop = null;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Name == item)
                {
                    _drop = Items[i];
                    Items.Remove(Items[i]);
                }
            }
            return _drop;
        }

        public void Move(string input)
        {
            Room _newRoom;
            Room _oldRoom = Position;
            Boolean _occupied = false;

            switch (input)
            {
                case "n":
                    _newRoom = Position.North;
                    break;
                case "s":
                    _newRoom = Position.South;
                    break;
                case "w":
                    _newRoom = Position.West;
                    break;
                case "e":
                    _newRoom = Position.East;
                    break;
                default:
                    Console.WriteLine("Falsche Eingabe");
                    _newRoom = Position;
                    break;
            }

            if (_newRoom == null)
            {
                if (this.GetType() == typeof(Player)) //only player
                {
                    Console.WriteLine("");
                    Console.WriteLine("Diesen Weg gibt es nicht. Ihre lebenspunkte haben sich um 5 reduziert");
                    this.Total -= 5;
                }
            }
            else
            {
                try
                {
                    _occupied = _newRoom.EnemyInRoom();
                }
                catch
                {
                    _occupied = false;
                }

                if (this.GetType() == typeof(Player) && _newRoom != Position)
                {
                    Position = _newRoom;
                    _oldRoom.Exit(this);
                    _newRoom.Entry(this);
                    Console.WriteLine("");
                    Console.WriteLine("Sie gingen von " + _oldRoom.Name + " nach " + _newRoom.Name);
                }

                if (this.GetType() == typeof(Enemy) && _newRoom != Position && _occupied != true)
                {
                    Position = _newRoom;
                    _oldRoom.Exit(this);
                    _newRoom.Entry(this);
                }
            }
        }
    }
}