using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Ailurophobia
{
    class Weapon:GameComponent
    {
        const double devAngle = (double)(20 * Math.PI / 180);
        public enum WeaponType {BASIC, SHOTGUN, GUIDED, CATNIP };
        WeaponType currentWeapon;
        WeaponType prevWeapon;

        //public WeaponType PrevWeapon
        //{
        //    get { return prevWeapon; }
        //    set { prevWeapon = value; }
        //}

        public WeaponType CurrentWeapon
        {
            get { return currentWeapon; }
        }

        public void SetWeapon(WeaponType newWep)
        {
            prevWeapon = currentWeapon;
            currentWeapon = newWep;
        }

        public Weapon(Game game)
            : base(game)
        {
            currentWeapon = 0;
            prevWeapon = currentWeapon;
        }

        // Pass in the positions of the character and mouse cursor, so as to
        // calculate the direction of the line generated for the projectile.
        public void FireWeapon(Vector2 mouse, float angle)
        {
            float nAngle = angle;

            switch(currentWeapon)
            {
                case WeaponType.BASIC: // single-shot squirt gun
                    {
                        const int vel = 7;
                        const int dmg = 3;
                        CreateProjectile(nAngle, -vel, dmg, mouse, 0);
                        break;
                    }

                case WeaponType.SHOTGUN: // three-shot squirt gun
                    {
                        const int vel = 7;
                        const int dmg = 3;
                        CreateProjectile((float)(nAngle + devAngle),  -vel, dmg, mouse, WeaponType.SHOTGUN);
                        CreateProjectile((float)(nAngle - devAngle),  -vel, dmg, mouse, WeaponType.SHOTGUN);
                        CreateProjectile(nAngle,  -vel, dmg, mouse, WeaponType.SHOTGUN);
                        break;
                    }
                case WeaponType.GUIDED: // guided single shot
                    {
                        const int vel = 7;
                        const int dmg = 5;
                        CreateProjectile(nAngle,  -vel, dmg, mouse, WeaponType.GUIDED);
                        break;
                    }
                case WeaponType.CATNIP: // catnip
                    {
                        const int dmg = 10;
                        MouseState ms = Microsoft.Xna.Framework.Input.Mouse.GetState();
                        CreateProjectile(0, 0, dmg, new Vector2(ms.X, ms.Y), WeaponType.CATNIP);
                        Console.WriteLine("Prev Weapon:{0} Current Weapon:{1}", prevWeapon, currentWeapon);
                        break;
                    }

                default:
                    break;
            }
        }

        // Create a new projectile object and draw the line based on a passed in angle
        public void CreateProjectile(float angle, int velocity, int damage, Vector2 position, WeaponType type)
        {
            this.Game.Components.Add(new Projectile(this.Game, position, angle, velocity, damage, type));
        }
    }
}