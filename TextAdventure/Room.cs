using System;
using System.Collections.Generic;
using System.Data;

namespace TextAdventure
{
    public class Room
    {
        public List<Item> Items = new List<Item>();
        public List<Character> Characters = new List<Character>();
        public string Name;
        public Room North;
        public Room South;
        public Room West;
        public Room East;

        public void Exit(Character character)
        {
            Characters.Remove(character);
        }

        public void Entry(Character character)
        {
            Characters.Add(character);
        }

        public Boolean EnemyInRoom()
        {
            for (int i = 0; i < Characters.Count; i++)
            {
                if (Characters[i].GetType() == typeof(Enemy))
                {
                    return true;
                }
            }
            return false;
        }

        public Enemy GetEnemy() // get data from enemy 
        {
            for (int i = 0; i < Characters.Count; i++)
            {
                if (Characters[i].GetType() == typeof(Enemy))
                {
                    if (Characters[i].Total > 0)
                    {
                        return (Enemy)Characters[i];
                    }
                }
            }
            return null;
        }

        public Item Take(string item)
        {
            Item _take = null;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Name == item)
                {
                    _take = Items[i];
                    Items.Remove(Items[i]);
                    Console.WriteLine("Sie haben " + item + " erfolgreich hinzugefügt.");
                }
            }
            return _take;
        }

        public void Drop(Item item)
        {
            Items.Add(item);
            Console.WriteLine("Sie haben " + item.Name + " erfolgreich abgelegt.");
        }

        public void Look()
        {
            Console.WriteLine("");
            Console.WriteLine("Hier liegen " + Items.Count + " Items. Geben Sie take(t) ein und anschließend den Namen des Items, um eins in Ihr Inventar einzufügen.");
            Console.WriteLine("Hier liegen folgende Items: ");

            for (int i = 0; i < Items.Count; i++)
            {
                Console.WriteLine("- " + Items[i].Name);
            }
        }
    }
}
