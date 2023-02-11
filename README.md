# CodeSample: Generic ObjectPool
Generic Object Pool system wrapping Unity3D's inbuilt ObjectPool which is an effective stack implementation under the hood that they optimize occasionally.<T>

This package of code allows you to quickly setup the needed events with some quick properties to help common use cases, while enabling the ability to over-ride for special use cases.


This repo contains:
* the core scripts for review in github
* a .unitypackage you can download to load into any Unity project with a quick double click

This package was coded in Unity 2021.3.6f1 but should work for any Unity 2021.1 or later version as that was when ObjectPool<T0> was introduced.

## ObjectPoolExample.UnityPackage
Once you download the package, simply double click while your Unity scene is open, the package contains all needed code/prefabs and a test scene pre-setup. 

On running the test scene, it will:
* Creates 20 objects and de-actives each one under >Enemies in the editor hierachy
* When you press F it will enable an object under >Enemies or create a new one under >Enemies
* When you hit G it will disable the object


## To use the package in your own project

Create a new class such as Bullet, code some movement logic for the bullet, then create a new class called BulletSpawner.
On BulletSpawner.cs simply inherit SpawnerBase (which includes monobehavior already).

Attach the BulletSpawner to a blank gameobject in your scene hierachy, configure starting values in inspector (hover for tooltips)
Then use your preference of event wiring or GetComponent methods to acquire the interface IPoolSpawner<Bullet> and call .Spawn(). 

Note: Without additions to the BulletSpawner.cs class it will spawn directly at the Transform of BulletSpawner but as you can see in the Enemey example this is extendable.

No credits required
