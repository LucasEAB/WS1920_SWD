using System;
using System.Collections.Generic;
using System.Data;

namespace TextAdventure
{
    public class Room
    {
        public List<Item> RoomItems = new List<Item>();
        public List<Characters> RoomPlayers = new List<Characters>();    
        public string Name;
        public Room North ; 
        public Room South; 
        public Room West; 
        public Room East;

        public void Exit (Characters character)
        {
            RoomPlayers.Remove(character);
        }

        public void Entry (Characters character)
        {
            RoomPlayers.Add(character);
        }

        public Enemy GetEnemy() // get data from enemy 
        {
            for (int i = 0; i < RoomPlayers.Count; i++)
            {   
                if(RoomPlayers[i].GetType() == typeof(Enemy))
                {       
                    if(RoomPlayers[i].Total > 0)
                    {
                        return (Enemy)RoomPlayers[i];
                    }
                }
            }
            return null;
        }

        public Item Take (string item)
        {
            Item _take = null;
            for(int i=0; i < RoomItems.Count ;i++)
            {
                if(RoomItems[i].Name == item)
                {
                    _take = RoomItems[i];
                    RoomItems.Remove(RoomItems[i]);
                    Console.WriteLine("Sie haben " +item +" erfolgreich hinzugefügt.");
                }
            }
            return _take;
        }
        
        public void Drop (Item item)
        {
            RoomItems.Add(item);
            Console.WriteLine("Sie haben " +item.Name +" erfolgreich abgelegt.");
        }

        public void Look()
        {
            Console.WriteLine("Hier liegen " + RoomItems.Count +" Items. Geben Sie take(t) ein und anschließend den Namen des Items, um eins in Ihr Inventar einzufügen.");
            Console.WriteLine("Hier liegen folgende Items: ");

            for(int i=0; i<RoomItems.Count; i++)
            {
                Console.WriteLine(RoomItems[i].Name );
            }            
        }
    }
} 
