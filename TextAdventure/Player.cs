using System;
using System.Collections.Generic;
using System.Data;

namespace TextAdventure
{
    public class Player : Character
    {
        public Player(string _name, Room _position) : base(_name, _position)
        {
        }

        int MyRandomTotal()
        {
            Random _randomNumber = new Random();
            int _random = _randomNumber.Next(0, 10);
            return _random;
        }

        int EnemyRandomTotal()
        {
            Random _randomNumber = new Random();
            int _random = _randomNumber.Next(0, 20);
            return _random;
        }

        public void Attack()
        {
            int _reduce = MyRandomTotal();
            Total = Total - _reduce;
            int _myReduce = _reduce;
            Console.WriteLine("Ihr Kontostand hat sich um " + _reduce + " Euro verringert.");

            Enemy enemy = Position.GetEnemy();
            _reduce = EnemyRandomTotal();
            int _enemyReduce = _reduce;
            enemy.Total -= _reduce;

            Console.WriteLine("Der Kontostand von deinem Gegner hat sich um " + _reduce + " Euro verringert.");
            
            if (_myReduce > _enemyReduce)
            {
                Console.WriteLine("Sie haben gegen " + enemy.Name + " verloren .");

            }
            if (enemy.Total > 0 && _myReduce < _enemyReduce)
            {
                Console.WriteLine("Sie haben das Duell gegen " + enemy.Name + " gewonnen. Sie können nun ein Item von " + enemy.Name + " hinzufügen indem Sie den Namen des Items eingeben.");
                TakeEnemyItems();
            }
            if (enemy.Total <= 0)
            {
                Console.WriteLine("Sie haben gegen " + enemy.Name + " gewonnen und Ihn somit aus dem Spiel gekickt");
            }
        }

        public Item TakeEnemyItems()
        {
            string _item = Console.ReadLine();
            Item _take = null;
            Enemy _enemy = null;
            for (int i = 0; i < Position.GetEnemy().Items.Count; i++)
            {
                _enemy = Position.GetEnemy();
                if (_enemy.Items[i].Name == _item)
                {
                    _take = _enemy.Items[i];
                    _enemy.Items.Remove(_take);
                    Insert(_take);

                    Console.WriteLine("Sie haben " + _item + " erfolgreich hinzugefügt.");
                }
            }
            return _take;
        }

    }
}


