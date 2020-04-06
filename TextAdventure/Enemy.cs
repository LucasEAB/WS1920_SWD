using System;
using System.Collections.Generic;
using System.Data;

namespace TextAdventure
{
    public class Enemy : Characters
    {
        public Enemy(string _name, Room _position) : base(_name, _position)
        {
        }
    }
}